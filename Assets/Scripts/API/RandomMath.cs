using System;

namespace RPG2D.API
{
    public static class RandomMath
    {
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