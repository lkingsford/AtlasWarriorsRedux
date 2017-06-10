using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Anything that can act. Most likely player, or monsters.
    /// </summary>
    class Actor
    {
        /// <summary>
        /// Create Actor without dungeon
        /// </summary>
        public Actor()
        {

        }

        /// <summary>
        /// Create Actor on given level
        /// </summary>
        /// <param name="Dungeon">Dungeon where actor is</param>
        public Actor(Dungeon Dungeon)
        {
            this.Dungeon = Dungeon;
            this.Location = Dungeon.StartLocation;
        }

        /// <summary>
        /// Dungeon which actor is in
        /// </summary>
        public Dungeon Dungeon { get; private set; }

        /// <summary>
        /// Current location of the actor
        /// </summary>
        public XY Location {get; private set; }

        /// <summary>
        /// Move the actor, if the dungeon allows it
        /// </summary>
        /// <param name="dxDy">Amount to move</param>
        public void Move(XY dxDy)
        {
            var newLocation = Location + dxDy;
            if (Dungeon.Walkable(newLocation))
            {
                Location = newLocation;
            }
        }
    }
}
