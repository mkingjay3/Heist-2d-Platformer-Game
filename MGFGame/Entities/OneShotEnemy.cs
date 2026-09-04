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
    public class OneShotEnemy : NPC
    {
        public float AggroRange { get; set; } = 600f;
        public float AttackRange { get; set; } = 80f;
        public float Speed { get; set; } = 3.5f;
        
        public static Texture[] moveAnimation { get; set; }
        public static Texture[] idleAnimation { get; set; }

        private Vector2 startPosition;
        private bool isAggro = false;
        private float attackTimer = 0f;
        private bool isRetreating = false;
        private float idleTimer = 0f;

        public OneShotEnemy(Vector2 position)
        {
            Position = position;
            startPosition = position;
            Health = 1;
            Size = new Vector2(100, 100);
        }

        public override void Update()
        {
            if (knockbackTimer > 0)
            {
                knockbackTimer -= 0.016f;
                Position += Velocity;
                Velocity = new Vector2(Velocity.X * 0.9f, Velocity.Y * 0.9f); // Bats fly, so no gravity knockback?
                return;
            }

            if (damageTimer > 0f)
            {
                damageTimer -= 0.01f;
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
                float moveDir = diff.X > 0 ? -Speed : Speed;
                Position += new Vector2(moveDir, 0);
                isFacingRight = moveDir > 0;
            }
            else if (distance < AggroRange)
            {
                isAggro = true;
            }
            else if (distance > AggroRange * 1.5f)
            {
                isAggro = false;
                isRetreating = false;
            }

            if (isAggro && !playerOnTop)
            {
                Vector2 targetPos = player.Position + new Vector2(0, 20);
                Vector2 direction = targetPos - Position;
                float len = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                
                if (len > 0)
                {
                    direction = new Vector2(direction.X / len, direction.Y / len);

                    if (isRetreating)
                    {
                        Position -= direction * (Speed * 0.8f);
                        attackTimer += 0.016f;
                        if (attackTimer > 1.2f)
                        {
                            isRetreating = false;
                            attackTimer = 0f;
                        }
                    }
                    else
                    {
                        Position += direction * Speed;
                        if (len < AttackRange || GetBounds().Overlaps(player.GetBounds()))
                        {
                            isRetreating = true;
                            attackTimer = 0f;
                        }
                    }
                    
                    isFacingRight = direction.X > 0;
                }
            }
            else if (!playerOnTop)
            {
                idleTimer += 0.016f;
                Position = new Vector2(startPosition.X, startPosition.Y + (float)Math.Sin(idleTimer * 2) * 30);
            }

            FrameTimer += 0.016f;

            if (FrameTimer >= 0.1f)
            {
                Texture[] currentAnim = isAggro ? moveAnimation : idleAnimation;
                
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

        //this should hopefulyu stop the bat from pushing the player under the map 
        //and just apply damage and not push

        public override void touch (Entity ent, Bounds2 a, Bounds2 b)
        {

        }
    }
}
