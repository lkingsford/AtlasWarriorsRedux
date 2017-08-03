using AtlasWarriorsGame.DungeonGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtlasWarriorsGame.Dungeon;

namespace AtlasWarriorsGame.Tests
{
    /// <summary>
    /// Tools for assisting with writing tests
    /// </summary>
    static class TestUtils
    {
        /// <summary>
        /// Convert string of " ", "#", "+", "-" and "." to a dungeon
        /// for testing
        /// </summary>
        /// <param name="MapToSet">Array of " " and "#"</param>
        /// <returns>Filling for given map</returns>
        public static AtlasWarriorsGame.Dungeon GetDungeon(params string[] MapToSet)
        {
            var width = MapToSet[0].Length;
            var height = MapToSet.Length;
            var d = new Dungeon(width, height);

            for (var ix = 0; ix < width; ++ix)
            {
                for (var iy = 0; iy < height; ++iy)
                {
                    switch (MapToSet[iy][ix])
                    {
                        case ' ':
                            d.SetCell(new XY(ix, iy), DungeonCell.Empty);
                            break;
                        case '+':
                            d.SetCell(new XY(ix, iy), DungeonCell.Door);
                            break;
                        case '#':
                            d.SetCell(new XY(ix, iy), DungeonCell.Wall);
                            break;
                        case '.':
                            d.SetCell(new XY(ix, iy), DungeonCell.Floor);
                            break;
                    }
                }
            }

            return d;
        }


        /// <summary>
        /// Convert string of " ", "#", "+", "-" and "." to a feature, adding doors on '+'s
        /// for testing
        /// </summary>
        /// <param name="MapToSet">Array of " " and "#"</param>
        /// <returns>Filling for given map</returns>
        public static Feature GetFeature(params string[] MapToSet)
        {
            var width = MapToSet[0].Length;
            var height = MapToSet.Length;
            var f = new Feature(width, height);

            for (var ix = 0; ix < width; ++ix)
            {
                for (var iy = 0; iy < height; ++iy)
                {
                    switch (MapToSet[iy][ix])
                    {
                        case ' ':
                            f.SetCell(new XY(ix, iy), DungeonCell.Empty);
                            break;
                        case '+':
                            f.SetCell(new XY(ix, iy), DungeonCell.Door);
                            f.AddPossibleDoor(new XY(ix, iy));
                            break;
                        case '#':
                            f.SetCell(new XY(ix, iy), DungeonCell.Wall);
                            break;
                        case '.':
                            f.SetCell(new XY(ix, iy), DungeonCell.Floor);
                            break;
                    }
                }
            }

            return f;
        }
    }
}
