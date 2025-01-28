namespace PolyhydraGames.IMVDB.API;

public interface IIMVDBAuthorization
{
    string APIKey { get; }
    string GetIncludes();
}