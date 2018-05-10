using System;
using UnityEngine;

namespace RPG2D.BaseClasses
{
    [Serializable]
    public abstract class Block
    {
		public Vector3Int Position;

        public string InternalName;

		public abstract bool Init (Vector3Int position);
        public abstract bool Init ();

        public abstract void BlockUpdate(Block cause);

        public abstract Block Copy();

        public abstract void Destroy();
    }
}