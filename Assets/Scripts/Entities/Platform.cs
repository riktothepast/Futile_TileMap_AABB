using UnityEngine;
using System.Collections;

public abstract class Platform :FContainer
{
    public enum CollisionTypes 
    { 
        Hollow,
        Solid,
        Platform,
        Ice,
        Liquid,
        LiquidHazard
    }

	    public FSprite sprite		{ get; set; }
        public CollisionTypes collisionType { get; set; }

		public Platform ()
		{

		}

		// this code is by mylesmar10
	public void drawHitBox (Rect rect, Color color)
	{
		Vector3 tL = new Vector3 (0f, 0f, 0f);
		Vector3 tR = new Vector3 (0f, 0f, 0f);
		Vector3 bL = new Vector3 (0f, 0f, 0f);
		Vector3 bR = new Vector3 (0f, 0f, 0f);
		tL.x = rect.xMin;
		tL.y = rect.yMin;
		
		tR.x = rect.xMax;
		tR.y = rect.yMin;
		
		bL.x = rect.xMin;
		bL.y = rect.yMax;
		
		bR.x = rect.xMax;
		bR.y = rect.yMax;
		Debug.DrawLine (tL, tR, color);
		Debug.DrawLine (tR, bR, color);
		Debug.DrawLine (bR, bL, color);
		Debug.DrawLine (bL, tL, color);
	}
}
