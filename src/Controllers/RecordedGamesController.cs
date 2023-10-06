using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.DTO;
using Microsoft.AspNetCore.Mvc;
using games_recording_service.Services;
using games_recording_service.Models;
using MongoDB.Bson;
using src.Models;

namespace games_recording_service.Controllers;

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
    public async Task<IEnumerable<DBGame>> GetGamesMongoDB(String database, String collection){
        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return await _gameService.GetGamesMongoDB(database, collection);
    }

    // GET: api/RecordedGames/{id}
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
        PostGameDTO updatedPostGameDto = await _gameService.PostGame(gameDTO);
        return CreatedAtAction(nameof(GetGame), new { id = updatedPostGameDto.Date }, updatedPostGameDto);
    }
}
