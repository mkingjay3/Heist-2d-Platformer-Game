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
    internal class Level4 : Level
    {
        public override void LoadEnemies()
        {
            enemies.Add(new BossEnemy(new Vector2(3668, 1077)));
            collectables.Add(new PowerUp(new Vector2(398, 476)));
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
                        else if (val == 44)
                        {
                            tile.Texture = Game.textures["ladder"];
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();
                            ladders.Add(new Ladder(worldPos));
                        }
                        else if (val == 8)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            levers.Add(new Lever(worldPos));
                        }
                        else if (val == 16)
                        {
                            tile.Texture = Game.textures["laser tip"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 61)
                        {
                            tile.Texture = Game.textures["laser tip reversed"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 18)
                        {
                            tile.Texture = Game.textures["laser"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 49)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            collectables.Add(new Collectable(worldPos));
                        }
                        else if (val == 15)
                        {
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            collectables.Add(new Collectable(worldPos));
                        }
                        else if (val == 300)
                        {
                            tile.Texture = Game.textures["conveyer 1"];
                            tile.IsEmpty = false;
                            tile.collide = new ConveyerCollision(tile, true);
                        }
                        else if (val == 301)
                        {
                            tile.Texture = Game.textures["conveyer 2"];
                            tile.IsEmpty = false;
                            tile.collide = new ConveyerCollision(tile, true);
                        }
                        else if (val == 302)
                        {
                            tile.Texture = Game.textures["conveyer 3"];
                            tile.IsEmpty = false;
                            tile.collide = new ConveyerCollision(tile, true);
                        }
                        else if (val == 303)
                        {
                            tile.Texture = Game.textures["conveyer 4"];
                            tile.IsEmpty = false;
                            tile.collide = new ConveyerCollision(tile, true);
                        }
                        else if (val == 85)
                        {
                            tile.Texture = Game.textures["metal base"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 86)
                        {
                            tile.Texture = Game.textures["metal floor"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 54)
                        {
                            tile.Texture = Game.textures["spike"];
                            tile.IsEmpty = false;
                            tile.collide = new IntervalDamageCollision(tile);
                        }
                        else if (val == 87)
                        {
                            tile.Texture = Game.textures["interval platform"];
                            tile.IsEmpty = false;
                            tile.collide = new ConveyerCollision(tile, true);
                        }
                        else if (val == 14)
                        {
                            tile.Texture = Game.textures["ledge"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 89)
                        {
                            tile.Texture = Game.textures["hazard"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 74 || val == 76)
                        {
                            tile.Texture = Game.textures["black box"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                            tile.Size = new Vector2(128, 128);
                        }
                        else if (val == 100)
                        {
                            tile.Texture = Game.textures["black box tl"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 101)
                        {
                            tile.Texture = Game.textures["black box tr"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 102)
                        {
                            tile.Texture = Game.textures["black box bl"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 103)
                        {
                            tile.Texture = Game.textures["black box br"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 200)
                        {
                            tile.Texture = Game.textures["brownbox tl"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 201)
                        {
                            tile.Texture = Game.textures["brownbox tr"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 202)
                        {
                            tile.Texture = Game.textures["brownbox bl"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 203)
                        {
                            tile.Texture = Game.textures["brownbox br"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
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
