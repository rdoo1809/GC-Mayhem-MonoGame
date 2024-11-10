/*
* Animation class
* used by various other classes to help manage animations
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BatSprint.Models
{
    public class Animation
    {
        //sprite sheet
        private readonly Texture2D _texture;

        //bounds of frame
        private List<Microsoft.Xna.Framework.Rectangle> _sourceRectangles = new();

        //num of frames on sheet
        private readonly int _frames;

        //index of current frame
        private int _frame;

        private readonly float _frameTime;
        private float _frameTimeLeft;
        private bool _isPlaying = true;

        /// <summary>
        /// constructor - called
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="framesX"></param>
        /// <param name="framesY"></param>
        /// <param name="frameTime"></param>
        /// <param name="row"></param>
        public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int row = 1)
        {
            //
            _texture = texture;
            //time per frame
            _frameTime = frameTime;
            _frameTimeLeft = _frameTime;
            //num of frames
            _frames = framesX;

            var frameWidth = _texture.Width / framesX;
            var frameHeight = _texture.Height / framesY;

            for (int i = 0; i < _frames; i++)
            {
                _sourceRectangles.Add(new(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
            }    
        }

        /// <summary>
        /// sets bool to false
        /// </summary>
        public void Stop()
        {
            _isPlaying = false;
        }

        /// <summary>
        /// sets bool to true
        /// </summary>
        public void Start()
        {
            _isPlaying = true;
        }

        public void Reset()
        {
           _frame = 0;
            _frameTimeLeft = _frameTime;
        }

        public void Update()
        {
            if (!_isPlaying)
            {
                return;
            }
            _frameTimeLeft -= Global.TotalSeconds;

            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;
                _frame = (_frame + 1) % _frames;
            }
        }

        public void Draw(Microsoft.Xna.Framework.Vector2 pos)
        {
            Global.sb.Draw(_texture, pos, _sourceRectangles[_frame], Microsoft.Xna.Framework.Color.White, 0,
                Microsoft.Xna.Framework.Vector2.Zero, Microsoft.Xna.Framework.Vector2.One, SpriteEffects.None, 1);
        }


    }//
}
