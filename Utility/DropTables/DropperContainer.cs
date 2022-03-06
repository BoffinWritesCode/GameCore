using System;
using System.Collections.Generic;
using System.Text;

using GameCore.Utility.DropTables.Droppers;

namespace GameCore.Utility.DropTables
{
    /// <summary>
    /// Concatenates the results of all droppers within it.
    /// Is considered the "base" object, handles some minor optimisation and convenience methods.
    /// </summary>
    public class DropperContainer<T>
    {
        private Dictionary<T, int> _results;
        private Dictionary<T, float> _chances;
        private List<IDropper<T>> _droppers;

        public DropperContainer()
        {
            _droppers = new List<IDropper<T>>();
            _results = new Dictionary<T, int>();
            _chances = new Dictionary<T, float>();
        }

        public DropperContainer(params IDropper<T>[] droppers)
        {
            _droppers = new List<IDropper<T>>(droppers);
            _results = new Dictionary<T, int>();
            _chances = new Dictionary<T, float>();
        }

        public DropperContainer<T> AddDropper(IDropper<T> dropper)
        {
            _droppers.Add(dropper);
            return this;
        }

        public DropperContainer<T> AddSimpleDrop(T value, float chance)
        {
            _droppers.Add(new SimpleDropper<T>(value, chance));
            return this;
        }

        public DropperContainer<T> AddSimpleDrop(T value, int minInclusive, int maxInclusive, float chance)
        {
            _droppers.Add(new SimpleDropper<T>(value, minInclusive, maxInclusive, chance));
            return this;
        }

        /// <summary>
        /// Drops.
        /// </summary>
        /// <param name="count">The amount of times to drop.</param>
        public Dictionary<T, int> Drop(Random random, int count = 1)
        {
            _results.Clear();
            for (int i = 0; i < count; i++)
            {
                foreach (IDropper<T> dropper in _droppers)
                {
                    dropper.Drop(random, _results);
                }
            }
            return _results;
        }

        /// <summary>
        /// Gets the chances of each drop dropping.
        /// </summary>
        public Dictionary<T, float> Chances()
        {
            _chances.Clear();
            foreach (IDropper<T> dropper in _droppers)
            {
                dropper.Chances(_chances, 1f);
            }
            return _chances;
        }

        /// <summary>
        /// Gets the chances associated with this container and WriteLine's them (using Debug, not Console).
        /// </summary>
        public void DebugChances(Func<T, string> keyConversion = null)
        {
            var dict = Chances();
            StringBuilder builder = new StringBuilder();
            foreach (var kvp in dict)
            {
                if (keyConversion == null)
                {
                    builder.AppendLine(kvp.Key.ToString() + ": " + kvp.Value);
                }
                else
                {
                    builder.AppendLine(keyConversion(kvp.Key) + ": " + kvp.Value);
                }
            }
            System.Diagnostics.Debug.WriteLine(builder.ToString());
        }
    }
}
