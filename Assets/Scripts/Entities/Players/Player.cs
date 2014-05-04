using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity
{
		public Player ()
		{
				sprite = new FSprite ("Futile_White");
				sprite.width = 24;
				sprite.height = 24;
				Size = new Vector2 (24, 24);
				ListenForUpdate (Update);
				Gravity = 9.8f;
                JumpImpulse = 6f;
                MaxSpeed = new Vector2(100f, Gravity);
				useGravity = true;
				AddChild (sprite);
                Position = new Vector2(TileMap.tileSize*3,-TileMap.tileSize*3);
		}
		
}