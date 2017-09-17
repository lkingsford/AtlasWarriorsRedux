using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Type of asset stream reader
    /// </summary>
    /// <param name="filename">Filename to open</param>
    /// <returns>Open stream</returns>
    public delegate Stream TGetAssetStream(string filename);

    /// <summary>
    /// Main game class
    /// Initialise again for each new game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Get an asset stream
        /// Default Asset Stream Getter is off filesystem in "data" folder
        /// </summary>
        public TGetAssetStream GetAssetStream = (string filename) =>
            { return File.Open($"data/{filename}", FileMode.Open); };

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="getAssetStream">Asset stream getter - if not default (filesystem)</param>
        public Game(TGetAssetStream getAssetStream = null)
        {
            // Set GetAssetStream if changed from default
            GetAssetStream = getAssetStream ?? GetAssetStream;

            // Load the data from Monsters.Json into the Monster Factory
            using (var monsterFileReader = new StreamReader(GetAssetStream("monsters.json")))
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
            // We get the parameters for building them, then we build them
            Dictionary<string, DungeonPrototype> dungeonsToDig;
            using (var dungeonsFileReader = new StreamReader(GetAssetStream("dungeons.json")))
            {
                var dungeonsFileText = dungeonsFileReader.ReadToEnd();
                dungeonsToDig = JsonConvert.DeserializeObject<Dictionary<string, DungeonPrototype>>
                    (dungeonsFileText);
            }
            foreach(var dungeonToDig in dungeonsToDig)
            {
                var dungeon = dungeonToDig.Value.Generator.Generate(
                    width: dungeonToDig.Value.Width,
                    height: dungeonToDig.Value.Height,
                    passages: dungeonToDig.Value.Passages);
                DungeonStore.Add(dungeonToDig.Key, dungeon);

                // Place enemies per the dungeon to dig
                for(int i = 0; i < 20; ++i)
                {
                    var monster = dungeonToDig.Value.MonstersToBuild.RandomItem();
                    var spawnArea = dungeon.SpawnAreas.RandomItem();
                    if (spawnArea.Area.Count > 0)
                    {
                        var spawnLocation = spawnArea.Area.RandomItem();
                        spawnArea.Area.Remove(spawnLocation);
                        MonsterFactory.CreateMonster(this, dungeon, monster, spawnLocation);
                    }
                }
            }

            // Set the Dungeon for each passage
            foreach (var dungeon in DungeonStore)
            {
                foreach (var passage in dungeon.Value.Passages)
                {
                    DungeonStore.TryGetValue(passage.DestinationID, out passage.Destination);
                }
            }

            // Find the start passage - and what dungeon it's in
            var start = DungeonStore.SelectMany(i => i.Value.Passages)
                                    .First(i => i.DestinationID == "START");
            var startDungeon = DungeonStore.First(i => i.Value.Passages.Contains(start)).Value;

            Player = new Player(startDungeon);

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

            if (Player.Dead)
            {
                GameOver = true;
                EndGameText = new List<string>()
                {
                    "Would you like your corpse identified?",
                    "Congratulations!",
                    "Your journey end here. In many pieces",
                    "Rest in many pieces",
                    "Death was too good for you!",
                    "Y. A. S. D",
                    "How sad. I'm sorry. I really am. <beat> I'm not."
                }.RandomItem();
                EndGameTitle = "You have died";
            }
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

        /// <summary>
        /// If the game is finished - for one reason or another
        /// </summary>
        public bool GameOver
        {
            get;
            private set;
        } = false;

        /// <summary>
        /// Title to show on win/loss screen
        /// </summary>
        public string EndGameTitle
        {
            get;
            private set;
        }

        /// <summary>
        /// Text to show on win/loss screen
        /// </summary>
        public string EndGameText
        {
            get;
            private set;
        }
    }
}
