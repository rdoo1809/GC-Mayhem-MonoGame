/*
* ActionScene
* scene containing main game
 */
using BatSprint.Managers;
using BatSprint.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BatSprint.Scenes
{
    internal class ActionScene : GameScene
    {
        private SpriteBatch sb;
        public SoundEffect hitSound;
        List<CollisionManager> collisions = new List<CollisionManager>();
        public Texture2D heart;
        public int heartX = 50;
        public Texture2D bruteSprite;
        public bool goAgain = false;
        private Game game;
        public int currentLevel = 1; // has 2 rounds same layout
        public int levOneRounds = 2;
        public int levTwoRounds = 5;
        public int rounds = -1;
        public int timeForComplete;

        /// <summary>
        /// const for action scene
        /// </summary>
        /// <param name="game"></param>
        public ActionScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this.game = g;
            sb = g._spriteBatch;
            heart = g.Content.Load<Texture2D>("images/heartTex");
            hitSound = g.Content.Load<SoundEffect>("audio/hitSound");
            bgMusic = g.Content.Load<Song>("audio/gameSong");
            regularFont = g.Content.Load<SpriteFont>("fonts/RegularFont");
            bruteSprite = g.Content.Load<Texture2D>("images/bruteSprite");
            batarangs = new List<Batarang>();
            hero.batarangs = batarangs;
            hero.isVisible = true;
            genWave();
        }

        /// <summary>
        /// helper func to indirectly call InitEntities
        /// </summary>
        public void genWave()
        {
            InitializeEntities(game, currentLevel);
        }

        /// <summary>
        /// main logic for generating levels and enemies
        /// </summary>
        /// <param name="game"></param>
        /// <param name="level">level passed in to set flow</param>
        private void InitializeEntities(Game game, int level)
        {
            rounds++;
            if (currentLevel == 1)
            {
           
                //stalkers
                //top left
                stalkers.Add(new Stalker(game, sb, new Vector2(-50, -50), hero));
                //bottom left
                stalkers.Add(new Stalker(game, sb, new Vector2(100, Global.stage.Y), hero));
                //bottom right
                stalkers.Add(new Stalker(game, sb, new Vector2(Global.stage.X, Global.stage.Y), hero));
                foreach (Stalker s in stalkers)
                {
                    Components.Add(s);
                }

                //thugs
                //bottom corner
                thugs.Add(new Thug(game, sb, new Vector2(50, (Global.stage.Y - 100)), hero, true, false));
                //top left
                thugs.Add(new Thug(game, sb, new Vector2(200, 200), hero, false, false));
                //middle
                thugs.Add(new Thug(game, sb, new Vector2(Global.stage.X / 2, Global.stage.Y / 2), hero, true, true));
                //bottom right
                thugs.Add(new Thug(game, sb, new Vector2(400, Global.stage.Y - 200), hero, false, true));
                foreach (Thug t in thugs)
                {
                    Components.Add(t);
                }
            }
            else if (currentLevel == 2)
            {
                if (rounds == 0) // same as easy level
                {
                    //stalkers
                    //top left
                    stalkers.Add(new Stalker(game, sb, new Vector2(-50, -50), hero));
                    //bottom left
                    stalkers.Add(new Stalker(game, sb, new Vector2(100, Global.stage.Y), hero));
                    //bottom right
                    stalkers.Add(new Stalker(game, sb, new Vector2(Global.stage.X, Global.stage.Y), hero));
                    foreach (Stalker s in stalkers)
                    {
                        Components.Add(s);
                    }

                    //thugs
                    //bottom corner
                    thugs.Add(new Thug(game, sb, new Vector2(50, (Global.stage.Y - 100)), hero, true, false));
                    //top left
                    thugs.Add(new Thug(game, sb, new Vector2(200, 200), hero, false, false));
                    //middle
                    thugs.Add(new Thug(game, sb, new Vector2(Global.stage.X / 2, Global.stage.Y / 2), hero, true, true));
                    //bottom right
                    thugs.Add(new Thug(game, sb, new Vector2(400, Global.stage.Y - 100), hero, false, true));
                    foreach (Thug t in thugs)
                    {
                        Components.Add(t);
                    }
                }
                else if (rounds == 1) // all stalkers
                {
                    //top left
                        stalkers.Add(new Stalker(game, sb, new Vector2(-50, -50), hero));
                        //bottom left
                        stalkers.Add(new Stalker(game, sb, new Vector2(100, Global.stage.Y), hero));
                        //bottom right
                        stalkers.Add(new Stalker(game, sb, new Vector2(Global.stage.X, Global.stage.Y), hero));
                        //bottom corner
                        stalkers.Add(new Stalker(game, sb, new Vector2(50, (Global.stage.Y - 50)), hero));
                        //top left
                        stalkers.Add(new Stalker(game, sb, new Vector2(200, 200), hero));
                        //middle
                      
                    stalkers.Add(new Stalker(game, sb, new Vector2(Global.stage.X / 2, Global.stage.Y / 2), hero));
                    foreach (Stalker s in stalkers)
                    {
                        Components.Add(s);
                    } 
                }
                else if (rounds == 2) //all thugs
                {
                    //thugs
                    //bottom corner
                    thugs.Add(new Thug(game, sb, new Vector2(50, (Global.stage.Y - 50)), hero, true, false));
                    //top left
                    thugs.Add(new Thug(game, sb, new Vector2(200, 200), hero, false, false));
                    //middle
                    thugs.Add(new Thug(game, sb, new Vector2(Global.stage.X / 2, Global.stage.Y / 2), hero, true, true));
                    //top left               
                    thugs.Add(new Thug(game, sb, new Vector2(Global.stage.X - 200, 50), hero, false, true));
                    //bottom left
                    thugs.Add(new Thug(game, sb, new Vector2(100, Global.stage.Y - 100), hero, true, false));
                        //bottom right
                        thugs.Add(new Thug(game, sb, new Vector2(Global.stage.X - 100, Global.stage.Y / 2), hero, false, true));
                    foreach (Thug t in thugs)
                    {
                        Components.Add(t);
                    }
                }
                else if (rounds == 3) //all brutes
                {
                    //
                    brutes.Add(new Brute(game, sb, 6, new Vector2(0, 100), hero, true, false));
                    brutes.Add(new Brute(game, sb, 6, new Vector2(Global.stage.X / 2 + 200, Global.stage.Y/2), hero, false, false));
                    brutes.Add(new Brute(game, sb, 6, new Vector2(400, Global.stage.Y - 200), hero, true, true));
                    foreach (Brute b in brutes)
                    {
                        Components.Add(b);
                    }
                }
                else if (rounds == 4) // all three enemy types
                {
                    //stalkers
                    //top left
                    stalkers.Add(new Stalker(game, sb, new Vector2(-50, -50), hero));
                    //bottom left
                    stalkers.Add(new Stalker(game, sb, new Vector2(100, Global.stage.Y), hero));
                    //bottom right
                    stalkers.Add(new Stalker(game, sb, new Vector2(Global.stage.X, Global.stage.Y), hero));
                    foreach (Stalker s in stalkers)
                    {
                        Components.Add(s);
                    }
                    brutes.Add(new Brute(game, sb, 6, new Vector2(400, Global.stage.Y -400), hero, true, true));
                    brutes.Add(new Brute(game, sb, 6, new Vector2(400, Global.stage.Y - 200), hero, true, true));
                    foreach (Brute b in brutes)
                    {
                        Components.Add(b);
                    }
                    //thugs
                    //bottom corner
                       thugs.Add(new Thug(game, sb, new Vector2(50, (Global.stage.Y - 50)), hero, true, false));
                    //top left
                       thugs.Add(new Thug(game, sb, new Vector2(200, 200), hero, false, false));
                    //middle
                    thugs.Add(new Thug(game, sb, new Vector2(Global.stage.X / 2, Global.stage.Y / 2), hero, true, true));
                    foreach (Thug t in thugs)
                    {
                        Components.Add(t);
                    }
                }
            }
            //cm
            collisions.Add(new CollisionManager(game, hero, batarangs, hitSound, thugs, stalkers, brutes,
                this));
            foreach (CollisionManager c in collisions)
            {
                Components.Add(c);
            }
            //
           // rounds++;
            MediaPlayer.Play(bgMusic);
        }


        /// <summary>
        /// draw method for ActionScene - drawing hearts and score
        /// </summary>
        /// <param name="gameTime">time related information</param>
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();

            for (int i = 0; i < hero.lives / 2; i++)
            {
                sb.Draw(heart, new Vector2(0 + (heartX * i), Global.stage.Y - heart.Height), Color.White);
            }
            sb.DrawString(regularFont, $"{playerScore}", new Vector2(50, 30), Color.White);
            //sb.DrawString(regularFont, $"{rounds}", new Vector2(50, 100), Color.White);
            sb.End();
            base.Draw(gameTime);
        }

    }//
}
