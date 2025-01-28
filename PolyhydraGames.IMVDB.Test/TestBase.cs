namespace PolyhydraGames.IMVDB.Test;

public abstract class TestBase
{
    [SetUp]
    public async Task Setup()
    {
        //await SystemsDatabase.Instance.Initialize();
    }
}