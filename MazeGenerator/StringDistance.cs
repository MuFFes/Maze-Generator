using System;

namespace MazeGenerator
{
    public static class StringDistance
    {
        /// Compute the distance between two strings.
        public static int Compute(string s, string t)
        {
            int distance = 0;

            for (int i = 0; i < s.Length; i++)
                if (s[i] != t[i])
                    distance++;

            return distance;
        }
    }
}