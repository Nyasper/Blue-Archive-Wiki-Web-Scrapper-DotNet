using System.Collections;

namespace Scanner.CharaDetails;
using Model;


public interface ICharaDetails
{
	Task<Student> ScanInfo(string charaNameParam);
	Task<IEnumerable<Student>> ScanInfo(IEnumerable<CharaListItem> charasItems);
	Task<IEnumerable<Student>> ScanInfo(IEnumerable<Student> students);

}