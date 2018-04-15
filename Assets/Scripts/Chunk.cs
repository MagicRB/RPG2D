using System.Collections.Generic;
using System;
using RPG2D.BaseClasses;
using UnityEngine;

namespace RPG2D
{
	[Serializable]
	public class Chunk
	{
		private Dictionary<Vector2Int, RPG2D.BaseClasses.Block> _blocks = new Dictionary<Vector2Int, RPG2D.BaseClasses.Block>();

		public Dictionary<Vector2Int, RPG2D.BaseClasses.Block> Blocks
		{
			get { return _blocks; }
		}
		
		public void SetBlockAt(Vector2Int position, BaseClasses.Block block)
		{
			_blocks[API.World.PositionToPositionInChunk(position)] = block;
		}

		public List<BaseClasses.Block> ReturnAllBlocks()
		{
			List<BaseClasses.Block> blocks = new List<Block>();
			foreach (var block in _blocks)
			{
				blocks.Add(block.Value);
			}

			return blocks;
		}
		
		public BaseClasses.Block GetBlockAt(Vector2Int position)
		{
			if (!_blocks.ContainsKey(API.World.PositionToPositionInChunk(position)))
				return new Air();
			return _blocks[API.World.PositionToPositionInChunk(position)];
		}
	}
}

