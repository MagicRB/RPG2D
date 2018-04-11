using System.Collections.Generic;
using System;
using UnityEngine;

namespace RPG2D.API
{
    public static class World
    {
		private static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();

		public Vector2Int PositionToChunk(Vector2Int position)
		{
			return new Vector2Int (Math.Floor(position.x / 64.0f), Math.Floor(position.y / 64.0f));
		}

		public Vector2Int PositionToPositionInChunk(Vector2Int position)
		{
			return new Vector2Int (position.x % 64, position.y % 64);
		}

		public static void SpawnBlock(Vector2Int position, System.Type blockType)
		{
			/*_blocks [position] = (BaseClasses.Block)Activator.CreateInstance (blockType);
			_blocks [position].Init ();*/

			_chunks [World.PositionToChunk (position)].GetBlockAt (position) = (BaseClasses.Block)Activator.CreateInstance (blockType);
			_chunks [World.PositionToChunk (position)].GetBlockAt (position).Init ();
		}

		public static void SpawnBlock(Vector2Int position, BaseClasses.Block block)
		{
			/*_blocks [position] = (BaseClasses.Block)Activator.CreateInstance (block.GetType());
			_blocks [position].Init ();*/

			_chunks [World.PositionToChunk (position)].GetBlockAt (position) = (BaseClasses.Block)Activator.CreateInstance (block.GetType());
			_chunks [World.PositionToChunk (position)].GetBlockAt (position).Init ();
		}

		public static BaseClasses.Block GetBlock(Vector2Int position)
		{
			return _chunks [World.PositionToChunk (position)].GetBlockAt (position);
		}
		public static System.Type GetBlockType(Vector2Int position)
		{
			return _chunks [World.PositionToChunk (position)].GetBlockAt (position).GetType ();
		}
    }
}