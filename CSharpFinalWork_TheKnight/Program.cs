using Console2048;
using CSharpFinalWork_TheKnight.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalWork_TheKnight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game();
        }

        static void Game()
        {
            UIHandler consoleUI = new ConsoleUI();
            Dialogue dialogue = new Dialogue();
            var gm = new GameManager(consoleUI, dialogue);
            gm.Game();
        }

    }
}
