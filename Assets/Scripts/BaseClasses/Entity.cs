using System;
using UnityEngine;

namespace RPG2D.BaseClasses
{
    public abstract class Entity
    {
        public Vector2 Position;

        public string InternalName;

        public abstract bool Init (Vector2 position);
        public abstract bool Init ();
        
        public abstract Entity Copy();

        public abstract void Destroy();
    }
}