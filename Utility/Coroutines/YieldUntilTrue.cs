using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameCore.Utility.Coroutines
{
    public class YieldUntilTrue : IYieldObject
    {
        private Func<bool> _test;

        public YieldUntilTrue(Func<bool> function)
        {
            _test = function;

            if (function == null)
            {
                _test = () => { return true; };
            }
        }

        public bool Handle()
        {
            return _test.Invoke();
        }
    }
}
