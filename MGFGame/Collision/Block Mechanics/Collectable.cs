using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Collision.Block_Mechanics
{
    public class Collectable
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; } = new Vector2(64, 64);
        public Color Color { get; set; } = Color.Purple;
        public bool IsCollected { get; set; } = false;

        public Collectable(Vector2 pos)
        {
            Position = pos;
        }

        public virtual void Draw(Vector2 cameraPos)
        {
            if (IsCollected) return;  // Don't draw if collected

            Vector2 screenPos = Position - cameraPos;
            Engine.DrawTexture(Game.textures["coin0"], screenPos, size: Size, scaleMode: TextureScaleMode.Nearest);
        }

        public Bounds2 GetBounds()
        {
            return new Bounds2(Position, Size);
        }

        public virtual void HandleCollision(Player player)
        {
            if (IsCollected) return;  // Already collected

            Bounds2 playerBounds = player.GetBounds();
            Bounds2 collectableBounds = GetBounds();

            if (playerBounds.Overlaps(collectableBounds))
            {
                IsCollected = true;
                player.CollectedItems++;
            }
        }
    }
}
