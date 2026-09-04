using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Entities;

namespace MGFGame
{
    public class Projectile : ICollidable
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Size { get; set; } = new Vector2(20, 20);
        public static Texture texture;
        public float Lifetime { get; set; } = 0f;

        //changed the bullet time rto match teh visuals
        public float MaxLifetime { get; set; } = 3.1f;
        public float Rotation { get; set; }
        public bool isSolid { get; set; } = false;

        public Vector2 VisualSize { get; set; } = new Vector2(150, 100);

        public Projectile(Vector2 position, Vector2 velocity, float rotation, float maxLifetime = 1.0f)
        {
            Position = position;
            Velocity = velocity;
            Rotation = rotation;
            MaxLifetime = maxLifetime;
        }

        public Projectile(Vector2 position, Vector2 velocity, float rotation, Vector2 visualSize, float maxLifetime = 1.0f)
        {
            Position = position;
            Velocity = velocity;
            Rotation = rotation;
            VisualSize = visualSize;
            MaxLifetime = maxLifetime;
        }

        public void Update()
        {
            Position += Velocity;

            Velocity = new Vector2(Velocity.X, Velocity.Y);

            Lifetime += 0.016f;
        }

        public bool IsAlive()
        {
            if(Lifetime >= MaxLifetime)
            {
                return false;
            }
            return true;
        }

        public void Draw(Vector2 screenPosition)
        {
            Vector2 drawPos = screenPosition - (VisualSize - Size) / 2;
            Engine.DrawTexture(texture, drawPos, size: VisualSize, scaleMode: TextureScaleMode.Nearest, rotation: Rotation);
        }

        public void touch(Entity player, Bounds2 play, Bounds2 tile)
        {
            throw new NotImplementedException();
        }

        public void ResC()
        {
            throw new NotImplementedException();
        }

        public Bounds2 GetBounds()
        {
            return new Bounds2(Position, Size);
        }
    }
}
