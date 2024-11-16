
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
        List<string> Hand { get; init; }
        PlayedApple? Play();
        PlayedApple Judge();
        void AddCard(string redApple);
        int GetScore();
        void IncrementScore();
    }

}
