using ApplesToApples.GameClasses;
using ApplesToApples.GameClasses.Interfaces;
using ApplesToApples.Players.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;

namespace ApplesToApples
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
            .AddScoped<IGameManager<IPlayer>, SingleJudgeGameManager>()
            .BuildServiceProvider();

            IPAddress adr = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(adr, 2048);



            string pathRed = @"C:\Users\Luca\OneDrive\Skrivbord\Esame Finale\Apples2Apples_C#\ApplesToApples\Cards\redApples.txt";
            List<string> redApples = new List<string>(File.ReadAllLines(pathRed));

            string pathGreen = @"C:\Users\Luca\OneDrive\Skrivbord\Esame Finale\Apples2Apples_C#\ApplesToApples\Cards\greenApples.txt";
            List<string> greenApples = new List<string>(File.ReadAllLines(pathGreen));

            //shuffle cards


            var rndR = new Random();
            for (int i = redApples.Count - 1; i > 0; i--)
            {
                int index = rndR.Next(i + 1);
                string a = redApples[index];
                redApples[index] = redApples[i];
                redApples[i] = a;
            }


            var rndG = new Random();
            for (int i = greenApples.Count - 1; i > 0; i--)
            {
                int index = rndG.Next(i + 1);
                string a = greenApples[index];
                greenApples[index] = greenApples[i];
                greenApples[i] = a;
            }

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
    }
}

