//
// TileMap.cs
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
using Assets.Scripts.Utils;

public class TileMap : FContainer
{
    int[,] Tiles = new int[25, 16];
    List<Platform> tileSprites = new List<Platform>();
    LevelData levelData;
    public int Columns { get; set; }
    public int Rows { get; set; }

    public static TileMap CurrentMap { get; private set; }

    public static float tileSize { get; set; }

    public TileMap()
    {

    }

    public void LoadTileMap(String mapText)
    {
        Array.Clear(Tiles, 0, Tiles.Length);
        levelData = new LevelData(mapText);

        // fill TileMap with zeroes.
        loadTiles();
        TileMap.CurrentMap = this;
    }

    void loadTiles()
    {
        foreach (PlatformData platform in levelData.getPlatformData())
        {
            FSprite plat = new FSprite(platform.image);
            tileSize = plat.localRect.width;
            Tiles[(int)platform.x, (int)platform.y] = 1;
            Static staticPlatform = new Static(new FSprite(platform.image));
            staticPlatform.SetPosition(new Vector2(platform.x * plat.localRect.width , -platform.y * plat.localRect.height ));
            tileSprites.Add(staticPlatform);
            AddChild(staticPlatform);
        }

    }

    // update the tilemap based on whats visible ?. 
    public void Update()
    {
        int lengthI = Tiles.GetLength(0); // Y
        int lengthJ = Tiles.GetLength(1); // X

        int drawMin_X = Math.Abs((int)(this.x / tileSize)) - 1;
        int drawMax_X = drawMin_X + (int)(Futile.screen.width / tileSize) + 2;
        int drawMin_Y = (int)((this.y - Futile.screen.height) / tileSize) - 1;
        int drawMax_Y = drawMin_Y + (int)(Futile.screen.height / tileSize) + 2;

        if (drawMin_X < 0)
            drawMin_X = 0;
        if (drawMax_X > lengthJ)
            drawMax_X = lengthJ;
        if (drawMin_Y < 0)
            drawMin_Y = 0;
        if (drawMax_Y > lengthI)
            drawMax_Y = lengthI;

        if (Futile.screen.width > getSize().x)
            drawMin_X = 0;

        // only draw visible Tiles
        foreach (Platform e in this.tileSprites)
        {
            int j = (int)(e.GetPosition().x / tileSize);
            int i = (int)(-1 * (e.GetPosition().y / tileSize));

            if (j >= drawMin_X && j <= drawMax_X && i >= drawMin_Y && i <= drawMax_Y)
            {
                e.isVisible = true;
            }
            else
            {
                e.isVisible = false;
            }
        }
    }

    public Vector2 getSize()
    {
        return new Vector2(Tiles.GetLength(0) * tileSize, Tiles.GetLength(1) * tileSize);
    }

    public Vector2 getSizeIntiles()
    {
        return new Vector2(Tiles.GetLength(0), Tiles.GetLength(1));
    }

    public int getTileType(int x, int y)
    {
        if ((x > 0 && x < Tiles.GetLength(0)) && (y > 0 && y < Tiles.GetLength(1)))
        {
            return Tiles[x, y];
        }
        else
        {
            return -1;
        }

    }

    public bool HasRoomForRectangle(Rectangle rectangleToCheck)
    {
        int P_X = Math.Abs((int)((rectangleToCheck.x) / tileSize));
        int P_Y = Math.Abs((int)((rectangleToCheck.y) / tileSize));

        int Min_X = P_X - 2;
        int Max_X = P_X + 2;
        int Min_Y = P_Y - 2;
        int Max_Y = P_Y + 2;

        if (Min_X < 0) Min_X = 0;
        if (Max_X > 24) Max_X = 25 - 1;
        if (Min_Y < 0) Min_Y = 0;
        if (Max_Y > 15) Max_Y = 16 - 1;

        for (int j = Min_Y; j <= Max_Y; j++)
        {
            for (int i = Min_X; i <= Max_X; i++)
            {
                Rectangle tileBounds = new Rectangle(i * tileSize, -j * tileSize , tileSize, tileSize);
                if (Tiles[i, j] == 1)
                    drawHitBox(tileBounds, Color.white);

                if (Tiles[i, j] == 1 && MovementWrapper.AABBCheck(tileBounds,rectangleToCheck))
                {
                    return false;
                }
            }
        }
        return true;
    }


    public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
    {
        MovementWrapper move = new MovementWrapper(originalPosition, destination, boundingRectangle);

        for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
        {
            Vector2 positionToTry = originalPosition + move.OneStep * i;
            Rectangle newBoundary = new Rectangle((int)positionToTry.x , (int)positionToTry.y , boundingRectangle.w, boundingRectangle.h);
            drawHitBox(newBoundary, Color.red);
            if (HasRoomForRectangle(newBoundary)) { move.FurthestAvailableLocationSoFar = positionToTry; }
            else
            {
                if (move.IsDiagonalMove)
                {
                    move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                }
                break;
            }
        }
        return move.FurthestAvailableLocationSoFar;
    }

    private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
    {
        if (wrapper.IsDiagonalMove)
        {
            int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

            Vector2 remainingHorizontalMovement = wrapper.OneStep.x * Vector2.right * stepsLeft;
            wrapper.FurthestAvailableLocationSoFar =
                WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

            Vector2 remainingVerticalMovement = wrapper.OneStep.y * Vector2.up * stepsLeft;
            wrapper.FurthestAvailableLocationSoFar =
                WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
        }

        return wrapper.FurthestAvailableLocationSoFar;
    }
    public void drawHitBox(Rectangle rect, Color color)
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
}