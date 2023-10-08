using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace src.Models;


// This class is a representation of the Database Representation of a Game
public class DBGame 
{
    public ObjectId Id { get; set; }
    public required List<string> winningListInOrder { get; set; }

    public required List<string> players { get; set; }
    
    [BsonElement("dateEntered")]
    public String? DateOfEntry {get; set;}

    [BsonElement("datePlayed")]
    public String? GameDate {get; set;}
}
