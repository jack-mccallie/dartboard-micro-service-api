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
    
    // GET: api/RecordedGames
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetGames() {
        
        var games = await _gameService.GetGames();

        // Check if there are any games
        if (games.Value == null || !games.Value.Any()) {
            return NotFound();
        }

        return games;
    }
    // GET: api/RecordedGames/{database}/{collection}
    [HttpGet("{database}/{collection}")]
    public async Task<IEnumerable<DBGame>> GetGamesMongoDB(String database, String collection)
    {
        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return await _gameService.GetGamesMongoDB(database, collection);
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
        double playerScore = await _gameService.GetPlayerScore(playerName);
        playerRecord.PlayerScore = playerScore;
        return playerRecord;
    }

    /// <summary>
    /// This will return the total score a player has earned
    /// It is equal to totalPointsEarned^2 / totalPossiblepoints
    /// 2 points for 2 player win
    /// 3 points for 3 player win
    /// </summary>
    /// <param name="playerName"></param>
    /// <returns></returns>
    [HttpGet("PlayerScore")]
    public async Task<double> GetPlayerScore(String playerName)
    {
        double playerScore = await _gameService.GetPlayerScore(playerName);
        return playerScore;
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
    // [HttpPost]
    // public async Task <ActionResult<Game>> PostGame(PostGameDTO gameDTO) 
    // {
    //     PostGameDTO updatedPostGameDto = await _gameService.PostGame(gameDTO);
    //     return CreatedAtAction(nameof(GetGame), new { id = updatedPostGameDto.Date }, updatedPostGameDto);
    // }


}
