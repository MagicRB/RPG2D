
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RPG2D.API;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

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

        public FilterMode FilterMode;

        /// <summary>
        /// Adds a new texture into the atlas, the identifier is the name.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="name"></param>
        public void AddTexture(Texture2D texture, string name)
        {
            _textures[name] = texture;
            _textureIDs[name] = _textures.Count - 1;
        }
        
        /// <summary>
        /// Adds a new texture into the atlas from file, the identifier is the name. 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
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
        
        /// <summary>
        /// Adds a new texture into the atlas from a resource, the identifier is the name. 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resource"></param>
        /// <param name="name"></param>
        public void AddTexture(Assembly assembly, string resource, string name)
        {
            Texture2D texture = null;
            byte[] resourceData;

            string formatedResourceName = assembly.GetName().Name + "." + resource.Replace(" ", "_")
                                                                                  .Replace("\\", ".")
                                                                                  .Replace("/", ".");
            
            using (Stream resourceStream = assembly.GetManifestResourceStream(formatedResourceName))
            {
                if (resourceStream == null) {
                    Debug.LogError("Texture not found as resource, name: " + formatedResourceName);
                    return;
                }
                
                resourceData = new byte[resourceStream.Length];
                
                resourceStream.Read(resourceData, 0, resourceData.Length);
                
                texture = new Texture2D(2, 2);
                texture.LoadImage(resourceData); //..this will auto-resize the texture dimensions.

                _textures[name] = texture;
                _textureIDs[name] = _textures.Count - 1;
            }
        }

        
        /// <summary>
        /// Gets the rect which is associated with texture name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Rect GetRect(string name)
        {
			return _rects[_textureIDs[name]];
        }

        /// <summary>
        /// Packages all textures.
        /// </summary>
        public void Pack()
        {
            _texture = new Texture2D(2, 2);
            _rects = _texture.PackTextures(_textures.Values.ToArray(), 0, 2048);
            _texture.filterMode = FilterMode;
        }
    }
}