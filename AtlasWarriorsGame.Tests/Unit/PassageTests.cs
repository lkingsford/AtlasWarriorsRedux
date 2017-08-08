using NUnit.Framework;
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
            var passage = new Passage(Passage.PassageTypeEnum.StairsDown, "PASSAGE",
                new XY(12, 10));
            Assert.AreEqual(Passage.PassageTypeEnum.StairsDown, passage.PassageType);
            Assert.AreSame("PASSAGE", passage.DestinationID);
            Assert.AreEqual(new XY(12, 10), passage.Location);
        }

        /// <summary>
        ///  Test that a passage returns same values as created with without a location
        /// </summary>
        [Test]
        public void CreateReadPassageNoLocation()
        {
            var passage = new Passage(Passage.PassageTypeEnum.StairsDown, "PASSAGE");
            Assert.AreEqual(Passage.PassageTypeEnum.StairsDown, passage.PassageType);
            Assert.AreEqual("PASSAGE", passage.DestinationID);
            Assert.AreEqual(null, passage.Location);
        }
    }
}
