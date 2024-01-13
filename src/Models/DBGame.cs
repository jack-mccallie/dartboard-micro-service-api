using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using src.DTO;
namespace src.Models;


// This class is a representation of the Database Representation of a Game
public class DBGame 
{
    [Key]
    public ObjectId Id { get; set; }
    public IEnumerable<String> WinningListInOrder { get; set; }
    
    [BsonElement("dateEntered")]
    public DateTime DateEntered {get; set;}

    //301, 501, cricket
    public String Type { get; set; }

    public DBGame(PostGameDTO postGameDTO) {
        DateEntered = DateTime.UtcNow;
        WinningListInOrder = postGameDTO.WinningListInOrder;
        Type = postGameDTO.Type;
    }
}
