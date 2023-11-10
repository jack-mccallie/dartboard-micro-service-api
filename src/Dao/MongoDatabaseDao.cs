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
    public async Task<IEnumerable<DBGame>> GetGames(String database, String collection) {

        var documents = _client.GetDatabase(database).GetCollection<DBGame>(collection) ?? throw new Exception("ERROR: Collection is null");

        var filter = Builders<BsonDocument>.Filter.Empty;
        return await documents.Find(x => true).ToListAsync();
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
        
        var filter = Builders<DBGame>.Filter.Eq(game => game.Id.ToString(), gameId);

        // delete the person
        var gameDeleteResult = await documents.DeleteOneAsync(filter);
        
        return true ? (gameDeleteResult.DeletedCount == 1) : false;
    }


}
