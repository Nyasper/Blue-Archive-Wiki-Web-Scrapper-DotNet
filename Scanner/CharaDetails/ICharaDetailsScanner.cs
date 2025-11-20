using System.Collections;

namespace Scanner.CharaDetails;
using Model;


public interface ICharaDetailsScanner
{
	Task<Student> ScanStudentDetails(string charaNameParam);
	Task<IEnumerable<Student>> ScanStudentDetails(IEnumerable<StudentListItem> studentListItems);
	Task<IEnumerable<Student>> ScanStudentDetails(IEnumerable<Student> students);

}