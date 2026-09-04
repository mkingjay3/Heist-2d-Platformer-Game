using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Collision.Block_Mechanics
{
    public class Ladder
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; } = new Vector2(64, 64);
        public Color Color { get; set; } = Color.Blue;

        public Ladder(Vector2 pos)
        {
            Position = pos;
        }

        public void Draw(Vector2 cameraPos)
        {
            Vector2 screenPos = Position - cameraPos;
            Engine.DrawTexture(Game.textures["ladder"], screenPos, size: Size, scaleMode: TextureScaleMode.Nearest);
        }

        public Bounds2 GetBounds()
        {
            return new Bounds2(Position, Size);
        }

        public void HandleCollision(Entity entity)
        {
            Bounds2 entityBounds = entity.GetBounds();
            Bounds2 ladderBounds = GetBounds();

            if (entity is Player player)
            {
                if (!entityBounds.Overlaps(ladderBounds))
                {
                    return;
                }

                player.IsTouchingLadder = true;

                if (player.WantsToExitLadder) return;

                if (!player.IsOnLadder)
                {
                    float ladderCenterX = Position.X + Size.X / 2 - entity.Size.X / 2;
                    entity.Position = new Vector2(ladderCenterX, entity.Position.Y);
                }

                player.IsOnLadder = true;

                // Only update if this ladder tile is HIGHER (smaller Y) than current
                if (Position.Y < player.CurrentLadderTop)
                {
                    player.CurrentLadderTop = Position.Y;
                }
            }
        }
    }
}
