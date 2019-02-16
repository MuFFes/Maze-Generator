using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public static class MazeGenerator
    {
        public static List<Maze> GenerateMany(int size, int count, int overload)
        {
            //The higher overload is the more different mazes will be generated
            //Bad thing: time complexity is increasing very fast

            List<Maze> mazeList = new List<Maze>();
            do
            {
                mazeList.Add(Generate(size));
                if (mazeList.Count == count * overload)
                    mazeList = mazeList.GroupBy(u => u.ToString()).Select(u => u.First()).ToList();
            } while (mazeList.Count != count * overload);

            foreach (Maze maze in mazeList)
            {
                List<int> distanceList = new List<int>();
                foreach (Maze mz in mazeList.Where(u => u != maze))
                {
                    distanceList.Add(StringDistance.Compute(
                                            mz.ToBinaryString(),
                                            maze.ToBinaryString())
                                            );
                }
                maze.MinWallDifference = distanceList.Min();
            }

            mazeList = mazeList.OrderByDescending(u => u.MinWallDifference).ToList();
            mazeList = mazeList.Take(count).ToList();

            return mazeList;
        }
        public static Maze Generate(int size)
        {
            //  289
            //  20 708
            //  5 123 268

            // 1123
            
            Random random = new Random();
            int seed = random.Next();
            Maze maze = new Maze(size);
            int position = seed % size;
            seed /= size;
            List<int> backtrack = new List<int>();
            maze.Stack.Push(position);

            while (maze.Stack.Count > 0)
            {
                if (seed == 0)
                    seed = random.Next();
                List<int> possibleMoves = new List<int>();

                //Find all possible moves
                if (position / size > 0 && !maze.Stack.Contains(position - size))
                    possibleMoves.Add(position - size);
                if (position % size > 0 && !maze.Stack.Contains(position - 1))
                    possibleMoves.Add(position - 1);
                if (position % size < size - 1 && !maze.Stack.Contains(position + 1))
                    possibleMoves.Add(position + 1);
                if (position / size < size - 1 && !maze.Stack.Contains(position + size))
                    possibleMoves.Add(position + size);

                //Remove already visited tiles
                foreach (int b in backtrack)
                    if (possibleMoves.Contains(b))
                        possibleMoves.Remove(b);

                //Move back when no possible move left
                if (possibleMoves.Count == 0)
                {
                    backtrack.Add(maze.Stack.Pop());
                    if (maze.Stack.Count == 0)
                        break;
                    position = maze.Stack.Peek();
                }
                else
                {
                    //Decide where to go, and modify seed
                    maze.Stack.Push(possibleMoves[seed % possibleMoves.Count]);
                    seed /= possibleMoves.Count;

                    //Wall deletion:
                    if (maze.Stack.Peek() - position == size)
                        if (maze.Field[position % size, position / size] > 1)
                            maze.Field[position % size, position / size] -= 2;

                    if (maze.Stack.Peek() - position == -size)
                        if (maze.Field[position % size, position / size - 1] > 1)
                            maze.Field[position % size, position / size - 1] -= 2;

                    if (maze.Stack.Peek() - position == 1)
                        if (maze.Field[position % size, position / size] % 2 == 1)
                            maze.Field[position % size, position / size] -= 1;

                    if (maze.Stack.Peek() - position == -1)
                        if (maze.Field[position % size - 1, position / size] % 2 == 1)
                            maze.Field[position % size - 1, position / size] -= 1;

                    //Set new base position
                    position = maze.Stack.Peek();
                }
            }

            //Find longest path
            List<List<int>> pathList = new List<List<int>>();
            for (int i = 0; i < size; i++)
            {
                pathList.Add(new List<int>() { i });
            }
            while (pathList.Where(u => u.Last() == -1).ToList().Count != pathList.Count)
            {
                List<List<int>> pathList2 = new List<List<int>>();
                foreach (List<int> path in pathList)
                {
                    int pos = path.Last();
                    if (pos != -1)
                    {
                        List<int> posMoves = new List<int>();
                        if (pos / size > 0 && !path.Contains(pos - size) && maze.Field[pos % size, pos / size - 1] < 2)
                            posMoves.Add(pos - size);
                        if (pos % size > 0 && !path.Contains(pos - 1) && maze.Field[pos % size - 1, pos / size] % 2 == 0)
                            posMoves.Add(pos - 1);
                        if (pos % size < size - 1 && !path.Contains(pos + 1) && maze.Field[pos % size, pos / size] % 2 == 0)
                            posMoves.Add(pos + 1);
                        if (pos / size < size - 1 && !path.Contains(pos + size) && maze.Field[pos % size, pos / size] < 2)
                            posMoves.Add(pos + size);

                        foreach (int move in posMoves)
                        {
                            List<int> pth = new List<int>();
                            pth = path.ToList();
                            pth.Add(move);
                            pathList2.Add(pth);
                        }
                        //Add -1 at the end of the path - to mark it as finished
                        if (posMoves.Count == 0)
                        {
                            List<int> pth = new List<int>();
                            pth = path.ToList();
                            pth.Add(-1);
                            pathList2.Add(pth);
                        }
                    }
                    else
                    {
                        pathList2.Add(path);
                    }
                }
                pathList = pathList2.ToList();
            }
            pathList.ForEach(u => u.Remove(-1));

            int startpos = 0;
            int tempStartpos = 0;
            int maxLength = 0;
            List<int> longestPath = new List<int>();
            foreach (List<int> path in pathList)
            {
                int length = 0;
                int tempLength = 0;
                foreach (int a in path)
                {
                    tempLength++;
                    if (a >= size * (size - 1))
                    {
                        length += tempLength;
                        tempLength = 0;
                        tempStartpos = a;
                    }
                }
                if (length > maxLength)
                {
                    maxLength = length;
                    longestPath = path;
                    startpos = tempStartpos;
                }
            }
            maze.Endpos = longestPath.First();
            maze.Startpos = startpos % size;

            int delete = -1;
            foreach (int a in longestPath)
            {
                if (a == startpos)
                {
                    delete = longestPath.IndexOf(a);
                }
            }
            longestPath.RemoveRange(delete + 1, longestPath.Count - delete - 1);

            //Mark every tile which is in longestPath as -1
            foreach (List<int> path in pathList.Where(u => u != longestPath))
            {
                for (int i = 0; i < path.Count; i++)
                {
                    if (longestPath.Contains(path[i]))
                        path[i] = -1;
                }
            }
            //Remove every path with no connection to the longestPath
            pathList = pathList.Where(u => u.Contains(-1)).ToList();

            List<List<int>> secondaryPathList = new List<List<int>>();
            foreach (List<int> path in pathList)
            {
                bool createNew = true;
                if (path != longestPath)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        if (path[i] == -1)
                            createNew = true;
                        else if (path[i] != -1 && !createNew)
                            secondaryPathList.Last().Add(path[i]);
                        else if (path[i] != -1 && createNew)
                        {
                            secondaryPathList.Add(new List<int>() { path[i] });
                            createNew = false;
                        }
                    }
                }
            }

            if (secondaryPathList.Count > 0)
            {
                List<int> longestSecondaryPath = secondaryPathList.First(u => u.Count == secondaryPathList.Max(z => z.Count));
                if (longestSecondaryPath.Count == 1)
                {
                    maze.TreasureX = longestSecondaryPath.First() % maze.Size;
                    maze.TreasureY = longestSecondaryPath.First() / maze.Size;
                }
                else
                {
                    List<int> accessibleFields = new List<int>();
                    foreach (int a in longestPath)
                    {
                        if (a / size > 0 && !longestPath.Contains(a - size) && maze.Field[a % size, a / size - 1] < 2)
                            accessibleFields.Add(a - size);
                        if (a % size > 0 && !longestPath.Contains(a - 1) && maze.Field[a % size - 1, a / size] % 2 == 0)
                            accessibleFields.Add(a - 1);
                        if (a % size < size - 1 && !longestPath.Contains(a + 1) && maze.Field[a % size, a / size] % 2 == 0)
                            accessibleFields.Add(a + 1);
                        if (a / size < size - 1 && !longestPath.Contains(a + size) && maze.Field[a % size, a / size] < 2)
                            accessibleFields.Add(a + size);
                    }
                    if (accessibleFields.Contains(longestSecondaryPath.First()))
                    {
                        maze.TreasureX = longestSecondaryPath.Last() % maze.Size;
                        maze.TreasureY = longestSecondaryPath.Last() / maze.Size;
                    }
                    else
                    {
                        maze.TreasureX = longestSecondaryPath.First() % maze.Size;
                        maze.TreasureY = longestSecondaryPath.First() / maze.Size;
                    }
                }
            }
            //If longestPath is going through every tile of maze
            else
            {
                maze.TreasureX = (maze.Endpos + maze.Startpos) / 2;
                maze.TreasureY = size / 2 - 1;
            }

            maze.Stack = null;
            return maze;
        }
    }
}
