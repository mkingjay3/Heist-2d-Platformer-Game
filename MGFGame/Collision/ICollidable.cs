using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGFGame.Entities;

namespace MGFGame
{
    public interface ICollidable
    {
        bool isSolid { get; set; }
        void touch(Entity player, Bounds2 play, Bounds2 tile);

        void ResC();
    }
}
