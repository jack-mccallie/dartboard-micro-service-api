using MongoDB.Bson;
using src.DTO;
using src.Models;

namespace src.Dao;
public interface IDatabaseDao
{
    Task<IEnumerable<DBGame>> GetGames(String database, String collection, int season); 

    Task PostGame(PostGameDTO postGameDTO);

    Task<Boolean> DeleteGame(String gameId);
}
