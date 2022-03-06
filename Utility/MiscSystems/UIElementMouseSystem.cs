using System;
using System.Collections.Generic;
using System.Text;

using GameCore.UI;

namespace GameCore.Utilty.MiscSystems
{
    public class UIElementMouseSystem : IMiscSystem
    {
        public bool CanDisplayMultipleElements { get; set; }

        private LinkedList<UIElement> _elements;

        public UIElementMouseSystem()
        {
            _elements = new LinkedList<UIElement>();
        }

        public void SetCurrentElement(UIElement element)
        {
            _elements.AddLast(element);
        }

        public void RemoveElement(UIElement element)
        {
            _elements.Remove(element);
        }

        public void Update()
        {
            if (_elements.Count == 0) return;

            if (!CanDisplayMultipleElements) _elements.Last.Value.Update();
            else foreach (var el in _elements) el.Update();
        }

        public void Draw()
        {
            if (_elements.Count == 0) return;

            if (!CanDisplayMultipleElements) _elements.Last.Value.Draw();
            else foreach (var el in _elements) el.Draw();
        }
    }
}
