using System.Collections.Generic;
using System;
using System.Linq;
using Mono.CSharp;
using RPG2D.BaseClasses;
using UnityEngine;
using Block = RPG2D.BaseClasses.Block;

namespace RPG2D.API
{
	[Serializable]
    public static class World
    {
		private static Dictionary<Vector3Int, Chunk> _chunks = new Dictionary<Vector3Int, Chunk>();

	    private static List<Entity> _entities = new List<Entity>();

	    public static Dictionary<Vector3Int, Chunk> Chunks
	    {
		    get { return _chunks; }
	    }

	    public static List<Entity> Entities
	    {
		    get { return _entities; }
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
		public static Vector3Int PositionToChunk(Vector3Int position)
		{
			return new Vector3Int ((int)Math.Floor(position.x / 64.0f), (int)Math.Floor(position.y / 64.0f), position.z);
		}

	    /// <summary>
	    /// Converts World space position to the local position inside the chunk.
	    /// </summary>
	    /// <param name="position"></param>
	    /// <returns>Position inside the chunk.</returns>
		public static Vector3Int PositionToPositionInChunk(Vector3Int position)
		{
			return new Vector3Int (position.x % 64, position.y % 64, position.z);
		}

	    /// <summary>
	    ///	Adds a new chunk.
	    /// </summary>
	    /// <param name="position"></param>
	    public static void AddChunk(Vector3Int position)
	    {
		    _chunks[position] = new Chunk();
	    }

	    public static BaseClasses.Block[] AdjecentBlocks(Vector3Int position)
	    {
		    BaseClasses.Block[] blocks = new BaseClasses.Block[8];

		    blocks[0] = GetBlock(position + new Vector3Int(-1, 1, 0));
		    blocks[1] = GetBlock(position + new Vector3Int(0, 1, 0));
		    blocks[2] = GetBlock(position + new Vector3Int(1, 1, 0));
		    blocks[3] = GetBlock(position + new Vector3Int(-1, 0, 0));
		    blocks[4] = GetBlock(position + new Vector3Int(1, 0, 0));
		    blocks[5] = GetBlock(position + new Vector3Int(-1, -1, 0));
		    blocks[6] = GetBlock(position + new Vector3Int(0, -1, 0));
		    blocks[7] = GetBlock(position + new Vector3Int(1, -1, 0));

		    return blocks;
	    }

	    public static void IssueBlockUpdate(Block cause)
	    {
		    Vector3Int pos = cause.Position;

		    World.GetBlock(new Vector3Int(pos.x - 1, pos.y, pos.z)).BlockUpdate(cause);
		    World.GetBlock(new Vector3Int(pos.x + 1, pos.y, pos.z)).BlockUpdate(cause);
		    World.GetBlock(new Vector3Int(pos.x, pos.y - 1, pos.z)).BlockUpdate(cause);
		    World.GetBlock(new Vector3Int(pos.x, pos.y + 1, pos.z)).BlockUpdate(cause);
	    }

	    public static byte CalculateAdjacent(Vector3Int position, System.Type blockType)
	    {
		    BaseClasses.Block[] blocks = AdjecentBlocks(position);
		    bool[] bools = new bool[8];
		    int i = 0;
		    
		    foreach (var block in blocks)
		    {
			    bools[i] = block.GetType() == blockType; 
			    i++;
		    }

		    bools[0] = false;
		    bools[2] = false;
		    bools[5] = false;
		    bools[7] = false;

		    return API.RandomHelperFunctions.ConvertBoolArrayToByte(bools);
	    }
	    
	    public static byte CalculateAdjacentStraight(Vector3Int position, System.Type blockType)
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
		public static void SpawnBlock(Vector3Int position, System.Type blockType)
		{
			if (!_chunks.ContainsKey(World.PositionToChunk(position)))
				AddChunk(World.PositionToChunk(position));
			if (_chunks[World.PositionToChunk(position)].GetBlockAt(position).GetType() != typeof(Air))
				_chunks[World.PositionToChunk(position)].GetBlockAt(position).Destroy();
				
			_chunks[World.PositionToChunk(position)].SetBlockAt(position, (BaseClasses.Block) Activator.CreateInstance(blockType));
			_chunks[World.PositionToChunk(position)].GetBlockAt(position).Init(position);
		}
	    
	    public static void SetBlock(Vector3Int position, BaseClasses.Block block)
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
		public static void SpawnBlock(Vector3Int position, BaseClasses.Block block)
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
		public static BaseClasses.Block GetBlock(Vector3Int position)
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
		public static System.Type GetBlockType(Vector3Int position)
		{
			if (!_chunks.ContainsKey(World.PositionToChunk(position)))
				return typeof(Air);
			return _chunks [World.PositionToChunk (position)].GetBlockAt (position).GetType ();
		}

	    public static Entity SpawnEntity(Vector2 position, System.Type entityType)
	    {
		    _entities.Add((Entity)Activator.CreateInstance(entityType));
		    _entities.Last().Init(position);
		    return _entities.Last();
	    }
	    
	    public static Entity SpawnEntity(Vector2 position, Entity entity)
	    {
		    _entities.Add(entity.Copy());
		    _entities.Last().Init(position);
		    return _entities.Last();
	    }
    }
}