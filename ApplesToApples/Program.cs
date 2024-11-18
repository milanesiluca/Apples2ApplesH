using ApplesToApples.GameClasses;
using ApplesToApples.GameClasses.Interfaces;
using ApplesToApples.Players.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ApplesToApples
{
    public class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
            .AddScoped<IGameManager<IPlayer>, SingleJudgeGameManager>()
            .AddScoped<IConsole, ConsoleManager>()
            .BuildServiceProvider();

            IPAddress adr = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(adr, 2048);




            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;

            string pathRed = Path.Combine(projectDirectory, "ApplesToApples", "Cards", "redApples.txt");
            List<string> redApples = (List<string>)GameCardsManager.SetupGameCards(pathRed);

            string pathGreen = Path.Combine(projectDirectory, "ApplesToApples", "Cards", "greenApples.txt");
            List<string> greenApples =  (List<string>)GameCardsManager.SetupGameCards(pathGreen);



            if (args.Length == 0)
            {

                Console.WriteLine("Waiting för players...");
                var gameManager = serviceProvider.GetService<IGameManager<IPlayer>>();

                Apples2Apples game = new(1, redApples, greenApples, listener, gameManager!);
                game.RunGame();

            }
            else { 
                string argument = args[0];
            
                if (int.TryParse(argument, out int result))
                {
                    if (result > 0)
                    {
                        Console.WriteLine("Waiting för players...");
                        var gameManager = serviceProvider.GetService<IGameManager<IPlayer>>();
                        Apples2Apples game = new(result, redApples, greenApples, listener, gameManager!);
                        
                        game.RunGame();
                    }
                    else {
                        Console.WriteLine("Invalid imput, goodbye");
                        return;
                    }
                }                  
                else {
                    Console.WriteLine("Invalid input, The game cannot begin. Try Again");
                    return;
                }
            }  
            
        }


        private static IEnumerable<string> ShuffleCard(List<string> cards)
        {
            
            var rndR = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int index = rndR.Next(i + 1);
                string a = cards[index];
                cards[index] = cards[i];
                cards[i] = a;
            }
            return cards;
        }
    }

    public static class GameCardsManager {


        public static IEnumerable<string> SetupGameCards(string path)
        {
            List<string> cards = new List<string>(File.ReadAllLines(path!));
            return GetShuffledCard(cards);
        }

        private static IEnumerable<string> GetShuffledCard(List<string> cards)
        {

            var rndR = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int index = rndR.Next(i + 1);
                string a = cards[index];
                cards[index] = cards[i];
                cards[i] = a;
            }
            return cards;
        }


    }
}

