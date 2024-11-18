using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses.Interfaces
{
    public interface IConsole
    {
        void WriteLine(string message);
        void Write(string message);
        void Clear();
        string? ReadLine();
        void SetBackgroundColor(ConsoleColor color);
        void SetForegroundColor(ConsoleColor color);
        void ResetColor();
    }

}
