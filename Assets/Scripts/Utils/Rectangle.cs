using UnityEngine;
using System.Collections;

// Axis Aligned rectangle, with velocity for swept.
public class Rectangle
{
	public Rectangle(float _x, float _y, float _w, float _h, float _vx, float _vy)
	{
		x = _x;
		y = _y;
		w = _w;
		h = _h;
		vx = _vx;
		vy = _vy;
	}
	
	public Rectangle(float _x, float _y, float _w, float _h)
	{
		x = _x;
		y = _y;
		w = _w;
		h = _h;
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