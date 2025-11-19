using System.Collections;

namespace Scanner.CharaDetails;
using Model;


public interface ICharaDetailsScanner
{
	Task<Student> ScanInfo(string charaNameParam);
	Task<IEnumerable<Student>> ScanInfo(IEnumerable<CharaListStudent> charasItems);
	Task<IEnumerable<Student>> ScanInfo(IEnumerable<Student> students);

}