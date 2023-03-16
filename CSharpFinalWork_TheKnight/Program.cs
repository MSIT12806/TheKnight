using Console2048;
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
            var gm = Singalton<GameManager>.SingletonObj;
            gm.Game();
        }

    }
}
