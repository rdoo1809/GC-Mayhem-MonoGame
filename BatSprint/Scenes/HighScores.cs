/*
* HighSCores
* class keeps track of 5 best scores, adding new score, reading and writing from txt file
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatSprint.Scenes
{
    internal class HighScores : GameScene
    {
        public List<int> highScores;
        private SpriteBatch sb;
        private string title;
        private Vector2 titlePos;
        private string scoresFilePath = @"..\..\..\scores.txt";

        /// <summary>
        /// HighScore scene const
        /// </summary>
        /// <param name="game"></param>
        public HighScores(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            sb = g._spriteBatch;
            highScores = new List<int>();
            title = "High Scores!";
            titlePos = new Vector2(Global.stage.X / 2 - title.Length * 8, 60);

            readScores();
        }

        /// <summary>
        /// draw method for HighScore page - writes line for each score in top 5
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            int yPos = 160;
            int xPos = 200;
            int rank = 1;
            sb.Begin();
            sb.DrawString(hiliFont, title, titlePos, Microsoft.Xna.Framework.Color.White);
            foreach (int i in highScores)
            {
                sb.DrawString(hiliFont, $"{rank} - " + i.ToString(), new Vector2(xPos, yPos), Microsoft.Xna.Framework.Color.White);
                yPos += 60;
                rank++;
            }
            sb.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// adds a new score to the list - replacing the lowest if higher
        /// </summary>
        /// <param name="score"></param>
        public void addScore(int score)
        {
            if (highScores.Count == 5) //if list already has 5 scores - replace lowest
            {
                int lowest = highScores.Min();
                if (score > lowest)
                {
                    highScores.Remove(lowest);
                    highScores.Add(score);
                    System.Diagnostics.Debug.WriteLine(" new " + score + " lowest " + lowest);
                }
            }
            else if (highScores.Count < 5)
            {
                highScores.Add(score);
                System.Diagnostics.Debug.WriteLine(" new " + score);
            }
            WriteScores(score);
        }

        /// <summary>
        /// writes the new score to the file
        /// </summary>
        private void WriteScores(int score)
        {
            File.WriteAllText(scoresFilePath, string.Empty);
            TextWriter entry = new StreamWriter(scoresFilePath, append: true);
            foreach (int s in highScores)
            {
                entry.Write(s + "\n");
            }
                entry.Close();

        }

        /// <summary>
        /// when scene is instant - assembles list of pre-existing scores to display
        /// </summary>
        private void readScores()
        {
            //read from file
            TextReader reader = new StreamReader(scoresFilePath);
            string line;

            //while ther is lines to read
            while ((line = reader.ReadLine()) != null)
            {
                //read line add to list
                if (int.TryParse(line, out int score))
                {
                    // Add the score to your list
                    highScores.Add(score);
                }
            }
            reader.Close();
            highScores.Sort((a, b) => b.CompareTo(a));
//            File.WriteAllText(scoresFilePath, string.Empty);
            //sort list so after it is read they are already in order
        }
    }//
}
