/*
* Game1
* game1 file gotham city mayhem project - main connector of everything
* Project Commenced: November 23rd 2023
* Project Completed: December 10th 2023
 */

using BatSprint.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BatSprint;
using BatSprint.Managers;
using static System.Net.Mime.MediaTypeNames;
using BatSprint.Models;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.ComponentModel;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using System.DirectoryServices.ActiveDirectory;

namespace BatSprint
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        //
        //SpriteFont regularFont;
        public Song themeMusic;
        private Texture2D backGround;
        //
        public int lifePoints = 5;
        public int timeForComplete = 0;
        //
        //delcare all scenes here
        private StartScene startScene;
        private HelpScene helpScene;
        private HighScores scoreScene;
        private ActionScene actionScene;
        private SelectScene selectScene;
        private EndScene endScene;
        private CreditScene creditScene;

        /// <summary>
        /// Game1 constructor
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// init gameScreen dimensions - starting background + music
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //dimensions of game screen - initialized immidiately
            Global.stage = new Vector2(_graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);
            Global.Content = Content;
            themeMusic = Content.Load<Song>("audio/themeSong");
            backGround = Content.Load<Texture2D>("images/rooftop");
            base.Initialize();
        }

        /// <summary>
        /// instants all scenes and adds to comp list
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Global.sb = _spriteBatch;
            // TODO: use this.Content to load your game content here

            //instantiate all scenes here
            startScene = new StartScene(this);
            this.Components.Add(startScene);
            helpScene = new HelpScene(this);
            this.Components.Add(helpScene);
            scoreScene = new HighScores(this);
            this.Components.Add(scoreScene);
            actionScene = new ActionScene(this);
            this.Components.Add(actionScene);
            selectScene = new SelectScene(this);
            this.Components.Add(selectScene);
            endScene = new EndScene(this, startScene);
            this.Components.Add(endScene);
            creditScene = new CreditScene(this);
            this.Components.Add(creditScene);
            //make only startscene active
            startScene.show();
        }

        /// <summary>
        /// update for Game1 - controls menu system - scene showing
        /// </summary>
        /// <param name="gameTime">time rel info</param>
        protected override void Update(GameTime gameTime)
        {
            // Update logic for menu system
            KeyboardState ks = Keyboard.GetState();
            int selectedIndex = 0;
            if (startScene.Enabled) //if start/menu screen shown
            {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == 0 && ks.IsKeyDown(Keys.Enter)) //enter on menu items loads that scene
                {
                    hideAllScenes();
                    selectScene.show();
                    //show select scene so user can choose difficulty
                }
                else if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.hide();
                    helpScene.show();
                }
                else if (selectedIndex == 2 && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.hide();
                    scoreScene.show();
                }
                else if (selectedIndex == 3 && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.hide();
                    creditScene.show();
                }
                else if (selectedIndex == 4 && ks.IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }
            //
            if (actionScene.Enabled || helpScene.Enabled || selectScene.Enabled || scoreScene.Enabled || endScene.Enabled || creditScene.Enabled)
            {
                if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    hideAllScenes();
                    startScene.show();
                }
            }

            //select easy or hard level
            if (selectScene.Enabled)
            {
                int selectedLevel = selectScene.Menu.SelectedIndex;
                if (selectedLevel == 0)
                {
                    selectScene.caption = "Controlled Chaos the Preem Choice for Beginners";
                }
                else if (selectedLevel == 1)
                {
                    selectScene.caption = "Five Rounds of All Out Mayhem";
                }

                if (selectedLevel == 0 && ks.IsKeyDown(Keys.S)) //easy
                {
                    actionScene.currentLevel = 1;
                    actionScene.playerScore = 0;
                    MediaPlayer.Stop();
                    hideAllScenes();
                    actionScene.show();
                    timeForComplete = 36000;
                    if (actionScene.allEnemiesDefeated())
                    {
                        actionScene.genWave();
                    }
                }
                else if (selectedLevel == 1 && ks.IsKeyDown(Keys.S))
                {
                    actionScene.currentLevel = 2;
                    actionScene.playerScore = 0;
                    MediaPlayer.Stop();
                    hideAllScenes();
                    actionScene.show();
                    timeForComplete = 36000;
                    if (actionScene.allEnemiesDefeated())
                    {
                        actionScene.genWave();
                    }
                }

            }

            //
            //determines if another wave should be instant or end game
            if (actionScene.allEnemiesDefeated())
            {
                if (actionScene.currentLevel == 1)
                {//level 1 has 2 rounds
                    if (actionScene.rounds < actionScene.levOneRounds)
                    {
                        actionScene.genWave();
                    }
                    else //end game
                    {
                        endGame(false);
                        actionScene.rounds = -1;
                    }
                }
                else if (actionScene.currentLevel == 2)
                {//level 2 has 5 rounds
                    if (actionScene.rounds < actionScene.levTwoRounds)
                    {
                        actionScene.genWave();
                    }
                    else //end game
                    {
                        endGame(false);
                        actionScene.rounds = -1;
                    }
                }
            }

            //
            //if hero runs out of lives end game
            if (actionScene.hero.lives == 0)
            {
                endGame(true);
            }

            //
            //re init game after game is over
            if (endScene.endGame)
            {
                endScene.hide();
                LoadContent();
            }
            if (timeForComplete != 0)
            {
                timeForComplete--;
            }
            Global.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// draw method for Game1
        /// </summary>
        /// <param name="gameTime">time information needed</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here 
            _spriteBatch.Begin();
            _spriteBatch.Draw(backGround, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// helper func - manages ending game - transition to endScene
        /// </summary>
        int endOnce = 0;
        public void endGame(bool dead)
        {
            if (endOnce == 0) // ensures not called repeatedly
            {
                if (dead == true)
                {
                    //hero died
                    endScene.finalString = "Gotham's underworld was too mcuch for this hero\nBetter luck next time";
                }
                else
                {
                    endScene.finalString = "Congratulations on saving Gotham ManBat!";
                }
                if (actionScene.currentLevel == 2) // save score information
                {
                    //other factors affecting score - how quickly you beat the level - how many lives left
                    int livesLeft = actionScene.hero.lives;
                    for (int i = 0; i < livesLeft; i++)
                    {
                        actionScene.playerScore += lifePoints; // 5 points per life left
                        actionScene.playerScore += (timeForComplete / 1000); //more points earned if beaten faster
                    }
                    scoreScene.addScore(actionScene.playerScore); //call method to add score to list
                }
                actionScene.hide();
                endScene.finalScore = actionScene.playerScore;
                endScene.Enabled = true;
                endScene.show();
            }
            endOnce++;
            //
            if (endScene.quitGame) //closes game if player preses Q in endScene
            {
                Exit();
            }
        }

        /// <summary>
        /// helper function - hides all scenes by default after instant
        /// </summary>
        private void hideAllScenes()
        {
            foreach (GameComponent item in Components)
            {
                if (item is GameScene)
                {
                    GameScene gs = (GameScene)item;
                    gs.hide();
                }
            }
        }
    }//
}