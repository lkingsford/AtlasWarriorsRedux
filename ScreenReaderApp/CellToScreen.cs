﻿using AtlasWarriorsGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static AtlasWarriorsGame.Dungeon;

namespace ScreenReaderApp
{
    /// <summary>
    /// Classes used to get characters for elements
    /// </summary>
    static class CellToScreen
    {
        /// <summary>
        /// Get the character used to display a particular type of cell
        /// </summary>
        /// <param name="Cell">Cell to check</param>
        /// <returns>Character to display on screen for cell</returns>
        public static char CellScreenChar(DungeonCell Cell)
        {
            switch (Cell)
            {
                case DungeonCell.Empty:
                    return ' ';
                case DungeonCell.Floor:
                    return '.';
                case DungeonCell.Door:
                    return '+';
                case DungeonCell.Wall:
                    return '#';
                case DungeonCell.StairUp:
                    return '<';
                case DungeonCell.StairDown:
                    return '>';
                default:
                    return 'X';
            }
        }

        /// <summary>
        /// Get the character to display a particular actor
        /// </summary>
        /// <param name="Actor"></param>
        /// <returns></returns>
        public static char ActorToChar(Actor Actor)
        {
            // Initialise sprites, if it's null
            if (Characters == null)
            {
                using (var spriteFileReader = new System.IO.StreamReader("data/sprites_sr.json"))
                {
                    var spriteFileText = spriteFileReader.ReadToEnd();
                    Characters = JsonConvert.DeserializeObject<Dictionary<String, char>>
                        (spriteFileText);
                }
            }
            return Characters[Actor.SpriteId];
        }

        private static Dictionary<string, char> Characters = null;
    }
}
