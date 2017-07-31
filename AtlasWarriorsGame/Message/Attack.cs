using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Message
{
    /// <summary>
    /// Message for an attack, by anybody, to anybody
    /// </summary>
    public class Attack : Message
    {
        /// <summary>
        /// Construct attack message with given values
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="defender">Defender</param>
        /// <param name="hit">Whether attack was a hit</param>
        /// <param name="damage">Damage of attack (net)</param>
        /// <param name="atkTotal">Attack roll + stat</param>
        /// <param name="defTotal">Defenders total def</param>
        public Attack(Actor attacker,
                      Actor defender,
                      bool hit,
                      int damage,
                      int atkTotal,
                      int defTotal)
        {
            Attacker = attacker;
            Defender = defender;
            Hit = hit;
            Damage = damage;
            AtkTotal = atkTotal;
            DefTotal = defTotal;
            MessageType = MessageTypes.ATTACK;
        }

        /// <summary>
        /// Attacker
        /// </summary>
        public readonly Actor Attacker;

        /// <summary>
        /// Defender
        /// </summary>
        public readonly Actor Defender;

        /// <summary>
        /// Whether the attack was a hit
        /// </summary>
        public readonly bool Hit;

        /// <summary>
        /// Damage of attack
        /// </summary>
        public readonly int Damage;

        /// <summary>
        /// Attack roll + stat
        /// </summary>
        public readonly int AtkTotal;

        /// <summary>
        /// Defenders total def
        /// </summary>
        public readonly int DefTotal;

        /// <summary>
        /// Get a string version of the attack message
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Attacker.SpriteId} attacked {Defender.SpriteId} and {(Hit ? "hit" : "missed")} ({AtkTotal} vs {DefTotal}) {Damage} damage";
        }
    }
}
