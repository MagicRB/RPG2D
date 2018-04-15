using System.Collections.Generic;
using System;
using Mono.CSharp;
using UnityEngine;
using Block = RPG2D.BaseClasses.Block;

namespace RPG2D.API
{
	[Serializable]
    public static class World
    {
		private static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();

	    public static Dictionary<Vector2Int, Chunk> Chunks
	    {
		    get { return _chunks; }
	    }
	    
	    public static List<BaseClasses.Block> ReturnAllBlocks()
	    {
		    List<BaseClasses.Block> blocks = new List<Block>();

		    foreach (var chunk in _chunks)
		    {
			    blocks.AddRange(chunk.Value.ReturnAllBlocks());
		    }

		    return blocks;
	    }
	    
		/// <summary>
		/// Converts World space position to chunk position, so like the chunk number.
		/// </summary>
		/// <param name="position"></param>
		/// <returns>Returns the chunk position.</returns>
		public static Vector2Int PositionToChunk(Vector2Int position)
		{
			return new Vector2Int ((int)Math.Floor(position.x / 64.0f), (int)Math.Floor(position.y / 64.0f));
		}

	    /// <summary>
	    /// Converts World space position to the local position inside the chunk.
	    /// </summary>
	    /// <param name="position"></param>
	    /// <returns>Position inside the chunk.</returns>
		public static Vector2Int PositionToPositionInChunk(Vector2Int position)
		{
			return new Vector2Int (position.x % 64, position.y % 64);
		}

	    /// <summary>
	    ///	Adds a new chunk.
	    /// </summary>
	    /// <param name="position"></param>
	    public static void AddChunk(Vector2Int position)
	    {
		    _chunks[position] = new Chunk();
	    }

	    public static BaseClasses.Block[] AdjecentBlocks(Vector2Int position)
	    {
		    BaseClasses.Block[] blocks = new BaseClasses.Block[8];

		    blocks[0] = GetBlock(position + new Vector2Int(-1, 1));
		    blocks[1] = GetBlock(position + new Vector2Int(0, 1));
		    blocks[2] = GetBlock(position + new Vector2Int(1, 1));
		    blocks[3] = GetBlock(position + new Vector2Int(-1, 0));
		    blocks[4] = GetBlock(position + new Vector2Int(1, 0));
		    blocks[5] = GetBlock(position + new Vector2Int(-1, -1));
		    blocks[6] = GetBlock(position + new Vector2Int(0, -1));
		    blocks[7] = GetBlock(position + new Vector2Int(1, -1));

		    return blocks;
	    }

	    public static byte CalculateAdjacent(Vector2Int position, System.Type blockType)
	    {
		    BaseClasses.Block[] blocks = AdjecentBlocks(position);
		    bool[] bools = new bool[8];
		    int i = 0;
		    
		    foreach (var block in blocks)
		    {
			    bools[i] = block.GetType() == blockType; 
			    i++;
		    }

		    return API.RandomHelperFunctions.ConvertBoolArrayToByte(bools);
	    }
	    
	    public static byte CalculateAdjacentStraight(Vector2Int position, System.Type blockType)
	    {
		    BaseClasses.Block[] blocks = AdjecentBlocks(position);
		    bool[] bools = new bool[8];
		    int i = 0;

		    bools[0] = false;
		    bools[2] = false;
		    bools[5] = false;
		    bools[7] = false;
		    
		    foreach (var block in blocks)
		    {
			    bools[i] = block.GetType() == blockType; 
			    i++;
		    }

		    return API.RandomHelperFunctions.ConvertBoolArrayToByte(bools);
	    }
	    
	    /// <summary>
	    /// Spawns a block at the position.
	    /// </summary>
	    /// <param name="position"></param>
	    /// <param name="blockType"></param>
		public static void SpawnBlock(Vector2Int position, System.Type blockType)
		{
			if (!_chunks.ContainsKey(World.PositionToChunk(position)))
				AddChunk(World.PositionToChunk(position));
			if (_chunks[World.PositionToChunk(position)].GetBlockAt(position).GetType() != typeof(Air))
				_chunks[World.PositionToChunk(position)].GetBlockAt(position).Destroy();;
				
			_chunks[World.PositionToChunk(position)].SetBlockAt(position, (BaseClasses.Block) Activator.CreateInstance(blockType));
			_chunks[World.PositionToChunk(position)].GetBlockAt(position).Init(position);
		}
	    
	    public static void SetBlock(Vector2Int position, BaseClasses.Block block)
	    {
		    if (!_chunks.ContainsKey(World.PositionToChunk(position)))
			    AddChunk(World.PositionToChunk(position));
		    if (_chunks[World.PositionToChunk(position)].GetBlockAt(position).GetType() != typeof(Air))
			    _chunks[World.PositionToChunk(position)].GetBlockAt(position).Destroy();
				
		    _chunks[World.PositionToChunk(position)].SetBlockAt(position, block);
		    _chunks[World.PositionToChunk(position)].GetBlockAt(position).Init(position);
	    }
		
	    /// <summary>
	    /// Spawns a block at the position.  (Creates a new instance)
	    /// </summary>
	    /// <param name="position"></param>
	    /// <param name="block"></param>
		public static void SpawnBlock(Vector2Int position, BaseClasses.Block block)
		{
			if (!_chunks.ContainsKey(World.PositionToChunk(position)))
				AddChunk(World.PositionToChunk(position));
			if (_chunks[World.PositionToChunk(position)].GetBlockAt(position).GetType() != typeof(Air))
				_chunks[World.PositionToChunk(position)].GetBlockAt(position).Destroy();
			
			_chunks[World.PositionToChunk(position)].SetBlockAt(position, (BaseClasses.Block) Activator.CreateInstance(block.GetType()));
			_chunks[World.PositionToChunk(position)].GetBlockAt(position).Init(position);
		}

	    /// <summary>
	    /// Gets the block instance at the specified position
	    /// </summary>
	    /// <param name="position"></param>
	    /// <returns>Block instance</returns>
		public static BaseClasses.Block GetBlock(Vector2Int position)
	    {
		    if (!_chunks.ContainsKey(World.PositionToChunk(position)))
			    return new Air();
			return _chunks [World.PositionToChunk (position)].GetBlockAt (position);
		}
	    
	    /// <summary>
	    /// Gets the block type at the specified position
	    /// </summary>
	    /// <param name="position"></param>
	    /// <returns>Block type</returns>
		public static System.Type GetBlockType(Vector2Int position)
		{
			if (!_chunks.ContainsKey(World.PositionToChunk(position)))
				return typeof(Air);
			return _chunks [World.PositionToChunk (position)].GetBlockAt (position).GetType ();
		}
    }
}