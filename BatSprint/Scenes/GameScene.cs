/*
* GameScene
* parent scene of all other scenes
 */

using BatSprint.Managers;
using BatSprint.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace BatSprint.Scenes
{
    internal class GameScene : DrawableGameComponent
    {
        //gameScene inherits from the DGC -> GC
        //Start, Action, Help, Credits scenes all inherit from Gamecene
        public Hero hero;
        public List<Batarang> batarangs;
        public SpriteFont regularFont;
        public SpriteFont hiliFont;
        private SpriteBatch sb;
        public Song bgMusic;
        public Texture2D backGround;
        //creating list of components which will be shown/hidden
        public List<GameComponent> Components { get; set; }
        public List<Thug> thugs = new List<Thug>();
        public List<Brute> brutes = new List<Brute>();
        public List<Stalker> stalkers = new List<Stalker>();
        public int playerScore;

        /// <summary>
        /// GameScene const
        /// </summary>
        /// <param name="game"></param>
        protected GameScene(Game game) : base(game)
        {
            hero = new(new(Global.stage.X, 0)) ;
            Components = new List<GameComponent>();
            sb = Global.sb;
            regularFont = game.Content.Load<SpriteFont>("fonts/RegularFont");
            hiliFont = game.Content.Load<SpriteFont>("fonts/HighlightFont");
            hide();
            MediaPlayer.Volume = 0.2f;
        }

        /// <summary>
        /// update for GS - updated comps in indiv scenes list
        /// </summary>
        /// <param name="gameTime">time related information</param>
        public override void Update(GameTime gameTime)
        {
            //check for input - and changes in hero
            InputManager.Update();
            hero.Update();
            //call update for each batarang
            if (batarangs != null)
            {       //remove ones that travel off screen
                for (int i = batarangs.Count - 1; i >= 0; i--)
                {
                    Batarang b = batarangs[i];
                    if (b.onScreen == false)
                    {
                        batarangs.RemoveAt(i);
                    }
                    else
                    {
                        b.Update();
                    }
                }
            }


            //iterating through all components in list
            foreach (GameComponent item in Components)
            {
                //only if component is currently enabled does it need to be updated
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// draw for GS - draws componenets added to indiv scnenes list
        /// </summary>
        /// <param name="gameTime">time related information</param>
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            hero.Draw();
            if (backGround != null)
            {
                sb.Draw(backGround, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
            }
            if (batarangs != null)
            {
                foreach (Batarang b in batarangs)
                {
                    b.Draw();
                }
            }
            sb.End();

            //if item is DGC - unbox it/typecast to a DGC - can now be drawn
            foreach (GameComponent item in Components)
            {
                if (item is DrawableGameComponent)
                {
                    DrawableGameComponent comp = (DrawableGameComponent)item;
                    if (comp.Visible)
                    {
                        comp.Draw(gameTime);
                    }
                }
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// checks if any enemies on screen
        /// </summary>
        /// <returns>returns true if no enemies on screen</returns>
        public bool allEnemiesDefeated()
        {
            if (thugs.Count + stalkers.Count + brutes.Count == 0)
            {
                return true;
            }
            return false;
        }

        //helper functions - hide/show app scene - start/stop bg music
        /// <summary>
        /// hides scene
        /// </summary>
        public virtual void hide()
        {
            Enabled = false;
            Visible = false;
            MediaPlayer.Stop();
        }
        /// <summary>
        /// shows scene
        /// </summary>
        public virtual void show()
        {
            Visible = true;
            Enabled = true;

            if (bgMusic != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(bgMusic);
            }
        }
    }//
}
