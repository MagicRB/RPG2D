using System;
using UnityEngine;

namespace RPG2D.BaseClasses
{
    [Serializable]
    public abstract class Block
    {
		public Vector2Int Position;

        public string InternalName;

		public abstract bool Init (Vector2Int position);
        public abstract bool Init ();

        public abstract Block Copy();

        public abstract void Destroy();
    }
}