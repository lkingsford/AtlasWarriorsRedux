using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    static class MonsterFactory
    {
        /// <summary>
        /// Monsters that may be built
        /// </summary>
        private static Dictionary<string, MonsterFactoryPrototype> Prototypes =
            new Dictionary<string, MonsterFactoryPrototype>();

        /// <summary>
        /// Add monster to the protoypes that can be constructed
        /// </summary>
        /// <param name="id">Identifier for monster</param>
        /// <param name="prototype">JObject containing monster details</param>
        public static void AddPrototype(string id, JObject prototype)
        {
            Prototypes.Add(id, prototype.ToObject<MonsterFactoryPrototype>());
        }

        /// <summary>
        /// Create a monster using the given prototype
        /// </summary>
        /// <param name="game">The game that is being played</param>
        /// <param name="dungeon">Dungeon to put monster in</param>
        /// <param name="monsterType">Type of monster to build</param>
        /// <param name="location">Location to build monster</param>
        /// <returns></returns>
        public static Monster CreateMonster(Game game, Dungeon dungeon, string monsterType, XY location)
        {
            // Get prototype from dict
            if (!Prototypes.TryGetValue(monsterType, out MonsterFactoryPrototype prototype))
                throw new KeyNotFoundException("Monster type not found");

            var monster = new Monster(dungeon, location)
            {
                // Set monster fields from prototype
                SpriteId = prototype.SpriteId,
                MaxHealth = prototype.Health,
                BaseAtk = prototype.BaseAtk,
                BaseDef = prototype.BaseDef,
                BaseDmg = prototype.BaseDmg
            };
            monster.SetHealth(prototype.Health);


            return monster;
        }
    }
}
