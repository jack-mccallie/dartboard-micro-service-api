using src.Models;

namespace src.DTO;

public class PlayerRecordDTO
{
    public String PlayerName { get; set; } = null!;
    public double PlayerScore { get; set; }
    public int TwoPlayerWins { get; set; }
    public int TwoPlayerTotalGames { get; set; }
    public int ThreePlayerWins { get; set; }
    public int ThreePlayerTotalGames { get; set; }
    public IEnumerable<HeadToHeadRecord> HeadToHeadRecords { get; set; } = null!;

}
