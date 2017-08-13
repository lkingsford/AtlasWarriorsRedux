using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Monsters
{
    class Assassin : Monster
    {
        /// <summary>
        /// Create assassin in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Assassin(Dungeon dungeon, XY location) : base(dungeon, location)
        {
            MaxHealth = 5;
            Health = 5;
            BaseAtk = 17;
            BaseDmg = 15;
            BaseDef = 6;
        }

        /// <summary>
        /// ID for the sake of front end. Still dunno if this is a good idea.
        /// </summary>
        public override String SpriteId
        {
            get { return "ASSASSIN"; }
        }
    }
}
