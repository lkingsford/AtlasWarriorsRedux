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
            Populate(_dungeon);
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
            CurrentDungeon.Clean();
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
        /// </summary>
        private List<Message.Message> MessagesData = new List<Message.Message>();

        /// <summary>
        /// Add message to message list
        /// </summary>
        /// <param name="message">Message to add</param>
        public void AddMessage(Message.Message message)
        {
            MessagesData.Add(message);
        }

        /// <summary>
        /// Messages, that can be read
        /// </summary>
        public IReadOnlyCollection<Message.Message> Message
        {
            get
            {
                return MessagesData.AsReadOnly();
            }
        }
    }
}
