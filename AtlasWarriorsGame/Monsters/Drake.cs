using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Monsters
{
    class Drake: Monster
    {
        /// <summary>
        /// Create assassin in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Drake(Dungeon dungeon, XY location) : base(dungeon, location)
        {
            MaxHealth = 8;
            Health = 8;
            BaseAtk = 8;
            BaseDmg = 8;
            BaseDef = 12;
        }

        /// <summary>
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public override String SpriteId
        {
            get { return "DRAKE"; }
        }
    }
}
