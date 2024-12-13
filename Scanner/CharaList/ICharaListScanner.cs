namespace Scanner.CharaList;
using Model;

public interface ICharaListScanner
{
	Task<IEnumerable<CharaListItem>> ScanCharaList();
}
