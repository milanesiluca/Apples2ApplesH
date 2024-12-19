using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses.Interfaces
{
    public interface IInputOutput
    {
        void ShowMessage(string message);
        void ShowOneLineMessage(string message);
        void Clear();
        string? GetInput();
        void SetBackgroundColor(ConsoleColor color);
        void SetForegroundColor(ConsoleColor color);
        void ResetColor();
    }

}
