using System.Collections.Generic;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Locations in a dungeon where monsters can be spawned.
    /// Should be treated as one region - not all the spawn in the level.
    /// </summary>
    /// <remarks>Should this be just in dungeon?</remarks>
    public class SpawnArea
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SpawnArea()
        {
        }

        /// <summary>
        /// Create a spawn area with just the given point
        /// </summary>
        public SpawnArea(XY point)
        {
            Area.Add(point);
        }

        /// <summary>
        /// Locations where can spawn
        /// </summary>
        public List<XY> Area = new List<XY>();

        /// <summary>
        /// Special ID if required by particular dungeon generator to place something particular
        /// (for instance, a boss)
        /// </summary>
        public string SpecialID = null;
    }
}