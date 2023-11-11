using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Models;

namespace src.Services;

public interface IGameService
{
    Task<Game?> GetGame(int id);
    Task PostGame(PostGameDTO gameDTO);
    Task<IEnumerable<Game>> GetGames(String database, String collection);
    Task<PlayerRecordDTO> GetPlayerRecord(String playerName);
    Task<List<KeyValuePair<String, int>>> GetPlayerGameWins(List<String> players);

    Task<double> GetPlayerScore(String playerName);

    Task<Boolean> DeleteGame(String gameId);

}
