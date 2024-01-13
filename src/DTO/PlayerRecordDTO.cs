using src.Models;

namespace src.DTO;

public class PlayerRecordDTO
{
    public String PlayerName { get; set; } = null!;
    public int PlayerScore { get; set; }
    public int PossiblePoints { get; set; }
    public int TwoPlayerWins { get; set; }
    public int TwoPlayerTotalGames { get; set; }
    public int ThreePlayerTotalGames { get; set; }
    public int ThreePlayerWins { get; set; }
    public IEnumerable<HeadToHeadRecord> HeadToHeadRecords { get; set; } = null!;

}
