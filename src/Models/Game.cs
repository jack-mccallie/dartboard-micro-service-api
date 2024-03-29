using System.ComponentModel.DataAnnotations;

namespace src.Models;

public class Game {
    public String Id { get; set; }

    // This is the list of players in order of who won
    // currently there is no logic for second place so the 
    // only important thing is the winning player is the first
    // in the list
    public IEnumerable<String> WinningListInOrder { get; set; }
    public String Winner { get; set; }
    public String Type { get; set; }
    public DateTime DateOfEntry { get; set; }
    public int Season { get; set; }

    public Game(DBGame dbGame) {
        Id = dbGame.Id.ToString();
        WinningListInOrder = dbGame.WinningListInOrder;
        Winner = dbGame.WinningListInOrder.First();
        Type = dbGame.Type;
        DateOfEntry = dbGame.DateEntered;
        Season = dbGame.Season;
    }

}