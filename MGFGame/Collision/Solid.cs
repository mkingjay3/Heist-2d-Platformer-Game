using System.Runtime.CompilerServices;
using MGFGame;
using MGFGame.Entities;

namespace MGFGame.Collision
{
    public class Solid : ICollidable
    {
        public bool isSolid { get; set; }
        private Tile tile;

        public Solid(Tile tile)
        {
            this.tile = tile;
            isSolid = true;
        }

        public Solid(Tile tile, Texture texture) : this(tile)
        {
            tile.Texture = texture;
        }

        public void touch(Entity entity, Bounds2 p, Bounds2 t)
        {
            float left = p.Right - t.Left;
            float right = t.Right - p.Left;
            float top = p.Bottom - t.Top;
            float bottom = t.Bottom - p.Top;
            float min = Math.Min(Math.Min(left, right), Math.Min(top, bottom));

            if (min == top)
            {
                entity.Position = new Vector2(entity.Position.X, t.Top - entity.Size.Y);
                if (entity.Velocity.Y > 0)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                    entity.IsOnGround = true;
                }
            }
            else if (min == bottom)
            {
                entity.Position = new Vector2(entity.Position.X, t.Bottom);
                if (entity.Velocity.Y < 0)
                {
                    entity.Velocity = new Vector2(entity.Velocity.X, 0);
                }
            }
            else if (min == left)
            {
                entity.Position = new Vector2(t.Left - entity.Size.X, entity.Position.Y);
                if (entity.Velocity.X > 0)
                {
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }

                // Wall jump detection
                if (entity is Player player && !player.IsOnGround && player.Velocity.Y >= 0)
                {
                    player.IsTouchingWallRight = true;
                    player.IsOnWall = true;
                }
            }
            else if (min == right)
            {
                entity.Position = new Vector2(t.Right, entity.Position.Y);
                if (entity.Velocity.X < 0)
                {
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }

                // Wall jump detection
                if (entity is Player player && !player.IsOnGround)
                {
                    player.IsTouchingWallLeft = true;
                    player.IsOnWall = true;
                }
            }
        }
        public void ResC()
        {
        }
    }
}
