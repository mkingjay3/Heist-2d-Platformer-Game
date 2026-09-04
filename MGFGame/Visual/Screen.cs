using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace MGFGame
{
    public class Screen
    {
        public float speed { get; set; }
        public Vector2 pos { get; set; }
        public float speed2 { get; set; }
        public Vector2 pos2 { get; set; }
        private int width { get; set; }
        private int height { get; set; }
        private int width2 { get; set; }
        private int height2 { get; set; }
        public Screen(int w, int h, int w2, int h2)
        {
            speed = 0.5f;
            pos = Vector2.Zero;
            speed2 = 0.8f;
            pos2 = Vector2.Zero;
            width = w;
            height = h;
            width2 = w2;
            height2 = h2;
        }

        public void update(Camera camera)
        {
            pos = new Vector2(camera.Position.X * speed, camera.Position.Y * speed);
            pos2 = new Vector2(camera.Position.X * speed2, camera.Position.Y* speed2);

        }
        public void draw(Camera cam)
        {
            pos = new Vector2(cam.Position.X * speed, cam.Position.Y);
            pos2 = new Vector2(cam.Position.X * speed2, cam.Position.Y);

            Vector2 l1 = pos - cam.Position;
            Vector2 l2 = pos2 - cam.Position;

            
        }
    }
}
