using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameCore.Utility.Coroutines
{
    public class YieldForFrames : IYieldObject
    {
        private int _framesLeft;

        public YieldForFrames(int frames)
        {
            _framesLeft = frames;
        }

        public bool Handle()
        {
            _framesLeft--;
            return _framesLeft <= 0;
        }
    }
}
