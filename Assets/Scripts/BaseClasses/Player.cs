using System;
using UnityEngine;

namespace RPG2D.BaseClasses
{
    [Serializable]
    public abstract class Player
    {
        public string InternalName;

        public Vector2 Position;
        
        public abstract void Init();

        public abstract UnityEngine.Camera GetCamera();
        
        public abstract void Update();
    }
}