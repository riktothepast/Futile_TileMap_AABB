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

public class TileMap :FContainer
		{
			int[,] tiles = new int[25,16] ;
			List<FSprite> tileSprites = new List<FSprite>();
			LevelData levelData;
			public float tileSize {get;set;}
				public TileMap ()
				{
					
				}

				public void LoadTileMap(String mapText)
				{
					levelData = new LevelData(mapText);
					loadGraphicTiles();
				}

				void loadGraphicTiles()
				{
					foreach (PlatformData platform in levelData.getPlatformData()) {
						FSprite plat =  new FSprite (platform.image);
						tileSize = plat.localRect.width;
						tiles[(int)platform.x,(int)platform.y] = 1;
						FSprite sprite = new FSprite(platform.image);
						sprite.SetPosition(new Vector2 (platform.x*plat.localRect.width, -platform.y*plat.localRect.height));
						tileSprites.Add(sprite);
						AddChild(sprite);
					}
				}
				
				// update the tilemap based on whats visible ?. 
				public void Update()
				{
					
				}
				
				public Vector2 getSize(){
						return new Vector2(tiles.GetLength(0)*tileSize,tiles.GetLength(1)*tileSize);
				}
		}