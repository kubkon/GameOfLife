using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class World
    {
        public readonly int rows;
        public readonly int columns;
        public readonly int[,] grid;

        public World(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid = new int[rows, columns];
        }

        public void Initialise(List<int[]> liveCells)
        {
            UpdateGrid(liveCells);
        }

        void UpdateGrid(List<int[]> liveCells)
        {
            Array.Clear(grid, 0, rows * columns);
            // FIX:ME check if indices do not exceed the grid dimensions
            foreach (int[] indices in liveCells)
            {
                grid[indices[0], indices[1]] = 1;
            }
        }

        public void Evolve()
        {
            List<int[]> survived = new List<int[]>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var currentCell = grid[i, j];
                    // Count live neighbours of the current cell
                    var liveNeighbours = CountLiveNeighbours(i, j);
                    // Apply rules
                    var result = ApplyRules(i, j, currentCell, liveNeighbours);
                    // Save result
                    if (result == 1)
                        survived.Add(new int[] { i, j });
                }
            }
            // Update the grid
            UpdateGrid(survived);
        }

        public int CountLiveNeighbours(int x, int y)
        {
            List<int> neighbours = new List<int>();
            for (int i = -1; i < 2; i++)
            {
                var k = x + i;

                for (int j = -1; j < 2; j++)
                {
                    var l = y + j;

                    if (k >= 0 && k < rows && l >= 0 && l < columns && (k != x || l != y))
                    {
                        neighbours.Add(grid[k, l]);
                    }
                }
            }
            return neighbours.Sum();
        }

        public int ApplyRules(int x, int y, int currentCell, int liveNeighbours)
        {
            int result = 0;
            if ((currentCell > 0 && liveNeighbours == 2) || liveNeighbours == 3)
            {
                result = 1;
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var el = grid[i, j] == 1 ? "x" : ".";
                    sb.Append(el);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            World world = new World(10, 12);
            List<int[]> liveCells = new List<int[]> () {
                new int[] {0, 1},
                new int[] {1, 2},
                new int[] {2, 0},
                new int[] {2, 1},
                new int[] {2, 2}
            };
            world.Initialise(liveCells);
            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                Console.Write(world);
                world.Evolve();
                Thread.Sleep(1000);
            }
            Console.ReadKey();
        }
    }
}
