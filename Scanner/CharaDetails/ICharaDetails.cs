namespace Scanner.CharaDetails;
using Model;


public interface ICharaDetails<S>
{
	Task<S> ScanInfo(string charaNameParam);
	Task<IEnumerable<S>> ScanMany(IEnumerable<CharaListItem> charasItems);
}