using MongoDB.Bson;
using MongoDB.Driver;
using src.DTO;
using src.Models;

namespace src.Dao;
public class MongoDatabaseDao : IDatabaseDao
{
    private MongoClient _client;
    private String _collection = "GameResults";
    private String _database = "Dartboard";

    public MongoDatabaseDao(String connectionString) {
        _client = new MongoClient(connectionString);
    }

    // returns all games in the database
    public async Task<IEnumerable<DBGame>> GetGames(String database, String collection, int season) {

        var documents = _client.GetDatabase(database).GetCollection<DBGame>(collection) ?? throw new Exception("ERROR: Collection is null");

        if(season > 0) {
            var filter = Builders<DBGame>.Filter.Where(x => x.Season == season);
            var results =  await documents.Find(filter).ToListAsync();
            return results;
        }
        else {
            var results =  await documents.Find(x => true).ToListAsync();
            return results;
        }
    }

    public async Task PostGame(PostGameDTO postGameDTO)
    {
        var documents = _client.GetDatabase(_database).GetCollection<DBGame>(_collection) ?? throw new Exception("ERROR: Collection is null");
        
        DBGame newGame = new DBGame(postGameDTO);
        await documents.InsertOneAsync(newGame);
    }

    public async Task<Boolean> DeleteGame(String gameId)
    {

        var documents = _client.GetDatabase(_database).GetCollection<DBGame>(_collection) ?? throw new Exception("ERROR: Collection is null");
        ObjectId objectId = ObjectId.Parse(gameId);
        var filter = Builders<DBGame>.Filter.Eq(game => game.Id, objectId);

        // delete the person
        var gameDeleteResult = await documents.DeleteOneAsync(filter);
        
        return true ? (gameDeleteResult.DeletedCount == 1) : false;
    }


}
