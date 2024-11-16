using ApplePlayerClient;
using ApplePlayerClient.Interfaces;
using ApplesToApples.GameClasses;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = new ServiceCollection()
            .AddScoped<IApplesPalyerClient, ApplesPalyerClient>()
            .BuildServiceProvider();

var clientManager1 = serviceProvider.GetRequiredService<IApplesPalyerClient>();


Console.WriteLine("Connected to the Game Session");

IApplesPalyerClient client = clientManager1;
client.StartGameSession();

