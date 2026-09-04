using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Entities;

namespace MGFGame
{
    public class BossEnemy : NPC
    {
        public int Direction { get; set; } = 1;
        public int MaxHealth { get; set; } = 10;
        private bool isPhaseTwo = false;
        private bool canMove = false;

        private enum FireCycle { Rapid, Regular, NoFire }
        private FireCycle currentCycle = FireCycle.Rapid;
        private int shotsLeft = 8;
        private float cycleTimer = 0f;
        private float shootTimer = 0f;

        private float range = 2000f;

        public float MoveTime { get; set; } = 0f;
        public float MaxTime { get; set; } = 1.2f;

        public static Texture[] moveAnimation { get; set; }
        public static Texture[] idleAnimation { get; set; }

        public BossEnemy(Vector2 position)
        {
            Position = position;
            Velocity = new Vector2(2f, 0);
            Health = MaxHealth;
            shotsLeft = 10;
        }

        public BossEnemy(Vector2 position, int health, bool phaseTwo)
        {
            Position = position;
            Velocity = new Vector2(2f, 0);
            Health = health;
            MaxHealth = 6;
            isPhaseTwo = phaseTwo;
            canMove = true;
            shotsLeft = 10;
        }

        public override void Update()
        {
            if (knockbackTimer > 0)
            {
                knockbackTimer -= 0.016f;
                Position += Velocity;
                Velocity = new Vector2(Velocity.X * 0.95f, Velocity.Y + Gravity); // Boss is heavy, less knockback decay
                return;
            }

            if (damageTimer > 0f)
            {
                damageTimer -= 0.016f;
            }
            else
            {
                isDamaged = false;
            }

            if (Health <= MaxHealth / 2 && !isPhaseTwo)
            {
                isPhaseTwo = true;
                canMove = true;
                BossEnemy secondBoss = new BossEnemy(new Vector2(500, 1000), MaxHealth / 2, true);
                Game.levels[Game.level].enemies.Add(secondBoss);
            }

            Player player = Game.player;
            Vector2 diff = player.Position - Position;
            float distance = (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);

            // Check if player is on top
            bool playerOnTop = Math.Abs(diff.X) < Size.X && player.Position.Y < Position.Y && distance < 150f;

            if (playerOnTop)
            {
                // Move out of the way
                float moveDir = diff.X > 0 ? -2.5f : 2.5f;
                Velocity = new Vector2(moveDir, Velocity.Y + Gravity);
                isFacingRight = moveDir > 0;
            }
            else if (canMove)
            {
                MoveTowardsPlayer();
            }
            else
            {
                Velocity = new Vector2(0, Velocity.Y + Gravity);
            }

            Position += Velocity;
            Velocity = new Vector2(0, Velocity.Y);

            float direction = player.Position.X - Position.X;
            isFacingRight = direction < 0;

            Vector2 rangeDiff = player.Position - Position;
            float dist = (float)Math.Sqrt(rangeDiff.X * rangeDiff.X + rangeDiff.Y * rangeDiff.Y);

            if (dist < range)
            {
                Attack();
            }

            FrameTimer += 0.016f;
            if (FrameTimer >= 0.1f)
            {
                Texture[] curr = canMove ? moveAnimation : idleAnimation;
                if (curr == null) curr = idleAnimation;

                if (curr != null && curr.Length > 0)
                {
                    frame++;
                    if (frame >= curr.Length)
                    {
                        frame = 0;
                    }
                    currentTexture = curr[frame];
                }
                FrameTimer = 0f;
            }
        }

        private void Attack()
        {
            shootTimer += 0.016f;

            switch (currentCycle)
            {
                case FireCycle.Rapid:
                    if (shotsLeft <= 0)
                    {
                        currentCycle = FireCycle.Regular;
                        shotsLeft = 5;
                        shootTimer = 0f;
                    }
                    else if (shootTimer >= 0.15f)
                    {
                        FireProjectile(6f);
                        shotsLeft--;
                        shootTimer = 0f;
                    }
                    break;

                case FireCycle.Regular:
                    if (shotsLeft <= 0)
                    {
                        currentCycle = FireCycle.NoFire;
                        cycleTimer = 0f;
                    }
                    else if (shootTimer >= 0.6f)
                    {
                        FireProjectile(4f);
                        shotsLeft--;
                        shootTimer = 0f;
                    }
                    break;

                case FireCycle.NoFire:
                    cycleTimer += 0.016f;
                    if (cycleTimer >= 2.0f)
                    {
                        currentCycle = FireCycle.Rapid;
                        shotsLeft = 10;
                        shootTimer = 0f;
                    }
                    break;
            }
        }

        private void FireProjectile(float speed)
        {
            Player player = Game.player;
            Vector2 targetPos = player.Position + new Vector2(0, -20);
            Vector2 dir = targetPos - Position;
            float len = (float)Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
            if (len > 0)
            {
                dir = new Vector2(dir.X / len, dir.Y / len);
            }

            Vector2 bullVel = dir * speed;
            Vector2 spawn = Position + new Vector2(Size.X / 2, Size.Y / 2);
            // 2made it a bit bigger. Notiucable? 
            Projectile proj = new Projectile(spawn, bullVel, 0, new Vector2(180, 120), 5.0f);
            Game.levels[Game.level].enemyProjectiles.Add(proj);
        }

        //half health
        private void MoveTowardsPlayer()
        {
            Player player = Game.player;
            float moveSpeed = 2.5f;
            int moveDir = player.Position.X > Position.X ? 1 : -1;
            
            Velocity = new Vector2(moveDir * moveSpeed, Velocity.Y + Gravity);

            // Jumping AI  if wall detected or player is higher
            int nextX = (int)((Position.X + (moveDir > 0 ? Size.X + 20 : -20)) / Game.TILE_SIZE);
            int nextY = (int)((Position.Y + Size.Y / 2) / Game.TILE_SIZE);
            var level = Game.levels[Game.level];
            
            bool wallAhead = false;
            if (nextX >= 0 && nextX < level.width && nextY >= 0 && nextY < level.height)
            {
                wallAhead = !level.tiles[nextY, nextX].IsEmpty && level.tiles[nextY, nextX].collide.isSolid;
            }

            if (IsOnGround && (wallAhead || (player.Position.Y < Position.Y - 100 && Math.Abs(player.Position.X - Position.X) < 200)))
            {
                Velocity = new Vector2(Velocity.X, -18f);
                IsOnGround = false;
            }
        }

        //drawiung the visuals for the boss
        public override void Draw(Vector2 screen)
        {
            base.Draw(screen);

            // Health Bar...might need to change varun
            float healthPercent = (float)Health / MaxHealth;
            Vector2 barPos = screen + new Vector2(0, -30);
            Vector2 barSize = new Vector2(Size.X, 10);
            
            Engine.DrawRectSolid(new Bounds2(barPos.X, barPos.Y, barSize.X, barSize.Y), Color.Black);
            Engine.DrawRectSolid(new Bounds2(barPos.X, barPos.Y, barSize.X * healthPercent, barSize.Y), Color.Red);

            // Shots Left rep
            if (currentCycle != FireCycle.NoFire)
            {
                float bulletSpacing = 15f;
                Vector2 bulletStart = screen + new Vector2(0, -50);
                for (int i = 0; i < shotsLeft; i++)
                {
                    Engine.DrawRectSolid(new Bounds2(bulletStart.X + (i * bulletSpacing), bulletStart.Y, 10, 15), Color.Yellow);
                }
            }
        }
    }
}
