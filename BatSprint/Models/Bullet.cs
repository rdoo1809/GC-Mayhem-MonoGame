/*
* Bullet class
* projectile used by thugs
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;
using BatSprint.Scenes;

namespace BatSprint.Models
{
    public class Bullet : DrawableGameComponent
    {
        private Vector2 direc;
        public Vector2 position;
        public bool onScreen = true;
        private const float bulletSpeed = 3.0f;
        private static Texture2D texture;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch sb;
        private Hero hero;
        private float rotation = 0f;
        private Vector2 target;
        private Rectangle srcRect;
        private Vector2 origin;
        private float scale = 1.0f;
        private SoundEffect hitSound;

        /// <summary>
        /// bullet constructor - takes in hero position to calc dir, rotation
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sb"></param>
        /// <param name="dir"></param>
        /// <param name="pos"></param>
        /// <param name="hero"></param>
        public Bullet(Game game, Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Vector2 dir, Vector2 pos, Hero hero) : base(game)
        {
            this.sb = sb;
            texture ??= Global.Content.Load<Texture2D>("images/bulletSprite");
            //assign start position
            this.position = pos;
            //access to hero ins
            this.hero = hero;
            this.srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            //origin is center of app
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            //direction to travel
            this.direc = dir;
            this.target = new Vector2(hero.position.X, hero.position.Y);
            hitSound = game.Content.Load<SoundEffect>("audio/hitSound");

            float xDiff = target.X - position.X;
            float yDiff = target.Y - position.Y;
            //rotation
            float deviation = 0; // tan theta = ydiff / xdiff - rotation is tan inverse of (ydiff / xdiff) arktan atan
            if (xDiff < 0) //if clicking on left hand side
            {
                deviation = (float)Math.PI; //
            }
            rotation = deviation + (float)Math.Atan(yDiff / xDiff);
        }

        //
        public void Update()
        {
            //update position continuously
            position += direc * bulletSpeed;

            //check for bullet hit on hero
            Rectangle bullRect = getBounds();
            Rectangle heroRect = hero.getBounds();
            if (bullRect.Intersects(heroRect))
            {
                hero.lives--;
                onScreen = false;
                hero.position.X += 20;
                hitSound.Play();
                
            } 

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
        }

        /// <summary>
        /// draws bullet
        /// </summary>
        /// <param name="gametime"></param>
        public override void Draw(GameTime gametime)
        {
            sb.Draw(texture, position, srcRect, Color.White, rotation, origin, scale,
                SpriteEffects.None, 0);

            base.Draw(gametime);
        }

        /// <summary>
        /// helper func to retirve bounds of bullet tex
        /// </summary>
        /// <returns></returns>
        public Rectangle getBounds()
        {
            // Adjust the size of the collision rectangle - too large be default
            int smallerWidth = 20;
            int smallerHeight = 20;

            return new Rectangle((int)position.X + smallerWidth / 2, (int)position.Y + smallerHeight / 2, texture.Width - smallerWidth, texture.Height - smallerHeight);
        }
    }//
}
