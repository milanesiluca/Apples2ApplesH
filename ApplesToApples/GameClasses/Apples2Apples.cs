using ApplesToApples.GameClasses.Interfaces;
using ApplesToApples.Players;
using ApplesToApples.Players.Interfaces;
using StreamingDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses
{
    public class Apples2Apples 
    {
        public static List<PlayedApple> PlayedApple { set; get; } = new();
        private List<IPlayer> players = new List<IPlayer>();
        private List<string>? redApples;
        private List<string>? greenApples;
        private TcpListener? socket; 
        private int pointsToWin;
        private int numPlayers;
        private IGameManager<IPlayer> gameManager;
        
        public Apples2Apples(int numPls, List<string> redApples, List<string> greenApples, TcpListener socket, IGameManager<IPlayer> gameManaging)
        {
            this.redApples = redApples;
            this.greenApples = greenApples;
            this.socket = socket;
            gameManager = gameManaging;

            


            numPlayers = numPls < 4 ? 4 : numPls;

            
            switch (numPlayers)
            {
                case 4:
                    pointsToWin = 8;
                    break;
                case 5:
                    pointsToWin = 7;
                    break;
                case 6:
                    pointsToWin = 6;
                    break;
                case 7:
                    pointsToWin = 5;
                    break;
                default:
                    pointsToWin = 4;
                    break;
            }

            
            //pointsToWin = 3;

            gameManager.Players = players;
            gameManager.RedApples = redApples;
            gameManager.GreenApples = greenApples;
            gameManager.Socket = socket;
            gameManager.PointsToWin = pointsToWin;

            

            SetPlayers(numPls);
           

        }
        private void SetPlayers(int realPlayers)
        {
            int totPlayers = realPlayers < 4 ? 4 : realPlayers;
            int botPlayers = totPlayers - realPlayers;

            int idPl = 0;


            socket!.Start();
            CountdownEvent connectedPlayer = new CountdownEvent(totPlayers);

            for (int i = 0; i < realPlayers; i++) {
                
                TcpClient connectionSocket = socket.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(_ => { 
                    StreamReader inFromClient = new StreamReader(connectionSocket.GetStream());
                    StreamWriter outToClient = new StreamWriter(connectionSocket.GetStream()) { AutoFlush = true };
                    List<string> playerCard = shareCards();
                    idPl = playerCard.Count;
                    HandDataObjects hand = new HandDataObjects {
                        PlayerId = idPl, 
                        PlayerCards = playerCard 
                    };
                    players.Add(new RealPlayer(idPl, playerCard, inFromClient, outToClient));
                    connectedPlayer.Signal();
                }); 
                
            
            }

            idPl++;

            for (int i = 0; i < botPlayers; i++)
            {
                List<string> playerCard = shareCards();
                players.Add(new BotPlayer(idPl, playerCard));
                connectedPlayer.Signal();
                idPl++;

            }

            connectedPlayer.Wait();


        }

        private List<string> shareCards() {
            List<string> playerCard = new List<string>();
            for (int y = 0; y < 7; y++)
            {
                playerCard.Add(redApples![0]);
                redApples.RemoveAt(0);
            }
            return playerCard;

        }

        internal void RunGame()
        {
            
            gameManager.ManageGameSession();
        }

       
    }
}
