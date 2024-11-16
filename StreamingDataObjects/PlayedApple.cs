using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses
{
    public class PlayedApple
    {
        public int PlayerID { get; private set; } 
        public string Card { get; private set; }  

        
        public PlayedApple(int playerID, string card)
        {
            PlayerID = playerID;
            Card = card;
        }


    }

}
