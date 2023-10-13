using System.ComponentModel.DataAnnotations;

namespace src.Models;

public class Game {
    [Key]
    public int Id { get; set; }
    public String Winner {get; set;} = null!;
    public String HomeTeam {get; set;} = null!;
    public String AwayTeam {get; set;} = null!;
    public int HomeTeamScore {get; set;}
    public int AwayTeamScore {get; set;}
    public DateTime DateOfEntry;

}