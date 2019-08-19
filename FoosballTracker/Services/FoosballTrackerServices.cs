using FoosballTracker.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using System;
//This class contains the CRUD operations required for this application
namespace FoosballTracker.Services
{
    public class FoosballTrackerService
    {
        private readonly IMongoCollection<Game> _games;

        public FoosballTrackerService(FoosballTrackerDatabaseSetting settings)
        {
            var client = new MongoClient(settings.ConnectionString);

            var database = client.GetDatabase(settings.DatabaseName);

            _games = database.GetCollection<Game>(settings.GamesCollectionName);


        }

        //The method below lists all games sorted by date in ascending order 
        public List<Game> Get() =>
            _games.Find(game => true).SortByDescending(game => game.Date).ToList();

        //The method to get a game given its name and date
           public Game Get(string name, DateTime date) =>
            _games.Find<Game>(game => game.Name == name && game.Date==date).FirstOrDefault();

        //A method to create a new game
        public Game Create(Game game)
        {
            var inGame = Get(game.Name, game.Date);
            if (inGame == null)
            {
               _games.InsertOne(game);
                return game;
            }
           
            return null;
        }

        //A method to update a game
        public void Update(ObjectId id, Game gameIn)
        {
            var filter = new BsonDocument("_id",id);
            _games.ReplaceOne(filter,gameIn);
        }


    }
    }
