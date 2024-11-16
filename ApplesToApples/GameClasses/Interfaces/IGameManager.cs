using ApplesToApples.Players.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses.Interfaces
{
    public interface IGameManager<T>
    {
        List<T>? Players { get; set; }
        int PointsToWin { get; set; }
        TcpListener? Socket { get; set; }
        List<string>? GreenApples { get; set; }
        List<string>? RedApples { get; set; }
        void ManageGameSession();
    }
}
