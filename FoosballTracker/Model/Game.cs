using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace FoosballTracker.Models
    //This is the model for the database containing details abour foosball games.
{
    public class Game
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        //The name of the game is made of the name of the first team spave the keyword "vs" and the name of the 
        //seconf team. for example "Eagles vs Bees"
        public string Name { get; set; }
        
        //The date of the game is the date when the game is scheduled 
        public DateTime Date { get; set; }

        //The status of a game is either "Scheduled", "Ongoing" or "Ended"
        public string Status { get; set; }

        //The GoalsTeam1 is the score of the first team
        public int GoalsTeam1 { get; set; }

        //The GoalsTeam1 is the score of the second team
        public int GoalsTeam2 { get; set; }

        public int SetNumber { get; set; }

    }
}