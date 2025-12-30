namespace Main.FileHandler.FileGenerator;

using System.Text.Json;

using Repository;

using Scanner.Model;

using Utils;


public class FileGenerator(IRepository<Student> repository) : IFileGenerator
{
	public async Task<string> GenerateJsonData()
	{
		var students = await repository.GetAll();

		const string fileName = "data";
		string finalPath = Path.Join(Constants.DataPath, fileName + ".json");

		string jsonData = JsonSerializer.Serialize<IEnumerable<Student>>(students);
		await File.WriteAllTextAsync(finalPath, jsonData);

		Notifier.MessageTaskCompleted($"data json generated in: {finalPath}");
		return finalPath;
	}

	public async Task<string> GenerateHtmlImagePreview()
	{
		var students = await repository.GetAll();
		string htmlPath = await CreateHtmlImagesPreview(students);

		Notifier.MessageTaskCompleted($"Images preview HTML generated in: {htmlPath}");
		return htmlPath;
	}

	private static async Task<string> CreateHtmlImagesPreview(IEnumerable<Student> students)
	{
		const string fileName = "imagesPreview";
		var allSchools = students.GroupBy(s => s.School).ToArray();

		string htmlContent = GenerateHtmlContent(allSchools);
		string finalHtml = $"{GetHtmlHeader()}\n{htmlContent}\n{GetHtmlFooter()}";

		string filePath = Path.Join(Constants.DataPath, $"{fileName}.html");
		await File.WriteAllTextAsync(filePath, finalHtml);

		return filePath;
	}

	private static string GetJavaScript()
	{
		return @"
	const modal = document.getElementById('imageModal');
	const modalImg = document.getElementById('modalImage');
	const modalCaption = document.querySelector('.modal-caption');
	const closeBtn = document.querySelector('.modal-close');

	document.querySelectorAll('.zoomable').forEach(img => {
	    img.addEventListener('click', function() {
	        modal.classList.add('show');
	        modalImg.src = this.src;
	        modalCaption.textContent = this.dataset.caption || this.alt;
	    });
	});

	closeBtn.addEventListener('click', () => {
	    modal.classList.remove('show');
	});

	modal.addEventListener('click', (e) => {
	    if (e.target === modal) {
	        modal.classList.remove('show');
	    }
	});

	document.addEventListener('keydown', (e) => {
	    if (e.key === 'Escape' && modal.classList.contains('show')) {
	        modal.classList.remove('show');
	    }
	});

	const schoolSections = document.querySelectorAll('.school-section');
	const schoolNav = document.getElementById('schoolNav');

	schoolSections.forEach(section => {
	    const schoolName = section.querySelector('.schoolTitle').textContent;
	    const schoolId = section.id;
	    
	    const navItem = document.createElement('div');
	    navItem.className = 'school-nav-item';
	    navItem.textContent = schoolName;
	    
	    navItem.addEventListener('click', () => {
	        section.scrollIntoView({ behavior: 'smooth', block: 'start' });
	    });
	    
	    schoolNav.appendChild(navItem);
	});
	";
	}

	private static string GetHtmlHeader()
	{
		return @"<!DOCTYPE html>
	<html lang=""en"">
	<head>
	    <meta charset=""UTF-8"">
	    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
	    <title>Students Images Preview</title>
	</head>
	<body>
	    <nav class=""sidebar"">
	        <div class=""sidebar-header"">
	            <h3>Schools</h3>
	        </div>
	        <div id=""schoolNav"" class=""school-nav""></div>
	    </nav>
	    <div class=""main-content"">";
	}

	private static string GetHtmlFooter()
	{
		return $@"
	    </div>
	    <div id=""imageModal"" class=""modal"">
	        <span class=""modal-close"">&times;</span>
	        <img class=""modal-content"" id=""modalImage"" alt=""Enlarged image"" />
	        <div class=""modal-caption""></div>
	    </div>
	<style>{GetStylesCss()}</style>
	<script>{GetJavaScript()}</script>
	</body>
	</html>";
	}

