using src.Models;

namespace src.DTO;

public class PlayerRecordDTO
{
    public String PlayerName { get; set; } = null!;
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int TotalGames { get; set; }

    public IEnumerable<HeadToHeadRecord> HeadToHeadRecords { get; set; } = null!;

}
