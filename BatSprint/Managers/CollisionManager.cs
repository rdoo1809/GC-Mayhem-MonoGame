/*
* CollisionManager class
* helps manage interactions between different game objects
 */

using BatSprint.Models;
using BatSprint.Scenes;
using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BatSprint.Managers
{
    internal class CollisionManager : GameComponent
    {
        public Hero hero;
        private Thug thug;
        private List<Batarang> batarangs;
        private List<Thug> thugs;
        private List<Stalker> stalkers;
        private List<Brute> brutes;
        private SoundEffect hitSound;
        private ActionScene currentGame;
        private StartScene endGame;

        /// <summary>
        /// constructor - takes all comps on screen - update checks for coll continuosuly
        /// </summary>
        /// <param name="game"></param>
        /// <param name="batarangs"></param>
        /// <param name="thugs"></param>
        /// <param name="stalkers"></param>
        public CollisionManager(Game game, Hero hero, List<Batarang> batarangs, SoundEffect hitSound, 
            List<Thug> thugs, List<Stalker> stalkers, List<Brute> brutes, ActionScene aS) : base(game)
        {
            this.hero = hero;
            this.batarangs = batarangs;
            this.thugs = thugs;
            this.stalkers = stalkers;
            this.brutes = brutes;
            this.hitSound = hitSound;
            this.currentGame = aS;
            endGame = new StartScene(game);
        }

        /// <summary>
        /// cm update - continuously checking hero, batarang, and thug boundaries - searching for hit
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Rectangle heroRect = hero.getBounds();

            //collisions between batarangs and enemies
            if (batarangs != null)
            {
                for (int batarangIndex = batarangs.Count - 1; batarangIndex >= 0; batarangIndex--)
                {
                    Batarang b = batarangs[batarangIndex];
                    Rectangle batarangRect = b.getBounds();
                    // Check collisions with thugs
                    for (int thugIndex = thugs.Count - 1; thugIndex >= 0; thugIndex--)
                    {
                        Rectangle thugRect = thugs[thugIndex].getBounds();

                        if (batarangRect.Intersects(thugRect))
                        {
                            thugs[thugIndex].lives--;
                            b.onScreen = false;
                            hitSound.Play();
                            currentGame.playerScore++;

                            if (thugs[thugIndex].lives == 0)
                            {
                                thugs[thugIndex].stillAlive = false;
                            }
                        }

                        // Remove thug if not still alive
                        if (!thugs[thugIndex].stillAlive)
                        {
                            thugs.RemoveAt(thugIndex);
                        }
                    }

                    // Check collisions with stalkers
                    for (int stalkerIndex = stalkers.Count - 1; stalkerIndex >= 0; stalkerIndex--)
                    {
                        Rectangle stalkerRect = stalkers[stalkerIndex].getBounds();

                        if (batarangRect.Intersects(stalkerRect))
                        {
                            stalkers[stalkerIndex].lives--;
                            b.onScreen = false;
                            hitSound.Play();
                            currentGame.playerScore++;

                            if (stalkers[stalkerIndex].lives == 0)
                            {
                                stalkers[stalkerIndex].stillAlive = false;
                            }
                        }
                        // Remove stalker if not still alive
                        if (!stalkers[stalkerIndex].stillAlive)
                        {
                            stalkers.RemoveAt(stalkerIndex);
                        }
                    }

                    //brutes
                    for (int bruteIndex = brutes.Count - 1; bruteIndex >= 0; bruteIndex--)
                    {
                        Rectangle bruteRect = brutes[bruteIndex].getBounds();

                        if (batarangRect.Intersects(bruteRect))
                        {
                            brutes[bruteIndex].lives--;
                            b.onScreen = false;
                            hitSound.Play();
                            currentGame.playerScore++;

                            if (brutes[bruteIndex].lives == 0)
                            {
                                brutes[bruteIndex].stillAlive = false;
                            }
                        }
                        // Remove stalker if not still alive
                        if (!brutes[bruteIndex].stillAlive)
                        {
                            brutes.RemoveAt(bruteIndex);
                        }
                    }
                }
            }

            //collision between hero and enemies - triggers method for enemy to hit - checks for F from hero
            foreach (var s in stalkers.ToList())
            {
                Rectangle stalkRect = s.getBounds();
                bool isIntersecting = stalkRect.Intersects(heroRect);
                if (isIntersecting)
                {
                    //check for hero hit on enemy
                    var keyBoardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                    if (keyBoardState.IsKeyDown(Keys.F))
                    {
                        //hero hits enemy
                        if (hero.canHit)
                        {
                            hero.canHit = false;
                            s.position -= new Vector2(0, 50);
                            s.lives--;
                            currentGame.playerScore += 2;
                            hitSound.Play();
                            if (s.lives == 0)
                            {
                                s.stillAlive = false;
                            }

                            //reset hero cooldown only when the hero successfully hits an enemy
                            hero.cooldownTime = 200;
                        }
                    }
                    //remove stalk if not still alive
                    if (!s.stillAlive)
                    {
                        stalkers.Remove(s);
                    }
                    //hit hero if cooldown is good - 10-second cooldown before able to hit again
                    if (s.canHit && s.stillAlive)
                    {
                        s.punchHero(hero, s);
                        // Apply cooldown
                        s.canHit = false;
                        s.cooldownTime = 600;
                    }
                }
                //check cooldown and reset canHit after the cooldown period
                if (!s.canHit && s.cooldownTime <= 0)
                {
                    s.canHit = true;
                }

                s.cooldownTime--;
            }
            foreach (var t in thugs.ToList())
            {
                Rectangle thugRect = t.getBounds();
                bool isIntersecting = thugRect.Intersects(heroRect);
                if (isIntersecting)
                {
                    //check for hero hit on enemy
                    var keyBoardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                    if (keyBoardState.IsKeyDown(Keys.F))
                    {
                        //hero hits enemy
                        if (hero.canHit)
                        {
                            hero.canHit = false;
                            t.position -= new Vector2(0, 50);
                            t.lives--;
                            currentGame.playerScore += 2;
                            hitSound.Play();
                            if (t.lives == 0)
                            {
                                t.stillAlive = false;
                            }
                            hero.cooldownTime = 200;
                        }
                    }
                    //remove thug if not still alive
                    if (!t.stillAlive)
                    {
                        thugs.Remove(t);
                    }
                    //if cooldown is good - 10-second cooldown before able to hit again
                    if (t.canHit && t.stillAlive)
                    {
                        t.punchHero(hero, t);
                        // Apply cooldown
                        t.canHit = false;
                        t.cooldownTime = 600;
                    }
                }
                //check cooldown and reset canHit after the cooldown period
                if (!t.canHit && t.cooldownTime <= 0)
                {
                    t.canHit = true;
                }

                t.cooldownTime--;
            }

            foreach (var b in brutes.ToList())
            {
                Rectangle bRect = b.getBounds();
                bool isIntersecting = bRect.Intersects(heroRect);
                if (isIntersecting)
                {
                    //check for hero hit on enemy
                    var keyBoardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                    if (keyBoardState.IsKeyDown(Keys.F))
                    {
                        //hero hits enemy
                        if (hero.canHit)
                        {
                            hero.canHit = false;
                            b.position -= new Vector2(0, 50);
                            b.lives--;
                            currentGame.playerScore += 2;
                            hitSound.Play();
                            if (b.lives == 0)
                            {
                                b.stillAlive = false;
                            }
                            hero.cooldownTime = 200;
                        }
                    }
                    //remove if not still alive
                    if (!b.stillAlive)
                    {
                        brutes.Remove(b);
                    }
                    //if cooldown is good - 10-second cooldown before able to hit again
                    if (b.canHit && b.stillAlive)
                    {
                        b.punchHero(hero, b);
                        // Apply cooldown
                        b.canHit = false;
                        b.cooldownTime = 600;
                    }
                }
                //check cooldown and reset canHit after the cooldown period
                if (!b.canHit && b.cooldownTime <= 0)
                {
                    b.canHit = true;
                }

                b.cooldownTime--;
            }


            //check hero cooldown and reset canHit after the cooldown period
            if (!hero.canHit && hero.cooldownTime <= 0)
            {
                hero.canHit = true;
            }
            hero.cooldownTime--;

            base.Update(gameTime);
        }
    }//
}
