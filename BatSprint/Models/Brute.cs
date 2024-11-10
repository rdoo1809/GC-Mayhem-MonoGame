/*
* Brute class
* 1/3 enemy types - larger tougher enemy
 */

using BatSprint.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using SharpDX.MediaFoundation.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using BatSprint.Scenes;
using Microsoft.Xna.Framework.Audio;

namespace BatSprint.Models
{
    public class Brute : DrawableGameComponent
    {
        //general props
        public Vector2 position;
        public Texture2D tex;
        private readonly float speed = 20f;
        public bool stillAlive = true;
        public int lives = 6;
        //dir related
        private bool movingRight; // = true;
        private bool movingUp; //= false;
        public float travelDuration = 150.0f;
        private float traveledRight = 0.0f;
        private float traveledLeft = 0.0f;
        private float traveledUp = 0.0f;
        private float traveledDown = 0.0f;
        //access to animation class
        private readonly AnimationManager anims = new();
        private SpriteBatch sb;
        //
        public Game game;
       // public List<Bullet> bullets = new List<Bullet>();
        //private int shotCooldown = 600; // 600 frames (assuming 60 FPS, this is 5 seconds)
        //private int timeSinceLastShot = 0;
        private Hero hero;
        private Vector2 target;
        public bool canHit = true;
        public int hitCooldown = 200;
        public int cooldownTime = 600;
        public SoundEffect hitSound;
        public Timer hitTimer;

        /// <summary>
        /// const - instant new thug at passed in position
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sb"></param>
        /// <param name="pos"></param>
        public Brute(Game game, SpriteBatch sb, int lives, Vector2 pos, Hero hero, bool movingRight, bool movingUp) : base(game)
        {
            this.game = game;
            this.tex = Global.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("images/bruteSprite");
            anims.AddAnimation(new Vector2(0, 1), new(tex, 3, 4, 0.1f, 1));
            anims.AddAnimation(new Vector2(-1, 0), new(tex, 3, 4, 0.1f, 2));
            anims.AddAnimation(new Vector2(1, 0), new(tex, 3, 4, 0.1f, 3));
            anims.AddAnimation(new Vector2(0, -1), new(tex, 3, 4, 0.1f, 4));
            this.position = pos;
            this.sb = sb;
            hitSound = game.Content.Load<SoundEffect>("audio/hitSound");
            this.hero = hero;
            this.movingRight = movingRight;
            this.movingUp = movingUp;
            this.lives = lives;
        }

        /// <summary>
        /// updates thug position - basic thug walks on pre-defined path
        /// </summary>
        public override void Update(GameTime gametime)
        {
            // Controls character movement animation - right, left, up, down
            if (movingRight && !movingUp)
            {
                //move Right
                position.X += speed * Global.TotalSeconds;
                traveledRight += speed * Global.TotalSeconds;
                // Check if reached the spec distance 
                if (traveledRight >= travelDuration) //Global.stage.X - 100)
                {
                    // If reached duration - move left
                    movingRight = false;
                    traveledRight = 0.0f;
                }
                // Update the animation based on the direction
                anims.Update(new Vector2(1, 0)); //right direction
            }
            else if (!movingRight && !movingUp)
            {
                // move Left
                position.X -= speed * Global.TotalSeconds;
                traveledLeft += speed * Global.TotalSeconds;
                // Check if reached the spec distance
                if (traveledLeft >= travelDuration)
                {
                    // If reached, start moving up
                    movingRight = true;
                    movingUp = true;
                    traveledLeft = 0.0f; // Reset the traveled distance
                }
                // Update the animation based on the direction
                anims.Update(new Vector2(-1, 0));  //left direction
            }
            else if (movingRight && movingUp)
            {
                // move Up
                position.Y -= speed * Global.TotalSeconds;
                traveledUp += speed * Global.TotalSeconds;
                if (traveledUp >= travelDuration)
                {
                    // If reached the top boundary, start moving right
                    movingRight = false;
                    movingUp = true;
                    traveledUp = 0.0f; // Reset the traveled distance
                }
                anims.Update(new Vector2(0, -1));
            }
            else if (!movingRight && movingUp)
            {
                // Down
                position.Y += speed * Global.TotalSeconds;
                traveledDown += speed * Global.TotalSeconds;
                if (traveledDown >= travelDuration)
                {
                    // If reached origin - reset on right
                    movingRight = true;
                    movingUp = false;
                    traveledDown = 0.0f;
                }
                anims.Update(new Vector2(0, 1));
            } 

            //shooting control
          /*  if (timeSinceLastShot > 0)
            {
                timeSinceLastShot--;
            }
            //hero position = bullet trajectory
            if (timeSinceLastShot <= 0)
            {
                if (stillAlive)
                {
                    AddBulletWithDelay();
                }
                timeSinceLastShot = shotCooldown;
            }
            List<Bullet> bulletsToRemove = new List<Bullet>();
            if (bullets != null)
            {
                foreach (Bullet b in bullets)
                {
                    b.Update();
                    if (!b.onScreen)
                    {
                        bulletsToRemove.Add(b);
                    }
                }
            }
            foreach (Bullet b in bulletsToRemove)
            {
                bullets.Remove(b);
            } */
        }

