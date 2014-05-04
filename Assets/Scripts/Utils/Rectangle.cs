using UnityEngine;
using System.Collections;

// Axis Aligned rectangle, with velocity for swept.
public class Rectangle
{
		public Rectangle (float center_x, float center_y, float width, float height, float velocity_x, float velocity_y)
		{
				// set x and y at top-left corner.
				x = center_x - width/2;
				y = center_y - height/2;
				w = width;
				h = height;
				vx = velocity_x;
				vy = velocity_y;
		}
	
		public Rectangle (float center_x, float center_y, float width, float height)
		{
				// set x and y at top-left corner.
				x = center_x - width/2;
				y = center_y - height/2;
				w = width;
				h = height;
				vx = 0.0f;
				vy = 0.0f;
		}
	
		// position of top-left corner
		public float x, y;
	
		// dimensions
		public float w, h;
	
		// velocity
		public float vx, vy;
}