using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace GameCore.Utility.Coroutines
{
    public class Coroutine
    {
        private IYieldObject _current;
        private Stack<IEnumerator> routines = new Stack<IEnumerator>();

        public bool Paused { get; set; }
        public bool Finished { get; protected set; }

        public void Reset(IEnumerator routine)
        {
            routines.Clear();
            routines.Push(routine);
            Paused = false;
            Finished = false;
        }

        public void Cancel()
        {
            Finished = true;
        }

        public bool Update()
        {
            if (Finished) 
            {
                return false;
            }

            if (!Paused)
            {
                if (_current != null)
                {
                    if (_current.Handle())
                    {
                        _current = null;
                    }
                    else
                    {
                        return true;
                    }
                }

                IEnumerator peeked = routines.Peek();
                if (peeked.MoveNext() == false)
                {
                    routines.Pop();
                    if (routines.Count == 0)
                    {
                        Finished = true;
                        return false;
                    }
                }
                else if (peeked.Current is IEnumerator enumerator)
                {
                    routines.Push(enumerator);
                }
                else if (peeked.Current is IYieldObject obj)
                {
                    _current = obj;
                }
            }

            return true;
        }
    }
}
