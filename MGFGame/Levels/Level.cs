using MGFGame.Collision;
using MGFGame.Collision.Block_Mechanics;
using System.Diagnostics;
using System.IO;
using static MGFGame.Collision.Push;


namespace MGFGame
{
    public class Level
    {
        public Tile[,] tiles { get; set; }
        public int height { get; set; }
        public int width { get; set; }

        public const int TILE_SIZE = 64;
        public List<NPC> enemies { get; set; }
        public List<MovingPlatform> movingPlatforms { get; set; }
        public List<Ladder> ladders { get; set; }

        public List<Collectable> collectables { get; set; }
        public List<Lever> levers { get; set; }
        public List<Projectile> enemyProjectiles { get; set; }


        public Level()
        {
            height = 0;
            width = 0;
            tiles = new Tile[0, 0];
            enemies = new List<NPC>();
            movingPlatforms = new List<MovingPlatform>();
            ladders = new List<Ladder>();
            collectables = new List<Collectable>();
            levers = new List<Lever>();
            enemyProjectiles = new List<Projectile>();

            BossEnemy.idleAnimation = new Texture[]
            {
                Game.textures["boss_idle_0"],
                Game.textures["boss_idle_1"],
                Game.textures["boss_idle_2"],
                Game.textures["boss_idle_3"],
                Game.textures["boss_idle_4"],
                Game.textures["boss_idle_5"],
                Game.textures["boss_idle_6"],
                Game.textures["boss_idle_7"]
            };
            BossEnemy.moveAnimation = new Texture[]
           {
                Game.textures["boss_idle_0"],
                Game.textures["boss_idle_1"],
                Game.textures["boss_idle_2"],
                Game.textures["boss_idle_3"],
                Game.textures["boss_idle_4"],
                Game.textures["boss_idle_5"],
                Game.textures["boss_idle_6"],
                Game.textures["boss_idle_7"]
           };

            MultiShotEnemy.moveAnimation = new Texture[]
            {
                Game.textures["Henchmen_Walking1"],
                Game.textures["Henchmen_Walking2"]
            };
            MultiShotEnemy.idleAnimation = new Texture[]
            {
                Game.textures["Henchmen_Walking1"],
                Game.textures["Henchmen_Walking2"]
            };

            InvulnerableEnemy.moveAnimation = new Texture[]
            {
                Game.textures["Guard_Walking_1"],
                Game.textures["Guard_Walking_2"],
                Game.textures["Guard_Walking_3"],
                Game.textures["Guard_Walking_4"]
            };
            InvulnerableEnemy.idleAnimation = new Texture[]
            {
                Game.textures["Guard_Walking_1"],
                Game.textures["Guard_Walking_2"],
                Game.textures["Guard_Walking_3"],
                Game.textures["Guard_Walking_4"]
            };

            OneShotEnemy.moveAnimation = new Texture[]
            {
                Game.textures["Bat_Moving_1"],
                Game.textures["Bat_Moving_2"],
                Game.textures["Bat_Moving_3"],
                Game.textures["Bat_Moving_4"],
                Game.textures["Bat_Moving_5"],
                Game.textures["Bat_Moving_6"],
                Game.textures["Bat_Moving_7"],
                Game.textures["Bat_Moving_8"],
                Game.textures["Bat_Moving_9"]
            };
            OneShotEnemy.idleAnimation = new Texture[]
             {
                Game.textures["Bat_Moving_1"],
                Game.textures["Bat_Moving_2"],
                Game.textures["Bat_Moving_3"],
                Game.textures["Bat_Moving_4"],
                Game.textures["Bat_Moving_5"],
                Game.textures["Bat_Moving_6"],
                Game.textures["Bat_Moving_7"],
                Game.textures["Bat_Moving_8"],
                Game.textures["Bat_Moving_9"]
            };
        }

        public void Update()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update();
            }

            foreach (MovingPlatform platform in movingPlatforms)
            {
                platform.Update();
            }

        }

        public void Draw(Vector2 cameraPos)
        {
            foreach (NPC enemy in enemies)
            {
                Vector2 enemyScreen = enemy.Position - cameraPos;
                enemy.Draw(enemyScreen);
            }

            foreach (MovingPlatform platform in movingPlatforms)
            {
                platform.Draw(cameraPos);
            }

            foreach (Ladder ladder in ladders)
            {
                ladder.Draw(cameraPos);
            }

            foreach (Collectable collectable in collectables)
            {
                collectable.Draw(cameraPos);
            }

            foreach (Lever lever in levers)
            {
                lever.Draw(cameraPos);
            }
        }

        public virtual void LoadEnemies()
        {
            return;
        }

        public virtual void ReloadLevel(string path)
        { 
            enemies.Clear();
            movingPlatforms.Clear();
            ladders.Clear();
            collectables.Clear();
            levers.Clear();
            enemyProjectiles.Clear();
            LoadLevel(path);
        }

        public virtual void LoadLevel(string path)
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

            if (!File.Exists(path)) return;

            string[] pixels = File.ReadAllLines(path);

            height = pixels.Length;
            width = pixels[0].Split(',').Length;

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

                        if (val == -1 || val == 15)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["sky"];
                            tile.IsEmpty = false;
                            tile.collide = new notSolid();
                        }
                        else if (val == 4)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["ladder"];
                            tile.IsEmpty = false;
                            tile.collide = new LadderCollision();
                        }

                        else if (val == 1610612749 || val == 13 || val == -1610612723 || val == -1073741811 || val == 1610612743 || val == 8 || val == 16)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["Walls(brick)"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 1610612742 || val == 6 || val == -1073741818 || val == -1610612730 || val == -536870906)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["Pipexfan"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 16)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["blockWall"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 3)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["glass"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }

                        else if (val == 9 || val == 1 || val == 16)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["Cave"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 2)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["CrackedWood"];
                            tile.IsEmpty = false;
                            tile.collide = new Push(tile);
                        }
                        else if (val == 0)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["Camera"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 1610612750 || val == -1610612722)
                        {
                            tile = new Tile(worldPos);
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                            levers.Add(new Lever(worldPos));
                        }
                        else if (val == 5)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["laser"];
                            tile.IsEmpty = false;
                            tile.collide = new DamageCollision(tile);
                        }
                        else if (val == 12)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["coin0"];
                            tile.IsEmpty = false;
                            tile.collide = new CollectibleCollision(tile);
                        }
                        else if (val == 11)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["necklace0"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 17)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["box"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 10)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["water"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else if (val == 1610612743 || val == 7 || val == -1073741817)
                        {
                            tile = new Tile(worldPos);
                            tile.Texture = Game.textures["rope"];
                            tile.IsEmpty = false;
                            tile.collide = new Solid(tile);
                        }
                        else
                        {
                            tile = new Tile(worldPos);
                            tile.IsEmpty = true;
                            tile.collide = new notSolid();
                        }

                        tiles[row, col] = tile;
                    }


                    catch
                    {
                        Vector2 worldPos = new Vector2(col * 64, row * 64);

                        tile = new Tile(worldPos);
                        tile.IsEmpty = true;
                        tile.collide = new notSolid();

                        tiles[row, col] = tile;
                    }
                }
            }
            if (enemies.Count == 0)
            {
                LoadEnemies();
            }
        }
    }
}
