using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGFCore.Engine;

namespace MGFGame.Collision.Block_Mechanics
{
    public class Lever
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; } = new Vector2(64, 64);
        public bool IsActivated { get; set; } = false;

        // Static so all lasers can check this
        public static bool LasersDisabled { get; set; } = false;
        public static float DisableTimer { get; set; } = 0f;
        public static float DisableDuration { get; set; } = 3f;

        public Lever(Vector2 pos)
        {
            Position = pos;
        }

        public static void Update()
        {
            if (LasersDisabled)
            {
                DisableTimer -= 0.016f; // ~60fps
                if (DisableTimer <= 0)
                {
                    LasersDisabled = false;
                    DisableTimer = 0f;
                }
            }
        }

        public static void Activate()
        {
            LasersDisabled = true;
            DisableTimer = DisableDuration;
        }

        public void Draw(Vector2 cameraPos)
        {
            Vector2 screenPos = Position - cameraPos;
            TextureMirror mirror = LasersDisabled ? TextureMirror.Horizontal : TextureMirror.None;
            Engine.DrawTexture(Game.textures["lever"], screenPos, size: Size, scaleMode: TextureScaleMode.Nearest, mirror: mirror);
        }

        public Bounds2 GetBounds()
        {
            return new Bounds2(Position, Size);
        }

        public void HandleCollision(Player player)
        {
            Bounds2 playerBounds = player.GetBounds();
            Bounds2 leverBounds = GetBounds();

            if (playerBounds.Overlaps(leverBounds))
            {
                if (Engine.GetKeyDown(Key.F))
                {
                    Activate();
                }
            }
        }
    }
}
