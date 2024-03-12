using System;

namespace HotUpdate.Utils
{
    public static partial class Utility
    {
        public static class Utility_Random
        {
            private static Random s_Random = new Random((int)DateTime.UtcNow.Ticks);

            public static void SetRandomSeed(int seed)
            {
                s_Random = new Random(seed);
            }

            public static int GetOneRandom()
            {
                return s_Random.Next();
            }

            public static int GetIntervalRandom(int minValue, int maxValue)
            {
                return s_Random.Next(minValue, maxValue);
            }

            public static double GetDoubleRandom()
            {
                return s_Random.NextDouble();
            }

            public static void GetBytesRandom(byte[] bytes)
            {
                s_Random.NextBytes(bytes);
            }
        }
    }

}