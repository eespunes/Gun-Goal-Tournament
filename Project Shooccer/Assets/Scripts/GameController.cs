public class GameController
{
    private static GameController _instance;
    public PlayerController PlayerController { get; set; }

    private GameController()
    {
    }

    public static GameController GetInstance()
    {
        return _instance ?? (_instance = new GameController());
    }
}