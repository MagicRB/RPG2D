using System.Collections.Generic;
using System;
using UnityEngine;

namespace RPG2D
{
	public class Chunk
	{
		private Dictionary<Vector2Int, RPG2D.BaseClasses.Block> _blocks = new Dictionary<Vector2Int, RPG2D.BaseClasses.Block>();

		public BaseClasses.Block GetBlockAt(Vector2Int position)
		{
			return _blocks[API.World.PositionToPositionInChunk (position)];
		}
	}
}

