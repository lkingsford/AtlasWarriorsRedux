using AtlasWarriorsGame.DungeonGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AtlasWarriorsGame
{
    public class DungeonPrototype
    {
        /// <summary>
        /// Generator attatched to prototype
        /// </summary>
        [JsonIgnore]
        public IDungeonGenerator Generator;

        /// <summary>
        /// String name of dungeon generator
        /// </summary>
        public string DungeonGeneratorId
        {
            get
            {
                return Generator.GetType().Name;
            }

            set
            {
                var assembly = Assembly.GetExecutingAssembly();
                Type generatorType = assembly.GetType(
                    $"AtlasWarriorsGame.DungeonGenerators.{value}");
                Generator = (IDungeonGenerator)Activator.CreateInstance(generatorType);
            }
        }

        /// <summary>
        /// Width of dungeon to make
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of dungeon to make
        /// </summary>
        public int Height;

        /// <summary>
        /// List of passages to place
        /// </summary>
        public List<Passage> Passages = new List<Passage>();

        /// <summary>
        /// List of monsters to put in this dungeon
        /// </summary>
        public List<String> MonstersToBuild = new List<String>();
    }
}
