using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame
{
    public class MultiShotEnemy : NPC
    {

        public int Direction { get; set; } = 1;
        public float MoveTime { get; set; } = 0f;
        public float MaxTime { get; set; } = 2.0f;
        public static Texture[] moveAnimation { get; set; }
        public static Texture[] idleAnimation { get; set; }

        private float shootTimer = 0f;
        private float shootCooldown = 2.0f;
        private float aggroRange = 600f;
        private Vector2 startPosition;
        private float followRange = 64f;

        public MultiShotEnemy(Vector2 position)
        {
            Position = position;
            startPosition = position;
            Velocity = Vector2.Zero;
            Health = 2;
            Size = new Vector2(64, 100);
        }

        public override void Update()
        {
            if (knockbackTimer > 0)
            {
                knockbackTimer -= 0.016f;
                Position += Velocity;
                Velocity = new Vector2(Velocity.X * 0.9f, Velocity.Y + Gravity);
                UpdateAnimation();
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

            Player player = Game.player;
            Vector2 diff = player.Position - Position;
            float distance = (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);

            // Check if player is on top
            bool playerOnTop = Math.Abs(diff.X) < Size.X && player.Position.Y < Position.Y && distance < 150f;

            if (playerOnTop)
            {
                // Move out of the way
                float moveDir = diff.X > 0 ? -1.5f : 1.5f;
                Velocity = new Vector2(moveDir, Velocity.Y + Gravity);
                isFacingRight = moveDir > 0;
                IsMoving = true;
            }
            else if (distance < aggroRange)
            {
                // dont face randomly, face the player
                isFacingRight = diff.X > 0;
                
                // follow player if too far
                if (distance > 64f && Math.Abs(Position.X - startPosition.X) < followRange)
                {
                    Vector2 moveDir = new Vector2(diff.X > 0 ? 1 : -1, 0);
                    
                    int nextX = (int)((Position.X + (moveDir.X > 0 ? Size.X + 10 : -10)) / Game.TILE_SIZE);
                    int nextY = (int)((Position.Y + Size.Y + 10) / Game.TILE_SIZE);
                    var level = Game.levels[Game.level];
                    bool groundAhead = nextX >= 0 && nextX < level.width && nextY >= 0 && nextY < level.height && !level.tiles[nextY, nextX].IsEmpty && level.tiles[nextY, nextX].collide.isSolid;

                    if (groundAhead || distance < 192f)
                    {
                        Velocity = new Vector2(moveDir.X * 1.5f, Velocity.Y + Gravity);
                        IsMoving = true;
                    }
                    else
                    {
                        IsMoving = false;
                    }
                }
                else if (Math.Abs(Position.X - startPosition.X) >= followRange)
                {
                    //make sure the mobvs dont wander
                    Vector2 homeDir = new Vector2(startPosition.X - Position.X > 0 ? 1 : -1, 0);
                    Velocity = new Vector2(homeDir.X * 1.5f, Velocity.Y + Gravity);
                    IsMoving = true;
                }
                else
                {
                    IsMoving = false;
                }

                // time
                shootTimer += 0.016f;
                if (shootTimer >= shootCooldown)
                {
                    ShootSpread();
                    shootTimer = 0f;
                }
            }
            else
            {
               
                Vector2 moveDir = new Vector2(Direction, 0);
                int nextX = (int)((Position.X + (moveDir.X > 0 ? Size.X + 10 : -10)) / Game.TILE_SIZE);
                int nextY = (int)((Position.Y + Size.Y + 10) / Game.TILE_SIZE);
                var level = Game.levels[Game.level];
                bool groundAhead = nextX >= 0 && nextX < level.width && nextY >= 0 && nextY < level.height && !level.tiles[nextY, nextX].IsEmpty && level.tiles[nextY, nextX].collide.isSolid;

                if (groundAhead)
                {
                    Velocity = new Vector2(Direction * 1.5f, Velocity.Y + Gravity);
                    isFacingRight = Direction > 0;
                    IsMoving = true;
                }
                else
                {
                    Direction *= -1;
                    Velocity = new Vector2(0, Velocity.Y + Gravity);
                    IsMoving = false;
                }

                MoveTime += 0.016f;
                if (MoveTime >= MaxTime)
                {
                    Direction *= -1;
                    MoveTime = 0f;
                }
                IsMoving = true;
            }

            Position += Velocity;
            Velocity = new Vector2(0, Velocity.Y);

            UpdateAnimation();
        }

        private void ShootSpread()
        {

            //reused this for any shooting section

            Vector2 targetPos = Game.player.Position + new Vector2(0, 20);
            Vector2 direction = targetPos - Position;
            float baseAngle = (float)Math.Atan2(direction.Y, direction.X);

            // fire 3 bullets at once
            float[] angles = { baseAngle - 0.2f, baseAngle, baseAngle + 0.2f };

            foreach (float angle in angles)
            {
                Vector2 bulletVel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 8f;
                Projectile p = new Projectile(Position + new Vector2(isFacingRight ? 40 : -20, 40), bulletVel, angle, 0.8f);
                Game.levels[Game.level].enemyProjectiles.Add(p);
            }
        }

        private void UpdateAnimation()
        {
            FrameTimer += 0.016f;
            float frameDuration = IsMoving ? 0.15f : 0.3f;

            if (FrameTimer >= frameDuration)
            {
                Texture[] currentAnim = IsMoving ? moveAnimation : idleAnimation;
                if (currentAnim != null && currentAnim.Length > 0)
                {
                    frame++;
                    if (frame >= currentAnim.Length)
                    {
                        frame = 0;
                    }
                    currentTexture = currentAnim[frame];
                }
                FrameTimer = 0f;
            }
        }
    }
}
