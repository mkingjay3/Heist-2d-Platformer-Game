using MGFGame.Collision;
using MGFGame.Collision.Block_Mechanics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Levels
{
    internal class Level3 : Level
    {
        public override void LoadEnemies()
        {
            enemies.Add(new OneShotEnemy(new Vector2(3632, 3068)));
            enemies.Add(new OneShotEnemy(new Vector2(4082, 3110)));
            enemies.Add(new OneShotEnemy(new Vector2(5724, 3950)));
            enemies.Add(new OneShotEnemy(new Vector2(6014, 2864)));

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
                        else if (val == 70)
                        {
                            tile.Texture = Game.textures["rocks"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 59)
                        {
                            tile.Texture = Game.textures["cave_floor"];
                            tile.IsEmpty = false;
                            tile.collide = new IntervalPlatformCollision(tile);
                        }
                        else if (val == 51)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            movingPlatforms.Add(new MovingPlatform(worldPos));
                        }
                        else if (val == 49)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            collectables.Add(new Collectable(worldPos));
                        }
                        else if (val == 64)
                        {
                            tile.Texture = Game.textures["lava_ground"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 54)
                        {
                            tile.Texture = Game.textures["spike"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 47)
                        {
                            tile.Texture = Game.textures["right one"];
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
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
                        else if (val == 48)
                        {
                            tile.Texture = Game.textures["left one"];
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
                        }
                        else if (val == 1073741878)
                        {
                            tile.Texture = Game.textures["spike"];
                            tile.IsEmpty = false;
                            tile.collide = new IntervalDamageCollision(tile);
                            tile.Mirror = TextureMirror.Vertical;
                        }
                        else if (val == 56)
                        {
                            tile.Texture = Game.textures["door"];
                            tile.IsEmpty = false;
                            tile.collide = new LevelTransitionCollision(tile, 3);
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
