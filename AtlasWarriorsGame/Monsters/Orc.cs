using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Monsters
{
    class Orc: Monster
    {
        /// <summary>
        /// Create assassin in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Orc(Dungeon dungeon, XY location) : base(dungeon, location)
        {
            MaxHealth = 12;
            Health = 12;
            BaseAtk = 9;
            BaseDmg = 4;
            BaseDef = 8;
        }

        /// <summary>
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public override String SpriteId
        {
            get { return "ORC"; }
        }
    }
}
