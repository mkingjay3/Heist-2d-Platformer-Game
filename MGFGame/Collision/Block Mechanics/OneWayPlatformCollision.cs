using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Collision
{
    public class OneWayPlatformCollision : ICollidable
    {
        private Tile tile;

        public OneWayPlatformCollision(Tile tile)
        {
            this.tile = tile;
        }

        public bool isSolid { get; set; } = false;

        public void touch(Entity entity, Bounds2 entityBounds, Bounds2 tileBounds)
        {
            float playerBottom = entityBounds.Position.Y + entityBounds.Size.Y;
            float platformTop = tileBounds.Position.Y;

            // Check if player's feet are near the top of the platform
            if (entity.Velocity.Y >= 0 && playerBottom <= platformTop + 25)
            {
                // Land on top
                entity.Position = new Vector2(entity.Position.X, platformTop - entity.Size.Y);
                entity.Velocity = new Vector2(entity.Velocity.X, 0);
                entity.IsOnGround = true;
            }
        }

        public void ResC()
        {
            return;
        }
    }
}

