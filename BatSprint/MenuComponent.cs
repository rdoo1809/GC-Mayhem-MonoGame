/*
* ManuComponent class
* file defining the MenuComponent comp used on StartScene and SelectScene
 */

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatSprint
{
    internal class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch sb;
        private SpriteFont regulalarFont, highlightFont;
        //list of menu options - start, help, quit, etc
        private List<string> menuItems;
        //curr selected menu item
        public int SelectedIndex { get; set; }
        private Vector2 position;
        //
        private Color regularColor = Color.Black;
        private Color highlightColor = Color.Red;
        //more later
        private KeyboardState oldState;

        /// <summary>
        /// MenuComponent const
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sb"></param>
        /// <param name="regulalarFont"></param>
        /// <param name="highlightFont"></param>
        /// <param name="menus"></param>
        /// <param name="menuPos"></param>
        public MenuComponent(Game game, SpriteBatch sb, SpriteFont regulalarFont,
            SpriteFont highlightFont, string[] menus, Vector2 menuPos) : base(game)
        {
            this.sb = sb;
            this.regulalarFont = regulalarFont;
            this.highlightFont = highlightFont;
            menuItems = menus.ToList();
            position = menuPos; 
        }

        /// <summary>
        /// draw for menucomp
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //declare tempPos
            Vector2 tempPos = position;
            sb.Begin();
            //drawing all menu items in list
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                {
                    sb.DrawString(highlightFont, menuItems[i], tempPos, highlightColor);
                    tempPos.Y += highlightFont.LineSpacing;
                }
                else
                {
                    sb.DrawString(regulalarFont, menuItems[i], tempPos, regularColor);
                    tempPos.Y += regulalarFont.LineSpacing;
                }
            }
            sb.End();


            base.Draw(gameTime);
        }

        /// <summary>
        /// update for menucomp
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //creating the current state
            KeyboardState ks = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {   //when selected index reaches bottom of options - return to top
                SelectedIndex++;
                if (SelectedIndex == menuItems.Count)
                {
                    SelectedIndex = 0;
                }
            }
            //
            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = menuItems.Count - 1;
                }
            }

            //saving its state value as old state - on next update ks is holding oldstate
            oldState = ks;
            //
            base.Update(gameTime);
        }
    }
}
