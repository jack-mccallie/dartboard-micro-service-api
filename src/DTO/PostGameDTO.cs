using Newtonsoft.Json;
using src.Models;

namespace src.DTO;

public class PostGameDTO
{
    // This is the list of players in order of the winner down to second place
    public required IEnumerable<String> WinningListInOrder { get; set; }

    public required String Type { get; set; }

    public int Season { get; set; }

    public PostGameDTO() 
    {
    }
    
    public PostGameDTO(Game game) {
        WinningListInOrder = game.WinningListInOrder;
        Type = game.Type;
        Season = game.Season;
    }

}
