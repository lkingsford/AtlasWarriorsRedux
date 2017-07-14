using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Random static class, with facilities to insert in values for tests
    /// </summary>
    public static class GlobalRandom
    {
        /// <summary>
        /// Random class, initialised once
        /// </summary>
        private static System.Random R = new System.Random();

        /// <summary>
        /// Extension method that returns a random element of a list using the Global Random
        /// generator
        /// </summary>
        /// <typeparam name="T">Type in list</typeparam>
        /// <param name="source">List to select from</param>
        /// <returns>Random T from list</returns>
        public static T RandomItem<T>(this IList<T> source)
        {
            return source[Next(source.Count)];
        }

        /// <summary>
        /// Return random (or seeded) integer min LTE result LT max
        /// </summary>
        /// <param name="min">Min allowed value</param>
        /// <param name="max">Max allowed value</param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            // Returning from Next so we can use a seeded value if there
            return min + Next(max);
        }

        /// <summary>
        /// Values to inject instead of real values
        /// </summary>
        private static Stack<int> SeededValues = new Stack<int>();

        /// <summary>
        /// Return random (or seeded) integer LT max
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int max)
        {
            // Using Next  so we can use a seeded value if there
            return R.Next() % max;
        }

        /// <summary>
        /// Return random (or seeded) integer
        /// If there's something in the values - return it instead
        /// </summary>
        /// <returns>Random value</returns>
        public static int Next()
        {
            if (SeededValues.Count == 0)
            {
                return R.Next();
            }
            else
            {
                return SeededValues.Pop();
            }
        }

        /// <summary>
        /// Put a value on to the stack of values to return instead of actual random numbers
        /// Used for testing
        /// </summary>
        /// <param name="value"></param>
        public static void Inject(int value)
        {
            SeededValues.Push(value);
        }
    }
}
