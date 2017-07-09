using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Random static initialiser
    /// </summary>
    public static class GlobalRandom
    {
        /// <summary>
        /// Random class, initialised once
        /// </summary>
        public static System.Random R = new System.Random();

        /// <summary>
        /// Extension method that returns a random element of a list using the Global Random
        /// generator
        /// </summary>
        /// <typeparam name="T">Type in list</typeparam>
        /// <param name="source">List to select from</param>
        /// <returns>Random T from list</returns>
        public static T RandomItem<T>(this IList<T> source)
        {
            return source[R.Next(source.Count)];
        }
    }
}
