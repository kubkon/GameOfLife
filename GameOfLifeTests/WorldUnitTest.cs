using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameOfLife;

namespace GameOfLifeTests
{
    [TestClass]
    public class WorldUnitTest
    {
        static World world;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            world = new World(2, 2);
            world.Initialise(new int[][] {
                new int[] {0, 0},
                new int[] {1, 1}
            });
        }

        [TestMethod]
        public void TestCountLiveNeighbours()
        {

            int live = world.CountLiveNeighbours(0, 0);
            Assert.AreEqual(1, live);
            live = world.CountLiveNeighbours(0, 1);
            Assert.AreEqual(2, live);
            live = world.CountLiveNeighbours(1, 0);
            Assert.AreEqual(2, live);
            live = world.CountLiveNeighbours(1, 1);
            Assert.AreEqual(1, live);
        }

        [TestMethod]
        public void TestApplyRules()
        {
            int x = 0, y = 0;
            int live = world.CountLiveNeighbours(x, y);
            int future = world.ApplyRules(x, y, world.Grid[x, y], live);
            Assert.AreEqual(0, future);
            x = 0;
            y = 1;
            live = world.CountLiveNeighbours(x, y);
            future = world.ApplyRules(x, y, world.Grid[x, y], live);
            Assert.AreEqual(0, future);
        }
    }
}
