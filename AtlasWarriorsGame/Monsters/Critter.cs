using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Monsters
{
    class Critter: Monster
    {
        /// <summary>
        /// Create assassin in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Critter(Dungeon dungeon, XY location) : base(dungeon, location)
        {
            MaxHealth = 5;
            Health = 5;
            BaseAtk = 3;
            BaseDmg = 3;
            BaseDef = 4;
        }

        /// <summary>
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public override String SpriteId
        {
            get { return "CRITTER"; }
        }
    }
}
