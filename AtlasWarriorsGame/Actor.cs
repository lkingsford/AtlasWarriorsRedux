using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Anything that can act. Most likely player, or monsters.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Create Actor without dungeon
        /// </summary>
        public Actor()
        {

        }

        /// <summary>
        /// ID for the sake of front end. Dunno if this is a good idea.
        /// </summary>
        public virtual String SpriteId { get {return "ACTOR";}}

        /// <summary>
        /// Create Actor on given level
        /// </summary>
        /// <param name="Dungeon">Dungeon where actor is</param>
        public Actor(Dungeon Dungeon)
        {
            this.Dungeon = Dungeon;
            Dungeon.Actors.Add(this);
            this.Location = Dungeon.StartLocation;
        }

        /// <summary>
        /// Act
        /// </summary>
        public virtual void DoTurn()
        {

        }

        /// <summary>
        /// Dungeon which actor is in
        /// </summary>
        public Dungeon Dungeon { get; protected set; }

        /// <summary>
        /// Current location of the actor
        /// </summary>
        public XY Location {get; protected set; }

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
