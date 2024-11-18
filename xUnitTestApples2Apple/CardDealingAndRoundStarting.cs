using ApplesToApples;
using ApplesToApples.GameClasses;
using ApplesToApples.GameClasses.Interfaces;
using ApplesToApples.Players;
using ApplesToApples.Players.Interfaces;
namespace xUnitTestApples2Apple
{
    public class CardDealingAndRoundStarting
    {
        //4 - Deal seven red apples to each player, including the judge

        [Fact]
        public void SetPlayers_ShouldCreateOnlyBotPlayersWithSevenCardsEach()
        {
            // Arrange: setup the game cards from the file
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName!;
            string path = Path.Combine(projectDirectory, "ApplesToApples", "Cards", "redApples.txt");
            List<string> cards = (List<string>)GameCardsManager.SetupGameCards(path);

            // Mock dependencies
            var greenApples = new List<string>(); // Empty green apples for simplicity
            var fakeSocket = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
            var fakeGameManager = new MockGameManager(); // Replace with your actual mock or implementation

            // Create the instance of the game
            var game = new Apples2Apples(0, cards, greenApples, fakeSocket, fakeGameManager); // 0 real players

            // Act: Call SetPlayers indirectly through the constructor
            var players = fakeGameManager.Players; // Players list populated by SetPlayers

            // Assert: Verify that all players are bot players and have 7 cards each
            Assert.All(players, player =>
            {
                Assert.IsType<BotPlayer>(player); // Ensure all are BotPlayers
                Assert.Equal(7, player.Hand.Count); // Ensure each has 7 cards
            });
        }

    }

    public class MockGameManager : IGameManager<IPlayer>
    {
        public List<IPlayer> Players { get; set; } = new List<IPlayer>();
        public List<string> RedApples { get; set; } = new List<string>();
        public List<string> GreenApples { get; set; } = new List<string>();
        public System.Net.Sockets.TcpListener Socket { get; set; } = null!;
        public int PointsToWin { get; set; }

        public void ManageGameSession() { }
    }
}
