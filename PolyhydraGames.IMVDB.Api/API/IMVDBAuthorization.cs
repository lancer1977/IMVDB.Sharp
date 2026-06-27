namespace PolyhydraGames.IMVDB.API
{
    public class IMVDBAuthorization : IIMVDBAuthorization
    {
        public string APIKey { get; set; } = string.Empty;

        public string GetIncludes()
        {
            return "include=credits,bts,countries";
        }
    }
}
