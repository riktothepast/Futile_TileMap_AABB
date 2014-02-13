using UnityEngine;
using System.Collections.Generic;

public class GamePage : Page {

	TileMap tileMap;
	bool started;
	Vector2 cameraPosition;
	public GamePage(){
			Debug.Log("In game page");

	}

	override public void Start () 
	{
		tileMap = new TileMap();
		tileMap.LoadTileMap("Texts/MapBig");
		AddChild(tileMap);
		cameraPosition = new Vector2(Futile.screen.halfWidth,Futile.screen.halfHeight);
		ListenForUpdate(Update);
	}

	// Update is called once per frame
	public void Update () 
	{
		FollowVector(cameraPosition);

		if(Input.GetKey("left")){
			cameraPosition.x-=5f;
		}
		if(Input.GetKey("right")){
			cameraPosition.x+=5f;
		}
		if(Input.GetKey("up")){
			cameraPosition.y+=5f;
		}
		if(Input.GetKey("down")){
			cameraPosition.y-=5f;
		}

	}
		// move the map based on a player or a centric point around various players
	public void FollowVector(Vector2 position)
	{
		Vector2 levelSize = tileMap.getSize();
		
		float screenHalfX = Futile.screen.halfWidth; 
		float screenHalfY = Futile.screen.halfHeight;
		
		float newXPosition = tileMap.x;
		float newYPosition = tileMap.y;
		
		newXPosition = (screenHalfX - position.x);
		newYPosition = (screenHalfY - position.y);
		
		// limit screen movement
		if (newXPosition > -tileMap.tileSize) 
			newXPosition = -tileMap.tileSize;
		if (newXPosition < -levelSize.x + screenHalfX*2 + tileMap.tileSize) 
			newXPosition = -levelSize.x + screenHalfX*2 + tileMap.tileSize;
		
		if (newYPosition < screenHalfY*2.0f)
			newYPosition = screenHalfY*2.0f;
		if (newYPosition > levelSize.y)
			newYPosition = levelSize.y;
		
		// center on screen for small maps
		if (screenHalfX*2.0f >= levelSize.x)
			newXPosition = ((screenHalfX*2.0f - levelSize.x) / 2.0f);
		if (screenHalfY*2.0f >= levelSize.y)
			newYPosition = screenHalfY*2.0f - ((screenHalfY*2.0f - levelSize.y) / 2.0f);
		
		// move the map
		tileMap.SetPosition(new Vector2((int)newXPosition, (int)newYPosition));
	}

}