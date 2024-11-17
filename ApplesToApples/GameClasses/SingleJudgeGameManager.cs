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
    internal class SingleJudgeGameManager : IGameManager<IPlayer>
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


        public void ManageGameSession()
        {
            Console.Clear();    
            while (!gameWon)
            {
                Console.WriteLine("Nuovo Round!");
                Console.WriteLine();

                //chose and show the judge
                if (firstRound == true)
                {
                    judgePlayer = GetJudgeIndex();
                }

                judge = Players![judgePlayer];
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" Player {Players![judgePlayer].PlayerID} is the judge ");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();

                //Chose and show the green apple
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" Green Apple: {GreenApples![0]} ");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
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
                            Console.WriteLine($"Player {card!.PlayerID}: {card.Card}");
                            if (receivedCard.Signal())
                            {
                                allThreadsCompleted.Set();
                            }
                        }
                    });
                    
                }

                //receivedCard.Wait();
                while (!allThreadsCompleted.WaitOne(300))
                {
                   
                }


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


                Console.WriteLine();
                Console.WriteLine("The winnint player: " + winningApple.PlayerID);
                Console.WriteLine("The Winning Card: " + winningApple.Card);
                Console.WriteLine();

                CheckGameStatus(winningPlayer);

                

            }

            Socket!.Stop();
            Socket = null;

        }//start game end



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


        private void CheckGameStatus(IPlayer winningPlayer)
        {
            if (winningPlayer.GetScore() >= PointsToWin)
            {
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" The player " + winningPlayer.PlayerID + " has won the match ");
                Console.ResetColor();
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Game Over! Thanks for playing with us ");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
                SendEndSignal(winningPlayer.PlayerID);
                gameWon = true;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("press enter to start the next round");
                _ = Console.ReadLine();
                Console.Clear();
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
