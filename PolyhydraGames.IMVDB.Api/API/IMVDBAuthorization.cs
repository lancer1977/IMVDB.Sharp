namespace PolyhydraGames.IMVDB.API;

public class IMVDBAuthorization : IIMVDBAuthorization
{
    public string APIKey { get; set; }
    public string GetIncludes()
    {
        return "include=credits,bts,countries";
    }
}