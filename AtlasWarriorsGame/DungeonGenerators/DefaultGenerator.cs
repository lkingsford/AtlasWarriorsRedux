using System;
using System.Collections.Generic;
using System.Text;
using static AtlasWarriorsGame.Dungeon;

namespace AtlasWarriorsGame.DungeonGenerators
{
    /// <summary>
    /// Generator of box of walls with floor inside
    /// </summary>
    static class DefaultGenerator
    {
        /// <summary>
        /// Generate a dungeon with just a box of walls and a floor
        /// </summary>
        /// <param name="Dungeon"></param>
        /// <returns>True on success</returns>
        public static bool Generate(Dungeon Dungeon)
        {
            // Fill the floor
            for (int ix = 0; ix < Dungeon.Width; ++ix)
            {
                for (int iy = 0; iy < Dungeon.Height; ++iy)
                {
                    Dungeon.SetCell(new XY(ix, iy), DungeonCell.FLOOR);
                }
            } 

            // Wall the top and bottom edges
            for (int ix = 0; ix < Dungeon.Width; ++ix)
            {
                Dungeon.SetCell(new XY(ix, 0), DungeonCell.WALL);
                Dungeon.SetCell(new XY(ix, Dungeon.Height - 1), DungeonCell.WALL);
            }

            // Wall the left and right edges
            for (int iy = 0; iy < Dungeon.Height; ++iy)
            {
                Dungeon.SetCell(new XY(0, iy), DungeonCell.WALL);
                Dungeon.SetCell(new XY(Dungeon.Width - 1, iy), DungeonCell.WALL);
            }

            // Set start to middle of room
            Dungeon.StartLocation = new XY(Dungeon.Width / 2, Dungeon.Height / 2);

            return true;
        }
    }
}
