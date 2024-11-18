using StreamingDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.Players.Interfaces
{
    internal interface IRealPlayer
    {
        void GetEndMessage(int id); // The real player get the result of the match when a play has won and close the session on the client side
        void GetCardList(); //The real player gets the list of the card played by other player when it's his/her turn to play as judge 

        void ResetRound(); //the funcion that switchs the play mode from judge to player
        void SetFirstRound(HandDataObjects handData); //Allow the real player to get the forst 7 cards and own PlayerID.
    }
}
