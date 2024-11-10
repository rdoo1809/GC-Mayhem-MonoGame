/*
* Batarang class
* projectile used by hero
 */

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace BatSprint.Models
{
    public class Batarang
    {
        private static Microsoft.Xna.Framework.Vector2 direc;
        private Microsoft.Xna.Framework.Vector2 position;
        private const float BatarangSpeed = 3.0f;
        public bool onScreen = true;
        //
        private static Texture2D texture;
        private readonly Animation anim;

        /// <summary>
        /// constructor - pos is hero current position - dir calc from mouse state
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        public Batarang(Microsoft.Xna.Framework.Vector2 pos, Microsoft.Xna.Framework.Vector2 dir)
        {
            //load specific texture
            texture ??= Global.Content.Load<Texture2D>("images/batarangSprite");
            //call Animation.cs to define the movement
            anim = new(texture, 4, 1, 0.1f);
            //assign start position
            position = pos;
            //direction to travel
            direc = dir;
        }

        /// <summary>
        /// position and animation updated continuously
        /// </summary>
        public void Update()
        {
            //update position continuously
            position += direc * BatarangSpeed;

            //remove from game once they are off screen
            //top wall - bottom wall
            if (position.Y <= 0 || position.Y >= Global.stage.Y)
            {
                onScreen = false;
            }
            //right wall - left wall
            if (position.X >= Global.stage.X || position.X <= 0)
            {
                onScreen = false;
            }


            anim.Update();
        }

        /// <summary>
        /// draw method for batarang object
        /// </summary>
        public void Draw()
        {
            anim.Draw(position);
        }
        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }//
}
