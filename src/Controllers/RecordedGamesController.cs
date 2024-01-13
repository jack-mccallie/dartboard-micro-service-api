using src.DTO;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecordedGamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public RecordedGamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    // GET: api/RecordedGames/{database}/{collection}
    [HttpGet("{database}/{collection}")]
    public async Task<IEnumerable<Game>> GetGames(String database, String collection)
    {
        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return await _gameService.GetGames(database, collection);
    }

    /// <summary>
    /// This will return a players overall record and a list of their head to
    /// head records with individual players
    /// </summary>
    /// <param name="playerName"></param>
    /// <returns></returns>
    [HttpGet("PlayerRecord")]
    public async Task<PlayerRecordDTO> GetPlayerRecord(String playerName)
    {
        PlayerRecordDTO playerRecord = await _gameService.GetPlayerRecord(playerName);
        return playerRecord;
    }

    /// <summary>
    /// This will return an ordered list of player records to fill out a leaderboard
    /// </summary>
    /// <param name="players"></param>
    /// <returns>Ordered list of player records based on score</returns>
    [HttpGet("PlayersRanked")]
    public async Task<IEnumerable<PlayerRecordDTO>> GetPlayersRanked([FromQuery] IEnumerable<String> players)
    {
        IEnumerable<PlayerRecordDTO> playersRanked = await _gameService.GetPlayersRanked(players);
        return playersRanked;
    }

    /// <summary>
    /// This will return the number of wins a each player has when playing a game against the 
    /// other provided plaeyer
    /// </summary>
    /// <param name="players"></param>
    /// <returns>List of key value pairs</returns>
    [HttpGet("GameWins")]
    public async Task<List<KeyValuePair<string, int>>> GetPlayerGameWins([FromQuery] List<String> players) 
    {
        return await _gameService.GetPlayerGameWins(players);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id) 
    {
        Game? game = await _gameService.GetGame(id);

        if (game == null) 
        {
            return NotFound();
        }
        return game;
    }

    // POST: api/RecordedGames/
    [HttpPost]
    public async Task <ActionResult<Game>> PostGame(PostGameDTO gameDTO) 
    {
        await _gameService.PostGame(gameDTO);
        return Ok();
    }

    
    [HttpDelete]
    public async Task<ActionResult> DeleteGame(String gameId)
    {
        Boolean result = await _gameService.DeleteGame(gameId);
        
        if (result) return new StatusCodeResult(202);
        else return NotFound();
    }

}
