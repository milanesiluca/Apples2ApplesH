
using ApplesToApples.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.Players.Interfaces
{
    public interface IPlayer
    {
        int PlayerID { get; init; }
        List<string> Hand { get; init; } //list of 7 cards
        PlayedApple? Play(); 
        PlayedApple Judge();
        void AddCard(string redApple); //called from second round: get a card at the beginning of a round in order to replace the plyed card and add that the the Hand 
        int GetScore(); //return the number of won rounds to cehch the match winner;
        void IncrementScore(); //To Increment the score every time the player wins a round
    }

}
