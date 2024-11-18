using System.Net.Sockets;
using System.Text.Json;
using ApplePlayerClient.Interfaces;
using StreamingDataObjects;


namespace ApplesToApples.GameClasses
{
    internal class ApplesPlayerClient : IApplesPlayerClient

    {
        private int id;
        private List<string>? hand;
        NetworkStream networkStream;
        StreamWriter outToServer;
        StreamReader inFromServer;
        TcpClient clientSocket;
 
        

        public ApplesPlayerClient()
        {
            clientSocket = new TcpClient("127.0.0.1", 2048);
            networkStream = clientSocket.GetStream();
            outToServer = new StreamWriter(networkStream) { AutoFlush = true };
            inFromServer = new StreamReader(networkStream);

        }//end constructor


        public virtual void StartGameSession()
        {
            
            while (true)
            {
                GetApple2AppleComunucation();
            }

        }
         
        private void PlayAsPlayer() {
            int cardIndex = -1;
            bool isValid;
            do
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Nuovo Round! ");
                Console.ResetColor();
                Console.WriteLine();
                int zi = 1;
                foreach (string handItem in hand!)
                {
                    Console.WriteLine(zi + " - " + handItem);
                    zi++;
                }
                Console.WriteLine();
                Console.Write("Insert the number of the card you want play: ");
                string? input = Console.ReadLine();


                isValid = int.TryParse(input, out cardIndex) && cardIndex >= 1 && cardIndex <= 7;

                if (!isValid)
                {
                    Console.WriteLine("Input not valid: chose among your cards");
                }



            } while (!isValid);

            PlayedApple myCard = new PlayedApple(id, hand[cardIndex - 1]);
            string jsonData = JsonSerializer.Serialize(myCard);
            outToServer.WriteLine(jsonData);
            hand.RemoveAt(cardIndex - 1);
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Card Played... you get the response on the game main board ");
            Console.ResetColor();
            
        }

        private void GetApple2AppleComunucation()
        {
             
            try
            {
                int category = -1;
                while (true) { 

                    string? serverMessage = inFromServer.ReadLine();
                    if (serverMessage != null)
                    {
                        
                        category = JsonDocument.Parse(serverMessage).RootElement.GetProperty("Category").GetInt32();
                        Console.WriteLine($"category: {category} ");
                       
                        
                        switch (category)
                        {
                            case 1:
                                ClientServerMessages<string> stringFromServer =  JsonSerializer.Deserialize<ClientServerMessages<string>>(serverMessage)!;
                                hand!.Add(stringFromServer.Message);
                                break;
                            case 2:
                                ClientServerMessages<List<PlayedApple>> appleListFromServer = JsonSerializer.Deserialize<ClientServerMessages<List<PlayedApple>>>(serverMessage)!;
                                ChoseWinner(appleListFromServer.Message);
                                break;
                            case 3:
                                ClientServerMessages<string> roundWinnerFromServer = JsonSerializer.Deserialize<ClientServerMessages<string>>(serverMessage)!;
                                CheckWinning(roundWinnerFromServer.Message);
                                break;
                            case 5:
                                ClientServerMessages<HandDataObjects> myCardFromServer = JsonSerializer.Deserialize<ClientServerMessages<HandDataObjects>>(serverMessage)!;
                                GetTheMatchCards(myCardFromServer.Message);
                                break;
                            default:
                                PlayAsPlayer();
                                break;
                        }
                        category = -1;
                        

                    }
                    else
                    {
                        Console.WriteLine("Connection close by server");

                    }

                }

            }
            catch (Exception e)
            {
                
                Console.WriteLine(e.Message);
            }

        }

        private void GetTheMatchCards(HandDataObjects handObject) {
            hand = handObject!.PlayerCards!;
            id = handObject.PlayerId;
        }

        private void ChoseWinner(List<PlayedApple> playedCards) {
            Console.Clear();
            
            Console.WriteLine("You are the judge");
            Console.WriteLine();
            if (playedCards != null) {
                int i = 1;
                foreach (var card in playedCards) { 
                    Console.WriteLine($"{i} - Player {card!.PlayerID}: {card.Card}");
                    i++;
                }
                Console.WriteLine();
                Console.Write("Chose the winning one: ");
                string? winner = Console.ReadLine();
                bool isValid; 
                do {
                    isValid = int.TryParse(winner, out int cardIndex) && cardIndex >= 1 && cardIndex <= 3;
                    outToServer.WriteLine($"{ cardIndex - 1 }");
                    
                } while (!isValid);
            }
            
        }


        private void CheckWinning(string winningPlayer)
        {

            int winningPlayerId = -1;

            bool validValue = int.TryParse(winningPlayer, out winningPlayerId);
            if (validValue)
            {
                Console.Clear();
                if (id == winningPlayerId)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" Congratualtion, you won! Thanks for playing with us ");
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" Game Over! Thanks for playing with us ");
                }

            }

            Console.ResetColor();
            Environment.Exit(0);
        }

        
    }//End Class

}
