/*
* SelectScene
* allows user to select easy or hard level
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BatSprint.Scenes
{
    internal class SelectScene : GameScene
    {
        private MenuComponent menu;
        private SpriteBatch sb;
        public Vector2 menuPos;
        private Texture2D bg;
        SpriteFont hiliFont;
        private string title = "Choose your Difficulty";
        private Vector2 titlePos = new Vector2((Global.stage.X / 2 - 200), (Global.stage.Y / 2 - 100));
        public string caption = string.Empty;
        private Vector2 captionPos = new Vector2(10, (Global.stage.Y / 2 + 200));

        //
        public SelectScene(Game game) : base(game)
        {
            //typecasting our paramater game to Game1
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            //adding components from game
            SpriteFont regularFont = g.Content.Load<SpriteFont>("fonts/RegularFont");
            hiliFont = g.Content.Load<SpriteFont>("fonts/HighlightFont");
            bg = g.Content.Load<Texture2D>("images/gothamHalf");
            string[] menuItems = { "Easy", "Hard" };
            menuPos = new Vector2((Global.stage.X / 2 - 200), (Global.stage.Y / 2 - 50));
            Menu = new MenuComponent(game, sb, regularFont, hiliFont, menuItems, menuPos);
            Components.Add(Menu);
        }

        //public property to acces private MenuComponent
        public MenuComponent Menu { get => menu; set => menu = value; }

        //
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(bg, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
            sb.DrawString(regularFont, "Press 'S' to select", new Vector2(0 ,0), Microsoft.Xna.Framework.Color.White);
            sb.DrawString(hiliFont, title, titlePos, Microsoft.Xna.Framework.Color.White);
            sb.DrawString(hiliFont, caption, captionPos, Microsoft.Xna.Framework.Color.White);
            sb.End();

            base.Draw(gameTime);
        }

    }//
}
