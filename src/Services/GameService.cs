
using src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Dao;
using MongoDB.Driver;

namespace src.Services;


public class GameService : IGameService
{
    private readonly GameContext _gameDBContext;
    private IDatabaseDao _dataBaseDao;
    public GameService(GameContext gameContext, IDatabaseDao databaseDao){
        _dataBaseDao = databaseDao;
        _gameDBContext = gameContext;
    }

    public async Task<IEnumerable<Game>> GetGames(String database, String collection) {
        
        return (await _dataBaseDao.GetGames(database, collection)).Select(game => {
            return new Game(game);
        });
    }

    public async Task<Game?> GetGame(int id)
    {
        Game? game = await _gameDBContext.Games.FindAsync(id);
        return game;
    }

    public async Task PostGame(PostGameDTO gameDTO) 
    {
        await _dataBaseDao.PostGame(gameDTO);
    }

    public async Task<PlayerRecordDTO> GetPlayerRecord(String playerName)
    {
        IEnumerable<DBGame> dartboardGames = await _dataBaseDao.GetGames("Dartboard", "GameResults");

        PlayerRecordDTO playerRecord = calculatePlayerRecordDTO(dartboardGames, playerName);

        return playerRecord;
    }

    public async Task<IEnumerable<PlayerRecordDTO>> GetPlayersRanked(IEnumerable<String> players)
    {
        IEnumerable<DBGame> dartboardGames = await _dataBaseDao.GetGames("Dartboard", "GameResults");

        if(players.Count() > 0) {
            IEnumerable<DBGame> dartboardGamesWithCorrectPlayers = dartboardGames.Where(game => {
                bool samePlayers = players.All(x => game.WinningListInOrder.Contains(x) && players.Count(a => a == x) == players.Count(b => b == x));
                return samePlayers;
            });

            IEnumerable<PlayerRecordDTO> playerRecordDTOs = players.Select(player => calculatePlayerRecordDTO(dartboardGamesWithCorrectPlayers, player));

            IEnumerable<PlayerRecordDTO> rankings = playerRecordDTOs.OrderBy(record => record.PlayerScore).Reverse();

            return rankings;
        }
       
        HashSet<String> allPlayers = dartboardGames.Select(game => game.WinningListInOrder).SelectMany(x => x).ToHashSet();

        IEnumerable<PlayerRecordDTO> allPlayerRecordDTOs = allPlayers.Select(player => calculatePlayerRecordDTO(dartboardGames, player));

        IEnumerable<PlayerRecordDTO> rankingsAllPlayers = allPlayerRecordDTOs.OrderBy(record => record.PlayerScore).Reverse();

        return rankingsAllPlayers;
        
    }

    public async Task<List<KeyValuePair<String, int>>> GetPlayerGameWins(List<String> players)
    {
        IEnumerable<DBGame> dartboardGames = await _dataBaseDao.GetGames("Dartboard", "GameResults");

        dartboardGames = dartboardGames.Where(game => {
                foreach(String player in players) {
                    if (game.WinningListInOrder.Count() != players.Count() || !game.WinningListInOrder.Contains(player)) return false;
                }
                return true;
            });
        List<KeyValuePair<String, int>> output = new List<KeyValuePair<String, int>>(); 

        if (!dartboardGames.Any()) return null;

        foreach (String player in players) {
            output.Add(new KeyValuePair<String, int>(
                player, 
                dartboardGames.Where(game => game.WinningListInOrder.FirstOrDefault() == player).Count())
                );
        }

        return output;
    }

    public async Task<Boolean> DeleteGame(String gameId)
    {
       Boolean result = await _dataBaseDao.DeleteGame(gameId);

       return result;
    }

    // Business logic helper functions
    // These functions should never call any other function in this class

    public PlayerRecordDTO calculatePlayerRecordDTO(IEnumerable<DBGame> dartboardGames, String playerName)
    {
        IEnumerable<DBGame> gamesWithPlayer = dartboardGames.Where(game => game.WinningListInOrder.Contains(playerName));

        // Calculating the two player game stats
        int twoPlayerWins = gamesWithPlayer.Where(game => (game.WinningListInOrder.FirstOrDefault() == playerName) && (game.WinningListInOrder.Count() == 2)).Count();
        int twoPlayerTotalGames = gamesWithPlayer.Where(game => game.WinningListInOrder.Count() == 2).Count();
        int twoPlayerlosses = twoPlayerTotalGames - twoPlayerWins;

        // Calculating the the three player game stats
        int threePlayerWins = gamesWithPlayer.Where(game => game.WinningListInOrder.FirstOrDefault() == playerName && game.WinningListInOrder.Count() == 3).Count();
        int threePlayerTotalGames = gamesWithPlayer.Where(game => game.WinningListInOrder.Count() == 3).Count();

        Dictionary<String, HeadToHeadRecord> HeadToHeadDictionary = new Dictionary<string, HeadToHeadRecord>();

        // Creating a set of the opponents a player has faced
        HashSet<String> allPlayers = gamesWithPlayer.Where(game => game.WinningListInOrder.Count() == 2)

                                                    .SelectMany(game => game.WinningListInOrder).ToHashSet();
        allPlayers.Remove(playerName);

        int totalEarnedPoints = (twoPlayerWins * 2) + (threePlayerWins * 3);
        int totalPossiblePoints = (twoPlayerTotalGames*2) +(threePlayerTotalGames*3);

        
        // Creating an object that has the head to head record with every 
        // opponent a player has faced
        IEnumerable<HeadToHeadRecord> headToHeadRecords = allPlayers.Select(player => {
            IEnumerable<DBGame> gamesAgainstOpponent = gamesWithPlayer.Where(game => game.WinningListInOrder.Count() == 2 && game.WinningListInOrder.Contains(player));
            HeadToHeadRecord output = new HeadToHeadRecord(
                player, 
                gamesAgainstOpponent.Where(game => game.WinningListInOrder.First() == playerName).Count(), 
                gamesAgainstOpponent.Count()
                );
            return output;
        });



        PlayerRecordDTO playerRecord = new PlayerRecordDTO() {
            PlayerName = playerName,
            TwoPlayerWins = twoPlayerWins,
            TwoPlayerTotalGames = twoPlayerTotalGames,
            ThreePlayerWins = threePlayerWins,
            ThreePlayerTotalGames = threePlayerTotalGames,
            PossiblePoints = totalPossiblePoints,
            PlayerScore = totalEarnedPoints,
            HeadToHeadRecords = headToHeadRecords
        
        };
        return playerRecord;
    }
}