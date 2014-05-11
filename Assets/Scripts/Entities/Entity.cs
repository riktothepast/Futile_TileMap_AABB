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
        Vector2 oldPosition; 
        Vector2 velocity;
		Vector2 position;
        Vector2 maxSpeed;
	    Vector2 size;
		float gravity;
		float jumpImpulse;
        float jumpTime;
        bool isJumping, wasJumping, doubleJump;
        float movementSpeed = 25f;
        float MaxJumpTime = 0.50f;
        float JumpControlPower = 0.14f;
        //how many pixels we are using to check boundaries
        public int detectionAccuracy = 1;
		public Rectangle BoundingBox;

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
        public Vector2 Size { get { return size; } set { size = value; } }
        public float Gravity { get { return gravity; } set { gravity = value; } }
        public float JumpImpulse { get { return jumpImpulse; } set { jumpImpulse = value; } }
        public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
        public int DetectionAccuracy { get { return detectionAccuracy; } set { detectionAccuracy = value; } }

		public bool onGround 		{ get; set; }

		public bool canJump			{ get; set; }

		public bool useGravity		{ get; set; }

		public bool noClip			{ get; set; }

		public FSprite sprite		{ get; set; }
		
		public Entity ()
		{
				
		}

        public virtual void CheckAndUpdateMovement()
        {
            float hInput = Input.GetAxis("Horizontal");
            float vInput = Input.GetAxis("Vertical");

            float forwardStep = movementSpeed * Time.deltaTime;

            if (hInput > 0){
                velocity += Vector2.right * forwardStep;
            } else if (hInput < 0){
                velocity -= Vector2.right * forwardStep;
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                    setJump();
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                velocity += Vector2.up * forwardStep;
            }

            if (velocity.x > maxSpeed.x)
            {
                velocity.x = maxSpeed.x;
            }
            else if (velocity.x < -maxSpeed.x)
            {
                velocity.x = -maxSpeed.x;
            }

            if (velocity.y < -maxSpeed.y)
            {
                velocity.y = -maxSpeed.y;
            }

        }

         void MoveAsFarAsPossible()
        {
            oldPosition = position;
            UpdatePosition();
            position = TileMap.CurrentMap.WhereCanIGetTo(oldPosition, position, getAABBBoundingBox());
        }

         void UpdatePosition()
        {
            position += velocity;
        }
				
		public virtual void applyGravity ()
		{
				if (!useGravity)
						return;

                velocity -= Vector2.up * gravity * Time.deltaTime;
		}

        public virtual void applyFriction()
        {
            // applies friction based on the last type of tile detected?. Of if the entity is not grounded.
            if (Ground())
            {
                velocity.x *= 0.8f;
            }
            else {
                velocity.x *= 0.85f;
            }
        }

		public virtual void setJump ()
		{
            isJumping = true;
            if (jumpTime>0.0f) {
                doubleJump = true ;
            }
        }

        public virtual void DoJump()
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && Ground()) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f) { }
                        // play sound effect here

                    jumpTime += Time.deltaTime;
                    // do animation switch here

                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives
                    // players more control over the top of the jump
                    velocity.y = JumpImpulse * (1.0f - Mathf.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;
        }

		public virtual void Update ()
		{
            CheckAndUpdateMovement();
            DoJump();
            applyGravity();
            applyFriction();
            MoveAsFarAsPossible();
            Vector2 newPos = TileMap.CurrentMap.WorldWrap(getAABBBoundingBox());
            if (newPos != Vector2.zero)
            {
                position = newPos;
            }
            StopMovingIfBlocked();
            SetPosition(position);
            isJumping = false;
		}

        void StopMovingIfBlocked()
        {
            Vector2 lastMovement = position - oldPosition;
            if (lastMovement.x == 0) { velocity.x *= 0; }
            if (lastMovement.y == 0) { velocity.y *= 0; }
        }

		public void Destroy ()
		{
			
		}

		public Rectangle getAABBBoundingBox ()
		{
            return new Rectangle(position.x, position.y, size.x, size.y);
		}


		public Vector2 positionOnTileMap (Vector2 position)
		{
				return new Vector2 ((int)(position.x / TileMap.tileSize), (int)(position.y / TileMap.tileSize));
		}

        //https://docs.unity3d.com/Documentation/ScriptReference/Rectangle.html
        public bool Ground()
        {
            Rectangle offsetOnePixel = getAABBBoundingBox();
            offsetOnePixel.y -= detectionAccuracy;
        //    drawHitBox(offsetOnePixel, Color.cyan);
            return !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);
        }
        public bool Ceiling()
        {
            Rectangle offsetOnePixel = getAABBBoundingBox();
            offsetOnePixel.y += detectionAccuracy*2;
        //    drawHitBox(offsetOnePixel, Color.magenta);
            return !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);
        }

        public bool LeftWall()
        {
            Rectangle offsetOnePixel = getAABBBoundingBox();
            offsetOnePixel.x -= detectionAccuracy*2;
         //   drawHitBox(offsetOnePixel, Color.blue);
            return !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);
        }

        public bool RightWall()
        {
            Rectangle offsetOnePixel = getAABBBoundingBox();
            offsetOnePixel.x += detectionAccuracy;
       //     drawHitBox(offsetOnePixel, Color.yellow);
            return !TileMap.CurrentMap.HasRoomForRectangle(offsetOnePixel);
        }
	
		// this code is by mylesmar10
		public void drawHitBox (Rectangle rect, Color color)
		{
            Vector3 tL = new Vector3(0f, 0f, 0f);
            Vector3 tR = new Vector3(0f, 0f, 0f);
            Vector3 bL = new Vector3(0f, 0f, 0f);
            Vector3 bR = new Vector3(0f, 0f, 0f);
            tL.x = rect.x;
            tL.y = rect.y;
            tR.x = rect.x + rect.w;
            tR.y = rect.y;
            bL.x = rect.x;
            bL.y = rect.y + rect.h;
            bR.x = rect.x + rect.w;
            bR.y = rect.y + rect.h;

            Debug.DrawLine(tL, tR, color);
            Debug.DrawLine(tR, bR, color);
            Debug.DrawLine(bR, bL, color);
            Debug.DrawLine(bL, tL, color);
		}

		public Vector2 RectIntersection (Rectangle rectA, Rectangle rectB)
		{
				float leftX = Math.Max (rectA.x, rectB.x);
				float rightX = Math.Min (rectA.x + rectA.w, rectB.x + rectB.w);
				float topY = Math.Max (Math.Abs (rectA.y), Math.Abs (rectB.y));
				float bottomY = Math.Min (Math.Abs (rectA.y) + rectA.h, Math.Abs (rectB.y) + rectB.h);
		
				return new Vector2 (rightX - leftX, bottomY - topY);
		}
}