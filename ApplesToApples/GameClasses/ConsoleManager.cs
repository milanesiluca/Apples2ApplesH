using ApplesToApples.GameClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesToApples.GameClasses
{
    public class ConsoleManager : IConsole
    {
        public void WriteLine(string message) => Console.WriteLine(message);
        public void Write(string message) => Console.Write(message);
        public void Clear() => Console.Clear();
        public string? ReadLine() => Console.ReadLine();
        public void SetBackgroundColor(ConsoleColor color) => Console.BackgroundColor = color;
        public void SetForegroundColor(ConsoleColor color) => Console.ForegroundColor = color;
        public void ResetColor() => Console.ResetColor();
    }
}
