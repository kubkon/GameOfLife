using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
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

        List<int[]> survived;

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

        public void Randomise()
        {
            var rnd = new Random();
            var seeded = new List<int[]> ();
            for (int i = 0; i < rnd.Next(rows * columns); i++)
            {
                var m = rnd.Next(rows);
                var n = rnd.Next(columns);
                var tuple = new int[] { m, n };
                if (seeded.Any(x => CompareArrays(tuple, x)))
                {
                    i--;
                    continue;
                }
                seeded.Add(tuple);
            }
            UpdateGrid(seeded);
        }

        public bool Evolve()
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
            ClearGrid();
            UpdateGrid(survived);
            // Check if evolution is finished
            var finished = false;
            if (this.survived != null)
            {
                finished = this.survived.Zip(survived, CompareArrays)
                                        .All(x => x);
            }
            this.survived = survived;
            return finished;
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
            var result = 0;
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

        void ClearGrid()
        {
            Array.Clear(grid, 0, rows * columns);
        }

        void UpdateGrid(List<int[]> liveCells)
        {
            // FIX:ME check if indices do not exceed the grid dimensions
            foreach (int[] indices in liveCells)
            {
                grid[indices[0], indices[1]] = 1;
            }
        }

        bool CompareArrays(int[] a1, int[] a2)
        {
            return a1.Zip(a2, (x, y) => x == y).All(x => x);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var culture = new CultureInfo("en-GB");
            Console.Write("Number of rows: ");
            var rows = ((IConvertible)Console.ReadLine()).ToInt32(culture.NumberFormat);
            Console.Write("Number of columns: ");
            var columns = ((IConvertible)Console.ReadLine()).ToInt32(culture.NumberFormat);

            var world = new World(rows, columns);
            world.Randomise();
            while (true)
            {
                Console.Clear();
                Console.Write(world);
                if (world.Evolve())
                    break;
                Thread.Sleep(1000);
            }
            Console.WriteLine("Simulation finished.");
            Console.ReadKey();
        }
    }
}
