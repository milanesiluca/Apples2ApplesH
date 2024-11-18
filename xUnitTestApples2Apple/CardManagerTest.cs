using ApplesToApples;
using ApplesToApples.Players.Interfaces;
using System.IO;

namespace xUnitTestApples2Apple
{
    public class CardManagerTest
    {
        

        [Fact]
        public void System_Read_Card_From_File()
        {
        /*
            1. Read all the green apples (adjectives) from a file and add to the green apples deck.
            2. Read all the red apples (nouns) from a file and add to the red apples deck.
            3. Shuffle both the green apples and red apples decks.
         */
            //Arrange
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName!;
            string path = Path.Combine(projectDirectory, "ApplesToApples", "Cards", "redApples.txt");

            //Act
            List<string> cards = (List<string>)GameCardsManager.SetupGameCards(path);

            //Assert
            Assert.NotEmpty(cards);
        }
    }
}