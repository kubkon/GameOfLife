using System;
using System.Collections.Generic;
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
            world = new World(2, 2, 2);
            world.Initialise(new List<int[]>() {
                new int[] {0, 0},
                new int[] {1, 1}
            });
        }

        [TestMethod]
        public void TestCountLiveNeighbours()
        {

            var live = world.CountLiveNeighbours(0, 0);
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
            var live = world.CountLiveNeighbours(x, y);
            var future = world.ApplyRules(x, y, world.grid[x, y], live);
            Assert.AreEqual(CellState.Dead, future);
            x = 0;
            y = 1;
            live = world.CountLiveNeighbours(x, y);
            future = world.ApplyRules(x, y, world.grid[x, y], live);
            Assert.AreEqual(CellState.Dead, future);
        }

        [TestMethod]
        public void TestEvolve()
        {
            var w = new World(4, 4, 2);
            var liveCells = new List<int[]> () {
                new int[] {0, 1},
                new int[] {1, 2},
                new int[] {2, 0},
                new int[] {2, 1},
                new int[] {2, 2}
            };
            w.Initialise(liveCells);
            for (int i = 0; i < 7; i++)
            {
                Assert.AreEqual(false, w.Evolve());
            }
            Assert.AreEqual(true, w.Evolve());
            for (int i = 0; i < w.rows; i++)
            {
                for (int j = 0; j < w.columns; j++)
                {
                    var cell = w.grid[i, j];
                    if ((i == 2 || i == 3) && (j == 2 || j == 3))
                        Assert.AreEqual(CellState.Live, cell);
                    else
                        Assert.AreEqual(CellState.Dead, cell);
                }
            }
        }
    }
}
