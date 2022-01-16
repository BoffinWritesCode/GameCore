using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameCore.Utility.Coroutines
{
    public class YieldForSeconds : IYieldObject
    {
        private float _timeLeft;

        public YieldForSeconds(float seconds)
        {
            _timeLeft = seconds;
        }

        public bool Handle()
        {
            _timeLeft -= Time.DeltaTime;
            return _timeLeft <= 0f;
        }
    }
}
