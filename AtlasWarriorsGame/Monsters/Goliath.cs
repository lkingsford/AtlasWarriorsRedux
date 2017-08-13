using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Monsters
{
    class Goliath: Monster
    {
        /// <summary>
        /// Create assassin in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Goliath(Dungeon dungeon, XY location) : base(dungeon, location)
        {
            MaxHealth = 16;
            Health = 16;
            BaseAtk = 8;
            BaseDmg = 10;
            BaseDef = 8;
        }

        /// <summary>
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public override String SpriteId
        {
            get { return "GOLIATH"; }
        }
    }
}
