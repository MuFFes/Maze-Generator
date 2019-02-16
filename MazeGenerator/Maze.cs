using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class Maze
    {
        public int Size { get; }
        public Stack<int> Stack { get; set; }
        public List<int> Path { get; set; }
        public int Startpos { get; set; }
        public int Endpos { get; set; }
        public int TreasureX { get; set; }
        public int TreasureY { get; set; }
        public int[,] Field { get; set; }
        public int MinWallDifference { get; set; }
        public Maze(int size)
        {
            Size = size;
            Stack = new Stack<int>();
            Field = new int[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == Size - 1 && j == Size - 1)
                        Field[i, j] = 0;
                    else if (i == Size - 1)
                        Field[i, j] = 2;
                    else if (j == Size - 1)
                        Field[i, j] = 1;
                    else
                        Field[i, j] = 3;
                }
            }
        }
        public string ToBinaryString()
        {
            StringBuilder returnString = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Field[j, i] == 0)
                        returnString.Append("00");
                    else if (Field[j, i] == 1)
                        returnString.Append("01");
                    else if (Field[j, i] == 2)
                        returnString.Append("10");
                    else if (Field[j, i] == 3)
                        returnString.Append("11");
                }  
            }
            returnString.Append(Endpos);
            returnString.Append(Startpos);

            return returnString.ToString();
        }
        public override string ToString()
        {
            StringBuilder returnString = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    returnString.Append(Field[j, i]);
                returnString.Append('\n');
            }
            returnString.Append(Endpos);
            returnString.Append(Startpos);
            returnString.Append('\n');
            returnString.Append(TreasureX);
            returnString.Append(TreasureY);

            return returnString.ToString();
        }
    }
}