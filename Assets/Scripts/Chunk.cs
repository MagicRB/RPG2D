using System.Collections.Generic;
using System;
using RPG2D.BaseClasses;
using UnityEngine;

namespace RPG2D
{
	[Serializable]
	public class Chunk
	{
		private Dictionary<Vector3Int, RPG2D.BaseClasses.Block> _blocks = new Dictionary<Vector3Int, RPG2D.BaseClasses.Block>();
		
		public Dictionary<Vector3Int, RPG2D.BaseClasses.Block> Blocks
		{
			get { return _blocks; }
		}
		
		public void SetBlockAt(Vector3Int position, BaseClasses.Block block)
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
		
		public BaseClasses.Block GetBlockAt(Vector3Int position)
		{
			if (!_blocks.ContainsKey(API.World.PositionToPositionInChunk(position)))
				return new Air();
			return _blocks[API.World.PositionToPositionInChunk(position)];
		}
	}
}

