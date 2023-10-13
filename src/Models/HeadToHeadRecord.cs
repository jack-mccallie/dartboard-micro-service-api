namespace src.Models;

public class HeadToHeadRecord
{
    public String Opponent { get; set; } = null!;

    public int Wins { get; set; }

    public int Losses { get; set; }

    public int Total { get; set; }

    public HeadToHeadRecord(String opponent, int wins, int total) {
        Opponent = opponent;
        Wins = wins;
        Total = total;
        Losses = total - wins;
    }

}
