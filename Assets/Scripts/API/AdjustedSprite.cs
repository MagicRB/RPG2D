using UnityEngine;

namespace RPG2D.API
{
    public static class AdjustedSprite
    {
        /// <summary>
        /// Creates a new Adjusted Sprite, the syntax is exactly the same as for the normal UnityEngine Sprite.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="rect"></param>
        /// <param name="pivot"></param>
        /// <param name="ppu"></param>
        /// <returns></returns>
        public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float ppu)
        {
            Rect adjustedRect = new Rect(rect.xMin * texture.width, rect.yMin * texture.height, rect.xMax * texture.width - rect.xMin * texture.width, rect.yMax * texture.height - rect.yMin * texture.height);
            
            return Sprite.Create(texture, adjustedRect, pivot, ppu);
        }
    }
}