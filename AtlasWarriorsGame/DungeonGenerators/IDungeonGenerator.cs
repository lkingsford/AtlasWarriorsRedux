using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.DungeonGenerators
{
    public interface IDungeonGenerator
    {
        /// <summary>
        /// Generate a dungeon with given parameters
        /// </summary>
        /// <param name="width">Width of dungeon</param>
        /// <param name="height">Height of dungeon</param>
        /// <param name="passages">Passages to place</param>
        /// <returns></returns>
        Dungeon Generate(int width, int height, List<Passage> passages = null);
    }
}
