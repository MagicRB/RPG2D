using UnityEngine;

namespace RPG2D.API
{
    public static class RandomHelperFunctions
    {
        /// <summary>
        /// Converts a Boolean Array to a byte, the Bool array can't be bigger then 8. {source https://stackoverflow.com/questions/24322417/how-to-convert-bool-array-in-one-byte-and-later-convert-back-in-bool-array}
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Byte.</returns>
        public static byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }
            return result;
        }
    }
}