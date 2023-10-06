using MongoDB.Bson;
using src.Models;

namespace src.Dao;
public interface IDatabaseDao
{
    Task<IEnumerable<DBGame>> GetGames(String database, String collection); 
}
