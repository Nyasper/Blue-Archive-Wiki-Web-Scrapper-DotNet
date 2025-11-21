using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scanner.CharaDetails;
using Model;


public interface ICharaDetailsScanner
{
	Task<StudentDetailsItem> ScanStudentDetails(string charaNameParam);
	Task<StudentDetailsItem[]> ScanStudentDetails(IEnumerable<StudentListItem> studentListItems);
	Task<StudentDetailsItem[]> ScanStudentDetails(IEnumerable<Student> students);

}