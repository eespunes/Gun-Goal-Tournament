public class MatchController
{
    private static MatchController _instance;
    public float Time { get; set; }
    public string TimeString { get; set; }

    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public bool Playing { get; set; }

    public ScoreboardController ScoreboardController { get; set; }
    public bool SplitScreen { get; set; }


    private MatchController()
    {
        Time = -1;
        HomeScore = 0;
        AwayScore = 0;
    }

    public static MatchController GetInstance()
    {
        return _instance ?? (_instance = new MatchController());
    }

    public void HomeGoal()
    {
        HomeScore++;
        ScoreboardController.Goal();
    }

    public void AwayGoal()
    {
        AwayScore++;
        ScoreboardController.Goal();
    }
}