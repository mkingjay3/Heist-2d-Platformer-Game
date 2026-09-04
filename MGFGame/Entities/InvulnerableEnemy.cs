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
    public class InvulnerableEnemy : NPC
    {
        public int Direction { get; set; } = 1;
        public float MoveTime { get; set; } = 0f;
        public float MaxTime { get; set; } = 1.2f;
        public static Texture[] moveAnimation { get; set; }
        public static Texture[] idleAnimation { get; set; }

        private float aggroRange = 600f;
        private float speed = 1.5f;

        public InvulnerableEnemy(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            Health = 1000;
            Size = new Vector2(64, 100);
        }

        public override void takeDamage()
        {
        }

        public void DieToEnvironment()
        {
            Health = 0;
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
                float moveDir = diff.X > 0 ? -speed : speed;
                Velocity = new Vector2(moveDir, Gravity);
                isFacingRight = moveDir > 0;
                IsMoving = true;
            }
            else if (distance < aggroRange)
            {
                // should follow player but the bait tactic isnt working all the time. 
                Vector2 direction = diff;
                float len = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                if (len > 0)
                {
                    direction = new Vector2(direction.X / len, direction.Y / len);
                    
                    int nextX = (int)((Position.X + (direction.X > 0 ? Size.X + 10 : -10)) / Game.TILE_SIZE);
                    int nextY = (int)((Position.Y + Size.Y + 10) / Game.TILE_SIZE);
                    var level = Game.levels[Game.level];
                    bool groundAhead = nextX >= 0 && nextX < level.width && nextY >= 0 && nextY < level.height && !level.tiles[nextY, nextX].IsEmpty && level.tiles[nextY, nextX].collide.isSolid;

                    if (groundAhead || distance < 192f)
                    {
                        Velocity = new Vector2(direction.X * speed, Gravity);
                        isFacingRight = direction.X > 0;
                        IsMoving = true;
                    }
                    else
                    {
                        Velocity = new Vector2(0, Gravity);
                        IsMoving = false;
                    }
                }
            }
            else
            {
                // Stay still
                Velocity = new Vector2(0, Gravity);
                IsMoving = false;
            }

            Position += Velocity;

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            FrameTimer += 0.016f;

            if (FrameTimer >= 0.1f)
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
