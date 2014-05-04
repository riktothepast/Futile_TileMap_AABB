using UnityEngine;
using System.Collections.Generic;

public class GamePage : Page
{

    TileMap tileMap;
    bool started;
    Vector2 cameraPosition;
    Player player;
    FLabel hudStuff;
    FLabel hudShadow;
    public GamePage()
    {
        Debug.Log("In game page");
        hudStuff = new FLabel("font", "Game Stats:");
        hudShadow = new FLabel("font", "Game Stats:");
        hudStuff.SetPosition(new Vector2(Futile.screen.halfWidth / 4, Futile.screen.height - 40));
        hudShadow.SetPosition(new Vector2((Futile.screen.halfWidth / 4)+1f, (Futile.screen.height - 41)));
        hudStuff.scale = 0.3f;
        hudStuff.color = Color.white;
        hudShadow.scale = 0.3f;
        hudShadow.color = Color.black;
    }

    override public void Start()
    {
        tileMap = new TileMap();
        tileMap.LoadTileMap("Texts/MapBig");
        AddChild(tileMap);
        player = new Player();
        cameraPosition = player.GetPosition();
        tileMap.AddChild(player);
        ListenForUpdate(Update);
        AddChild(hudShadow);
        AddChild(hudStuff);
    }

    // Update is called once per frame
    public void Update()
    {
        player.Update();
        FollowVector(player.GetPosition());
        hudStuff.text = "Game Stats:  FPS : " + 1 / Time.deltaTime+ "\n Ground :  " + player.Ground() + " \n Ceiling : " + player.Ceiling() + "\n Left : " + player.LeftWall() + " \n Right : " + player.RightWall() + " \n Velocity : " + player.Velocity ;
        hudShadow.text = hudStuff.text;    
    }
    // move the map based on a player or a centric point around various players
    public void FollowVector(Vector2 position)
    {
        Vector2 levelSize = tileMap.getSize();

        float halfOfTheScreenX = Futile.screen.halfWidth;
        float halfOfTheScreenY = Futile.screen.halfHeight;

        float newXPosition = tileMap.x;
        float newYPosition = tileMap.y;

        newXPosition = (halfOfTheScreenX - position.x);
        newYPosition = (halfOfTheScreenY - position.y);

        // limit screen movement
        if (newXPosition > -TileMap.tileSize)
            newXPosition = -TileMap.tileSize;
        if (newXPosition < -levelSize.x + halfOfTheScreenX * 2 + TileMap.tileSize)
            newXPosition = -levelSize.x + halfOfTheScreenX * 2 + TileMap.tileSize;

        if (newYPosition < halfOfTheScreenY * 2.0f)
            newYPosition = halfOfTheScreenY * 2.0f;
        if (newYPosition > levelSize.y)
            newYPosition = levelSize.y;

        // center on screen for small maps
        if (halfOfTheScreenX * 2.0f >= levelSize.x)
            newXPosition = ((halfOfTheScreenX * 2.0f - levelSize.x) / 2.0f);
        if (halfOfTheScreenY * 2.0f >= levelSize.y)
            newYPosition = halfOfTheScreenY * 2.0f - ((halfOfTheScreenY * 2.0f - levelSize.y) / 2.0f);

        // move the map
        tileMap.SetPosition(new Vector2((int)newXPosition, (int)newYPosition));
    }

}