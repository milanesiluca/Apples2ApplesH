using ApplePlayerClient;
using ApplePlayerClient.Interfaces;
using ApplesToApples.GameClasses;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = new ServiceCollection()
            .AddScoped<IApplesPlayerClient, ApplesPlayerClient>()
            .BuildServiceProvider();

var clientManager1 = serviceProvider.GetRequiredService<IApplesPlayerClient>();


Console.WriteLine("Connected to the Game Session");

IApplesPlayerClient client = clientManager1;
client.StartGameSession();

