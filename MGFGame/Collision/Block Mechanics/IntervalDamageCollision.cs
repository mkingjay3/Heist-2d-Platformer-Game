using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Collision
{
    public class IntervalDamageCollision : ICollidable
    {
        private Tile tile;
        private float interval = 2f;

        public IntervalDamageCollision(Tile tile)
        {
            this.tile = tile;
        }

        public bool isSolid { get; set; } = true;

        public void touch(Entity entity, Bounds2 entityBounds, Bounds2 tileBounds)
        {
            bool isActive = (IntervalPlatformCollision.globalTimer % (interval * 2)) < interval;

            if (!isActive) return;

            float left = entityBounds.Right - tileBounds.Left;
            float right = tileBounds.Right - entityBounds.Left;
            float top = entityBounds.Bottom - tileBounds.Top;
            float bottom = tileBounds.Bottom - entityBounds.Top;
            float min = Math.Min(Math.Min(left, right), Math.Min(top, bottom));

            if (min == top)
            {
                entity.Position = new Vector2(entity.Position.X, tileBounds.Top - entity.Size.Y);
                if (entity.Velocity.Y > 0)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                    entity.IsOnGround = true;
                }
                entity.Health = 0;
                entity.Armor = 0;
            }
            else if (min == bottom)
            {
                entity.Position = new Vector2(entity.Position.X, tileBounds.Bottom);
                if (entity.Velocity.Y < 0)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                }
                entity.Health = 0;
                entity.Armor = 0;
            }
            else if (min == left)
            {
                entity.Position = new Vector2(tileBounds.Left - entity.Size.X, entity.Position.Y);
                if (entity.Velocity.X > 0)
                {
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }
                entity.Health = 0;
                entity.Armor = 0;
            }
            else if (min == right)
            {
                entity.Position = new Vector2(tileBounds.Right, entity.Position.Y);
                if (entity.Velocity.X < 0)
                {
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }
                entity.Health = 0;
                entity.Armor = 0;
            }
        }

        public static void UpdateTimer()
        {
            // No longer needed as it uses IntervalPlatformCollision.globalTimer
        }

        public static bool IsCurrentlyActive(float interval = 2f)
        {
            return (IntervalPlatformCollision.globalTimer % (interval * 2)) < interval;
        }

        public void ResC()
        {
            return;
        }
    }
}
