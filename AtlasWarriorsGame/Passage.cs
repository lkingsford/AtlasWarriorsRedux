using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// A passage between two dungeons
    /// </summary>
    class Passage
    {
        /// <summary>
        /// Create passage with given values
        /// </summary>
        /// <param name="PassageType">Type of passage</param>
        /// <param name="Destination">DungeonID of passage</param>
        public Passage(PassageTypeEnum PassageType, String Destination)
        {
            this.PassageType = PassageType;
            this.Destination = Destination;
        }

        /// <summary>
        /// Possible types that passage can form - mostly important for UI
        /// </summary>
        /// <remarks>Better suggestion of naming?</remarks>
        public enum PassageTypeEnum { OneWay, StairsUp, StairsDown };

        /// <summary>
        /// Type of passage
        /// </summary>
        public readonly PassageTypeEnum PassageType;

        /// <summary>
        /// Where it goes - name of dungeon in Game
        /// </summary>
        public readonly string Destination;

        /// <summary>
        /// Where on the map it is placed
        /// </summary>
        public XY Location;
    }
}
