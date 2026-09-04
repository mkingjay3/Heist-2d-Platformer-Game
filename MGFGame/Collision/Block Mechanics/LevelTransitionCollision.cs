using MGFGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Collision.Block_Mechanics
{
    public class LevelTransitionCollision : ICollidable
    {
        private Tile tile;
        private int targetLevel;

        public LevelTransitionCollision(Tile tile, int targetLevel)
        {
            this.tile = tile;
            this.targetLevel = targetLevel;
        }

        public bool isSolid { get; set; } = false;

        public void touch(Entity entity, Bounds2 entityBounds, Bounds2 tileBounds)
        {
            if (entity is Player player)
            {
                player.NextLevel = targetLevel;
            }
        }

        public void ResC()
        {
            return;
        }
    }
}
