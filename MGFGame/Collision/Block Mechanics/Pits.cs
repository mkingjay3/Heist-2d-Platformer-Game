using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Collision.Block_Mechanics
{
    public class PitCollision : ICollidable
    {
        private Tile tile;

        public PitCollision(Tile tile)
        {
            this.tile = tile;
        }

        public bool isSolid { get; set; } = false;

        public void touch(Entity entity, Bounds2 entityBounds, Bounds2 tileBounds)
        {
            entity.Health = 0;
        }

        public void ResC()
        {
            return;
        }
    }
}
