using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Models;

namespace src.Services;

public interface IGameService
{
    Task<Game?> GetGame(int id);
    Task PostGame(PostGameDTO gameDTO);
    Task<IEnumerable<Game>> GetGames(String database, String collection, int season);
    Task<PlayerRecordDTO> GetPlayerRecord(String playerName, int season);
    Task<List<KeyValuePair<String, int>>> GetPlayerGameWins(List<String> players, int season);

    Task<IEnumerable<PlayerRecordDTO>> GetPlayersRanked(IEnumerable<String> players, int season);
    Task<Boolean> DeleteGame(String gameId);

}
