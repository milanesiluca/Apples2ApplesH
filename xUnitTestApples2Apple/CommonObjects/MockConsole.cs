using ApplesToApples.GameClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnitTestApples2Apple.CommonObjects
{
    public class MockConsole : IConsole
    {
        public List<string> WrittenMessages { get; } = new();
        public string? Input { get; set; }

        public void WriteLine(string message) => WrittenMessages.Add(message);
        public void Write(string message) => WrittenMessages.Add(message);
        public void Clear() { /* Ignorato */ }
        public string? ReadLine() => Input;
        public void SetBackgroundColor(ConsoleColor color) { /* Ignored */ }
        public void SetForegroundColor(ConsoleColor color) { /* Ignored */ }
        public void ResetColor() { /* Ignorato */ }
    }

}
