using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests.Unit
{
    /// <summary>
    /// Tests for the player class specifically
    /// </summary>
    [TestFixture]
    class PlayerTests
    { 
        /// <summary>
        /// Tests that the player starts on the start point when created
        /// </summary>
        [Test]
        public void PlayerStartsOnStart()
        {
            var dungeon = new Dungeon(10, 10);
            dungeon.Passages.Add(
                new Passage(Passage.PassageTypeEnum.OneWay, "START", new XY(4, 3)));
            var player = new Player(dungeon);

            Assert.AreEqual(new XY(4, 3), player.Location, "Player started in incorrect location");
        }
    }
}
