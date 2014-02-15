//
// Entity.cs
//
// Author:
//       Rik <>
//
// Copyright (c) 2014 Rik
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity :FContainer
{
	public Vector2 position;

	public Vector2 velocity;

	public float gravity;

	public float jumpImpulse;

	public CollisionDetection collisionDetection;

		public bool onGround 		{ get; set; }

		public bool canJump			{ get; set; }

		public bool useGravity		{ get; set; }

		public bool noClip			{ get; set; }

		public FSprite sprite		{ get; set; }
		
		public Entity ()
		{
					
		}

		public virtual void doCollisionCheck ()
		{
				if (noClip)
						return;
			
				
		}
				
		public virtual void applyGravity ()
		{
				if (!useGravity)
						return;

				velocity.y -= gravity * Time.deltaTime;
		}

		public virtual void doJump ()
		{
				velocity.y += jumpImpulse * Time.deltaTime;
		}



		// this code is by mylesmar10
		public void drawHitBox (FSprite sprite, Color color)
		{
				if (sprite.GetTextureRectRelativeToContainer () != null) {
			
						Vector3 tL = new Vector3 (0f, 0f, 0f);
						Vector3 tR = new Vector3 (0f, 0f, 0f);
						Vector3 bL = new Vector3 (0f, 0f, 0f);
						Vector3 bR = new Vector3 (0f, 0f, 0f);
			
						tL.x = sprite.GetTextureRectRelativeToContainer ().xMin;
						tL.y = sprite.GetTextureRectRelativeToContainer ().yMin;
			
						tR.x = sprite.GetTextureRectRelativeToContainer ().xMax;
						tR.y = sprite.GetTextureRectRelativeToContainer ().yMin;
			
						bL.x = sprite.GetTextureRectRelativeToContainer ().xMin;
						bL.y = sprite.GetTextureRectRelativeToContainer ().yMax;
			
						bR.x = sprite.GetTextureRectRelativeToContainer ().xMax;
						bR.y = sprite.GetTextureRectRelativeToContainer ().yMax;
			
						Debug.DrawLine (tL, tR, color);
						Debug.DrawLine (tR, bR, color);
						Debug.DrawLine (bR, bL, color);
						Debug.DrawLine (bL, tL, color);
				}
		}
}