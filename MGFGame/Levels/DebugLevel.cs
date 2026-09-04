using MGFGame.Collision;
using MGFGame.Collision.Block_Mechanics;
using System.Diagnostics;
using System.IO;

namespace MGFGame.Levels
{
    public class DebugLevel : Level
    {
        public void LoadDebugLevel(string path)
        {
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

            Debug.WriteLine($"Looking for: {path}");
            if (!File.Exists(path))
            {
                Debug.WriteLine("FILE NOT FOUND!");
                return;
            }
            Debug.WriteLine($"DEBUG LEVEL FOUND: {path}");


            string[] pixels = File.ReadAllLines(path);

            height = pixels.Length;
            width = pixels[0].Split(',').Length;

            Debug.WriteLine($"Loaded debug level size {width}x{height}");

            tiles = new Tile[height, width];

            for (int row = 0; row < height; row++)
            {
                string[] vals = pixels[row].Split(',');

                for (int col = 0; col < width; col++)
                {
                    Tile tile;

                    try
                    {
                        int val = int.Parse(vals[col].Trim());
                        Vector2 worldPos = new Vector2(col * TILE_SIZE, row * TILE_SIZE);

                        tile = new Tile(worldPos);

                        if (val == 7) // One-Way Platform - Grey
                        {
                            tile.IsEmpty = false;
                            tile.collide = new OneWayPlatformCollision(tile);
                            tile.DebugColor = Color.Gray;
                        }
                        else if (val == 8) // Static Obstacle - Black
                        {
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                            tile.DebugColor = Color.Black;
                        }
                        else if (val == 9) // Conveyer - Dark Green
                        {
                            tile.IsEmpty = false;
                            tile.collide = new ConveyerCollision(tile, true); 
                            tile.DebugColor = new Color(0, 100, 0);
                        }
                        else if (val == 10) // Lever - Light Green
                        {
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile); 
                            tile.DebugColor = Color.LightGreen;
                        }
                        else if (val == 11) // Interval Platform - Yellow
                        {
                            tile.IsEmpty = false;
                            tile.collide = new IntervalPlatformCollision(tile);
                            tile.DebugColor = Color.Yellow;
                        }
                        else if (val == 12) // Ladder - Blue
                        {
                            tile.IsEmpty = false; 
                            tile.collide = new notSolid();
                            tile.DebugColor = Color.LightGray; 

                            // Create a ladder object 
                            ladders.Add(new Ladder(worldPos));
                        }
                        else if (val == 13) // Collectable - Purple
                        {
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();  
                            tile.DebugColor = Color.LightGray; 

                            // Create a collectable object
                            collectables.Add(new Collectable(worldPos));
                        }
                        else if (val == 14) // Moving Platform - Red
                        {
                            tile.IsEmpty = false; 
                            tile.collide = new notSolid();
                            tile.DebugColor = Color.LightGray;

                            // Create a moving platform object 
                            movingPlatforms.Add(new MovingPlatform(worldPos));
                        }
                        else
                        {
                            // Empty
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();
                            tile.DebugColor = Color.LightGray;
                        }

                        tiles[row, col] = tile;
                    }
                    catch
                    {
                        Vector2 worldPos = new Vector2(col * TILE_SIZE, row * TILE_SIZE);
                        tile = new Tile(worldPos);
                        tile.IsEmpty = false;
                        tile.collide = new notSolid();
                        tile.DebugColor = Color.LightGray;
                        tiles[row, col] = tile;
                    }
                }
            }

            if (enemies.Count == 0)
            {
                LoadEnemies();
            }
        }

        public override void LoadEnemies()
        {
            enemies.Add(new MultiShotEnemy(new Vector2(500, 500)));
            enemies.Add(new OneShotEnemy(new Vector2(800, 500)));
        }
    }
}

