using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtlasWarriorsGame.GlobalRandom;

namespace AtlasWarriorsGame.Tests
{
    /// <summary>
    /// Tests for the GlobalRandom class
    /// </summary>
    [TestFixture]
    class GlobalRandom
    {
        /// <summary>
        /// Test that R.RandomItem returns an element from its list
        /// </summary>
        [Test]
        public void ListRandom()
        {
            var list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
            var random = list.RandomItem();
            Assert.Contains(random, list, "List item not found");
        }
    }
}
