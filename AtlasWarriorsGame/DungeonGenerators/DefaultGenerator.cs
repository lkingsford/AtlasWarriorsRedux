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
        /// <returns>Generated dungeon</returns>
        public static Dungeon Generate(int width, int height)
        {
            var dungeon = new Dungeon(width, height);

            // Fill the floor
            for (int ix = 0; ix < dungeon.Width; ++ix)
            {
                for (int iy = 0; iy < dungeon.Height; ++iy)
                {
                    dungeon.SetCell(new XY(ix, iy), DungeonCell.Floor);

                    // Create new spawn area for each cell
                    dungeon.SpawnAreas.Add(new SpawnArea(new XY(ix, iy)));
                }
            } 

            // Wall the top and bottom edges
            for (int ix = 0; ix < dungeon.Width; ++ix)
            {
                dungeon.SetCell(new XY(ix, 0), DungeonCell.Wall);
                dungeon.SetCell(new XY(ix, dungeon.Height - 1), DungeonCell.Wall);
            }

            // Wall the left and right edges
            for (int iy = 0; iy < dungeon.Height; ++iy)
            {
                dungeon.SetCell(new XY(0, iy), DungeonCell.Wall);
                dungeon.SetCell(new XY(dungeon.Width - 1, iy), DungeonCell.Wall);
            }

            // Set start to middle of room
            dungeon.StartLocation = new XY(dungeon.Width / 2, dungeon.Height / 2);

            return dungeon;
        }
    }
}
