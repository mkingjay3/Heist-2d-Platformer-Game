using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Entities
{
    public class Entity : ICollidable
    {
        public bool isSolid { get; set; } = false;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsOnGround { get; set; }
        public Vector2 Size { get; set; } = new Vector2(64, 100);
        public float FrameTimer { get; set; } = 0f;
        public Texture currentTexture;
        public int frame;
        public float Gravity { get; set; } = 0.5f;
        public bool IsMoving { get; set; } = false;
        public int Health { get; set; } = 1;
        public int Armor { get; set; } = 0;
        public bool isDamaged { get; set; } = false;
        public float damageTimer { get; set; } = 0f;
        public float knockbackTimer { get; set; } = 0f;

        public virtual Bounds2 GetBounds()
        {
            return new Bounds2(Position, Size);
        }

        public virtual void takeDamage()
        {
            if (damageTimer <= 0f)
            {
                Health--;
                isDamaged = true;
                damageTimer = 0.25f;
            }
        }

        public bool isDead()
        {
            return Health <= 0 && Armor <= 0;
        }

        public virtual void ResC()
        {
            return;
        }

        public virtual void touch(Entity e, Bounds2 play, Bounds2 tile)
        {
            return;
        }
    }
}
