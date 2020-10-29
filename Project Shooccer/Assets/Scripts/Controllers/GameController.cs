public class GameController
{
    private static GameController _instance;

    private GameController()
    {
    }

    public static GameController GetInstance()
    {
        return _instance ?? (_instance = new GameController());
    }
}