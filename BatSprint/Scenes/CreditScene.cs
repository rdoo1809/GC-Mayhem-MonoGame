/*
* CreditScene
* developer information
 */
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BatSprint.Scenes
{
    internal class CreditScene : GameScene
    {
        private SpriteBatch sb;

        public CreditScene(Game game) : base(game)
        {
            //typecasting our paramater game to Game1
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            //adding components from game
            regularFont = g.Content.Load<SpriteFont>("fonts/RegularFont");
            hiliFont = g.Content.Load<SpriteFont>("fonts/HighlightFont");       
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            sb.Begin();
            sb.DrawString(hiliFont, "Created by: Ryan Dooley", new Vector2(100, 100), Microsoft.Xna.Framework.Color.White);
            sb.DrawString(regularFont, "Purpose: PROG2370 SEC3 Final Project", new Vector2(100, 200), Microsoft.Xna.Framework.Color.White);
            sb.DrawString(regularFont, "Instructor - Sabbir Ahmed", new Vector2(100, 300), Microsoft.Xna.Framework.Color.White);
            sb.DrawString(hiliFont, "Thanks For Playing!", new Vector2(100, 400), Microsoft.Xna.Framework.Color.White);
            sb.End();

            base.Draw(gameTime);
        }

    }
}
