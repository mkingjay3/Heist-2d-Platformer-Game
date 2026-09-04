using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Entities;

namespace MGFGame
{
    public class notSolid : ICollidable
    {
        public bool isSolid { get; set; }
        public notSolid()
        {
            isSolid = false;
        }
        public void touch(Entity player, Bounds2 play, Bounds2 tile)
        {
            return;
        }
        public void ResC()
        {
            return;
        }
    }
}
