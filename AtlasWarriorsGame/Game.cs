using System;
using System.Collections.Generic;
using System.Linq;
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
            Populate(_dungeon);
            // Create empty messages list - prevent crash on first turn
            MessagesData.Add(new List<Message.Message>());
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
            var turnMessages = CurrentDungeon.DoTurn();
            MessagesData.Add(turnMessages);
        }

        /// <summary>
        /// Populate a dungeon with bad guys
        /// </summary>
        /// <remarks>This will likely be moved</remarks>
        public void Populate(Dungeon d)
        {
            foreach (SpawnArea a in d.SpawnAreas)
            {
                // Randomly pick a point in the spawn area
                var monsterPoint = a.Area.RandomItem();
                // Not sure if this is silly, but add the monster to the point. It adds itself.
                new Monster(d, monsterPoint); 
            }
        }

        /// <summary>
        /// Messages in the game to display
        /// Underlying variable
        /// Separated by turns
        /// </summary>
        private List<List<Message.Message>> MessagesData = new List<List<Message.Message>>();

        /// <summary>
        /// Messages, that can be read
        /// </summary>
        public IReadOnlyCollection<Message.Message> Messages
        {
            get
            {
                // Flatten messages to return
                return MessagesData.SelectMany(x=>x).ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Get last turns messages only
        /// </summary>
        public IReadOnlyCollection<Message.Message> LastTurnMessages
        {
            get
            {
                return MessagesData.Last().AsReadOnly();
            }
        }
    }
}
