
using games_recording_service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using src.DTO;
using src.Dao;
using MongoDB.Bson;
using src.Models;
using System.Reflection.Metadata;

namespace games_recording_service.Services;


public class GameService : IGameService
{
    private readonly GameContext _gameDBContext;
    private IDatabaseDao _dataBaseDao;
    public GameService(GameContext gameContext, IDatabaseDao databaseDao){
        _dataBaseDao = databaseDao;
        _gameDBContext = gameContext;
    }

    public async Task<IEnumerable<DBGame>> GetGamesMongoDB(String database, String collection) {
        
        return await _dataBaseDao.GetGames(database, collection);
    }

    public async Task<ActionResult<IEnumerable<Game>>> GetGames() {
        
        return await _gameDBContext.Games.ToListAsync();
    }

    public async Task<Game?> GetGame(int id)
    {
        Game? game = await _gameDBContext.Games.FindAsync(id);
        return game;
    }

    public async Task<PostGameDTO> PostGame(PostGameDTO gameDTO) 
    {
        gameDTO.Date = DateTime.UtcNow;
        Game inputGame = new Game {
            HomeTeam = gameDTO.HomeTeam.ToUpper(),
            AwayTeam = gameDTO.AwayTeam.ToUpper(),
            Winner = gameDTO.HomeScore > gameDTO.AwayScore ? gameDTO.HomeTeam.ToUpper() : gameDTO.AwayTeam.ToUpper(),
            HomeTeamScore = gameDTO.HomeScore,
            AwayTeamScore = gameDTO.AwayScore,
            DateOfEntry = DateTime.UtcNow
        };

        PostGameDTO updatedPostGameDto = new PostGameDTO(inputGame);

        _gameDBContext.Games.Add(inputGame);
        await _gameDBContext.SaveChangesAsync();

        return updatedPostGameDto;
    }
}