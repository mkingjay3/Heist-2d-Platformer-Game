using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame.Visual
{
    public class Parallax
    {
        private List<(Texture tex, float speed)> layer;
         
        public Parallax()
        {
            layer = new List<(Texture tex, float speed)>();
        } 

        public void addLayer(Texture tex, float speed)
        {
            layer.Add((tex, speed));
        }

        public void Draw(Vector2 cameraPos)
        {
            for(int i = 0; i < layer.Count; i++)
            {
                Vector2 pos = new Vector2(-cameraPos.X * layer[i].speed, -cameraPos.Y * layer[i].speed);
                Engine.DrawTexture(layer[i].tex, pos);
            }
        }


    }
}
