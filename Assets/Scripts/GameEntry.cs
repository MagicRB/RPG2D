﻿using System.Collections;
using System.Collections.Generic;
using RPG2D.API;
using RPG2D.Managers;
using UnityEngine;

namespace RPG2D {
	public class GameEntry : MonoBehaviour
	{
		// Use this for initialization
		void Start ()
		{
			Debug.Log(RandomMath.NextPowerOf2(3));
			
			RPG2D.Managers.ModManager.LoadMods();
			
			TextureManager.PackAllTextures();

			Registers.BlockRegister.GetRegisteredBlockInstance("TestMod-Wall");
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}