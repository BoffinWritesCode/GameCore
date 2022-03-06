using System;
using System.Collections;
using System.Collections.Generic;

using GameCore.Maths;
using GameCore.Utility.Coroutines;

namespace GameCore.Animation
{
    public class Animator
    {
        protected List<IValueDriver> _drivers;
        protected int _currentState;
        protected int _currentlyTransitioningTo;
        protected CoroutineManager _coroutines;
        protected Coroutine _currentTransition;

        public Coroutine CurrentTransition => _currentTransition;
        public bool CanTransitionWhenTransitioning { get; set; }

        public Animator(int startState = 0)
        {
            _currentState = startState;
            _drivers = new List<IValueDriver>();
            _coroutines = new CoroutineManager();
        }

        public void AddDriver(IValueDriver driver) => _drivers.Add(driver);

        public T GetDriver<T>() where T : class, IValueDriver
        {
            foreach (IValueDriver driver in _drivers)
            {
                if (driver is T cast) return cast;
            }
            return null;
        }

        public void SetState(int index, bool cancelTransition = true)
        {
            if (cancelTransition)
            {
                _currentTransition?.Cancel();
            }

            // drive all drivers to 1f to the new index.
            foreach (IValueDriver driver in _drivers)
            {
                driver.Drive(_currentState, index, 1f);
            }

            _currentState = index;
        }

        public bool TransitionToState(int index, float time, Easing ease)
        {
            // end the current transition coroutine if there is one
            if (_currentTransition != null && !_currentTransition.Finished) 
            {
                if (!CanTransitionWhenTransitioning) return false;
                
                SetState(_currentlyTransitioningTo);
            }

            _currentTransition = _coroutines.StartCoroutine(DoTransition(index, time, ease));
            return true;
        }

        public void Update()
        {
            _coroutines.UpdateCoroutines();
        }

        public bool CanTransition()
        {
            if (CanTransitionWhenTransitioning) return true;

            return _currentTransition == null || _currentTransition.Finished;
        }

        IEnumerator DoTransition(int index, float time, Easing ease)
        {
            _currentlyTransitioningTo = index;
            float progress = 0f;
            while (progress < time)
            {
                progress += Time.DeltaTime;

                // drive all the values
                foreach (IValueDriver driver in _drivers)
                {
                    driver.Drive(_currentState, index, ease.Ease(progress / time));
                }

                yield return null;
            }

            SetState(index);
        }
    }
}