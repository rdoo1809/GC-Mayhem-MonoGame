/*
* StartScene
* scene loaded first when game is run - main menu
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
    internal class StartScene : GameScene
    {
        private MenuComponent menu;
        private SpriteBatch sb;
        public Vector2 menuPos;

        /// <summary>
        /// start scene constructor
        /// </summary>
        /// <param name="game"></param>
        public StartScene(Game game) : base(game)
        {
            backGround = game.Content.Load<Texture2D>("images/gothamCity");
            bgMusic = game.Content.Load<Song>("audio/themeSong");
            //typecasting our paramater game to Game1
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            menuPos = new Vector2((Global.stage.X / 2 + 100), (Global.stage.Y / 2 - 100));

            //adding components from game
            SpriteFont regularFont = g.Content.Load<SpriteFont>("fonts/RegularFont");
            SpriteFont hiliFont = g.Content.Load<SpriteFont>("fonts/HighlightFont");
            string[] menuItems = { "Start Game", "Help", "High Scores", "Credits", "Quit" };
            Menu = new MenuComponent(game, sb, regularFont, hiliFont, menuItems, menuPos);
            Components.Add(Menu);
        }

        //property
        public MenuComponent Menu { get => menu; set => menu = value; }
    }//
}
