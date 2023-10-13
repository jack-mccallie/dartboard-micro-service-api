using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Models;

namespace src.Services;

public interface IGameService
{
    Task<ActionResult<IEnumerable<Game>>> GetGames();
    Task<Game?> GetGame(int id);
    Task<PostGameDTO> PostGame(PostGameDTO gameDTO);
    Task<IEnumerable<DBGame>> GetGamesMongoDB(String database, String collection);
    Task<PlayerRecordDTO> GetPlayerRecord(String playerName);
    Task<List<KeyValuePair<String, int>>> GetPlayerGameWins(List<String> players);

}
