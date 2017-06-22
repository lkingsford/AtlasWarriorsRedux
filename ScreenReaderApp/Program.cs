using AtlasWarriorsGame;
using DavyKager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenReaderApp
{
    /// <summary>
    /// Console version of Atlas Warriors, optimised for Screen Readers
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Tolk.TrySAPI(true);
            Tolk.Load();

            //Console.WriteLine("Querying for the active screen reader driver...");
            //string name = Tolk.DetectScreenReader();
            //if (name != null) {
            //  Console.WriteLine("The active screen reader driver is: {0}", name);
            //}
            //else {
            //  Console.WriteLine("None of the supported screen readers is running");
            //}

            //if (Tolk.HasSpeech()) {
            //  Console.WriteLine("This screen reader driver supports speech");
            //}
            //if (Tolk.HasBraille()) {
            //  Console.WriteLine("This screen reader driver supports braille");
            //}

            //Console.WriteLine("Let's output some text...");
            //if (!Tolk.Output("Hello, World!")) {
            //  Console.WriteLine("Failed to output text");
            //}

            //Console.WriteLine("Finalizing Tolk...");
            //Tolk.Unload();

            //Console.WriteLine("Done!");
            var G = new AtlasWarriorsGame.Game();
            bool quit = false;
            while (!quit)
            {
                DrawMap(G.CurrentDungeon);
                Console.Write("\n>");
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.H:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        G.Player.NextMove = Player.Instruction.MOVE_W;
                        break;
                    case ConsoleKey.K:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        G.Player.NextMove = Player.Instruction.MOVE_N;
                        break;
                    case ConsoleKey.L:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        G.Player.NextMove = Player.Instruction.MOVE_E;
                        break;
                    case ConsoleKey.J:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        G.Player.NextMove = Player.Instruction.MOVE_S;
                        break;
                    case ConsoleKey.Y:
                    case ConsoleKey.NumPad7:
                        G.Player.NextMove = Player.Instruction.MOVE_NW;
                        break;
                    case ConsoleKey.U:
                    case ConsoleKey.NumPad9:
                        G.Player.NextMove = Player.Instruction.MOVE_NE;
                        break;
                    case ConsoleKey.B:
                    case ConsoleKey.NumPad1:
                        G.Player.NextMove = Player.Instruction.MOVE_SW;
                        break;
                    case ConsoleKey.N:
                    case ConsoleKey.NumPad3:
                        G.Player.NextMove = Player.Instruction.MOVE_SE;
                        break;
                }
                G.DoTurn();
            }
        }

        static void DrawMap(AtlasWarriorsGame.Dungeon Dungeon)
        {
            for (int iy = 0; iy < Dungeon.Height; ++iy)
            {
                Console.WriteLine();
                for (int ix = 0; ix < Dungeon.Width; ++ix)
                {
                    var tileChar = UiCommon.CellToScreen.CellScreenChar(
                        Dungeon.GetCell(new XY(ix, iy)));
                    var tileActors = (Dungeon.Actors.Where(i => i.Location == (new XY(ix, iy))));
                    if (tileActors.Count() > 0) 
                    {
                        tileChar = UiCommon.CellToScreen.ActorToChar(tileActors.First());
                    }
                    Console.Write(tileChar);
                }
            }
        }
    }
}
