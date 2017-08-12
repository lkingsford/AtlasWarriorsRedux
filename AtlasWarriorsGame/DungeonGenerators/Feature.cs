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
        public Feature(int width, int height)
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
                    SetCell(new XY(ix, iy), Feature.FeatureMap[ix, iy]);
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
        /// Get the current parts of the feature where bad dudes can spawn.
        /// At the moment, all floor bits
        /// </summary>
        virtual public SpawnArea SpawnArea
        {
            get
            {
                var spawnArea = new SpawnArea();
                for (int ix = 0; ix < Width; ++ix)
                {
                    for (int iy = 0; iy < Height; ++iy)
                    {
                        if (GetCell(new XY(ix, iy)) == DungeonCell.Floor)
                        {
                            spawnArea.Area.Add(new XY(ix, iy));
                        }
                    }
                }
                return spawnArea;
            }
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

        /// <summary>
        /// Return width of feature map
        /// </summary>
        public int Width
        {
            get
            {
                return FeatureMap.GetLength(0);
            }
        }

        /// <summary>
        /// Return height of feature map
        /// </summary>
        public int Height
        {
            get
            {
                return FeatureMap.GetLength(1);
            }
        }

        /// <summary>
        /// Right angle rotations
        /// </summary>
        public enum Rotation
        {
            UP = 0,
            RIGHT = 1,
            DOWN = 2,
            LEFT = 3
        }

        /// <summary>
        /// Return point in this feature after moved
        /// </summary>
        /// <param name="Point">Point to rotate</param>
        /// <param name="Angle">Amount to rotate</param>
        /// <returns></returns>
        protected XY RotatePoint(XY Point, Rotation Angle)
        {
            switch (Angle)
            {
                case Rotation.RIGHT:
                    return new XY(Height - Point.Y - 1, Point.X);
                case Rotation.DOWN:
                    return new XY(Width - Point.X - 1, Height - Point.Y - 1);
                case Rotation.LEFT:
                    return new XY(Point.Y, Width - Point.X - 1);
                default:
                    // Doing up like this 'cause VS didn't think it returned a value
                    return Point;
            }
        }

        /// <summary>
        /// Create a copy of this feature rotated
        /// </summary>
        /// <param name="Angle">Angle to rotate to</param>
        /// <returns>New feature, rotated</returns>
        public Feature Rotate(Rotation Angle)
        {
            // Short circuit if same using copy constructor
            if (Angle == Rotation.UP)
            {
                return new Feature(this);
            }

            // Unsure if this is best way to handle this - but readable, and simple
            Feature f = Angle == Rotation.DOWN ?
                new Feature(Width, Height) : new Feature(Height, Width);

            // Set rotated map
            for (int ix = 0; ix < Width; ++ix)
            {
                for (int iy = 0; iy < Height; ++iy)
                {
                    f.SetCell(RotatePoint(new XY(ix, iy), Angle), FeatureMap[ix, iy]);
                }
            }

            // Add rotated doors
            foreach (var i in PossibleDoors)
            {
                f.AddPossibleDoor(RotatePoint(i, Angle));
            }

            return f;
        }

        /// <summary>
        /// Amount of floor in the room
        /// </summary>
        public int FloorSpace
        {
            get
            {
                var result = 0;
                for (int ix = 0; ix < Width; ++ix)
                {
                    for (int iy = 0; iy < Height; ++iy)
                    {
                        if (GetCell(new XY(ix, iy)) == DungeonCell.Floor)
                        {
                            ++result;
                        }
                    }
                }
                return result;
            }
        }
    }
}
