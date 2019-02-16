using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //  289
            //  20 708
            //  5 123 268

            // 1123

            int size = 8;
            List<Maze> mazeList = MazeGenerator.GenerateMany(size, 1000, 1);

            for (int i = 0; i < 100; i++)
            {
                if (mazeList.Count(u => u.MinWallDifference == i) != 0)
                    Console.WriteLine(i + ": " + mazeList.Count(u => u.MinWallDifference == i));
            }
        }
    }
}