/*
* Hero class
* main controllable char in game
 */

using Microsoft.Xna.Framework.Graphics;
using BatSprint.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using Microsoft.Xna.Framework.Input;

namespace BatSprint.Models
{
    public class Hero
    {
        public Vector2 position = new(150, 100);
        public float speed = 150f;
        //access to animation class
        public Microsoft.Xna.Framework.Graphics.Texture2D texture;
        private readonly AnimationManager anims = new();
        private Vector2 frameDimension;
        private int rows = 4;
        private int cols = 3;
        public List<Batarang> batarangs = new List<Batarang>();
        private int batarangCooldown = 200; // 600 frames (assuming 60 FPS, this is 3 seconds)
        private int timeSinceLastBatarang = 0;
        public int lives = 20;
        public bool isVisible = false;

        public bool canHit = true;
        public int hitCooldown = 200;
        public int cooldownTime = 200;

        /// <summary>
        /// constructor - instan once for main char
        /// calls AddAnimation in Animation.cs 
        /// </summary>
        public Hero(Vector2 pos)
        {
            texture = Global.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("images/batSprite");
            //var heroTexture = Global.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("images/batSprite");
            anims.AddAnimation(new Vector2(0, 1), new(texture, 3, 4, 0.1f, 1));
            anims.AddAnimation(new Vector2(-1, 0), new(texture, 3, 4, 0.1f, 2));
            anims.AddAnimation(new Vector2(1, 0), new(texture, 3, 4, 0.1f, 3));
            anims.AddAnimation(new Vector2(0, -1), new(texture, 3, 4, 0.1f, 4));
            position = pos;
            frameDimension = new Vector2(texture.Height / rows, texture.Width / cols);
        }

        /// <summary>
        /// updates hero position using input manager and 
        /// creates batarang with mouseState and left button
        /// </summary>
        public void Update()
        {
            //updates position based on InputManager checks
            if (InputManager.Moving)
            {
                position += Vector2.Normalize(InputManager.Direction) * speed * Global.TotalSeconds;
            }

            //batarang
            if (timeSinceLastBatarang > 0)
            {
                timeSinceLastBatarang--;
            }
            //mouse input for dir
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && timeSinceLastBatarang <= 0)
            {
                //creating new vector target based on where we are clicking on
                Vector2 target = new Vector2(ms.X, ms.Y);
                // Calculate the direction from the hero's position to the mouse cursor's position
                Vector2 direction = Vector2.Normalize(target - position); // Assuming 'pos' is the hero's position

                //new batarang at hero position
                Batarang newBat = new Batarang(position, direction);
                batarangs.Add(newBat);

                timeSinceLastBatarang = batarangCooldown;
            }

            // Monitor for hero at edge of screen - don't allow past
            // Top wall - Bottom wall
            if (position.Y <= 0)
            {
                position.Y = 0;
            }
            else if (position.Y >= Global.stage.Y - frameDimension.Y)
            {
                position.Y = Global.stage.Y - frameDimension.Y;
            }
            // Right wall - Left wall
            if (position.X >= Global.stage.X - frameDimension.X)
            {
                position.X = Global.stage.X - frameDimension.X;
            }
            else if (position.X <= 0)
            {
                position.X = 0;
            }

            anims.Update(InputManager.Direction);
        }

        /// <summary>
        /// draws hero
        /// </summary>
        public void Draw()
        {
            if (isVisible == true)
            {
                anims.Draw(position);
            }
        }

        /// <summary>
        /// helper function - returns bounds of hero texture
        /// </summary>
        /// <returns></returns>
        public Microsoft.Xna.Framework.Rectangle getBounds()
        {
            return new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, texture.Width / 3, texture.Height / 4); 
        }
    
    }//
}
