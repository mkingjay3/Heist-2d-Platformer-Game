using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFGame;
public class Camera
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get;  set; }
    public float moveWidth = 100f;
    public float moveHeight = 100f;

    public Camera()
    {
        Position = Vector2.Zero;
    }


    //to change asap change the numbers to be variables
    public void movingPlayer(Vector2 playerPos)
    {
        //change the dumb names later lol - sriram

       Vector2 center = new Vector2(Position.X + 1280/2, Position.Y + 720/2);

         float lefty = center.X - moveWidth / 2;
         float righty = center.X + moveWidth / 2;
        //top 
         float topy = center.Y - moveHeight / 2;
         float bottomy = center.Y + moveHeight / 2;

        float newPosX = Position.X;
        float newPosY = Position.Y;

        if(playerPos.X < lefty)
        {
            newPosX = playerPos.X - (1280 / 2) + (moveWidth / 2);
        }

        else if(playerPos.X > righty)
        {
            newPosX = playerPos.X - (1280 / 2) - (moveWidth / 2);
        }


        if(playerPos.Y < topy)
        {
            newPosY = playerPos.Y - (720 / 2) + (moveHeight / 2);
        }

        else if(playerPos.Y > bottomy)
        {
            newPosY = playerPos.Y - (720 / 2) - (moveHeight / 2);
        }


        Position = new Vector2(newPosX, newPosY);

    }
  //  public void MoveUp()
    //{
      //  Position += new Vector2(0, -8);
    //}

 //   public void MoveDown()
   // {
     //   Position += new Vector2(0, 8);
//    }

  //  public void MoveLeft()
    //{
      //  Position += new Vector2(-8, 0);
  //  }

    //public void MoveRight()
 //   {
   //     Position += new Vector2(8, 0);
   // }

   // public Vector2 WorldToScreen(Vector2 world)
  //  {
    //    return world - Position;
   // }

  //  public Vector2 ScreenToWorld(Vector2 screen)
  //  {
    //    return screen + Position;
  //  }

}
