using src.Models;

namespace src.DTO;

public class PostGameDTO
{
    public String HomeTeam { get; set; } = null!;
    public String AwayTeam { get; set; } = null!;
    public String? Winner { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public DateTime? Date { get; set; }

    public PostGameDTO() {}
    public PostGameDTO(Game game) {
        HomeTeam = game.HomeTeam;
        AwayTeam = game.AwayTeam;
        HomeScore = game.HomeTeamScore;
        AwayScore = game.AwayTeamScore;
        Winner = game.Winner;
        Date = game.DateOfEntry;
    }

}
