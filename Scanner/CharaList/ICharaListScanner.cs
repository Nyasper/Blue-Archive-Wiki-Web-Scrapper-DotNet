namespace Scanner.CharaList;
using Model;

public interface ICharaListScanner
{
	Task<CharaListStudent[]> ScanCharaList();
}