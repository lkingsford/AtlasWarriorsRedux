using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            // Load the data from Monsters.Json into the Monster Factory
            using (var monsterFileReader = new System.IO.StreamReader("data/monsters.json"))
            {
                var monsterFileText = monsterFileReader.ReadToEnd();
                var deserializedMonsterPrototypes =
                    JsonConvert.DeserializeObject<Dictionary<String, JObject>>(monsterFileText);
                foreach (var prototype in deserializedMonsterPrototypes)
                {
                    MonsterFactory.AddPrototype(prototype.Key, prototype.Value);
                }
            }

            // Construct dungeons
            // Not doing this tidily and programmicably - 'cause it is going to change and we don't
            // want 10 identically boring levels
            DungeonStore["D01"] = DungeonGenerators.RoomsGenerator.Generate(40, 20, new List<Passage>()
            {
                new Passage(Passage.PassageTypeEnum.OneWay, "START"),
                new Passage(Passage.PassageTypeEnum.StairsDown, "D02")
            });

            DungeonStore["D02"] = DungeonGenerators.RoomsGenerator.Generate(40, 20, new List<Passage>()
            {
                new Passage(Passage.PassageTypeEnum.StairsUp, "D01"),
                new Passage(Passage.PassageTypeEnum.StairsDown, "D03")
            });

            DungeonStore["D03"] = DungeonGenerators.RoomsGenerator.Generate(40, 20, new List<Passage>()
            {
                new Passage(Passage.PassageTypeEnum.StairsUp, "D02"),
                new Passage(Passage.PassageTypeEnum.StairsDown, "D04")
            });

            DungeonStore["D04"] = DungeonGenerators.RoomsGenerator.Generate(40, 20, new List<Passage>()
            {
                new Passage(Passage.PassageTypeEnum.StairsUp, "D03"),
                new Passage(Passage.PassageTypeEnum.StairsDown, "D05")
            });

            DungeonStore["D05"] = DungeonGenerators.RoomsGenerator.Generate(40, 20, new List<Passage>()
            {
                new Passage(Passage.PassageTypeEnum.StairsUp, "D05")
            });

            // Set the Dungeon for each passage
            foreach (var dungeon in DungeonStore)
            {
                foreach (var passage in dungeon.Value.Passages)
                {
                    DungeonStore.TryGetValue(passage.DestinationID, out passage.Destination);
                }
            }

            Player = new Player(DungeonStore["D01"]);

            // Create empty messages list - prevent crash on first turn
            MessagesData.Add(new List<Message.Message>());
        }

        /// <summary>
        /// All dungeons in the current game, by ID
        /// </summary>
        Dictionary<String, Dungeon> DungeonStore = new Dictionary<string, Dungeon>();

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
                return Player.Dungeon;
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

        /// <summary>
        /// Get dungeon from store with given ID
        /// </summary>
        /// <param name="dungeonId">Dungeon to get</param>
        /// <returns>Dungeon that was got</returns>
        /// <remarks>May generate dungeon rather than restore it... in the future</remarks>
        public Dungeon GetDungeon(string dungeonId)
        {
            return DungeonStore[dungeonId];
        }
    }
}
