using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public class ToggleGroup
    {
        private List<UIToggle> _toggles;
        private UIToggle _currentlySelected;

        public ToggleGroup()
        {
            _toggles = new List<UIToggle>();
        }

        public void AddToggle(UIToggle toggle)
        {
            _toggles.Add(toggle);
        }

        public void RemoveToggle(UIToggle toggle)
        {
            _toggles.Add(toggle);
        }

        public void ToggleTo(UIToggle toggle)
        {
            if (_currentlySelected != null) _currentlySelected.IsSelected = false;

            _currentlySelected = toggle;

            _currentlySelected.IsSelected = true;
        }
    }
}