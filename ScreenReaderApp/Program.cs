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
        enum DisplayMode
        {
            REFRESH_ONLY,
            ROOM,
            ALL
        }

        /// <summary>
        /// What to show
        /// </summary>
        static DisplayMode displayMode;

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
            displayMode = DisplayMode.ROOM;
            bool skipDraw = false;
            while (!quit)
            {
                // If not skipping draw, draw map. If so - reset skipDraw to draw map next time.
                if (!skipDraw)
                {
                    if (displayMode != DisplayMode.REFRESH_ONLY)
                    {
                        DrawMap(G.CurrentDungeon);
                    }
                }
                else
                {
                    skipDraw = false;
                }
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

                    // Non move options
                    // Refresh screen
                    case ConsoleKey.Spacebar:
                        DrawMap(G.CurrentDungeon, true);
                        skipDraw = true;
                        break;

                    // Change draw mode - F1, F2, F3
                    case ConsoleKey.F1:
                        displayMode = DisplayMode.ALL;
                        Console.Write("Display mode set to show all");
                        break;
                    case ConsoleKey.F2:
                        displayMode = DisplayMode.ROOM;
                        Console.Write("Display mode set to show current visible area only");
                        break;
                    case ConsoleKey.F3:
                        displayMode = DisplayMode.REFRESH_ONLY;
                        Console.Write("Display mode set to only show on refresh (Space)");
                        break;
                }
                G.DoTurn();
            }
        }

        /// <summary>
        /// Draw the map to the console
        /// </summary>
        /// <param name="Dungeon">Dungeon to draw</param>
        /// <param name="ForceDrawAll">Whether to force drawing whole map</param>
        static void DrawMap(AtlasWarriorsGame.Dungeon Dungeon, bool ForceDrawAll = false)
        {
            // Get the furthest on each edge that's visible
            // Initialising with max/min possible for min/max respectively
            int leftToShow = Dungeon.Width;
            int rightToShow = 0;
            int topToShow = Dungeon.Height;
            int bottomToShow = 0;

            for (int iy = 0; iy < Dungeon.Height; ++iy)
            {
                for (int ix = 0; ix < Dungeon.Width; ++ix)
                {
                    // If in room mode and visible, or if seen
                    if (ForceDrawAll ||
                        ((displayMode == DisplayMode.ROOM) &&
                        (Dungeon.GetVisibility(new XY(ix, iy)) == Dungeon.CellVisibility.VISIBLE)) ||
                        ((displayMode == DisplayMode.ALL) &&
                        (Dungeon.GetVisibility(new XY(ix, iy)) != Dungeon.CellVisibility.UNSEEN)))
                    {
                        leftToShow = Math.Min(leftToShow, ix);
                        rightToShow = Math.Max(rightToShow, ix);
                        topToShow = Math.Min(topToShow, iy);
                        bottomToShow = Math.Max(bottomToShow, iy);
                    }
                }
            }

            for (int iy = topToShow; iy <= bottomToShow; ++iy)
            {
                Console.WriteLine();
                for (int ix = leftToShow; ix <= rightToShow; ++ix)
                {
                    // Get character, unless unseen in which get space
                    var tileChar = Dungeon.GetVisibility(new XY(ix, iy))
                        != Dungeon.CellVisibility.UNSEEN ?
                        CellToScreen.CellScreenChar(Dungeon.GetCell(new XY(ix, iy))) : ' ';
                    var tileActors = (Dungeon.Actors.Where(i => i.Location == (new XY(ix, iy))));
                    if (tileActors.Count() > 0) 
                    {
                        tileChar = CellToScreen.ActorToChar(tileActors.First());
                    }
                    Console.Write(tileChar);
                }
            }
        }
    }
}
