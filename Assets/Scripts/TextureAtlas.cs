
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RPG2D.API;
using UnityEngine;

namespace RPG2D
{
    public class TextureAtlas
    {
        private Texture2D _texture;
        private Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private Dictionary<string, int> _textureIDs = new Dictionary<string, int>();
        private Rect[] _rects;

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public void AddTexture(Texture2D texture, string name)
        {
            _textures[name] = texture;
            _textureIDs[name] = _textures.Count - 1;
        }
        
        public void AddTexture(string file, string name)
        {
            Texture2D texture = null;
            byte[] fileData;

            if (File.Exists(file))
            {
                fileData = File.ReadAllBytes(file);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                _textures[name] = texture;
                _textureIDs[name] = _textures.Count - 1;
            }
            else
            {
                Debug.LogError("Texture not found on disk, path: " + file);
            }
        }
        
        public Rect GetRect(string name)
        {
            return _rects[0];
        }

        public void Pack()
        {
            int width = 0;
            int height = 0;
            
            int size = 0;
            
            foreach (var texture in _textures)
            {
                width += texture.Value.width;
                height += texture.Value.height;
            }

            if (width <= height)
            {
                size = RandomMath.NextPowerOf2(height);
            }
            else if (height < width)
            {
                size = RandomMath.NextPowerOf2(width);
            }
            
            Debug.Log("TextureAtlas params: " + width + ";" + height + ";" + size + ";");
            
            _texture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
            _rects = _texture.PackTextures(_textures.Values.ToArray(), 0, 32);
        }
    }
}