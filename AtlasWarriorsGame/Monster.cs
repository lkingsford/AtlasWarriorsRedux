using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// A monster is any bad guy at all. Even if they're actually human. 
    /// Because, humanity are the true monsters.
    /// </summary>
    class Monster: Actor
    {
        /// <summary>
        /// Create monster in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Monster(Dungeon dungeon, XY location) : base(dungeon)
        {
            Location = location;
        }
    }
}
