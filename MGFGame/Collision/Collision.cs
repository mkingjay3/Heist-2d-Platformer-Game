using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Entities;

namespace MGFGame.Collision
{
    // background - black, walkable, no collision
    public class BackgroundCollision : ICollidable
    {
        public bool isSolid { get; set; } = false;

        public void touch(Entity player, Bounds2 playerBounds, Bounds2 tileBounds)
        {
            //no collisions
        }
        public void ResC()
        {
            return;
        }
    }

    // wall - grey, solid, blocks movement
    public class WallCollision : ICollidable
    {
        private Tile tile;

        public WallCollision(Tile tile)
        {
            this.tile = tile;
        }

        public bool isSolid { get; set; } = true;

        public void touch(Entity entity, Bounds2 entityBounds, Bounds2 tileBounds)
        {
            // tile.FlashRed();
            // player.Color = Color.Blue;

            // calculate overlap on each axis
            float overlapLeft = entityBounds.Position.X + entityBounds.Size.X - tileBounds.Position.X;
            float overlapRight = tileBounds.Position.X + tileBounds.Size.X - entityBounds.Position.X;
            float overlapTop = entityBounds.Position.Y + entityBounds.Size.Y - tileBounds.Position.Y;
            float overlapBottom = tileBounds.Position.Y + tileBounds.Size.Y - entityBounds.Position.Y;

            // find minimum overlap
            float minOverlapX = Math.Min(overlapLeft, overlapRight);
            float minOverlapY = Math.Min(overlapTop, overlapBottom);

            // push player back on the axis with least overlap
            if (minOverlapX < minOverlapY)
            {
                // horizontally
                if (overlapLeft < overlapRight)
                {
                    entity.Position = new Vector2(entity.Position.X - overlapLeft, entity.Position.Y);
                }
                else
                {
                    entity.Position = new Vector2(entity.Position.X + overlapRight, entity.Position.Y);
                }
                entity.Velocity = new Vector2(0, entity.Velocity.Y);
            }
            else
            {
                // vertically
                if (overlapTop < overlapBottom)
                {
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y - overlapTop);
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                    entity.IsOnGround = true;
                }
                else
                {
                    entity.Position = new Vector2(entity.Position.X, entity.Position.Y + overlapBottom);
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                }
            }
        }
        public void ResC()
        {
            return;
        }
    }
    // ladder - green, climbable
    public class LadderCollision : ICollidable
    {
        public bool isSolid { get; set; } = false;

        public void touch(Entity player, Bounds2 playerBounds, Bounds2 tileBounds)
        {
            // alllow player to climb
            if (player is Player p)
            {
                p.IsOnLadder = true;
            }

        }
        public void ResC()
        {
            return;
        }
    }
    public class Push : ICollidable
    {
        public bool isSolid { get; set; } = true;
        private Tile tile;
        private float speed = 10f;
        public Push(Tile tile)
        {
            this.tile = tile;
        }

        public void touch(Entity entity, Bounds2 entityBounds, Bounds2 tileBounds)
        {
            //super similar to normal collisions except check direction 
            float overlapLeft = entityBounds.Position.X + entityBounds.Size.X - tileBounds.Position.X;
            float overlapRight = tileBounds.Position.X + tileBounds.Size.X - entityBounds.Position.X;
            float overlapTop = entityBounds.Position.Y + entityBounds.Size.Y - tileBounds.Position.Y;
            float overlapBottom = tileBounds.Position.Y + tileBounds.Size.Y - entityBounds.Position.Y;

            float min = Math.Min(Math.Min(overlapLeft, overlapRight), Math.Min(overlapTop, overlapBottom));
            //easier way ^ I lwk complicated it the first time 
            if (min == overlapTop && entity.Velocity.Y >= 0)
            {
                entity.Position = new Vector2(entity.Position.X, entity.Position.Y - overlapTop);
                entity.Velocity = new Vector2(entity.Velocity.X, 0);
                entity.IsOnGround = true;
            }
            if (entity is Player player)
            {
                //had to search up the is player thingy, change if theres a better way to do this
                if (player.Velocity.X > 0)
                {
                    player.Velocity = new Vector2(player.Velocity.X + speed, player.Velocity.Y);
                }
                if (player.Velocity.X == 0)
                {
                    player.Velocity = new Vector2(speed, player.Velocity.Y);
                }
            }
        }
        public void ResC()
        {
            return;
        }

        //collectible - purple, pickup item
        public class CollectibleCollision : ICollidable
        {
            private Tile tile;

            public CollectibleCollision(Tile tile)
            {
                this.tile = tile;
            }

            public bool isSolid { get; set; } = false;

            public void touch(Entity player, Bounds2 playerBounds, Bounds2 tileBounds)
            {
                if (player is Player p && !tile.IsCollected)
                {
                    SoundManager.PlayPickupCoin();
                    tile.IsCollected = true;
                    p.CollectedItems++;

                    // make tile disappear if collected
                    tile.IsEmpty = true;
                    tile.Texture = null;
                }
            }
            public void ResC()
            {
                return;
            }
        }
    }
}