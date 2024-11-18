using ApplesToApples.GameClasses;
using ApplesToApples.Players.Interfaces;
using System.Reflection;
using xUnitTestApples2Apple.CommonObjects;


namespace xUnitTestApples2Apple
{
    public class RandomizeJudge
    {
        //Randomise which player starts being the judge.
        [Fact]
        public void FirstJudge_IsChosenRandomlyFromPlayers()
        {
            // Arrange
            var players = new List<IPlayer> {
                    new MockPlayer(0),
                    new MockPlayer(1),
                    new MockPlayer(2),
                    new MockPlayer(3)
            };

            MockConsole mkConsoe = new MockConsole();
            SingleJudgeGameManager gameManager = new SingleJudgeGameManager(mkConsoe)
            {
                Players = players
            };

            // Act
            var firstJudgeIndex = GetPrivateJudgeIndex(gameManager);

            // Assert
            Assert.InRange(firstJudgeIndex, 0, players.Count - 1);
        }

        private int GetPrivateJudgeIndex(SingleJudgeGameManager gameManager) //using reflection to get the private method.
        {
            var method = typeof(SingleJudgeGameManager).GetMethod("GetJudgeIndex", BindingFlags.NonPublic | BindingFlags.Instance);
            return (int)method!.Invoke(gameManager, null)!;
        }
    }


   
}
