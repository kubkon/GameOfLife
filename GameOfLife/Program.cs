using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class World
    {
        int rows;
        int columns;
        int[,] grid;

        public World(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid = new int[rows, columns];
        }

        public int[,] Grid
        {
            get { return grid; }
        }

        public void Initialise(int[][] live)
        {
            for (int i = 0; i < live.GetLength(0); i++)
            {
                SetCellLive(live[i]);
            }
        }

        void SetCellLive(int[] indices)
        {
            grid[indices[0], indices[1]] = 1;
        }

        void SetCellDead(int[] indices)
        {
            grid[indices[0], indices[1]] = 0;
        }

        public void Evolve()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int currentCell = grid[i, j];
                    // Count live neighbours of the current cell
                    int liveNeighbours = CountLiveNeighbours(i, j);
                    // Apply rules
                    int result = ApplyRules(i, j, currentCell, liveNeighbours);
                    // Save result
                }
            }
        }

        public int CountLiveNeighbours(int x, int y)
        {
            List<int> neighbours = new List<int>();
            for (int i = -1; i < 2; i++)
            {
                int k = x + i;

                for (int j = -1; j < 2; j++)
                {
                    int l = y + j;

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
                    string el = grid[i, j] == 1 ? "x" : ".";
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
            int[][] liveCells = new int[][] {
                new int[] {0, 1},
                new int[] {1, 2},
                new int[] {2, 0},
                new int[] {2, 1},
                new int[] {2, 2}
            };
            world.Initialise(liveCells);
            world.Evolve();
            Console.Write(world);
            Console.ReadKey();
        }
    }
}
