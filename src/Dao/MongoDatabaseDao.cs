using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Any;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using src.Models;

namespace src.Dao;
public class MongoDatabaseDao : IDatabaseDao
{
    private MongoClient _client;

    public MongoDatabaseDao(String connectionString) {
        _client = new MongoClient(connectionString);
    }

    // returns all games in the database
    public async Task<IEnumerable<DBGame>> GetGames(String database, String collection) {

        var documents = _client.GetDatabase(database).GetCollection<DBGame>(collection) ?? throw new Exception("ERROR: Collection is null");

        var filter = Builders<BsonDocument>.Filter.Empty;
        return await documents.Find(x => true).ToListAsync();
    }


}
