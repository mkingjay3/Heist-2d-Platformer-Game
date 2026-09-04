using MGFGame.Collision;
using MGFGame.Collision.Block_Mechanics;
using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Levels
{
    internal class Level1 : Level
    {
        public override void LoadEnemies()
        {
            enemies.Add(new MultiShotEnemy(new Vector2(1200, 900)));
            enemies.Add(new MultiShotEnemy(new Vector2(2400, 900)));
            enemies.Add(new MultiShotEnemy(new Vector2(3600, 900)));
        }

        public override void LoadLevel(string path)
        {
            movingPlatforms.Clear();
            ladders.Clear();
            collectables.Clear();

            if (!Path.IsPathRooted(path))
            {
                string rootAssetPath = "Assets";
                while (!Directory.Exists(rootAssetPath))
                {
                    rootAssetPath = Path.Combine("..", rootAssetPath);
                    if (rootAssetPath.Length > 100) break;
                }
                path = Path.Combine(rootAssetPath, path);
            }

            if (!File.Exists(path))
            {
                return;
            }

            string[] lines = File.ReadAllLines(path);

            height = lines.Length;
            width = lines[0].Split(',').Length;

            tiles = new Tile[height, width];

            for (int row = 0; row < height; row++)
            {
                string[] vals = lines[row].Split(',');

                for (int col = 0; col < width; col++)
                {
                    Vector2 worldPos = new Vector2(col * TILE_SIZE, row * TILE_SIZE);
                    Tile tile = new Tile(worldPos);

                    try
                    {
                        int val = int.Parse(vals[col].Trim());

                        if (val == -1)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                        }
                        else if (val == 22)
                        {
                            tile.Texture = Game.textures["hazard"];
                            tile.IsEmpty = false;
                            tile.collide = new IntervalPlatformCollision(tile);
                        }
                        else if (val == 27)
                        {
                            tile.Texture = Game.textures["hazard"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 30)
                        {
                            tile.Texture = Game.textures["red brick 1"];  
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 32)
                        {
                            tile.Texture = Game.textures["red window"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 33)
                        {
                            tile.Texture = Game.textures["left red wall"];  
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 34)
                        {
                            tile.Texture = Game.textures["top left red"];  
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 35)
                        {
                            tile.Texture = Game.textures["red wall top"];  
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 36)
                        {
                            tile.Texture = Game.textures["red wall right"]; 
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 37)
                        {
                            tile.Texture = Game.textures["top right red wall"];  
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 39 || val == 42 || val == 43 || val == 45)
                        {
                            string signKey = "sign 1";
                            if (val == 42) signKey = "sign 2";
                            else if (val == 43) signKey = "sign 3";
                            else if (val == 45) signKey = "sign 4";

                            tile.Texture = Game.textures[signKey];
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();
                            tile.Size = new Vector2(128, 64);
                        }
                        else if (val == 40)
                        {
                            tile.Texture = Game.textures["chim mid"]; 
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 41)
                        {
                            tile.Texture = Game.textures["chim top"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 44)
                        {
                            tile.Texture = Game.textures["ladder"];
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();
                            ladders.Add(new Ladder(worldPos));
                        }
                        else if (val == 46)
                        {
                            tile.Texture = Game.textures["mid one"];
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
                        }
                        else if (val == 47)
                        {
                            tile.Texture = Game.textures["right one"];
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
                        }
                        else if (val == 48)
                        {
                            tile.Texture = Game.textures["left one"];
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
                        }
                        else if (val == 49)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            collectables.Add(new Collectable(worldPos));
                        }
                        else if (val == 51)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            movingPlatforms.Add(new MovingPlatform(worldPos));
                        }
                        else if (val == 54)
                        {
                            tile.Texture = Game.textures["spike"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 56)
                        {
                            tile.Texture = Game.textures["door"];
                            tile.IsEmpty = false;
                            tile.collide = new LevelTransitionCollision(tile, 1);
                            tile.Size = new Vector2(64, 128);
                            tile.Position = new Vector2(worldPos.X, worldPos.Y - 64);
                        }
                        else
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading tile at {row},{col} (val: {vals[col]}): {ex.Message}");
                        tile.IsEmpty = true;
                        tile.collide = new notSolid();
                    }

                    tiles[row, col] = tile;
                }
            }

            if (enemies.Count == 0)
            {
                LoadEnemies();
            }
        }

        public override void ReloadLevel(string path)
        {
            enemies.Clear();
            movingPlatforms.Clear();
            ladders.Clear();
            collectables.Clear();
            LoadLevel(path);
        }
    }
}
