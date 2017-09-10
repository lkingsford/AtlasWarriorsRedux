using System;
using System.Collections.Generic;
using System.Text;
using static AtlasWarriorsGame.Dungeon;

namespace AtlasWarriorsGame.DungeonGenerators
{
    /// <summary>
    /// Generator of box of walls with floor inside
    /// </summary>
    public class DefaultGenerator : IDungeonGenerator
    {
        /// <summary>
        /// Generate a dungeon with just a box of walls and a floor
        /// </summary>
        /// <param name="Dungeon"></param>
        /// <returns>Generated dungeon</returns>
        public Dungeon Generate(int width, int height, List<Passage> passages = null)
        {
            var dungeon = new Dungeon(width, height);

            // Fill the floor
            for (int ix = 1; ix < (dungeon.Width - 1); ++ix)
            {
                for (int iy = 1; iy < (dungeon.Height - 1); ++iy)
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

            // Add passages randomly
            foreach (var passage in passages ?? new List<Passage>())
            {
                var area = dungeon.SpawnAreas.RandomItem();
                var startPosition = area.Area.RandomItem();

                // Remove spawn area from list - don't want surrounded soon as down, and don't want
                // multiple passages in same area
                dungeon.SpawnAreas.Remove(area);

                // Add passage to dungeon
                dungeon.Passages.Add(new Passage(passage.PassageType, passage.DestinationID,
                    startPosition));

                // Set tile to appropriate for passage type
                switch (passage.PassageType)
                {
                    case Passage.PassageTypeEnum.StairsUp:
                        dungeon.SetCell(startPosition, DungeonCell.StairUp);
                        break;
                    case Passage.PassageTypeEnum.StairsDown:
                        dungeon.SetCell(startPosition, DungeonCell.StairDown);
                        break;
                }
            }

            return dungeon;
        }
    }
}
