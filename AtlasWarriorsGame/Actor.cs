using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Anything that can act. Most likely player, or monsters.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Create Actor without dungeon
        /// </summary>
        public Actor()
        {

        }

        /// <summary>
        /// ID for the sake of front end. Dunno if this is a good idea.
        /// </summary>
        public virtual String SpriteId { get {return "ACTOR";}}

        /// <summary>
        /// Create Actor on given level
        /// </summary>
        /// <param name="Dungeon">Dungeon where actor is</param>
        public Actor(Dungeon Dungeon)
        {
            this.Dungeon = Dungeon;
            Dungeon.Actors.Add(this);
            this.Location = Dungeon.StartLocation;
        }

        /// <summary>
        /// Act
        /// </summary>
        public virtual void DoTurn()
        {

        }

        /// <summary>
        /// Dungeon which actor is in
        /// </summary>
        public Dungeon Dungeon { get; protected set; }

        /// <summary>
        /// Current location of the actor
        /// </summary>
        public XY Location {get; protected set; }

        /// <summary>
        /// Move the actor, if the dungeon allows it
        /// Attack, if there's somebody there
        /// </summary>
        /// <param name="dxDy">Amount to move</param>
        public void Move(XY dxDy)
        {
            var newLocation = Location + dxDy;
            var occupant = Dungeon.Occupant(newLocation);

            if (occupant != null)
            {
                Attack(occupant);
            }
            else if (Dungeon.Walkable(newLocation))
            {
                Location = newLocation;
            }
        }

        /// <summary>
        /// Attack a given opponent
        /// </summary>
        /// <param name="opponent">Opponent to attack</param>
        virtual protected void Attack(Actor opponent)
        {
            // Do attack roll
            int roll = 1 + GlobalRandom.Next(20);

            // Check if attack
            if ((roll + Atk) >= opponent.Def)
            {
                opponent.Injure(Dmg);
            }
        }

        /// <summary>
        /// Health of player underlying variable
        /// </summary>
        protected int Health = 10;

        /// <summary>
        /// Health of player
        /// Read only, as needs to be set in specific ways (healing, injuring, resetting to value)
        /// which will have gameplay ramifications (such as death)
        /// </summary>
        /// <remarks>Default of 10</remarks>
        public int CurrentHealth
        {
            get
            {
                return Health; 
            }
        }

        /// <summary>
        /// Reduce health due to attack
        /// </summary>
        /// <param name="injury">Amount to reduce</param>
        /// <returns>New health</returns>
        virtual public int Injure(int injury)
        {
            // Allowed to go <0 in this game - but needs to (TODO) trigger Kill Or Be Killed for
            // player, or kill the monster in other circumstance
            Health -= injury;

            return Health;
        }

        /// <summary>
        /// Increase health
        /// </summary>
        /// <param name="healing">Amount to increase</param>
        /// <returns>New health</returns>
        virtual public int Heal(int healing)
        {
            Health += healing;

            // Can't go above max health
            Health = Math.Min(Health, MaxHealth);

            return Health;
        }

        /// <summary>
        /// Set health to desired values. For tests, and a couple of specific things. Not always.
        /// </summary>
        /// <param name="health">Value to set it to</param>
        virtual public void SetHealth(int health)
        {
            Health = health;
        }

        /// <summary>
        /// Maximum allowed health
        /// </summary>
        /// <remarks>Default of 10</remarks>
        public int MaxHealth = 10;

        /// <summary>
        /// Current defence statistic
        /// Attack roll + Attack > Defence == Successful attack
        /// </summary>
        virtual public int Def
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Current attack statistic
        /// Attack roll + Attack > Defence == Successful attack
        /// </summary>
        virtual public int Atk
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Current damage statistic
        /// </summary>
        virtual public int Dmg
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Whether the Actor is Dead (Health less than 0 by default)
        /// </summary>
        virtual public bool Dead
        {
            get
            {
                return Health <= 0;
            }
        }

    }
}
