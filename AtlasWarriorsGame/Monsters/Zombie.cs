using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Monsters
{
    class Zombie: Monster
    {
        /// <summary>
        /// Create assassin in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Zombie(Dungeon dungeon, XY location) : base(dungeon, location)
        {
            MaxHealth = 6;
            Health = 6;
            BaseAtk = 4;
            BaseDmg = 4;
            BaseDef = 4;
        }

        /// <summary>
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public override String SpriteId
        {
            get { return "ZOMBIE"; }
        }
    }
}
