using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Monsters
{
    class Healer: Monster
    {
        /// <summary>
        /// Create assassin in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Healer(Dungeon dungeon, XY location) : base(dungeon, location)
        {
            MaxHealth = 6;
            Health = 6;
            BaseAtk = 3;
            BaseDmg = 5;
            BaseDef = 6;
        }

        /// <summary>
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public override String SpriteId
        {
            get { return "HEALER"; }
        }
    }
}
