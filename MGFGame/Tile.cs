using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Collision;

namespace MGFGame
{
    public class Tile
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; } = new Vector2(64, 64);

        public Texture Texture { get; set; }
        //lwk dont know if we use this bool yet..check this later
        public bool IsEmpty { get; set; } = false;
        //same with value...
        public int value { get; set; } // value from csv
        public ICollidable collide { get; set; }
        public bool IsCollected { get; set; } = false;
        public Color DebugColor { get; set; } = Color.Blue;
        public TextureMirror Mirror { get; set; } = TextureMirror.None;


        public Tile(Vector2 pos)
        {
            Position = pos;
            Size = new Vector2(64, 64);
        }

        public void Draw(Vector2 screen)
        {
            Color drawColor = DebugColor;

            // Fade interval platforms when inactive
            if (collide is IntervalPlatformCollision && !IntervalPlatformCollision.IsCurrentlyActive())
            {
                drawColor = new Color(DebugColor.R, DebugColor.G, DebugColor.B, (byte)80);
            }

            // If we don't have a texture, draw a colored rectangle
            if (Texture != null)
            {
                Engine.DrawTexture(Texture, screen, size: Size, scaleMode: TextureScaleMode.Nearest, mirror: Mirror);
            }
            else
            {
                Engine.DrawRectSolid(new Bounds2(screen, Size), drawColor);
            }
        }

        public Bounds2 GetBounds()  
        {
            return new Bounds2(Position, Size);
        }

    }
}
