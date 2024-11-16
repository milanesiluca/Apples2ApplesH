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
        void GetEndMessage(int id);
        void GetCardList(); //judge funcion

        void ResetRound();
        void SetFirstRound(HandDataObjects handData);
    }
}
