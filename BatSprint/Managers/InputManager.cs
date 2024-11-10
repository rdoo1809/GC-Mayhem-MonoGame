/*
* InputManager class 
* searches for input to control hero direction position
 */

using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BatSprint.Managers
{
    public static class InputManager
    {
        private static Vector2 _direction;
        public static Vector2 Direction => _direction;
        //hero is moving when the dir affected by key press is not zero
        public static bool Moving => _direction != Vector2.Zero;

        /// <summary>
        /// continuously checking for key presses - movement, f for punch
        /// </summary>
        public static void Update()
        {
            _direction = Vector2.Zero;
            //keyboard input check
            var keyBoardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            //first checks for any pressed keys
            if (keyBoardState.GetPressedKeyCount() > 0)
            {
                //movement - w-a-s-d or directional keys moves hero
                if (keyBoardState.IsKeyDown(Keys.A) || keyBoardState.IsKeyDown(Keys.Left))
                {
                    _direction.X--;
                }
                else if (keyBoardState.IsKeyDown(Keys.D) || keyBoardState.IsKeyDown(Keys.Right))
                {
                    _direction.X++;
                }
                else if (keyBoardState.IsKeyDown(Keys.W) || keyBoardState.IsKeyDown(Keys.Up))
                {
                    _direction.Y--;
                }
                else if (keyBoardState.IsKeyDown(Keys.S) || keyBoardState.IsKeyDown(Keys.Down))
                {
                    _direction.Y++;
                }
            }
        }

    }//
}
