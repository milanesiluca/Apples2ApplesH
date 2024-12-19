using ApplesToApples.GameClasses.Interfaces;
using ApplesToApples.Players;
using ApplesToApples.Players.Interfaces;
using StreamingDataObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses
{
    public class SingleJudgeGameManager : IGameManager<IPlayer>
    {
        public List<IPlayer>? Players { get; set; } = new();
        public int PointsToWin { get; set; }
        public TcpListener? Socket { get; set; }
        public List<string>? GreenApples { get; set; }
        public List<string>? RedApples { get; set; }
        private bool gameWon = false;
        IPlayer? judge;
        int judgePlayer = -1;
        bool firstRound = true;

        private readonly IInputOutput _console;

        public SingleJudgeGameManager(IInputOutput console)
        {
            _console = console;
        }

        public void ManageGameSession()
        {
            _console.Clear();    
            while (!gameWon)
            {
                _console.ShowMessage("Nuovo Round!");
                _console.ShowMessage("");

                //chose and show the judge
                if (firstRound == true)
                {
                    judgePlayer = GetJudgeIndex();
                }

                judge = Players![judgePlayer];
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.White;
                _console.ShowOneLineMessage($" Player {Players![judgePlayer].PlayerID} is the judge ");
                Console.ResetColor();
                _console.ShowMessage("");
                _console.ShowMessage(""); ;

                //Chose and show the green apple
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                _console.ShowOneLineMessage($" Green Apple: {GreenApples![0]} ");
                Console.ResetColor();
                _console.ShowMessage(""); ;
                _console.WriteLine("");
                GreenApples!.Remove(GreenApples[0]);



                if (firstRound)
                {
                    foreach (IPlayer player in Players!) 
                    { 
                        if (player is IRealPlayer rp)
                        {
                            HandDataObjects handDta = new HandDataObjects
                            {
                                PlayerId = player.PlayerID,
                                PlayerCards = player.Hand
                            };

                            rp.SetFirstRound(handDta);
                           
                        }
                    }
                    firstRound = false;

                    foreach (IPlayer player in Players!) {
                        if (player.PlayerID != Players[judgePlayer].PlayerID && player is IRealPlayer rp) {
                            rp.ResetRound();
                        }
                    }
                }



                var receivedCard = new CountdownEvent(Players!.Count -1);
                var allThreadsCompleted = new ManualResetEvent(false);

                foreach (IPlayer player in Players)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        if (player.PlayerID != Players[judgePlayer].PlayerID)
                        {
                            var card = player.Play();
                            Apples2Apples.PlayedApple!.Add(card!);                
                            _console.ShowMessage($"Player {card!.PlayerID}: {card.Card}");                           
                            if (receivedCard.Signal())
                            {
                                allThreadsCompleted.Set();
                            }
                        }
                    });
                    
                }

                while (!allThreadsCompleted.WaitOne(1300)) { }


                judge = Players![judgePlayer];
                if (judge is IRealPlayer realJudge)
                {
                    realJudge.GetCardList();
                }

                

                PlayedApple winningApple = judge!.Judge();
                int winningPlayerId = winningApple.PlayerID;
                int winningPlayerIndex = Players
                                            .Select((player, idx) => new { player, idx })
                                            .Where(x => x.player.PlayerID == winningPlayerId)
                                            .Select(x => x.idx)
                                            .FirstOrDefault();
                IPlayer winningPlayer = Players[winningPlayerIndex];
                winningPlayer.IncrementScore();
                Apples2Apples.PlayedApple.Clear();


                _console.ShowMessage("");
                _console.ShowMessage("The winnint player: " + winningApple.PlayerID);
                _console.ShowMessage("The Winning Card: " + winningApple.Card);
                _console.ShowMessage("");

                CheckGameStatus(winningPlayer);

                

            }

            Socket!.Stop();
            Socket = null;

        }//start game end


        /*
         The method to get the Judge Index
         */
        private int GetJudgeIndex()
        {
            if (judgePlayer == -1)
            {
                Random rnd = new Random();
                return rnd.Next(Players!.Count);

            }
            else
            {
                int returValue; 
                if (judgePlayer == Players!.Count - 1)
                    returValue = 0;
                else
                    returValue = judgePlayer + 1;
                
                return returValue;
            }


        }

        /*
         chek the status of the game: close the game sending the endMessage or call the method to send a new card to the players for the next round
         */
        private void CheckGameStatus(IPlayer winningPlayer)
        {
            if (winningPlayer.GetScore() >= PointsToWin)
            {
                _console.ShowMessage("");
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                _console.ShowMessage(" The player " + winningPlayer.PlayerID + " has won the match ");
                Console.ResetColor();
                _console.ShowMessage("");
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                _console.ShowMessage(" Game Over! Thanks for playing with us ");
                Console.ResetColor();
                _console.ShowMessage("");
                _console.ShowMessage("");
                SendEndSignal(winningPlayer.PlayerID);
                gameWon = true;
            }
            else
            {
                _console.ShowMessage("");
                _console.ShowMessage("press enter to start the next round");
                _ = _console.GetInput();
                _console.Clear();
                AddOneAppleToPlayers();

                judgePlayer = GetJudgeIndex();
                foreach (IPlayer player in Players!)
                {
                    if (player.PlayerID != Players[judgePlayer]!.PlayerID && player is IRealPlayer rp)
                    {
                        rp.ResetRound();
                    }
                }
            }
        }


        //send the card to players in order to replace the played one
        private void AddOneAppleToPlayers() {
            foreach (IPlayer player in Players!) {
                if (player.PlayerID != judge!.PlayerID)
                {
                    
                    string newRedApple = RedApples![0];
                    player.AddCard(newRedApple);
                    RedApples!.Remove(RedApples[0]);
                }
            }


        }

        private void SendEndSignal(int id)
        {
            foreach (IPlayer player in Players!)
            {
                if (player is IRealPlayer realPlayer)
                {
                    realPlayer.GetEndMessage(id);
                }
                
                
            }

        }

    }//class end
}//namespace end
