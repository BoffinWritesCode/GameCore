using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public class UILayoutElement : UIElement
    {
        protected bool _dirty;

        public Padding Padding { get; set; }

        public event Action OnLayoutUpdate;

        /// <summary>
        /// The amount of space that the children take up. Used for scrolling.
        /// </summary>
        public Vector2 ChildArea { get; protected set; }

        public UILayoutElement() : base()
        {
        }

        public override void Update()
        {
            if (_dirty) 
            {
                _dirty = false;
                UpdateChildLayouts();
            }

            base.Update();
        }

        public override void SetDirtyRect()
        {
            base.SetDirtyRect();
            SetDirtyLayout();
        }

        public void SetDirtyLayout()
        {
            _dirty = true;
        }

        public override void AddChild(UIElement child)
        {
            base.AddChild(child);
            //child.Rect.OnModifyAnchorPoint += SetDirtyLayout;
            child.Rect.OnChangeConstraints += SetDirtyLayout;
            SetDirtyLayout();
        }

        public virtual void UpdateChildLayouts()
        {
            OnLayoutUpdate?.Invoke();
        }
    }
}