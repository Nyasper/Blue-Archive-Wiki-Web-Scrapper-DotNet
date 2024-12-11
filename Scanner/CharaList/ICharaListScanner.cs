namespace Scanner.CharaList;
using Model;

public interface ICharaListScanner
{
	Task<HashSet<CharaListItem>> ScanCharaList();
}
