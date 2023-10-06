using games_recording_service.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using src.DTO;
using src.Models;

namespace games_recording_service.Services;

public interface IGameService
{
    Task<ActionResult<IEnumerable<Game>>> GetGames();
    Task<Game?> GetGame(int id);
    Task<PostGameDTO> PostGame(PostGameDTO gameDTO);
    Task<IEnumerable<DBGame>> GetGamesMongoDB(String database, String collection);

}
