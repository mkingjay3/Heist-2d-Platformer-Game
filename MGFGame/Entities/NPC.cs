using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Entities;

namespace MGFGame
{
    public class NPC : Entity
    {

        public bool isFacingRight { get; set; } = true;
        public virtual void Update()
        {
        }

        public override Bounds2 GetBounds()
        {
            if (currentTexture != null)
            {
                float scale = Size.Y / currentTexture.Height;
                Vector2 drawSize = new Vector2(currentTexture.Width * scale, currentTexture.Height * scale);
                Vector2 drawPos = Position;

                if (drawSize.Y > Size.Y)
                {
                    drawPos.Y -= (drawSize.Y - Size.Y);
                }

                if (drawSize.X > Size.X)
                {
                    if (!isFacingRight)
                    {
                        drawPos.X -= (drawSize.X - Size.X);
                    }
                }
                return new Bounds2(drawPos, drawSize);
            }
            return base.GetBounds();
        }

        public virtual void Draw(Vector2 screen)
        {
            if (currentTexture != null)
            {
                float scale = Size.Y / currentTexture.Height;
                
                Vector2 drawSize = new Vector2(currentTexture.Width * scale, currentTexture.Height * scale);
                Vector2 drawPos = screen;

                if (drawSize.Y > Size.Y)
                {
                    drawPos.Y -= (drawSize.Y - Size.Y);
                }

                if (drawSize.X > Size.X)
                {
                    if (!isFacingRight)
                    {
                        drawPos.X -= (drawSize.X - Size.X);
                    }
                }

                if (!isFacingRight)
                {
                    if (isDamaged)
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest, mirror: TextureMirror.Horizontal, color: new Color(200, 0, 0, 150));
                    }
                    else
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest, mirror: TextureMirror.Horizontal);
                    }
                }
                else
                {
                    if (isDamaged)
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest, color: new Color(200,0,0, 150));
                    }
                    else
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest);
                    }
                        
                }

            }
        }
    }
}
