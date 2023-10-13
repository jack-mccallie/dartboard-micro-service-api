
using src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Dao;

namespace src.Services;


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

    public async Task<PlayerRecordDTO> GetPlayerRecord(String playerName)
    {
        IEnumerable<DBGame> dartboardGames = await _dataBaseDao.GetGames("Dartboard", "GameResults");

        IEnumerable<DBGame> gamesWithPlayer = dartboardGames.Where(game => game.players.Contains(playerName));

        int wins = gamesWithPlayer.Where(game => game.winningListInOrder[0] == playerName).Count();
        int totalGames = gamesWithPlayer.Count();
        int losses = totalGames - wins;

        Dictionary<String, HeadToHeadRecord> HeadToHeadDictionary = new Dictionary<string, HeadToHeadRecord>();

        HashSet<String> allPlayers = gamesWithPlayer.Where(game => game.players.Count() == 2)

                                                    .SelectMany(game => game.players).ToHashSet();
        allPlayers.Remove(playerName);

        IEnumerable<HeadToHeadRecord> headToHeadRecords = allPlayers.Select(player => {
            IEnumerable<DBGame> gamesAgainstOpponent = gamesWithPlayer.Where(game => game.players.Count() == 2 && game.players.First() == player);
            HeadToHeadRecord output = new HeadToHeadRecord(
                player, 
                gamesAgainstOpponent.Where(game => game.winningListInOrder.FirstOrDefault() == playerName).Count(), 
                gamesAgainstOpponent.Count()
                );
            return output;
        });


        PlayerRecordDTO playerRecord = new PlayerRecordDTO() {
            PlayerName = playerName,
            Wins = wins,
            Losses = losses,
            TotalGames = totalGames,
            HeadToHeadRecords = headToHeadRecords
        };

        return playerRecord;
    }

    public async Task<List<KeyValuePair<String, int>>> GetPlayerGameWins(List<String> players)
    {
        IEnumerable<DBGame> dartboardGames = await _dataBaseDao.GetGames("Dartboard", "GameResults");


        dartboardGames = dartboardGames.Where(game => {
                foreach(String player in players) {
                    if (game.players.Count() != players.Count() || !game.players.Contains(player)) return false;
                }
                return true;
            });
        List<KeyValuePair<String, int>> output = new List<KeyValuePair<String, int>>(); 

        if (!dartboardGames.Any()) return null;

        foreach (String player in players) {
            output.Add(new KeyValuePair<String, int>(
                player, 
                dartboardGames.Where(game => game.winningListInOrder[0] == player).Count())
                );
        }

        return output;
    }
}