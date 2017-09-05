using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests
{
    /// <summary>
    /// Tests for the message passing between actor/dungeon/game
    /// </summary>
    [TestFixture]
    class Messages
    {
        /// <summary>
        /// Test that a message, in an actor, gets sent to the dungeon and to the game
        /// </summary>
        [Test]
        public void MessageFromActorToGameTest()
        {
            // Testing by making the Player do an ordinary attack, and checking the message was 
            // sent
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
            var game = new Game();
            var defender = new Actor(game.CurrentDungeon);
            // The attack would normally happen _in_ the turn rather than after - but the message
            // queue is reset at the end of each turn
            game.Player.Attack(defender);
            game.DoTurn();

            Assert.GreaterOrEqual(game.LastTurnMessages.Count, 1, "No message forwarded");

            Assert.AreEqual(1,
                game.LastTurnMessages.Where(i => i.GetType() == typeof(Message.Attack)).Count(),
                "No, or multiple attack messages found");
        }
    }
}
