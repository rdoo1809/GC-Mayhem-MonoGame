/*
* Stalker class
* 1/3 enemy types - punches and follows
 */

using BatSprint.Managers;
using BatSprint.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BatSprint.Models
{
    internal class Stalker : DrawableGameComponent
    {
        //general props
        public Vector2 position;
        public Texture2D tex;
        private readonly float speed = 20f;
        public bool stillAlive = true;
        public int lives = 2;
        public bool canHit = true;
        public int hitCooldown = 200;
        public int cooldownTime = 600;
        public SoundEffect hitSound;
        public Timer hitTimer;
        public Hero heroIns { get; set; }
        private SpriteBatch sb;

        /// <summary>
        /// const - instant new stalker at passed in position - position off screen
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sb"></param>
        /// <param name="pos"></param>
        public Stalker(Game game, SpriteBatch sb, Vector2 pos, Hero heroIns) : base(game)
        {
            this.tex = Global.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("images/stalkSprite");
            this.position = pos;
            this.sb = sb;
            this.heroIns = heroIns;
            hitSound = game.Content.Load<SoundEffect>("audio/hitSound");
        }

        /// <summary>
        /// updates stalker position - updated to follow hero
        /// </summary>
        public override void Update(GameTime gametime)
        {
            //getting hero current position and direction to travel
            Vector2 heroPos = heroIns.position;
            Vector2 direction = heroIns.position - position;
            direction.Normalize();

            position += direction * speed * (float)gametime.ElapsedGameTime.TotalSeconds;

            base.Update(gametime);
        }

        /// <summary>
        /// draws stalker
        /// </summary>
        public override void Draw(GameTime gametime)
        {
            sb.Begin();
            if (this.stillAlive == true)
            {
                sb.Draw(tex, position, Color.White);
            }
            sb.End();
            base.Draw(gametime);
        }

        /// <summary>
        /// helper function - returns bounds of stalker texture
        /// </summary>
        /// <returns></returns>
        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
        }

        /// <summary>
        /// random num between 1-10 for stalk to punch hero - 
        /// leaves for chance to punch b4 getting punched
        /// </summary>
        /// <param name="hero">needs access to hero obejct to inflict damage</param>
        public void punchHero(Hero hero, Stalker s)
        {
            Rectangle heroRect = hero.getBounds();
            Rectangle stalkRect = s.getBounds();

            // Calculate a random value between 1-3 seconds enemy will hit in that window
            float randomDelay = (float)(new Random().NextDouble() * 2) + 1;
            // Allow hit after the random delay
            Timer timer = new Timer();
            timer.Interval = randomDelay * 1000; // Convert to ms
            timer.Start();
            timer.Elapsed += (_, _) =>
            { //if still intersecting after timer - register hit
                bool isStillIntersecting = stalkRect.Intersects(heroRect);
                //after elapsed - if still intersecting - register hit
                if (isStillIntersecting)
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
