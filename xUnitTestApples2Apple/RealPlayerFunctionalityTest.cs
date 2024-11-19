using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTestApples2Apple
{
    using System.IO;
    using System.Text;
    using System.Text.Json;
    using ApplesToApples.GameClasses;
    using Moq;
    using StreamingDataObjects;
    using Xunit;

    public class RealPlayerTests
    {
        private readonly RealPlayer _realPlayer;
        private readonly Mock<StreamReader> _mockStreamReader;
        private readonly Mock<StreamWriter> _mockStreamWriter;
        private readonly List<string> _initialHand;

        public RealPlayerTests()
        {
            // Mock dei flussi
            _mockStreamReader = new Mock<StreamReader>(Stream.Null);
            _mockStreamWriter = new Mock<StreamWriter>(Stream.Null);

            // Mano iniziale per il test
            _initialHand = new List<string> { "Card1", "Card2", "Card3" };

            // Creazione di RealPlayer
            _realPlayer = new RealPlayer(1, new List<string>(_initialHand), _mockStreamReader.Object, _mockStreamWriter.Object);
        }

        [Fact]
        public void RealPlayer_ShouldInitializeWithCorrectPlayerID()
        {
            Assert.Equal(1, _realPlayer.PlayerID);
        }

        [Fact]
        public void RealPlayer_ShouldInitializeWithCorrectHand()
        {
            Assert.Equal(_initialHand, _realPlayer.Hand);
        }

        [Fact]
        public void Play_ShouldReadPlayedCardFromStream()
        {
            // Arrange
            var playedApple = new PlayedApple(1, "Card1");
            var json = JsonSerializer.Serialize(playedApple);

            _mockStreamReader.Setup(sr => sr.ReadLine()).Returns(json);

            // Act
            var result = _realPlayer.Play();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(playedApple.PlayerID, result.PlayerID);
            Assert.Equal(playedApple.Card, result.Card);
        }

        [Fact]
        public void Judge_ShouldReturnCorrectPlayedAppleFromIndex()
        {
            // Arrange
            Apples2Apples.PlayedApple = new List<PlayedApple>
        {
            new PlayedApple(2, "CardA"),
            new PlayedApple(3, "CardB"),
            new PlayedApple(4, "CardC")
        };

            _mockStreamReader.Setup(sr => sr.ReadLine()).Returns("1");

            // Act
            var result = _realPlayer.Judge();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CardB", result.Card);
        }

        [Fact]
        public void AddCard_ShouldSendCorrectMessageToClient()
        {
            // Arrange
            string newCard = "Card4";
            var expectedMessage = new ClientServerMessages<string>(1, newCard);
            var expectedJson = JsonSerializer.Serialize(expectedMessage);

            // Act
            _realPlayer.AddCard(newCard);

            // Assert
            _mockStreamWriter.Verify(sw => sw.WriteLine(expectedJson), Times.Once);
            _mockStreamWriter.Verify(sw => sw.Flush(), Times.Once);
        }

        [Fact]
        public void SetFirstRound_ShouldSendHandDataToClient()
        {
            // Arrange
            var handData = new HandDataObjects { PlayerId = 1, PlayerCards = _initialHand };
            var expectedMessage = new ClientServerMessages<HandDataObjects>(5, handData);
            var expectedJson = JsonSerializer.Serialize(expectedMessage);

            // Act
            _realPlayer.SetFirstRound(handData);

            // Assert
            _mockStreamWriter.Verify(sw => sw.WriteLine(expectedJson), Times.Once);
            _mockStreamWriter.Verify(sw => sw.Flush(), Times.Once);
        }

        [Fact]
        public void ResetRound_ShouldSendResetMessageToClient()
        {
            // Arrange
            var expectedMessage = new ClientServerMessages<string>(9, "reset");
            var expectedJson = JsonSerializer.Serialize(expectedMessage);

            // Act
            _realPlayer.ResetRound();

            // Assert
            _mockStreamWriter.Verify(sw => sw.WriteLine(expectedJson), Times.Once);
            _mockStreamWriter.Verify(sw => sw.Flush(), Times.Once);
        }

        [Fact]
        public void IncrementScore_ShouldIncreasePlayerScore()
        {
            // Arrange
            int initialScore = _realPlayer.GetScore();

            // Act
            _realPlayer.IncrementScore();

            // Assert
            Assert.Equal(initialScore + 1, _realPlayer.GetScore());
        }
    }

}
