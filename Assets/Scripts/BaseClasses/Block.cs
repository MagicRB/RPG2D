using UnityEngine;

namespace RPG2D.BaseClasses
{
    public abstract class Block
    {
		public Vector2Int position;

		public abstract bool Init ();

        public abstract Block Copy();
    }
}