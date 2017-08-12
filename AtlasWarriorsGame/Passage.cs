using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// A passage between two dungeons
    /// </summary>
    public class Passage
    {
        /// <summary>
        /// Create passage with given values
        /// </summary>
        /// <param name="PassageType">Type of passage</param>
        /// <param name="DestinationID">Dungeon passage goes to</param>
        /// <param name="Location">Location on map of passage</param>
        public Passage(PassageTypeEnum PassageType, String DestinationID, XY Location = null)
        {
            this.PassageType = PassageType;
            this.DestinationID = DestinationID;
            this.Location = Location;
        }

        /// <summary>
        /// Possible types that passage can form - mostly important for UI
        /// </summary>
        /// <remarks>Better suggestion of naming?</remarks>
        public enum PassageTypeEnum {
            /// <summary>
            /// One direction only - used as a destination
            /// </summary>
            OneWay,
            /// <summary>
            /// Stairs going up
            /// </summary>
            StairsUp,
            /// <summary>
            /// Stairs going down
            /// </summary>
            StairsDown };

        /// <summary>
        /// Type of passage
        /// </summary>
        public readonly PassageTypeEnum PassageType;

        /// <summary>
        /// Where it goes - name of dungeon in Game
        /// </summary>
        public readonly String DestinationID;

        /// <summary>
        /// ... and where it goes - the actual dungeon, as set by Game
        /// </summary>
        public Dungeon Destination = null;

        /// <summary>
        /// Where on the map it is placed
        /// Null if not placed yet
        /// </summary>
        public XY Location;
    }
}
