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
        /// Create a new spawn area, translated by a certain amount
        /// </summary>
        /// <param name="translation">Translation amount</param>
        /// <returns></returns>
        public SpawnArea Translate(XY translation)
        {
            var spawnArea = new SpawnArea();
            foreach (var point in Area)
            {
                spawnArea.Area.Add(point + translation);
            }
            spawnArea.SpecialID = SpecialID;
            return spawnArea;
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