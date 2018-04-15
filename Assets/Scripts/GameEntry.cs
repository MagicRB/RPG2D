using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RPG2D.API;
using RPG2D.Managers;
using RPG2D.Registers;
using UnityEngine;

namespace RPG2D {
	public class GameEntry : MonoBehaviour
	{
		// Use this for initialization
		void Start ()
		{
			Debug.Log(RandomMath.NextPowerOf2(3));
			
			ModManager.LoadMods();
			
			TextureManager.PackAllTextures();

			SaveManager.Load();
			
			/*World.SpawnBlock (new Vector2Int (-1, -1), Registers.BlockRegister.GetRegisteredBlockType ("TestMod-Wall"));
			World.SpawnBlock (new Vector2Int (0, -1), Registers.BlockRegister.GetRegisteredBlockType ("TestMod-Wall"));
			World.SpawnBlock (new Vector2Int (1, -1), Registers.BlockRegister.GetRegisteredBlockType ("TestMod-Wall"));*/
		}
		
		// Update is called once per frame
		void Update () {
			if (PlayerRegister.Player != null)
			{
				PlayerRegister.Player.Update();
			}

			if (Input.GetMouseButton(0))
			{
				Vector3 mousePos = PlayerRegister.Player.GetCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
				Debug.Log(mousePos);
				Debug.Log(new Vector2Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y)));
				World.SpawnBlock(new Vector2Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y)), Registers.BlockRegister.GetRegisteredBlockType ("TestMod-Wall"));
			}
			
			if (Input.GetMouseButton(1))
			{
				Vector3 mousePos = PlayerRegister.Player.GetCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
				Debug.Log(mousePos);
				Debug.Log(new Vector2Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y)));
				World.SpawnBlock(new Vector2Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y)), typeof(Air));
			}
		}
	}
}