using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class RevertableAction
    {
        private bool _hasRun;
        private Action _forward;
        private Action _backward;

        public RevertableAction(Action action, Action revert)
        {
            _forward = action;
            _backward = revert;
        }

        public void Run()
        {
            if (_hasRun) return;

            _forward?.Invoke();
            _hasRun = true;
        }

        public void Revert()
        {
            if (!_hasRun) return;

            _backward?.Invoke();
            _hasRun = false;
        }
    }
}
