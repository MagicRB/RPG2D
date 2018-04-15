using System.Collections.Generic;
using UnityEngine;

namespace RPG2D
{
    public struct SaveGame
    {
        public List<string> SaveName;
        
        public static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
    }
}