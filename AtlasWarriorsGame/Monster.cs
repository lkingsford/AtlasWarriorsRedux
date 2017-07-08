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
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public virtual String SpriteId { get { return "MONSTER"; } }
    }
}
