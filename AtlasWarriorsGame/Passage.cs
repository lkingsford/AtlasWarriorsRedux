using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        /// Default constructor. Used by Json.net.
        /// </summary>
        public Passage()
        {

        }

        /// <summary>
        /// Create passage with given values
        /// </summary>
        /// <param name="passageType">Type of passage</param>
        /// <param name="destinationID">Dungeon passage goes to</param>
        /// <param name="location">Location on map of passage</param>
        public Passage(PassageTypeEnum passageType, String destinationID, XY location = null)
        {
            this.PassageType = passageType;
            this.DestinationID = destinationID;
            this.Location = location;
        }

        /// <summary>
        /// Create passage with given values, with a dungeon instead of ID
        /// </summary>
        /// <param name="passageType">Type of passage</param>
        /// <param name="destination">Dungeon passage goes to</param>
        /// <param name="location">Location on map of passage</param>
        public Passage(PassageTypeEnum passageType, Dungeon destination, XY location = null)
        {
            this.PassageType = passageType;
            this.Destination = destination;
            this.Location = location;
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
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public PassageTypeEnum PassageType { get; private set; }

        /// <summary>
        /// Where it goes - name of dungeon in Game
        /// </summary>
        [JsonProperty]
        public String DestinationID { get; private set; }

        /// <summary>
        /// ... and where it goes - the actual dungeon, as set by Game
        /// </summary>
        [JsonIgnore]
        public Dungeon Destination = null;

        /// <summary>
        /// Where on the map it is placed
        /// Null if not placed yet
        /// </summary>
        [JsonIgnore]
        public XY Location;
    }
}
