using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Data that is stored from a JSON file to create a monster with
    /// </summary>
    class MonsterFactoryPrototype
    {
        public string SpriteId = "";
        public string Description = "";
        public int Health = 10;
        public int BaseAtk = 10;
        public int BaseDef = 10;
        public int BaseDmg = 10;
    }
}
