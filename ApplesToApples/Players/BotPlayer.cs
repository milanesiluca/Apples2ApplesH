using ApplesToApples.GameClasses;
using ApplesToApples.Players.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.Players
{
    public class BotPlayer : IPlayer
    {
        public int PlayerID {  get;  init; } 
        public List<string> Hand { get; init; }
        private List<string> greenApples = new List<string>();
        private int score;
        


        public BotPlayer(int playerID, List<string> hand)
        {
            this.PlayerID = playerID;
            Hand = hand;
        }

        public PlayedApple Play()
        {
            Random rnd = new Random();

            int index = rnd.Next(Hand.Count);
            string chosenCard = Hand[index];
            PlayedApple card = new PlayedApple(PlayerID, chosenCard);
            Hand.RemoveAt(0);

            return card;
        }

        public PlayedApple Judge()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, Apples2Apples.PlayedApple!.Count - 1);
            return Apples2Apples.PlayedApple![index]; 
        }

        public void AddCard(string redApple)
        {
            Hand.Add(redApple);
        }

        public int GetScore()
        {
            return score;
        }

        public void IncrementScore()
        {
            score++;
        }

    }
}
