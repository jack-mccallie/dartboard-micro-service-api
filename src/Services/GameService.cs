
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

        PlayerRecordDTO playerRecord = calculatePlayerRecordDTO(dartboardGames, playerName);

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

    public async Task<double> GetPlayerScore(String playerName)
    {
        IEnumerable<DBGame> dartboardGames = await _dataBaseDao.GetGames("Dartboard", "GameResults");
        PlayerRecordDTO playerRecordDTO = calculatePlayerRecordDTO(dartboardGames, playerName);
        int totalEarnedPoints = (playerRecordDTO.TwoPlayerWins * 2) + (playerRecordDTO.ThreePlayerWins * 3);
        int totalPossiblePoints = playerRecordDTO.TwoPlayerTotalGames + playerRecordDTO.ThreePlayerTotalGames;

        return Math.Pow(totalEarnedPoints, 2) / totalPossiblePoints;

    }

    // Business logic helper functions
    // These functions should never call any other function in this class

    public PlayerRecordDTO calculatePlayerRecordDTO(IEnumerable<DBGame> dartboardGames, String playerName)
    {
        IEnumerable<DBGame> gamesWithPlayer = dartboardGames.Where(game => game.players.Contains(playerName));

        // Calculating the two player game stats
        int twoPlayerWinswins = gamesWithPlayer.Where(game => game.winningListInOrder[0] == playerName && game.players.Count() == 2).Count();
        int twoPlayertotalGames = gamesWithPlayer.Where(game => game.players.Count() == 2).Count();
        int twoPlayerlosses = twoPlayertotalGames - twoPlayerWinswins;

        // Calculating the the three player game stats
        int threePlayerWins = gamesWithPlayer.Where(game => game.winningListInOrder[0] == playerName && game.players.Count() == 3).Count();
        int threePlayerTotalGames = gamesWithPlayer.Where(game => game.players.Count() == 3).Count();

        Dictionary<String, HeadToHeadRecord> HeadToHeadDictionary = new Dictionary<string, HeadToHeadRecord>();

        // Creating a set of the opponents a player has faced
        HashSet<String> allPlayers = gamesWithPlayer.Where(game => game.players.Count() == 2)

                                                    .SelectMany(game => game.players).ToHashSet();
        allPlayers.Remove(playerName);

        
        // Creating an object that has the head to head record with every 
        // opponent a player has faced
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
            TwoPlayerWins = twoPlayerWinswins,
            TwoPlayerTotalGames = twoPlayertotalGames,
            ThreePlayerWins = threePlayerWins,
            ThreePlayerTotalGames = threePlayerTotalGames,
            HeadToHeadRecords = headToHeadRecords
        };
        return playerRecord;
    }
}