using System;
using System.Collections.Generic;
using System.Text;
using static AtlasWarriorsGame.Dungeon;

namespace AtlasWarriorsGame.DungeonGenerators
{
    /// <summary>
    /// Feature (such as a room) used by the RoomsGenerator
    /// A feature is a space, made up of DungeonCells, with possible door locations.
    /// Anything 'Nothing' may intersect in that part with another feature
    /// </summary>
    class Feature
    {
        /// <summary>
        /// Generate a feature with given parameters
        /// </summary>
        /// <param name="width">Width of Feature</param>
        /// <param name="height">Height of Feature</param>
        public Feature(uint width, uint height)
        {
            FeatureMap = new DungeonCell[width, height];
        }

        /// <summary>
        /// Elements of the map
        /// </summary>
        protected DungeonCell[,] FeatureMap;

        /// <summary>
        /// Set cell at coord to be value
        /// </summary>
        /// <param name="coord">Coordinates to set</param>
        /// <param name="value">Value to set</param>
        public void SetCell(XY coord, DungeonCell value)
        {
            FeatureMap[coord.X, coord.Y] = value;
        }

        /// <summary>
        /// Return cell at coord
        /// </summary>
        /// <param name="coord">Coordinates to get</param>
        /// <returns>Value of cell at coord</returns>
        public DungeonCell GetCell(XY coord)
        {
            return FeatureMap[coord.X, coord.Y];
        }
    }
}