        /// <summary>
        /// draws thug
        /// </summary>
        public override void Draw(GameTime gametime)
        {
            sb.Begin();
            if (this.stillAlive == true)
            {
                anims.Draw(position);
            }
            //
            /*   if (bullets != null)
               {
                   foreach (Bullet b in bullets)
                   {
                       if (b.onScreen)
                       {
                           b.Draw(gametime);
                       }
                   } 

               } */
            sb.End();
            base.Draw(gametime);
        }

        /// <summary>
        /// helper function - returns bounds of thug texture
        /// </summary>
        /// <returns></returns>
        public Rectangle getBounds()
        {
           return new Rectangle((int)position.X, (int)position.Y, tex.Width / 3, tex.Height / 4);
        }

        /// <summary>
        /// helper func - adds a bullet to thugs list after random delat - so shot will happen randomly
        /// </summary>
       /* private void AddBulletWithDelay()
        {
            // Calculate a random value between 1-10 seconds
            float randomDelay = (float)(new Random().NextDouble() * 9) + 1;

            //add bullet to list after random delay
            Timer timer = new Timer();
            timer.Interval = randomDelay * 1000; //convert to ms

            timer.Elapsed += (_, _) =>
            {
                // Calculate the direction to the heros position
                Vector2 target = new Vector2(hero.position.X, hero.position.Y + 20);//hero.position - new Vector2(20, 20);
                Vector2 direction = Vector2.Normalize(target - position);

                // Add the bullet to the list
                Bullet newBull = new Bullet(game, sb, direction, position, hero);
                this.bullets.Add(newBull);
                timer.Dispose();
            };
            timer.Start();
        }
        */

        /// <summary>
        /// random num between 1-10 for stalk to punch hero - 
        /// leaves for chance to punch b4 getting punched
        /// </summary>
        /// <param name="hero"></param>
        public void punchHero(Hero hero, Brute b)
        {
            Rectangle heroRect = hero.getBounds();
            Rectangle stalkRect = b.getBounds();

            //random value between 1-3 seconds enemy will hit in that window
            float randomDelay = (float)(new Random().NextDouble() * 2) + 1;
            //hit after random delay
            Timer timer = new Timer();
            timer.Interval = randomDelay * 1000;
            timer.Start();
            timer.Elapsed += (_, _) =>
            { //if still intersecting after timer - register hit
                bool isStillIntersecting = stalkRect.Intersects(heroRect);
                //after elapsed - if still intersecting - register hit
                if (isStillIntersecting && b.stillAlive)
                {
                    hitSound.Play();
                    hero.lives--;
                    hero.position += new System.Numerics.Vector2(0, 50);
                }
                timer.Dispose();
            };
        }
    }//
}