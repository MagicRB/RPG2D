using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RPG2D.API;
using RPG2D.BaseClasses;
using RPG2D.Managers;
using RPG2D.Registers;
using UnityEngine;

namespace RPG2D {
	public class GameEntry : MonoBehaviour
	{
		// Use this for initialization
		void Start ()
		{
			ModManager.LoadMods();
			
			TextureManager.PackAllTextures();
			
			SaveManager.Load();

			//BaseClasses.Player player = (BaseClasses.Player)Activator.CreateInstance(PlayerRegister.PlayerType);
			//PlayerRegister.Player = player;
			//player.Init();

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
				World.SpawnBlock(new Vector3Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y), 0), Registers.BlockRegister.GetRegisteredBlockType ("TestMod-Wall"));
			}
			
			if (Input.GetMouseButton(1))
			{
				Vector3 mousePos = PlayerRegister.Player.GetCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
				World.SpawnBlock(new Vector3Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y), 0), typeof(Air));
			}

			if (Input.GetKeyDown(KeyCode.H))
			{
				Vector3 mousePos = PlayerRegister.Player.GetCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
				World.SpawnBlock(new Vector3Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y), 1), Registers.BlockRegister.GetRegisteredBlockType ("TestMod-SandstoneFloor"));
			}
			
			if (Input.GetKeyDown(KeyCode.Y))
			{
				Vector3 mousePos = PlayerRegister.Player.GetCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
				World.SpawnBlock(new Vector3Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y), 1), Registers.BlockRegister.GetRegisteredBlockType ("TestMod-SpawnFloor"));
			}
			
			if (Input.GetKeyDown(KeyCode.J))
			{
				Vector3 mousePos = PlayerRegister.Player.GetCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
				World.SpawnBlock(new Vector3Int((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y), 1), typeof(Air));
			}
			
			if (Input.GetKeyDown(KeyCode.K))
			{
				Vector3 mousePos = PlayerRegister.Player.GetCamera().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
				World.SpawnEntity(new Vector2((int)Math.Round(mousePos.x), (int)Math.Round(mousePos.y)),  Registers.EntityRegister.GetRegisteredEntityType("TestMod-TestBox"));
			}
		}
	}
}