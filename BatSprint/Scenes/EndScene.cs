/*
* EndScene
* scene displayed when game is ended either by winning or losing all lives
 */
using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    internal class EndScene : GameScene
    {
        private SpriteBatch sb;
        private Texture2D bg;
        private StartScene startScene;
        public bool endGame;
        public bool quitGame = false;
        public string finalString = string.Empty;// "Congratulations on saving Gotham ManBat!";
        public int finalScore;


        //
        public EndScene(Game game, StartScene startOver) : base(game)
        {
            //typecasting our paramater game to Game1
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            bg = game.Content.Load<Texture2D>("images/gothamHalf");
            this.startScene = startOver;
            endGame = false;
        }


        public override void Update(GameTime gameTime)
        {
            var ks = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            // Check if the Escape key was just pressed (not held down)
            if (ks.IsKeyDown(Keys.Escape))
            {
                endGame = true;
            }
            else if (ks.IsKeyDown(Keys.Q))
            {
                quitGame = true;
            }


            base.Update(gameTime);
        }

        //
        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(bg, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
            sb.DrawString(regularFont, finalString, new Vector2(20, 10), Microsoft.Xna.Framework.Color.Red);
            sb.DrawString(regularFont, $"Score: {finalScore}", new Vector2(Global.stage.X / 2 - 40, Global.stage.Y / 2 + 100), Microsoft.Xna.Framework.Color.Red);
            sb.DrawString(regularFont, "esc to Main Menu", new Vector2(20, Global.stage.Y - 40), Microsoft.Xna.Framework.Color.Red);
            sb.DrawString(regularFont, "Q to Quit Game", new Vector2(Global.stage.X - 200, Global.stage.Y - 40), Microsoft.Xna.Framework.Color.Red);
            sb.End();

            base.Draw(gameTime);
        }

        public int retrieveScore()
        {
            return playerScore;
        }

    }//
}
