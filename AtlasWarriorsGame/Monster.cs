using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// A monster is any bad guy at all. Even if they're actually human. 
    /// Because, humanity are the true monsters.
    /// </summary>
    class Monster: Actor
    {
        /// <summary>
        /// Create monster in dungeon in location
        /// </summary>
        /// <param name="dungeon">Dungeon monster in</param>
        /// <param name="location">Location monster in</param>
        public Monster(Dungeon dungeon, XY location) : base(dungeon)
        {
            Location = location;
            Team = 1;
        }

        /// <summary>
        /// Act.
        /// At the moment - move towards player
        /// </summary>
        public override void DoTurn()
        {
            // Find nearest actor on different team
            var enemies = Dungeon.Actors.FindAll(i => (i.Team != this.Team));
            var friends = Dungeon.Actors.FindAll(i => (i.Team == this.Team));

            // Do nothing if no enemies
            if (enemies.Count == 0)
            {
                return;
            }

            // Get walkable map
            var walkable = new bool[Dungeon.Width, Dungeon.Height];
            for (var x = 0; x < Dungeon.Width; x++)
            {
                for (var y = 0; y < Dungeon.Height; y++)
                {
                    walkable[x, y] = Dungeon.Walkable(new XY(x, y));
                }
            }

            // Set can't walk to prevent walking over friends
            foreach (var i in friends)
            {
                walkable[i.Location.X, i.Location.Y] = false;
            }

            var path = Pathfinder.FindPath(walkable, this.Location, enemies[0].Location,
                failureAllowed: false);

            // Not sure why path[1] might equal null yet - but it's happened :/
            if (path == null || path.Count < 2 || path[1] is null)
            {
                return;
            }
            var nextPoint = path[1];

            // Go towards nearest
            Move(nextPoint - this.Location);

        }
    }
}
