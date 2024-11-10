/*
* global
* file used to store some shared characteristics needed for game
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatSprint
{
    public static class Global
    {
        //stage is sared between all scenes
        public static Microsoft.Xna.Framework.Vector2 stage;

        //
        public static float TotalSeconds { get; set; }

        public static ContentManager Content { get; set; }

        public static SpriteBatch sb { get; set; }

        public static void Update(GameTime gt)
        {
            TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
        }




    }//
}
