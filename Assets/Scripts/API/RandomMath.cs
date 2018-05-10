using System;
using System.IO;

namespace RPG2D.API
{
    public static class RandomMath
    {
        /// <summary>
        /// Calculates the next power of two, the power of two is always bigger then n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Next power of two</returns>
        public static int NextPowerOf2(int n)
        {
            if (n > 0)
            {
                return (int)Math.Pow(2, Math.Ceiling(Math.Log(n) / Math.Log(2)));
            }
            if (n < 0)
            {
                return (int)Math.Pow(2, Math.Ceiling(Math.Log(n) / Math.Log(2))) * -1;
            }
            
            return 0;
        }
    }
}