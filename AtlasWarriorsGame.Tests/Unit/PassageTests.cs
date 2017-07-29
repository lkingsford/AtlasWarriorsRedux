﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests.Unit
{
    [TestFixture]
    class PassageTests
    {
        /// <summary>
        ///  Test that a passage returns same values as created with
        /// </summary>
        [Test]
        public void CreateReadPassage()
        {
            var passage = new Passage(Passage.PassageTypeEnum.StairsDown, "DUNGEON_1");
            Assert.AreEqual(Passage.PassageTypeEnum.StairsDown, passage.PassageType);
            Assert.AreEqual("DUNGEON_1", passage.Destination);
        }
    }
}