	private static string GetStylesCss()
	{
		return @"
	:root {
	    --bg-primary: #0f0f0f;
	    --bg-secondary: #1a1a1a;
	    --bg-card: #2a2a2a;
	    --text-primary: #ffffff;
	    --text-secondary: #b3b3b3;
	    --accent: #4a9eff;
	    --accent-hover: #6bb0ff;
	    --border-radius: 16px;
	    --shadow: 0 4px 12px rgba(0, 0, 0, 0.5);
	}

	* {
	    box-sizing: border-box;
	    margin: 0;
	    padding: 0;
	}

	body {
	    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, sans-serif;
	    background: linear-gradient(135deg, var(--bg-primary) 0%, var(--bg-secondary) 100%);
	    color: var(--text-primary);
	    line-height: 1.6;
	    min-height: 100vh;
	    display: flex;
	    overflow-x: hidden;
	}

	.sidebar {
	    position: fixed;
	    left: 0;
	    top: 0;
	    width: 250px;
	    height: 100vh;
	    background: var(--bg-card);
	    border-right: 1px solid rgba(255, 255, 255, 0.1);
	    padding: 20px;
	    overflow-y: auto;
	    z-index: 100;
	    box-shadow: 4px 0 12px rgba(0, 0, 0, 0.3);
	}

	.sidebar-header {
	    margin-bottom: 25px;
	    padding-bottom: 15px;
	    border-bottom: 2px solid var(--accent);
	}

	.sidebar-header h3 {
	    font-size: 1.4rem;
	    font-weight: 700;
	    background: linear-gradient(135deg, var(--accent), var(--accent-hover));
	    -webkit-background-clip: text;
	    -webkit-text-fill-color: transparent;
	    background-clip: text;
	}

	.school-nav {
	    display: flex;
	    flex-direction: column;
	    gap: 10px;
	}

	.school-nav-item {
	    padding: 12px 16px;
	    background: var(--bg-secondary);
	    border-radius: 8px;
	    cursor: pointer;
	    transition: all 0.3s ease;
	    border: 1px solid transparent;
	    font-size: 0.95rem;
	    font-weight: 500;
	}

	.school-nav-item:hover {
	    background: var(--bg-primary);
	    border-color: var(--accent);
	    transform: translateX(5px);
	}

	.main-content {
	    margin-left: 250px;
	    padding: 20px;
	    width: calc(100% - 250px);
	}

	.schoolTitle {
	    font-size: clamp(1.8rem, 4vw, 2.6rem);
	    font-weight: 700;
	    margin: 40px 0 25px;
	    text-align: center;
	    background: linear-gradient(135deg, var(--accent), var(--accent-hover));
	    -webkit-background-clip: text;
	    -webkit-text-fill-color: transparent;
	    background-clip: text;
	    letter-spacing: -0.5px;
	}

	.schoolContainer {
	    background: var(--bg-card);
	    border-radius: var(--border-radius);
	    padding: 30px 20px;
	    margin: 0 auto 40px;
	    max-width: 1400px;
	    display: grid;
	    grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
	    gap: 25px;
	    box-shadow: var(--shadow);
	}

	.studentContainer {
	    background: var(--bg-secondary);
	    border-radius: 12px;
	    padding: 20px;
	    transition: transform 0.3s ease, box-shadow 0.3s ease;
	    border: 1px solid rgba(255, 255, 255, 0.05);
	    max-width: 350px;
	    width: 100%;
	}

	.studentContainer:hover {
	    transform: translateY(-5px);
	    box-shadow: 0 8px 20px rgba(74, 158, 255, 0.2);
	}

	.studentSubtitleContainer {
	    display: flex;
	    align-items: center;
	    gap: 12px;
	    margin-bottom: 15px;
	    padding-bottom: 12px;
	    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
	}

	.smallImage {
	    width: 45px;
	    height: 45px;
	    border-radius: 50%;
	    object-fit: cover;
	    border: 2px solid var(--accent);
	    box-shadow: 0 2px 8px rgba(74, 158, 255, 0.3);
	}

	.studentContainer h2 {
	    font-size: 1.1rem;
	    font-weight: 600;
	    color: var(--text-primary);
	    margin: 0;
	}

	.imageContainer {
	    display: flex;
	    gap: 10px;
	    margin: 15px 0;
	    height: 180px;
	    background: var(--bg-primary);
	    border-radius: 8px;
	    padding: 10px;
	    overflow: hidden;
	}

	.imageContainer img {
	    flex: 1;
	    height: 100%;
	    object-fit: contain;
	    border-radius: 6px;
	    transition: transform 0.3s ease;
	    cursor: pointer;
	}

	.imageContainer img:hover {
	    transform: scale(1.05);
	}

	.modal {
	    display: none;
	    position: fixed;
	    z-index: 1000;
	    left: 0;
	    top: 0;
	    width: 100%;
	    height: 100%;
	    background-color: rgba(0, 0, 0, 0.95);
	    animation: fadeIn 0.3s ease;
	}

	.modal.show {
	    display: flex;
	    align-items: center;
	    justify-content: center;
	}

	.modal-content {
	    max-width: 90%;
	    max-height: 90%;
	    object-fit: contain;
	    border-radius: 8px;
	    animation: zoomIn 0.3s ease;
	}

	.modal-close {
	    position: absolute;
	    top: 20px;
	    right: 35px;
	    color: #fff;
	    font-size: 40px;
	    font-weight: bold;
	    cursor: pointer;
	    transition: color 0.3s ease;
	}

	.modal-close:hover {
	    color: var(--accent);
	}

	.modal-caption {
	    position: absolute;
	    bottom: 20px;
	    left: 50%;
	    transform: translateX(-50%);
	    color: #fff;
	    font-size: 1.1rem;
	    text-align: center;
	    background: rgba(0, 0, 0, 0.7);
	    padding: 10px 20px;
	    border-radius: 8px;
	}

	@keyframes fadeIn {
	    from { opacity: 0; }
	    to { opacity: 1; }
	}

	@keyframes zoomIn {
	    from { transform: scale(0.8); opacity: 0; }
	    to { transform: scale(1); opacity: 1; }
	}

	audio {
	    width: 100%;
	    height: 35px;
	    margin-top: 10px;
	    border-radius: 20px;
	    outline: none;
	}

	audio::-webkit-media-controls-panel {
	    background: var(--bg-primary);
	}

	@media (max-width: 768px) {
	    .sidebar {
	        width: 200px;
	    }
	    
	    .main-content {
	        margin-left: 200px;
	        width: calc(100% - 200px);
	        padding: 10px;
	    }
	    
	    .schoolContainer {
	        grid-template-columns: 1fr;
	        padding: 20px 15px;
	    }
	    
	    .imageContainer {
	        height: 150px;
	    }
	}

	@media (max-width: 580px) {
	    .sidebar {
	        transform: translateX(-100%);
	        transition: transform 0.3s ease;
	    }
	    
	    .sidebar.active {
	        transform: translateX(0);
	    }
	    
	    .main-content {
	        margin-left: 0;
	        width: 100%;
	    }
	}";
	}

