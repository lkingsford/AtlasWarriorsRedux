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
        /// Copy constructor
        /// </summary>
        /// <param name="Feature">Feature to copy</param>
        public Feature(Feature Feature)
        {
            // Create new map to avoid clone
            this.FeatureMap = new DungeonCell[Feature.FeatureMap.GetLength(0),
                Feature.FeatureMap.GetLength(1)];
            // ... and make it identical
            for (int ix = 0; ix < Feature.FeatureMap.GetLength(0); ++ix)
            {
                for (int iy = 0; iy < Feature.FeatureMap.GetLength(1); ++iy)
                {
                    this.FeatureMap[ix, iy] = Feature.FeatureMap[ix, iy];
                }
            }

            // Copy door list
            _PossibleDoors = new List<XY>(Feature._PossibleDoors);
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

        /// <summary>
        /// Underlying value for PossibleDoors
        /// </summary>
        private List<XY> _PossibleDoors = new List<XY>();

        /// <summary>
        /// Places where a door can be placed to connect to another feature
        /// </summary>
        public IReadOnlyCollection<XY> PossibleDoors
        {
            get
            {
                return _PossibleDoors.AsReadOnly();
            }
        }

        /// <summary>
        /// Add coordinate where door could be added
        /// </summary>
        /// <param name="coord"></param>
        public void AddPossibleDoor(XY Coord)
        {
            _PossibleDoors.Add(Coord);
        }
    }
}
