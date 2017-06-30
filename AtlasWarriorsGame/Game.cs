using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Main game class
    /// Initialise again for each new game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Game()
        {
            _dungeon = new Dungeon(40, 20, DungeonGenerators.RoomsGenerator.Generate); 
            Player = new Player(_dungeon);
        }

        /// <summary>
        /// TEMPORARY - Will be replaced by complete store of dungeons
        /// The dungeon
        /// </summary>
        Dungeon _dungeon;

        /// <summary>
        /// Main player of the game
        /// </summary>
        public Player Player;

        /// <summary>
        /// Current dungeon being played
        /// </summary>
        public Dungeon CurrentDungeon
        {
            get
            {
                return _dungeon;
            }
        }

        /// <summary>
        /// Do a turn in the game
        /// </summary>
        public void DoTurn()
        {
            foreach (var Actor in CurrentDungeon.Actors)
            {
                Actor.DoTurn();
            }
        }
    }
}
