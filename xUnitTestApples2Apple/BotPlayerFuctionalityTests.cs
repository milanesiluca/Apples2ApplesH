using ApplesToApples.GameClasses;
using ApplesToApples.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTestApples2Apple
{
    public class BotPlayerTests
    {
        private readonly BotPlayer _botPlayer;
        private readonly List<string> _initialHand;

        public BotPlayerTests()
        {
            // Setup comune per tutti i test
            _initialHand = new List<string> { "Card1", "Card2", "Card3" };
            _botPlayer = new BotPlayer(1, new List<string>(_initialHand));
        }

        [Fact]
        public void BotPlayer_ShouldInitializeWithCorrectPlayerID()
        {
            Assert.Equal(1, _botPlayer.PlayerID);
        }

        [Fact]
        public void BotPlayer_ShouldInitializeWithCorrectHand()
        {
            Assert.Equal(_initialHand, _botPlayer.Hand);
        }

        [Fact]
        public void Play_ShouldReturnPlayedAppleAndRemoveCardFromHand()
        {
            var playedCard = _botPlayer.Play();

            Assert.NotNull(playedCard);
            Assert.Equal(1, playedCard.PlayerID);
            Assert.Contains(playedCard.Card, _initialHand);
            Assert.Equal(_initialHand.Count - 1, _botPlayer.Hand.Count);
        }

        [Fact]
        public void Judge_ShouldSelectValidPlayedApple()
        {
            // Mock played apples
            Apples2Apples.PlayedApple = new List<PlayedApple>
        {
            new PlayedApple(2, "CardA"),
            new PlayedApple(3, "CardB"),
            new PlayedApple(4, "CardC")
        };

            var judgedCard = _botPlayer.Judge();

            Assert.NotNull(judgedCard);
            Assert.Contains(judgedCard, Apples2Apples.PlayedApple);
        }

        [Fact]
        public void AddCard_ShouldAddCardToHand()
        {
            string newCard = "Card4";
            _botPlayer.AddCard(newCard);

            Assert.Contains(newCard, _botPlayer.Hand);
        }

        [Fact]
        public void IncrementScore_ShouldIncreaseScore()
        {
            int initialScore = _botPlayer.GetScore();
            _botPlayer.IncrementScore();

            Assert.Equal(initialScore + 1, _botPlayer.GetScore());
        }
    }

}
