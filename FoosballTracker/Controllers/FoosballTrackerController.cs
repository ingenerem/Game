using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoosballTracker.Models;
using FoosballTracker.Services;


namespace FoosballTracker.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    /*This class contains functions that communicate with the servicefor CRUD operations*/
    namespace FoosballGameTracker.Controllers
    {
        [Route("api/foosballtracker")]
        [ApiController]
        public class gameListController : ControllerBase
        {
            private readonly FoosballTrackerService _foosballTrackerService;
             

           public gameListController(FoosballTrackerService foosballTrackerService)
           {
               _foosballTrackerService = foosballTrackerService;

           }

            [Route("listgames")]
            [HttpGet]
            public ActionResult<List<Game>> Get()
            {

                return _foosballTrackerService.Get();
            }

            /*The method bellow cheks for information needed to get a particular game and returns it if it 
            found in the database otherwise returns an error message*/
            [Route("getone")]
            [HttpGet]
            public ActionResult<Results> Get(string name, DateTime date)
            {
                Results res=new Results();
                var game = _foosballTrackerService.Get(name,date);
                
                if (game == null)
                {
                    res.messageResp = "Game not found";
                    return res;
                }

                res.messageResp = "ok";
                res.gameReturn = game;
                return res;
                
                 
            }

            /*The method below checks for required information to create a game record and creates it if ir is not a 
             duplicate*/
            [Route("create")]
            [HttpPost]
            public ActionResult<Results> Create(Game game)
            {
                Results res = new Results();
                string stringDate = game.Date + "";
                if (!stringDate.Equals("1/1/0001 12:00:00 AM") && !string.IsNullOrEmpty(game.Name))
                {
                    //Assigning defaults vaalues if no values are given for optional fields 
                    if (string.IsNullOrEmpty(game.Status)||!game.Status.Equals("Scheduled")||!game.Status.Equals("Ongoing")){

                        game.Status = "Scheduled";

                    }
                    if (game.SetNumber == 0)
                    {
                        game.SetNumber = 1;
                    }
                    if (game.SetNumber > 3)
                    {
                        game.SetNumber = 3;
                    }
                    //Creating the game record 
                    Game _game = _foosballTrackerService.Create(game);
                    //Returning error message in case there is a duplicate
                    if (_game == null)
                    {
                        res.messageResp = "Duplicate game found";
                        res.gameReturn = _game;
                    }
                    else
                    {
                        res.messageResp = "ok";
                        res.gameReturn = _game;
                    }
                    
                    
                    return res;
                }

                //Returning error message in case no game created
                res.messageResp = "Game not created, missing information";
                return res;
                    
            }

            /*The method below checks for required information to update a game and updates it. It also up checks for 
            required information to automaticaly change the status of the game*/
            [HttpPut("update")]
            public ActionResult<Results> Update(Game game)
            {
                string stringDate = game.Date + "";
                Results res = new Results();

                if(!stringDate.Equals("1/1/0001 12:00:00 AM" )&& !string.IsNullOrEmpty(game.Name))
                {
                    //Checking if the game exists in the database
                    var gameIn = _foosballTrackerService.Get(game.Name, game.Date);
                    if (gameIn == null)
                    {
                        res.messageResp = "The game was not found";
                        return res;
                    }

                    //Updating the score, set number and status if the game is not marked as ended 
                    else if (!gameIn.Status.Equals("Ended"))
                    {
                        gameIn.GoalsTeam1 = game.GoalsTeam1;
                        gameIn.GoalsTeam2 = game.GoalsTeam2;
                        if (gameIn.SetNumber < 3 && (gameIn.GoalsTeam1 >= 10 || game.GoalsTeam2 >= 10) )
                        {
                            gameIn.SetNumber = gameIn.SetNumber+1;
                            gameIn.Status = "Ongoing";
                        }

                        if (gameIn.GoalsTeam1 >= 20 || gameIn.GoalsTeam2 >= 20)
                        {
                            gameIn.Status = "Ended";

                        }
                        _foosballTrackerService.Update(gameIn.Id, gameIn);
                        res.messageResp = "ok";
                        res.gameReturn = gameIn; 
                        return res;

                    }

                    else
                    {
                        res.messageResp = "Cannot update an ended game";
                        res.gameReturn = gameIn;
                        return res;
                    }
               
                }

                res.messageResp = "The game was not updated, missing information";
                return res;


            }
        }
    }
  
}
