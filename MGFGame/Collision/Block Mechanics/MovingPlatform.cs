using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Entities;

namespace MGFGame.Collision.Block_Mechanics
{
    public class MovingPlatform
    {
        public Vector2 Position { get; set; }
        public Vector2 StartPosition { get; set; }
        public Vector2 Size { get; set; } = new Vector2(64, 64);
        public Color Color { get; set; } = Color.Red;

        private float moveDistance = 200f;
        private float speed = 2f;
        private bool movingRight = true;

        public MovingPlatform(Vector2 startPos)
        {
            StartPosition = startPos;
            Position = startPos;
        }

        public void Update()
        {
            if (movingRight)
            {
                Position = new Vector2(Position.X + speed, Position.Y);
                if (Position.X >= StartPosition.X + moveDistance)
                {
                    movingRight = false;
                }
            }
            else
            {
                Position = new Vector2(Position.X - speed, Position.Y);
                if (Position.X <= StartPosition.X)
                {
                    movingRight = true;
                }
            }
        }
        
        public void Draw(Vector2 cameraPos)
        {
            Vector2 screenPos = Position - cameraPos;
            Engine.DrawTexture(Game.textures["purple_moving"], screenPos, size: Size, scaleMode: TextureScaleMode.Nearest);
        }

        public Bounds2 GetBounds()
        {
            return new Bounds2(Position, Size);
        }

        public void HandleCollision(Entity entity)
        {
            Bounds2 entityBounds = entity.GetBounds();
            Bounds2 platBounds = GetBounds();

            if (!entityBounds.Overlaps(platBounds)) return;

            float left = entityBounds.Right - platBounds.Left;
            float right = platBounds.Right - entityBounds.Left;
            float top = entityBounds.Bottom - platBounds.Top;
            float bottom = platBounds.Bottom - entityBounds.Top;
            float min = Math.Min(Math.Min(left, right), Math.Min(top, bottom));

            if (min == top)
            {
                // Land on top
                entity.Position = new Vector2(entity.Position.X, platBounds.Top - entity.Size.Y);
                if (entity.Velocity.Y > 0)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                    entity.IsOnGround = true;
                }

                // Carry player with platform (only once per frame)
                if (entity is Player player && !player.IsOnMovingPlatform)
                {
                    player.IsOnMovingPlatform = true;
                    float platformMovement = movingRight ? speed : -speed;
                    entity.Position = new Vector2(entity.Position.X + platformMovement, entity.Position.Y);
                }
            }
            else if (min == bottom)
            {
                // Hit head on bottom
                entity.Position = new Vector2(entity.Position.X, platBounds.Bottom);
                if (entity.Velocity.Y < 0)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                }
            }
            else if (min == left)
            {
                // Hit left side
                entity.Position = new Vector2(platBounds.Left - entity.Size.X, entity.Position.Y);
                if (entity.Velocity.X > 0)
                {
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }
            }
            else if (min == right)
            {
                // Hit right side
                entity.Position = new Vector2(platBounds.Right, entity.Position.Y);
                if (entity.Velocity.X < 0)
                {
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }
            }
        }
    }
}
