using UnityEngine;
using System;
using System.Collections;

/*
 * Code adapted from:
 * http://www.gamedev.net/page/resources/_/technical/game-programming/swept-aabb-collision-detection-and-response-r3084
 */

public class CollisionDetection
{

		// returns true if the Rectanglees are colliding (velocities are not used)
		public static bool AABBCheck (Rect rect1, Rect rect2)
		{
				return !(rect1.x + rect1.width < rect2.x || rect1.x > rect2.x + rect2.width || rect1.y + rect1.height < rect2.y || rect1.y > rect2.y + rect2.height);
		}

		

		// returns true if the Rectanglees are colliding (velocities are not used)
		// moveX and moveY will return the movement the rect1 must move to avoid the collision
		public static bool AABB (Rectangle rect1, Rectangle rect2, out float moveX, out float moveY)
		{
				moveX = moveY = 0.0f;

				float l = rect2.x - (rect1.x + rect1.w);
				float r = (rect2.x + rect2.w) - rect1.x;
				float t = rect2.y - (rect1.y + rect1.h);
				float b = (rect2.y + rect2.h) - rect1.y;

				// check that there was a collision
				if (l > 0 || r < 0 || t > 0 || b < 0)
						return false;

				// find the offset of both sides
				moveX = Math.Abs (l) < r ? l : r;
				moveY = Math.Abs (t) < b ? t : b;

				// only use whichever offset is the smallest
				if (Math.Abs (moveX) < Math.Abs (moveY))
						moveX = 0.0f;
				else
						moveY = 0.0f;

				return true;
		}

}