using src.Models;

namespace src.DTO;

public class PostGameDTO
{
    // This is the list of players in order of the winner down to second place
    public required IEnumerable<String> WinningListInOrder { get; set; }

    public PostGameDTO() 
    {
    }
    
    public PostGameDTO(Game game) {
        WinningListInOrder = game.WinningListInOrder;
    }

}
