/*
* AnimationManager class
* used by various other classes to help manage animations
 */

using BatSprint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BatSprint.Managers
{
    public class AnimationManager
    {
        //dict used to store animation frames and ref
        private readonly Dictionary<object, Animation> _anims = new();
        private object _lastKey;

        /// <summary>
        /// accepts key and ani - adds to dict
        /// </summary>
        /// <param name="key"></param>
        /// <param name="animation"></param>
        public void AddAnimation(object key, Animation animation)
        {
            _anims.Add(key, animation);
            _lastKey ??= key;
        }

        /// <summary>
        /// updates to correct ani sequence
        /// </summary>
        /// <param name="key"></param>
        public void Update(object key)
        {
            if (_anims.ContainsKey(key))
            {
                _anims[key].Start();
                _anims[key].Update();
                _lastKey = key;
            }
            else
            {
                _anims[_lastKey].Stop();
                _anims[_lastKey].Reset();
            }
        }

        /// <summary>
        /// draw method
        /// </summary>
        /// <param name="position"></param>
        public void Draw(Microsoft.Xna.Framework.Vector2 position)
        {
            _anims[_lastKey].Draw(position);
        }
    }
}
