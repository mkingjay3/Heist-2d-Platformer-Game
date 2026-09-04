using MGFGame.Collision;
using MGFGame.Collision.Block_Mechanics;
using MGFGame.Entities;
using MGFGame.Levels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MGFGame
{
    /// <summary>
    /// Entry point for the game.
    /// </summary>
    class Game : GameBase
    {
        Font font;
        Font titleFont;
        public static Level[] levels;
        public static int level;
        Camera camera;
        public static Player player;
        Screen scream;
        Leaderboard leaderboard;

        // Screen management
        GameState currentState;
        StartScreen startScreen;
        RulesScreen rulesScreen;
        CreditsScreen creditsScreen;
        EndScreen endScreen;

        bool clearConfirmation = false; // for double press C confirmation

        // default player spawn]
        Vector2 spawn;
        Vector2[] spawnP = new Vector2[5];
        public const int TILE_SIZE = 64;

        public static Dictionary<string, Texture> textures;

        public Game()
        {
            Title = "The Heist";
            Resolution = new(1280, 720);
        }

        public override void Initialize()
        {
            levels = new Level[5];
            level = 0;
            currentState = GameState.StartScreen; // Start with the start screen
        }

        public override void LoadContent()
        {
            textures = new Dictionary<string, Texture>
            {
                {"Guard_Walking_1", Engine.LoadTexture("Guard_Walking/Guard_Walking_1.png") },
                {"Guard_Walking_2", Engine.LoadTexture("Guard_Walking/Guard_Walking_2.png") },
                { "Guard_Walking_3", Engine.LoadTexture("Guard_Walking/Guard_Walking_3.png") },
                { "Guard_Walking_4", Engine.LoadTexture("Guard_Walking/Guard_Walking_4.png") },
                { "Henchmen_Melee1", Engine.LoadTexture("Henchmen_Melee/Henchmen_Melee1.png") },
                { "Henchmen_Melee2", Engine.LoadTexture("Henchmen_Melee/Henchmen_Melee2.png") },
                { "Henchmen_Walking1", Engine.LoadTexture("Henchmen_Walking/Henchmen_Walking1.png") },
                { "Henchmen_Walking2", Engine.LoadTexture("Henchmen_Walking/Henchmen_Walking2.png") },
                { "boss_idle_0", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_0.png") },
                { "boss_idle_1", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_1.png") },
                { "boss_idle_2", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_2.png") },
                { "boss_idle_3", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_3.png") },
                { "boss_idle_4", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_4.png") },
                { "boss_idle_5", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_5.png") },
                { "boss_idle_6", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_6.png") },
                { "boss_idle_7", Engine.LoadTexture("Mafia - Idle/mafia_boss_idle_7.png") },
                { "blockWall", Engine.LoadTexture("blockWall.png") },
                { "box", Engine.LoadTexture("box.png") },
                { "building_frame", Engine.LoadTexture("building frame.png") },
                { "Camera", Engine.LoadTexture("Camera.png") },
                { "Cave", Engine.LoadTexture("Cave.png") },
                { "coin0", Engine.LoadTexture("coin0.png") },
                { "coin1", Engine.LoadTexture("coin1.png") },
                { "coin2", Engine.LoadTexture("coin2.png") },
                { "CrackedWood", Engine.LoadTexture("CrackedWood.png") },
                { "glass", Engine.LoadTexture("glass.png") },
                { "ladder", Engine.LoadTexture("ladder.png") },
                { "laser", Engine.LoadTexture("laser base.png") },
                { "lever", Engine.LoadTexture("lever.png") },
                { "necklace0", Engine.LoadTexture("necklace0.png") },
                { "necklace1", Engine.LoadTexture("necklace1.png") },
                { "necklace2", Engine.LoadTexture("necklace2.png") },
                { "Pipexfan", Engine.LoadTexture("Pipexfan.png") },
                { "rope", Engine.LoadTexture("rope.png") },
                { "sky", Engine.LoadTexture("sky.png") },
                { "Stalectite", Engine.LoadTexture("Stalectite.png") },
                { "Walls(brick)", Engine.LoadTexture("Walls(brick).png") },
                { "water", Engine.LoadTexture("water.png") },
                { "heart", Engine.LoadTexture("heart.png") },
                { "armor", Engine.LoadTexture("armor.png") },
                { "Bat_Moving_1", Engine.LoadTexture("Bat_Moving/Bat (moving)-1.png.png") },
                { "Bat_Moving_2", Engine.LoadTexture("Bat_Moving/Bat (moving)-2.png.png") },
                { "Bat_Moving_3", Engine.LoadTexture("Bat_Moving/Bat (moving)-3.png.png") },
                { "Bat_Moving_4", Engine.LoadTexture("Bat_Moving/Bat (moving)-4.png.png") },
                { "Bat_Moving_5", Engine.LoadTexture("Bat_Moving/Bat (moving)-5.png.png") },
                { "Bat_Moving_6", Engine.LoadTexture("Bat_Moving/Bat (moving)-6.png.png") },
                { "Bat_Moving_7", Engine.LoadTexture("Bat_Moving/Bat (moving)-7.png.png") },
                { "Bat_Moving_8", Engine.LoadTexture("Bat_Moving/Bat (moving)-8.png.png") },
                { "Bat_Moving_9", Engine.LoadTexture("Bat_Moving/Bat (moving)-9.png.png") },
                { "cave_background", Engine.LoadTexture("CaveScene/back.png") },
                { "one_way", Engine.LoadTexture("CaveScene/one way.png") },
                { "lava_ground", Engine.LoadTexture("CaveScene/lava ground.png") },
                { "lava_block", Engine.LoadTexture("CaveScene/lava block.png") },
                { "cave_floor", Engine.LoadTexture("CaveScene/cave floor.png") },
                { "dirt_brown", Engine.LoadTexture("CaveScene/dirt brown.png") },
                { "light_purple", Engine.LoadTexture("CaveScene/light purple.png") },
                { "dark_purple", Engine.LoadTexture("CaveScene/dark purple.png") },
                { "smoke", Engine.LoadTexture("smoke.png") },
                { "purple_moving", Engine.LoadTexture("CaveScene/purple moving.png") },
                { "red brick 1", Engine.LoadTexture("Building Level/red brick 1.png") },
                { "red brick 2", Engine.LoadTexture("Building Level/red brick 2.png") },
                { "red brick 3", Engine.LoadTexture("Building Level/red brick 3.png") },
                { "water floor", Engine.LoadTexture("Building Level/water floor.png") },
                { "brown brick", Engine.LoadTexture("Building Level/zbrown brick.png") },
                { "brown brick 2", Engine.LoadTexture("Building Level/zbrown brick 2.png") },
                { "building rside corner roof", Engine.LoadTexture("Building Level/zbuilding rside corner roof.png") },
                { "building rside roof", Engine.LoadTexture("Building Level/zbuilding rside roof.png") },
                { "building lside corner roof", Engine.LoadTexture("Building Level/zbuilding lside corner roof.png") },
                { "building lside roof", Engine.LoadTexture("Building Level/zbuilding lside roof.png") },
                { "building tan rside", Engine.LoadTexture("Building Level/zbuilding tan rside.png") },
                { "building top roof reverse", Engine.LoadTexture("Building Level/zbuilding top roof reverse.png") },
                { "building top roof", Engine.LoadTexture("Building Level/zbuilding top roof.png") },
                { "dark brick", Engine.LoadTexture("Building Level/zdark brick.png") },
                { "hazard", Engine.LoadTexture("Building Level/zhazard.png") },
                { "tan building lside", Engine.LoadTexture("Building Level/ztan building lside.png") },
                { "window bot left", Engine.LoadTexture("Building Level/zwindow bot left.png") },
                { "window bot right", Engine.LoadTexture("Building Level/zwindow bot right.png") },
                { "window mid left", Engine.LoadTexture("Building Level/zwindow mid left.png") },
                { "window mid right", Engine.LoadTexture("Building Level/zwindow mid right.png") },
                { "window top left", Engine.LoadTexture("Building Level/zwindow top left.png") },
                { "window top right", Engine.LoadTexture("Building Level/zwindow top right.png") },
                { "gateway border", Engine.LoadTexture("Building Level/gateway border.png") },
                { "gateway", Engine.LoadTexture("Building Level/gateway.png") },
                { "ledge", Engine.LoadTexture("ledge.png") },
                { "ledge2", Engine.LoadTexture("Building Level/ledge 2.png") },
                { "chimney base", Engine.LoadTexture("Building Level/chimney base.png") },
                { "chimney tip", Engine.LoadTexture("Building Level/chimney tip.png") },
                { "building rside corner roof reversed", Engine.LoadTexture("Building Level/zbuilding rside corner roof reversed.png") },
                { "building lside corner roof reversed", Engine.LoadTexture("Building Level/zbuilding lside corner roof reversed.png") },
                { "buildings_background", Engine.LoadTexture("Building Level/buildings background.png") },
                { "Player_Walk_0", Engine.LoadTexture("WalkingChar/Walking Char0.png") },
                { "Player_Walk_1", Engine.LoadTexture("WalkingChar/Walking Char1.png") },
                { "Player_Walk_2", Engine.LoadTexture("WalkingChar/Walking Char2.png") },
                { "Player_Walk_3", Engine.LoadTexture("WalkingChar/Walking Char3.png") },
                { "Player_Walk_4", Engine.LoadTexture("WalkingChar/Walking Char4.png") },
                { "Player_Shoot_0", Engine.LoadTexture("ShootChar/ShootChar0.png") },
                { "Player_Shoot_1", Engine.LoadTexture("ShootChar/ShootChar1.png") },
                { "Player_Shoot_2", Engine.LoadTexture("ShootChar/ShootChar2.png") },
                { "Player_Shoot_3", Engine.LoadTexture("ShootChar/ShootChar3.png") },
                { "Player_Shoot_4", Engine.LoadTexture("ShootChar/ShootChar4.png") },
                { "Player_Shoot_5", Engine.LoadTexture("ShootChar/ShootChar5.png") },
                { "Player_Climb_0", Engine.LoadTexture("ClimbingChar/sprite_0.png") },
                { "Player_Climb_1", Engine.LoadTexture("ClimbingChar/sprite_1.png") },
                { "Player_Climb_2", Engine.LoadTexture("ClimbingChar/sprite_2.png") },
                { "Player_Climb_3", Engine.LoadTexture("ClimbingChar/sprite_3.png") },
                { "Player_Climb_4", Engine.LoadTexture("ClimbingChar/sprite_4.png") },
                { "Player_Glide_0", Engine.LoadTexture("GlidingChar/Gliding0.png") },
                { "Player_Glide_1", Engine.LoadTexture("GlidingChar/Gliding1.png") },
                { "Player_Glide_2", Engine.LoadTexture("GlidingChar/Gliding2.png") },
                { "Player_Glide_3", Engine.LoadTexture("GlidingChar/Gliding3.png") },
                { "Player_Glide_4", Engine.LoadTexture("GlidingChar/Gliding4.png") },
                { "Player_Glide_5", Engine.LoadTexture("GlidingChar/Gliding5.png") },
                { "Player_Fight_0", Engine.LoadTexture("FightChar/sprite_0.png") },
                { "Player_Fight_1", Engine.LoadTexture("FightChar/sprite_1.png") },
                { "Player_Fight_2", Engine.LoadTexture("FightChar/sprite_2.png") },
                { "Player_Fight_3", Engine.LoadTexture("FightChar/sprite_3.png") },
                { "Player_Fight_4", Engine.LoadTexture("FightChar/sprite_4.png") },
                { "Player_Slide_0", Engine.LoadTexture("SlidingChar/SlidingFinal0.png") },
                { "Player_Slide_1", Engine.LoadTexture("SlidingChar/SlidingFinal1.png") },
                { "Player_Slide_2", Engine.LoadTexture("SlidingChar/SlidingFinal2.png") },
                { "Player_Slide_3", Engine.LoadTexture("SlidingChar/SlidingFinal3.png") },
                { "Bullet", Engine.LoadTexture("Bullet.png") },
                { "powerup", Engine.LoadTexture("powerup.png") },
                //Boss Level textures
                { "laser tip", Engine.LoadTexture("laser tip.png") },
                { "laser tip reversed", Engine.LoadTexture("laser tip reversed.png") },
                { "metal base", Engine.LoadTexture("Boss Level/metal base.png") },
                { "metal floor", Engine.LoadTexture("Boss Level/metal floor.png") },
                { "conveyer 1", Engine.LoadTexture("Boss Level/conveyer 1.png") },
                { "conveyer 2", Engine.LoadTexture("Boss Level/conveyer 2.png") },
                { "conveyer 3", Engine.LoadTexture("Boss Level/conveyer 3.png") },
                { "conveyer 4", Engine.LoadTexture("Boss Level/conveyer 4.png") },
                { "black box tl", Engine.LoadTexture("Boss Level/black box tl.png") },
                { "black box tr", Engine.LoadTexture("Boss Level/black box tr.png") },
                { "black box bl", Engine.LoadTexture("Boss Level/black box bl.png") },
                { "black box br", Engine.LoadTexture("Boss Level/black box br.png") },
                { "brownbox tl", Engine.LoadTexture("Boss Level/brown box tl.png") },
                { "brownbox tr", Engine.LoadTexture("Boss Level/brown box tr.png") },
                { "brownbox bl", Engine.LoadTexture("Boss Level/brown box bl.png") },
                { "brownbox br", Engine.LoadTexture("Boss Level/brown box br.png") },
                { "boss background", Engine.LoadTexture("Boss Level/boss background.png") },
                { "black box", Engine.LoadTexture("Boss Level/black box.png") },
                //Museum Level Textures
                { "museum floor", Engine.LoadTexture("Museum Level/museum floor.png") },
                { "interval platform", Engine.LoadTexture("Museum Level/interval.png") },
                { "museum background", Engine.LoadTexture("Museum Level/museum background.png") },
                { "chim mid", Engine.LoadTexture("chim mid.png") },
                { "chim top", Engine.LoadTexture("chim top.png") },
                { "coin0-1", Engine.LoadTexture("coin0-1.png.png") },
                { "left one", Engine.LoadTexture("left one.png") },
                { "left red wall", Engine.LoadTexture("left red wall.png") },
                { "mid one", Engine.LoadTexture("mid one.png") },
                { "movingwood", Engine.LoadTexture("movingwood.png") },
                { "red wall right", Engine.LoadTexture("red wall right.png") },
                { "red wall top", Engine.LoadTexture("red wall top.png") },
                { "red window", Engine.LoadTexture("red window.png") },
                { "right one", Engine.LoadTexture("right one.png") },
                { "rocks", Engine.LoadTexture("rocks.png") },
                { "sign 1", Engine.LoadTexture("sign 1.png") },
                { "sign 2", Engine.LoadTexture("sign 2.png") },
                { "sign 3", Engine.LoadTexture("sign 3.png") },
                { "sign 4", Engine.LoadTexture("sign 4.png") },
                { "spike", Engine.LoadTexture("spike.png") },
                { "top left red", Engine.LoadTexture("top left red.png") },
                { "top right red wall", Engine.LoadTexture("top right red wall.png") },
                { "door", Engine.LoadTexture("door.png") },
            };

            font = Engine.LoadFont("Retro Gaming.ttf", 24);
            titleFont = Engine.LoadFont("Retro Gaming.ttf", 48);

            // Initialize image-based screens
            startScreen = new StartScreen();
            startScreen.LoadContent();

            rulesScreen = new RulesScreen();
            rulesScreen.LoadContent();

            creditsScreen = new CreditsScreen();
            creditsScreen.LoadContent();

            leaderboard = new Leaderboard();
            leaderboard.LoadContent(font, titleFont);

            endScreen = new EndScreen();
            endScreen.LoadContent(font);

            camera = new Camera();
            scream = new Screen(1000, 300, 1000, 300);

            spawnP[0] = new Vector2(128, 1700);
            spawnP[1] = new Vector2(188, 1420);
            spawnP[2] = new Vector2(128, 900);
            spawnP[3] = new Vector2(310, 996);
            spawnP[4] = new Vector2(4500, 1600);

            // Load CSV-based levels
            levels[0] = new Level1();
            levels[0].LoadLevel("level1.csv");

            levels[1] = new Level2();
            levels[1].LoadLevel("level2.csv");

            levels[2] = new Level3();
            levels[2].LoadLevel("level3.csv");

            levels[3] = new Level4();
            levels[3].LoadLevel("boss.csv"); // boss


            DebugLevel debugLevel = new DebugLevel();
            debugLevel.LoadDebugLevel("Debug Level 2.csv");
            levels[4] = debugLevel;

            LoadPlayer();

            SoundManager.LoadSounds();
        }

        public void LoadPlayer()
        {
            player = new Player(spawnP[0]);
            player.idleAnimation = new Texture[]
            {
                textures["Player_Walk_0"]
            };
            player.moveAnimation = new Texture[]
            {
                textures["Player_Walk_0"],
                textures["Player_Walk_1"],
                textures["Player_Walk_2"],
                textures["Player_Walk_3"],
                textures["Player_Walk_4"]
            };
            player.shootAnimation = new Texture[]
            {
                textures["Player_Shoot_0"],
                textures["Player_Shoot_1"],
                textures["Player_Shoot_2"],
                textures["Player_Shoot_3"],
                textures["Player_Shoot_4"],
                textures["Player_Shoot_5"]
            };
            player.climbAnimation = new Texture[]
            {
                textures["Player_Climb_0"],
                textures["Player_Climb_1"],
                textures["Player_Climb_2"],
                textures["Player_Climb_3"],
                textures["Player_Climb_4"]
            };
            player.glideAnimation = new Texture[]
            {
                textures["Player_Glide_0"],
                textures["Player_Glide_1"],
                textures["Player_Glide_2"],
                textures["Player_Glide_3"],
                textures["Player_Glide_4"],
                textures["Player_Glide_5"]
            };
            player.fightAnimation = new Texture[]
            {
                textures["Player_Fight_0"],
                textures["Player_Fight_1"],
                textures["Player_Fight_2"],
                textures["Player_Fight_3"],
                textures["Player_Fight_4"]
            };
            player.slideAnimation = new Texture[]
            {
                textures["Player_Slide_0"],
                textures["Player_Slide_1"],
                textures["Player_Slide_2"],
                textures["Player_Slide_3"]
            };
            Projectile.texture = textures["Bullet"];
        }

        private void DrawHealth()
        {
            Vector2 heartStartPos = new Vector2(1000, 50);
            Vector2 heartSize = new Vector2(80, 80);
            float heartSpacing = 45;

            for (int i = 0; i < player.Health; i++)
            {
                Vector2 heartPos = new Vector2(heartStartPos.X + (i * heartSpacing), heartStartPos.Y);
                Engine.DrawTexture(textures["heart"], heartPos, size: heartSize, scaleMode: TextureScaleMode.Nearest);
            }
        }

        private void DrawArmor()
        {
            Vector2 armorStartPos = new Vector2(1000, 130);
            Vector2 armorSize = new Vector2(80, 80);
            float armorSpacing = 45;

            for (int i = 0; i < player.Armor; i++)
            {
                Vector2 armorPos = new Vector2(armorStartPos.X + (i * armorSpacing), armorStartPos.Y);
                Engine.DrawTexture(textures["armor"], armorPos, size: armorSize, scaleMode: TextureScaleMode.Nearest);
            }
        }

        private void DrawAmmo()
        {
            Engine.DrawString($"Ammo: {player.CurrentAmmo}", new Vector2(10, 40), Color.White, font);
            
            if (player.CurrentAmmo < player.MaxAmmo)
            {
                float progress = player.AmmoRechargeTimer / player.AmmoRechargeTime;
                Engine.DrawRectSolid(new Bounds2(10, 70, 100 * progress, 5), Color.White);
            }
        }

        public void LevelChange()
        {
            if (Engine.GetKeyDown(Key.NumRow1))
            {
                level = 0;
                spawn = spawnP[0];
                levels[0].ReloadLevel("level1.csv");
                camera.Position = Vector2.Zero;
                player.Position = spawn;
                player.Velocity = Vector2.Zero;
                player.IsOnGround = false;
            }

            if (Engine.GetKeyDown(Key.NumRow2))
            {
                level = 1;
                spawn = spawnP[1];
                levels[1].ReloadLevel("level2.csv");
                camera.Position = Vector2.Zero;
                player.Position = spawn;
                player.Velocity = Vector2.Zero;
                player.IsOnGround = false;
            }

            if (Engine.GetKeyDown(Key.NumRow3))
            {
                if (Engine.GetKeyDown(Key.NumRow3))
                {
                    level = 2;
                    spawn = spawnP[2];
                    levels[2].enemies.Clear();
                    levels[2].movingPlatforms.Clear();
                    levels[2].ladders.Clear();
                    levels[2].collectables.Clear();
                    levels[2].LoadLevel("level3.csv");
                    camera.Position = Vector2.Zero;
                    player.Position = spawn;
                    player.Velocity = Vector2.Zero;
                    player.IsOnGround = false;
                }
            }

            if (Engine.GetKeyDown(Key.B))
            {
                level = 3;
                spawn = spawnP[3];
                levels[3].ReloadLevel("boss.csv");
                camera.Position = Vector2.Zero;
                Debug.WriteLine($"B key pressed: Loading level 3, spawning at {spawn}");
                player.Position = spawn;
                player.Velocity = Vector2.Zero;
                player.IsOnGround = false;
            }

            if (Engine.GetKeyDown(Key.NumRow5))
            {
                level = 4;
                spawn = spawnP[4];
                camera.Position = Vector2.Zero;
                player.Position = spawn;
                player.Velocity = Vector2.Zero;
                player.IsOnGround = false;
            }
        }

        public override void Update()
        {
            // Handle different game states
            switch (currentState)
            {
                case GameState.StartScreen:
                    UpdateStartScreen();
                    break;
                case GameState.Playing:
                    UpdateGameplay();
                    break;
                case GameState.Leaderboard:
                case GameState.LeaderboardFromEnd:
                    UpdateLeaderboard();
                    break;
                case GameState.Rules:
                    UpdateRules();
                    break;
                case GameState.Credits:
                    UpdateCredits();
                    break;
                case GameState.EndScreen:
                    UpdateEndScreen();
                    break;
            }
        }

        private void UpdateStartScreen()
        {
            GameState newState = startScreen.Update();

            // Start background music
            SoundManager.PlayBackgroundMusic();

            if (newState != GameState.StartScreen)
            {
                currentState = newState;

                // If starting the game, initialize player and camera
                if (newState == GameState.Playing)
                {
                    level = 0;
                    spawn = spawnP[0];
                    camera.Position = Vector2.Zero;
                    player.Position = spawn;
                    player.Velocity = Vector2.Zero;
                    player.IsOnGround = false;
                    player.Health = 5;
                    player.Armor = 3;
                    player.CollectedItems = 0;
                }

                // Load leaderboard if needed
                if (newState == GameState.Leaderboard)
                {
                    leaderboard.LoadScores();
                }
            }

            startScreen.Draw();
        }

        private void UpdateRules()
        {
            GameState newState = rulesScreen.Update();
            if (newState != GameState.Rules)
            {
                currentState = newState;
            }
            rulesScreen.Draw();
        }

        private void UpdateCredits()
        {
            GameState newState = creditsScreen.Update();
            if (newState != GameState.Credits)
            {
                currentState = newState;
            }
            creditsScreen.Draw();
        }

        private void UpdateEndScreen()
        {
            GameState newState = endScreen.Update();

            if (newState != GameState.EndScreen)
            {
                // Handle the three button actions
                if (newState == GameState.LeaderboardFromEnd)
                {
                    // Going to leaderboard from end screen
                    leaderboard.LoadScores();
                    currentState = GameState.LeaderboardFromEnd;
                }
                else if (newState == GameState.StartScreen)
                {
                    // Play Again - restart current level
                    spawn = spawnP[level];
                    
                    string path;
                    if (level == 0) path = "level1.csv";
                    else if (level == 1) path = "level2.csv";
                    else if (level == 2) path = "level3.csv";
                    else if (level == 3) path = "boss.csv";
                    else path = "Debug Level 2.csv";

                    levels[level].ReloadLevel(path);
                    camera.Position = Vector2.Zero;
                    player.Position = spawn;
                    player.Velocity = Vector2.Zero;
                    player.IsOnGround = false;
                    player.Health = 5;
                    player.Armor = 3;
                    player.CollectedItems = 0;
                    currentState = GameState.Playing;
                }
            }

            DrawGameBackground();

            // Draw the end screen popup on top
            endScreen.Draw();
        }

        private void UpdateLeaderboard()
        {
            if (Engine.GetKeyDown(Key.C))
            {
                if (clearConfirmation)
                {
                    ScoreManager.ClearLeaderboard();
                    leaderboard.LoadScores(); // refresh the leaderboard display
                    clearConfirmation = false;
                    Console.WriteLine("Leaderboard cleared!");
                }
                else
                {
                    // first press - show confirmation
                    clearConfirmation = true;
                }
            }

            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                Vector2 mousePos = Engine.MousePosition;
                // Return button region
                Bounds2 returnButton = new Bounds2(1000, 600, 280, 120);
                if (returnButton.Contains(mousePos))
                {
                    // If we came from end screen, return to end screen
                    if (currentState == GameState.LeaderboardFromEnd)
                    {
                        currentState = GameState.EndScreen;
                    }
                    else
                    {
                        currentState = GameState.StartScreen;
                    }
                    clearConfirmation = false;
                }
            }

            leaderboard.Draw();
        }

        private void UpdateGameplay()
        {
            // Allow returning to menu with ESC
            if (Engine.GetKeyDown(Key.Escape))
            {
                currentState = GameState.StartScreen;
                return;
            }




            LevelChange();
            IntervalPlatformCollision.UpdateTimer();
            IntervalDamageCollision.UpdateTimer();
            Lever.Update(); 
            Level curr = levels[level];

            if (Engine.GetKeyDown(Key.F))
            {
                player.Melee();
                SoundManager.PlaySword();
            }

            Vector2 mouseScreen = Engine.MousePosition;
            Vector2 mouseWorld = mouseScreen + camera.Position;

            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                player.Shoot(mouseWorld);
                SoundManager.PlayLaserShoot();
            }


            if (player.IsOnLadder)
            {
                if (Engine.GetKeyHeld(Key.W))
                {
                    player.ClimbUp();
                }
                else if (Engine.GetKeyHeld(Key.S))
                {
                    player.ClimbDown();

                }
                if (Engine.GetKeyDown(Key.Space))
                {
                    if (Engine.GetKeyHeld(Key.A))
                    {
                        // Jump off to the left
                        player.ExitLadder();
                        player.Velocity = new Vector2(-player.Speed, -15f);
                    }
                    else if (Engine.GetKeyHeld(Key.D))
                    {
                        // Jump off to the right
                        player.ExitLadder();
                        player.Velocity = new Vector2(player.Speed, -15f);
                    }
                    else
                    {
                        player.Jump();
                    }
                }
            }
            else
            {
                // Normal movement
                if (Engine.GetKeyDown(Key.Space))
                {
                    if (player.IsOnWall && !player.IsOnGround)
                    {
                        player.WallJump();
                    }
                    else
                    {
                        player.Jump();
                    }
                }
                if (!Engine.GetKeyHeld(Key.LeftAlt))
                {
                    if (Engine.GetKeyHeld(Key.Space))
                    {
                        player.ContinueJump();
                    }
                    if (Engine.GetKeyUp(Key.Space))
                    {
                        player.EndJump();
                    }
                    if (Engine.GetKeyHeld(Key.A))
                    {
                        player.MoveLeft();
                    }
                    if (Engine.GetKeyHeld(Key.D))
                    {
                        player.MoveRight();
                    }
                }


                if (Engine.GetKeyHeld(Key.LeftShift))
                {
                    player.StartGlide();
                }
                else
                {
                    player.StopGlide();
                }
                if (Engine.GetKeyDown(Key.LeftControl))
                {
                    player.StartSlide();
                }

                if (Engine.GetKeyDown(Key.C))
                {
                    player.NoClip = !player.NoClip;
                }

                if (player.NoClip)
                {
                    if (Engine.GetKeyHeld(Key.W)) player.MoveUp();
                    if (Engine.GetKeyHeld(Key.S)) player.MoveDown();
                }

            }

            player.Update();
            curr.Update();

            CheckCollisions(player, curr.enemies, curr);

            if (player.Position.Y > curr.height * TILE_SIZE + 100)
            {
                player.Position = spawn;
                player.Velocity = Vector2.Zero;
                player.IsOnGround = false;
                camera.Position = new Vector2(spawn.X - 640, spawn.Y - 450);
            }

            if (player.NextLevel >= 0 && player.NextLevel < levels.Length)
            {
                level = player.NextLevel;
                spawn = spawnP[level];
                Debug.WriteLine($"Transitioning to level {level}, spawning at {spawn}");
                player.Position = spawn;
                player.Velocity = Vector2.Zero;
                player.IsOnGround = false;
                player.NextLevel = -1;
                camera.Position = Vector2.Zero; // Match key press logic
            }

            // Debug camera fly mode 
            if (Engine.GetKeyHeld(Key.LeftAlt))
            {
                if (Engine.GetKeyHeld(Key.W))
                {
                    camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 15);
                }
                if (Engine.GetKeyHeld(Key.S))
                {
                    camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 15);
                }
                if (Engine.GetKeyHeld(Key.A))
                {
                    camera.Position = new Vector2(camera.Position.X - 15, camera.Position.Y);
                }
                if (Engine.GetKeyHeld(Key.D))
                {
                    camera.Position = new Vector2(camera.Position.X + 15, camera.Position.Y);
                }
            }
            else
            {
                
                camera.movingPlayer(player.Position);
            }

            if (player.isDead())
            {
                endScreen.Show(false, player.CollectedItems);
                currentState = GameState.EndScreen;
                return;
            }

            if (level == 3 && curr.enemies.Count == 0)
            {
                endScreen.Show(true, player.CollectedItems); 
                currentState = GameState.EndScreen;
                return;
            }

            // Manual reset level with R key
            if (Engine.GetKeyDown(Key.R))
            {
                string path;
                if (level == 0)
                {
                    path = "level1.csv";
                }
                else if (level ==1)
                {
                    path = "level2.csv";
                }
                else if (level == 2)
                {
                    path = "level3.csv";
                }
                else if (level == 3)
                {
                    path = "boss.csv";
                }
                else
                {
                    path = $"Level {level + 1} WITH ASSETS.csv";
                }
                levels[level].LoadLevel(path);
                camera.Position = Vector2.Zero;
                player.Position = spawn;
                player.Velocity = Vector2.Zero;
                player.IsOnGround = false;
                player.Health = 5;
                player.Armor = 3;
            }

            // bullets
            for (int i = player.ActiveProjectiles.Count - 1; i >= 0; i--)
            {
                //tiles
                Projectile proj = player.ActiveProjectiles[i];
                bool hitSomething = false;
                int projStartX = (int)(proj.Position.X / TILE_SIZE) - 1;
                int projEndX = (int)(proj.Position.X / TILE_SIZE) + 2;
                int projStartY = (int)(proj.Position.Y / TILE_SIZE) - 1;
                int projEndY = (int)(proj.Position.Y / TILE_SIZE) + 2;

                for (int y = projStartY; y <= projEndY && !hitSomething; y++)
                {
                    for (int x = projStartX; x <= projEndX && !hitSomething; x++)
                    {
                        if (x < 0 || y < 0 || x >= curr.width || y >= curr.height)
                            continue;

                        Tile t = curr.tiles[y, x];
                        if (!t.IsEmpty && t.collide != null && t.collide.isSolid)
                        {
                            Bounds2 projBounds = proj.GetBounds();
                            Bounds2 tileBounds = t.GetBounds();

                            if (projBounds.Overlaps(tileBounds))
                            {
                                hitSomething = true;
                                player.ActiveProjectiles.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                // enemies
                if (!hitSomething)
                {
                    for (int e = curr.enemies.Count - 1; e >= 0; e--)
                    {
                        NPC enemy = curr.enemies[e];
                        Bounds2 projBounds = proj.GetBounds();
                        Bounds2 enemyBounds = enemy.GetBounds();

                        if (projBounds.Overlaps(enemyBounds))
                        {
                            curr.enemies[e].takeDamage();
                            if (curr.enemies[e].isDead())
                            {
                                curr.enemies.RemoveAt(e);
                                Debug.WriteLine("dead");
                            }
                            player.ActiveProjectiles.RemoveAt(i);
                            hitSomething = true;
                            break;
                        }
                    }
                }
            }

            // enemy bullets
            for (int i = curr.enemyProjectiles.Count - 1; i >= 0; i--)
            {
                Projectile proj = curr.enemyProjectiles[i];
                proj.Update();

                if (!proj.IsAlive())
                {
                    curr.enemyProjectiles.RemoveAt(i);
                    continue;
                }

                bool hitSomething = false;

                // tiles
                int projStartX = (int)(proj.Position.X / TILE_SIZE) - 1;
                int projEndX = (int)(proj.Position.X / TILE_SIZE) + 2;
                int projStartY = (int)(proj.Position.Y / TILE_SIZE) - 1;
                int projEndY = (int)(proj.Position.Y / TILE_SIZE) + 2;

                for (int y = projStartY; y <= projEndY && !hitSomething; y++)
                {
                    for (int x = projStartX; x <= projEndX && !hitSomething; x++)
                    {
                        if (x < 0 || y < 0 || x >= curr.width || y >= curr.height)
                            continue;

                        Tile t = curr.tiles[y, x];
                        if (!t.IsEmpty && t.collide != null && t.collide.isSolid)
                        {
                            Bounds2 projBounds = proj.GetBounds();
                            Bounds2 tileBounds = t.GetBounds();

                            if (projBounds.Overlaps(tileBounds))
                            {
                                hitSomething = true;
                                curr.enemyProjectiles.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                // player
                if (!hitSomething)
                {
                    Bounds2 projBounds = proj.GetBounds();
                    Bounds2 playerBounds = player.GetBounds();

                    if (projBounds.Overlaps(playerBounds))
                    {
                        player.takeDamage();
                        SoundManager.PlayHitHurt();
                        curr.enemyProjectiles.RemoveAt(i);
                        hitSomething = true;
                    }
                }
            }

            scream.update(camera);
            scream.draw(camera);

            if (level == 0)
            {
                Engine.DrawTexture(textures["buildings_background"], Vector2.Zero, size: new Vector2(1280, 720));

            }
            else if (level == 1)
            {
                Engine.DrawTexture(textures["museum background"], Vector2.Zero, size: new Vector2(1280, 720));
            }
            else if (level == 2)
            {
                Engine.DrawTexture(textures["cave_background"], Vector2.Zero, size: new Vector2(1280, 720));
            }
            else if (level == 3)
            {
                Engine.DrawTexture(textures["boss background"], Vector2.Zero, size: new Vector2(1280, 720));
            }

            //drawing tiles
            for (int y = 0; y < curr.height; y++)
            {
                for (int x = 0; x < curr.width; x++)
                {
                    Tile tile = curr.tiles[y, x];
                    if (tile.IsEmpty) continue;
                    if (tile.collide is DamageCollision && Lever.LasersDisabled)
                        continue;
                    if (tile.collide is IntervalPlatformCollision && !IntervalPlatformCollision.IsCurrentlyActive())
                        continue;
                    if (tile.collide is IntervalDamageCollision && !IntervalDamageCollision.IsCurrentlyActive())
                        continue;

                    Vector2 world = tile.Position;
                    Vector2 screen = world - camera.Position;

                    if (screen.X >= -TILE_SIZE && screen.X <= Resolution.X && screen.Y >= -TILE_SIZE && screen.Y <= Resolution.Y)
                        tile.Draw(screen);
                }
            }

            // draw level objects 
            curr.Draw(camera.Position);

            // draw player last 
            Vector2 playerScreen = player.Position - camera.Position;
            player.Draw(playerScreen);

            if (player.CooldownTimer > 0)
            {
                Engine.DrawString($"Cooldown: {player.CooldownTimer:F2}s", new Vector2(10, 10), Color.White, font);
            }
            else
            {
                Engine.DrawString("Attack Ready!", new Vector2(10, 10), Color.White, font);
            }

            foreach (var projectile in player.ActiveProjectiles)
            {
                Vector2 projScreen = projectile.Position - camera.Position;
                projectile.Draw(projScreen);
            }

            foreach (var projectile in curr.enemyProjectiles)
            {
                Vector2 projScreen = projectile.Position - camera.Position;
                projectile.Draw(projScreen);
            }

            DrawScore();
            DrawHealth();
            DrawArmor();
            DrawAmmo();
        }

        // Helper method to draw the game background
        private void DrawGameBackground()
        {
            Level curr = levels[level];

            scream.update(camera);
            scream.draw(camera);

            if (level == 2)
            {
                Engine.DrawTexture(textures["cave_background"], Vector2.Zero, size: new Vector2(1280, 720));
            }

            //drawing tiles
            for (int y = 0; y < curr.height; y++)
            {
                for (int x = 0; x < curr.width; x++)
                {
                    Tile tile = curr.tiles[y, x];
                    if (tile.IsEmpty) continue;
                    if (tile.collide is DamageCollision && Lever.LasersDisabled)
                        continue;
                    if (tile.collide is IntervalPlatformCollision && !IntervalPlatformCollision.IsCurrentlyActive())
                        continue;
                    if (tile.collide is IntervalDamageCollision && !IntervalDamageCollision.IsCurrentlyActive())
                        continue;

                    Vector2 world = tile.Position;
                    Vector2 screen = world - camera.Position;

                    if (screen.X >= -TILE_SIZE && screen.X <= Resolution.X && screen.Y >= -TILE_SIZE && screen.Y <= Resolution.Y)
                        tile.Draw(screen);
                }
            }

            // draw level objects 
            curr.Draw(camera.Position);

            // draw player last 
            Vector2 playerScreen = player.Position - camera.Position;
            player.Draw(playerScreen);

            foreach (var projectile in player.ActiveProjectiles)
            {
                Vector2 projScreen = projectile.Position - camera.Position;
                projectile.Draw(projScreen);
            }

            DrawScore();
            DrawHealth();
            DrawAmmo();
        }

        private void DrawScore()
        {
            string scoreText = $"Score: {player.CollectedItems}";
            Engine.DrawString(scoreText, new Vector2(1050, 10), Color.Yellow, font);

            Engine.DrawString("F: Melee | ESC: Menu", new Vector2(10, 680), Color.White, font);
        }

        // check tiles in a 3x3 around the player.
        void CheckCollisions(Player p, List<NPC> entities, Level level)
        {
            p.IsOnGround = false;


            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    Tile t = level.tiles[y, x];
                    if (t != null && !t.IsEmpty)
                    {
                        t.collide.ResC();
                    }
                }
            }

            int startX = (int)(p.Position.X / TILE_SIZE) - 2;
            int endX = (int)(p.Position.X / TILE_SIZE) + 3;
            int startY = (int)(p.Position.Y / TILE_SIZE) - 2;
            int endY = (int)(p.Position.Y / TILE_SIZE) + 3;

            if (!p.NoClip)
            {
                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        if (x < 0 || y < 0 || x >= level.width || y >= level.height)
                            continue;

                        Tile t = level.tiles[y, x];
                        if (!t.IsEmpty)
                        {
                            if (t.collide != null)
                            {
                                Bounds2 pb = p.GetBounds();
                                Bounds2 tb = t.GetBounds();

                                if (pb.Overlaps(tb))
                                {
                                    t.collide.touch(p, pb, tb);
                                }
                            }
                        }
                    }
                }
            }

            for (int i = entities.Count - 1; i >= 0; i--)
            {
                NPC e = entities[i];
                e.IsOnGround = false;
                Bounds2 eb;
                startX = (int)(e.Position.X / TILE_SIZE) - 2;
                endX = (int)(e.Position.X / TILE_SIZE) + 3;
                startY = (int)(e.Position.Y / TILE_SIZE) - 2;
                endY = (int)(e.Position.Y / TILE_SIZE) + 3;

                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        if (x < 0 || y < 0 || x >= level.width || y >= level.height)
                            continue;

                        Tile t = level.tiles[y, x];
                        if (!t.IsEmpty)
                        {
                            if (t.collide != null)
                            {
                                eb = e.GetBounds();
                                Bounds2 tb = t.GetBounds();

                                if (eb.Overlaps(tb))
                                {
                                    t.collide.touch(e, eb, tb);
                                }
                            }
                        }
                    }
                }
                eb = e.GetBounds();
                Bounds2 pb = p.GetBounds();
                if (eb.Overlaps(pb))
                {
                    if (p.IsAttacking)
                    {
                        e.takeDamage();
                        if (e.isDead())
                        {
                            entities.RemoveAt(i);
                            continue;
                        }
                        // Apply knockback
                        float knockbackForce = 8f;
                        Vector2 direction = e.Position - p.Position;
                        if (direction.X > 0) e.Velocity = new Vector2(knockbackForce, 0);
                        else e.Velocity = new Vector2(-knockbackForce, 0);
                        e.knockbackTimer = 0.2f;
                    }
                    else if (!p.IsInvulnerable && !p.NoClip)
                    {
                        p.takeDamage();
                        SoundManager.PlayHitHurt();
                    }
                //    else
                //    {
                //        p.takeDamage();
                //        SoundManager.PlayHitHurt();
                //    }
                    
                    if (!p.IsAttacking && !p.NoClip)
                    {
                        p.touch(e, pb, eb);
                    }
                }
                
                // Check if NPC died from tile collision (e.g. laser)
                if (e.isDead())
                {
                    entities.RemoveAt(i);
                }
            }
            for (int c = level.collectables.Count - 1; c >= 0; c--)
            {
                Collectable collectable = level.collectables[c];
                collectable.HandleCollision(p);
                if (collectable.IsCollected)
                {
                    SoundManager.PlayPickupCoin();
                    level.collectables.RemoveAt(c);
                }
            }

            // Check moving platforms
            foreach (MovingPlatform platform in level.movingPlatforms)
            {
                platform.HandleCollision(p);
            }

            foreach(Lever lever in level.levers)
            {
                lever.HandleCollision(p);
            }

            // Check ladders
            bool touchingAnyLadder = false;
            p.IsTouchingLadder = false;
            p.CurrentLadderTop = float.MaxValue;

            foreach (Ladder ladder in level.ladders)
            {
                Bounds2 playerBounds = p.GetBounds();
                Bounds2 ladderBounds = ladder.GetBounds();

                if (playerBounds.Overlaps(ladderBounds))
                {
                    touchingAnyLadder = true;
                    ladder.HandleCollision(p);
                }
            }

            if (!touchingAnyLadder)
            {
                p.WantsToExitLadder = false;
                p.IsOnLadder = false;
                p.CurrentLadderTop = float.MinValue;
            }
        }
    }
}
