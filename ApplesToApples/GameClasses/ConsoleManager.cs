using ApplesToApples.GameClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses
{
    public class ConsoleManager : IInputOutput
    {
        public void ShowMessage(string message) => Console.WriteLine(message);
        public void ShowOneLineMessage(string message) => Console.Write(message);
        public void Clear() => Console.Clear();
        public string? GetInput() => Console.ReadLine();
        public void SetBackgroundColor(ConsoleColor color) => Console.BackgroundColor = color;
        public void SetForegroundColor(ConsoleColor color) => Console.ForegroundColor = color;
        public void ResetColor() => Console.ResetColor();
    }
}
