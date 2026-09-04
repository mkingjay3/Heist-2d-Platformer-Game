using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Collision.Block_Mechanics
{
    public class PowerUp : Collectable
    {
        public PowerUp(Vector2 pos) : base(pos)
        {
        }

        public override void Draw(Vector2 cameraPos)
        {
            if (IsCollected) return;

            Vector2 screenPos = Position - cameraPos;
            Engine.DrawTexture(Game.textures["powerup"], screenPos, size: Size, scaleMode: TextureScaleMode.Nearest);
        }

        public override void HandleCollision(Player player)
        {
            if (IsCollected) return;

            Bounds2 playerBounds = player.GetBounds();
            Bounds2 collectableBounds = GetBounds();

            if (playerBounds.Overlaps(collectableBounds))
            {
                IsCollected = true;
                player.HasTripleShot = true;
                SoundManager.PlayPickupCoin(); // Reuse sound or add new one
            }
        }
    }
}
