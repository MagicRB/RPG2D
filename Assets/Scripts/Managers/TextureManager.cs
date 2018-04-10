using System.Collections.Generic;
using UnityEngine;

namespace RPG2D.Managers
{
    public static class TextureManager
    {
        private static Dictionary<string, TextureAtlas> _textureDictionary = new Dictionary<string, TextureAtlas>();

        public static Dictionary<string, TextureAtlas> TextureDictionary
        {
            get { return _textureDictionary; }
        }

        public static TextureAtlas CreateNewTextureAtlas(string name)
        {
            _textureDictionary[name] = new TextureAtlas();
            return _textureDictionary[name];
        }

        public static TextureAtlas CreateNewTextureAtlas(string name, TextureAtlas textureAtlas)
        {
            _textureDictionary[name] = textureAtlas;
            return _textureDictionary[name];
        }

        public static void PackAllTextures()
        {
            foreach (var textureAtlas in _textureDictionary)
            {
                textureAtlas.Value.Pack();
            }
        }
    }
}