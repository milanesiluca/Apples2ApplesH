using ApplesToApples.GameClasses;
using ApplesToApples.Players.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTestApples2Apple.CommonObjects
{
    public class MockPlayer : IPlayer
    {
        public int PlayerID { get; init; }
        public List<string> Hand { get; init; } = new List<string>();
        public PlayedApple? Play() => null;
        public PlayedApple Judge() => null!;
        public void AddCard(string redApple) { }
        public int GetScore() => 0;
        public void IncrementScore() { }

        public MockPlayer(int id, List<string> redApples)
        {
            PlayerID = id;
            Hand = redApples;
        }

        public MockPlayer(int id) { PlayerID = id; }
    }
}