	private static string GenerateHtmlContent(IGrouping<string, Student>[] allSchools)
	{
		var schoolsHtml = allSchools.Select(school => GenerateSchoolSection(school));
		return string.Join("\n", schoolsHtml);
	}

	private static string GenerateSchoolSection(IGrouping<string, Student> school)
	{
		var studentsHtml = school.Select((student, index) =>
				GenerateStudentCard(student, index + 1));

		string schoolId = school.Key.Replace(" ", "-").ToLower();

		return $@"
			<section id=""{schoolId}"" class=""school-section"">
			    <h2 class=""schoolTitle"">{school.Key}</h2>
			    <div class=""schoolContainer"">
			        {string.Join("\n        ", studentsHtml)}
			    </div>
			</section>";
	}

	private static string GenerateStudentCard(Student student, int position)
	{
		string schoolPath = $"../media/{student.School}";
		string studentName = student.CharaName;

		return $@"<div class=""studentContainer"">
	        <div class=""studentSubtitleContainer"">
	            <img src=""{schoolPath}/{studentName}_small.png"" 
	                 class=""smallImage"" 
	                 alt=""{studentName}"" />
	            <h2>{position}: {studentName}</h2>
	        </div>
	        <div class=""imageContainer"">
	            <img src=""{schoolPath}/{studentName}.png"" 
	                 class=""profileImage zoomable"" 
	                 alt=""{studentName} profile"" 
	                 data-caption=""{studentName} - Profile image"" />
	            <img src=""{schoolPath}/{studentName}_full.png"" 
	                 class=""fullImage zoomable"" 
	                 alt=""{studentName} full"" 
	                 data-caption=""{studentName} - Full image"" />
	        </div>
	        <audio controls>
	            <source src=""{schoolPath}/{studentName}.ogg"" type=""audio/ogg"" />
	            Your browser does not support the audio element.
	        </audio>
	    </div>";
	}
}