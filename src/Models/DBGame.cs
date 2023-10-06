using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace src.Models;


// This class is a representation of the Database Representation of a Game
public class DBGame 
{
    public ObjectId Id { get; set; }
    public String Winner {get; set;} = null!;
    public String HomeTeam {get; set;} = null!;
    public String AwayTeam {get; set;} = null!;
    public int? HomeTeamScore {get; set;}
    public int? AwayTeamScore {get; set;}
    
    [BsonElement("DateInputted")]
    public DateTime? DateOfEntry {get; set;}

    [BsonElement("Date")]
    public DateTime? GameDate {get; set;}
}
