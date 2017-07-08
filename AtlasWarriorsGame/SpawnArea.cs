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
        /// Default constructor. Initialised list of points only.
        /// </summary>
        public SpawnArea()
        {
            Area = new List<XY>();
        }

        /// <summary>
        /// Locations where can spawn
        /// </summary>
        public List<XY> Area;

        /// <summary>
        /// Special ID if required by particular dungeon generator to place something particular
        /// (for instance, a boss)
        /// </summary>
        public string SpecialID = null;
    }
}