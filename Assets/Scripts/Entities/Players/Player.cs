using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity
{
		public Player ()
		{
				sprite = new FSprite ("Futile_White");
				sprite.width = 10;
				sprite.height = 12;
                Size = new Vector2(sprite.width, sprite.height);
				ListenForUpdate (Update);
				Gravity = 9.8f;
                JumpImpulse = 15f;
                MaxSpeed = new Vector2(100f, Gravity);
				useGravity = true;
				AddChild (sprite);
                Position = new Vector2(TileMap.tileSize*3,-TileMap.tileSize*3);
		}
}