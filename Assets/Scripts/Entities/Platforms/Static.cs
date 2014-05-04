using UnityEngine;
using System.Collections;

public class Static : Platform {

	public Static(FSprite sprite)
	{
		this.sprite=sprite;
		Start ();

	}
	// Use this for initialization
	void Start () {
	
		ListenForUpdate(Update);
		
		AddChild(sprite);
	}
	
	// Update is called once per frame
	void Update () {
		drawHitBox(new Rect(GetPosition().x,GetPosition().y,sprite.width,sprite.height), Color.cyan);
	}
}
