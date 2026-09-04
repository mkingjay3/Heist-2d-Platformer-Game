using MGFGame.Collision;
using MGFGame.Collision.Block_Mechanics;
using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Levels
{
    internal class Level2 : Level
    {
        public Level2()
        {
            levers = new List<Lever>();
        }

        public override void LoadEnemies()
        {
            enemies.Add(new InvulnerableEnemy(new Vector2(600, 900)));
            enemies.Add(new InvulnerableEnemy(new Vector2(1500, 900)));
            enemies.Add(new InvulnerableEnemy(new Vector2(2500, 900)));
            enemies.Add(new InvulnerableEnemy(new Vector2(1832, 512)));
            enemies.Add(new InvulnerableEnemy(new Vector2(5606, 1382)));
            enemies.Add(new InvulnerableEnemy(new Vector2(3884, 1688)));
            enemies.Add(new InvulnerableEnemy(new Vector2(3164, 1406)));
        }
        public override void LoadLevel(string path)
        {
            Debug.WriteLine($"=== LEVEL 2 (Museum) LoadLevel START ===");

            if (levers == null)
                levers = new List<Lever>();

            movingPlatforms.Clear();
            ladders.Clear();
            collectables.Clear();
            levers.Clear();
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
                Debug.WriteLine($"Level file not found: {path}");
                return;
            }

            string[] lines = File.ReadAllLines(path);

            height = lines.Length;
            width = lines[0].Split(',').Length;

            Debug.WriteLine($"Loaded Level 2 (Museum) - size {width}x{height}");

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

                        if (val == 72 || val == 82)
                        {
                            tile.Texture = Game.textures["one_way"];
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
                        }
                        else if (val == 46 || val == 99)
                        {
                            tile.Texture = Game.textures["mid one"];
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
                        }
                        else if (val == 117)
                        {
                            tile.Texture = Game.textures["ladder"];
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();
                            ladders.Add(new Ladder(worldPos));
                        }
                        else if (val == 10)
                        {
                            tile.Texture = Game.textures["ladder"];
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();
                            ladders.Add(new Ladder(worldPos));
                        }
                        else if (val == 70 || val == 1073741894 || val == -1073741756 || val == -2147483578)
                        {
                            tile.Texture = Game.textures["lava_ground"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 66 || val == 81)
                        {
                            tile.Texture = Game.textures["lava_block"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 54)
                        {
                            tile.Texture = Game.textures["spike"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 118)
                        {
                            tile.Texture = Game.textures["laser"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 119)
                        {
                            tile.Texture = Game.textures["laser tip reversed"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 120)
                        {
                            tile.Texture = Game.textures["laser tip"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 71 || val == -2147483577 || val == 536870983 || val == 23)
                        {
                            tile.Texture = Game.textures["cave_floor"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 76)
                        {
                            tile.Texture = Game.textures["dirt_brown"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 69)
                        {
                            tile.Texture = Game.textures["light_purple"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 73)
                        {
                            tile.Texture = Game.textures["dark_purple"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 12)
                        {
                            tile.Texture = Game.textures["smoke"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 86 || val == 90)
                        {
                            tile.Texture = Game.textures["metal floor"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 74)
                        {
                            tile.Texture = Game.textures["black box"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                            tile.Size = new Vector2(128, 128);
                        }
                        else if (val == 124)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            levers.Add(new Lever(worldPos));
                        }
                        else if (val == 56 || val == 167 || val == 169)
                        {
                            tile.Texture = Game.textures["door"];
                            tile.IsEmpty = false;
                            tile.collide = new LevelTransitionCollision(tile, 2);
                            tile.Size = new Vector2(64, 128);
                            tile.Position = new Vector2(worldPos.X, worldPos.Y - 64);
                        }
                        else if (val == 87)
                        {
                            tile.Texture = Game.textures["interval platform"];
                            tile.IsEmpty = false;
                            tile.collide = new IntervalPlatformCollision(tile);
                        }
                        else if (val == 103 || val == 124 || val == 125 || val == 126)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            collectables.Add(new Collectable(worldPos));
                        }
                        else if (val == 49 || val == 145)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            collectables.Add(new Collectable(worldPos));
                        }
                        // Empty
                        else if (val == -1)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                        }
                        else if (val == 67)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            movingPlatforms.Add(new MovingPlatform(worldPos));
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
