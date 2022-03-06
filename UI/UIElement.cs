using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;
using GameCore.Animation;
using GameCore.Input;
using Microsoft.Xna.Framework.Input;

namespace GameCore.UI
{
    // TODO: Implement correct IDisposable pattern? one where you call Dispose() in the finalizer unless it's been called elsewhere?
    public class UIElement : IDisposable
    {
        // only create an animator if it's needed by the element.
        protected Animator _animator;
        public Animator Animator { get => _animator ?? (_animator = new Animator()); private set => _animator = value; }

        public UIElement Parent { get; protected set; }
        public List<UIElement> Children { get; protected set; }
        public UIRect Rect { get; protected set; }
        public UITreeSettings Settings { get; set; }

        protected RectangleF _calculatedRect;
        protected bool _dirtyRect;

        public bool Active { get; set; } = true;
        public bool ClickThrough { get; set; } = true;
        public bool DoesInputCheck { get; set; }
        public bool ForceInputUpdate { get; set; }
        protected event Action<UIElement> _onMouseEnter;
        public event Action<UIElement> OnMouseEnter { add { _onMouseEnter += value; DoesInputCheck = true; ClickThrough = false; } remove { _onMouseEnter -= value; }}
        protected event Action<UIElement> _onMouseExit;
        public event Action<UIElement> OnMouseExit { add { _onMouseExit += value; DoesInputCheck = true; ClickThrough = false; } remove { _onMouseExit -= value; }}
        protected event Action<UIElement, Input.MouseInput> _onMouseDown;
        public event Action<UIElement, Input.MouseInput> OnMouseDown { add { _onMouseDown += value; DoesInputCheck = true; ClickThrough = false; } remove { _onMouseDown -= value; }}
        protected event Action<UIElement, Input.MouseInput> _onMousePressed;
        public event Action<UIElement, Input.MouseInput> OnMousePressed { add { _onMousePressed += value; DoesInputCheck = true; ClickThrough = false; } remove { _onMousePressed -= value; }}
        protected event Action<UIElement, Input.MouseInput> _onMouseReleased;
        public event Action<UIElement, Input.MouseInput> OnMouseReleased { add { _onMouseReleased += value; DoesInputCheck = true; ClickThrough = false; } remove { _onMouseReleased -= value; }}
        protected event Action<UIElement> _onLeftClickElsewhere;
        public event Action<UIElement> OnLeftClickElsewhere { add { _onLeftClickElsewhere += value; DoesInputCheck = true; ClickThrough = false; } remove { _onLeftClickElsewhere -= value; }}
        protected event Action<UIElement> _onScroll;
        public event Action<UIElement> OnScroll { add { _onScroll += value; DoesInputCheck = true; ClickThrough = false; } remove { _onScroll -= value; }}
        
        public UIElement()
        {
            Children = new List<UIElement>();
            Rect = new UIRect();
            Rect.FillParent();

            Rect.AnchorRect.OnModifyAnchorPoint += SetDirtyRect;
            Rect.OnChangeConstraints += SetDirtyRect;

            SetDirtyRect();
        }

        public virtual void AddChild(UIElement child)
        {
            if (child.Parent != null) child.Parent.RemoveChild(this);
            
            Children.Add(child);
            if (child.Parent != this) child.Parent = this;
            child.SetDirtyRect();
            child.Settings = Settings;
            child.ForceUpdateSettings();
        }

        public virtual void RemoveChild(UIElement child)
        {
            Children.Remove(child);
            child.Parent = null;
            child.SetDirtyRect();
        }

        public virtual void ClearChildren()
        {
            foreach (UIElement child in Children)
            {
                child.Parent = null;
                child.SetDirtyRect();
            }
            Children.Clear();
        }

        public virtual void SetParent(UIElement parent)
        {
            if (Parent != null) Parent.RemoveChild(this);

            parent.AddChild(this);
        }

        public virtual void Update()
        {
            _animator?.Update();

            if (RectangleF.Intersection(CalculateRect(), ScissorTesting.CurrentScreenAreaFloat).Contains(GameInput.MousePosition))
            {
                if (!ClickThrough)
                {
                    UIInputManager.SetForemostElement(this);
                    if (ForceInputUpdate) UIInputManager.ForceInputCheck(this);
                }
            }

            int count = Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (Children[i].Active) Children[i].Update();
            }

            DoElsewhereCheck();
        }

        protected void DoElsewhereCheck()
        {
            if (DoesInputCheck && _onLeftClickElsewhere != null && UIInputManager.IsSomethingElseClicked(this, out UIElement element))
            {
                _onLeftClickElsewhere.Invoke(element);
            }
        }

        public virtual void Draw()
        {
            int count = Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (Children[i].Active) Children[i].Draw();
            }
        }

        /// <summary>
        /// Calculates the current rectangle position based on the parent.
        /// </summary>
        /// <returns></returns>
        public virtual RectangleF CalculateRect()
        {
            if (!_dirtyRect) return _calculatedRect;

            _dirtyRect = false;

            RectangleF rect = Rect.Calculate(Parent == null ? Engine.WindowManager.ScreenSizeRectangleF : Parent.CalculateRect());

            if (Settings != null && Settings.PixelPerfect)
            {
                return _calculatedRect = rect.RoundToInt();
            }
            
            return _calculatedRect = rect;
        }

        public virtual void SetDirtyRect()
        {
            _dirtyRect = true;

            foreach (var child in Children)
            {
                child.SetDirtyRect();
            }
        }

        /// <summary>
        /// Updates the settings of all child elements from this point in the UI tree.
        /// Use this if you need to ensure settings are consistent for whatever reason.
        /// </summary>
        public virtual void ForceUpdateSettings()
        {
            if (Parent != null) Settings = Parent.Settings;
            foreach (UIElement child in Children)
            {
                child.ForceUpdateSettings();
            }
        }

        public virtual void Dispose()
        {
            Active = false;
            foreach (var child in Children)
            {
                child.Dispose();
            }
        }

        private void DoIsDown(MouseInput input)
        {
            bool scroll = input == MouseInput.ScrollDown || input == MouseInput.ScrollUp;

            if (scroll) return;

            // if no click related functions then ignore
            if (_onMouseDown == null && _onMousePressed == null && _onMouseReleased == null) return;

            if (GameInput.IsJustPressed(input, true)) 
            {
                UIInputManager.SetJustPressed(this, input);
            }
        }

        internal void HandleMouseOver(bool newlyEntered)
        {
            if (!DoesInputCheck) return;

            if (newlyEntered) _onMouseEnter?.Invoke(this);
            
            // check all the input downs
            DoIsDown(MouseInput.Left);
            DoIsDown(MouseInput.Middle);
            DoIsDown(MouseInput.Right);
            DoIsDown(MouseInput.MouseButton1);
            DoIsDown(MouseInput.MouseButton2);

            if (GameInput.DeltaScroll != 0)
            {
                _onScroll?.Invoke(this);
            }
        }

        internal void HandleMouseNotOver(bool newlyExited)
        {
            if (!DoesInputCheck) return;

            if (newlyExited) _onMouseExit?.Invoke(this);
        }

        internal void HandleMousePressed(MouseInput button)
        {
            if (!DoesInputCheck) return;

            _onMousePressed?.Invoke(this, button);
        }

        internal void HandleMouseDown(MouseInput button)
        {
            if (!DoesInputCheck) return;

            _onMouseDown?.Invoke(this, button);
        }

        internal void HandleMouseUp(MouseInput button)
        {
            if (!DoesInputCheck) return;

            _onMouseReleased?.Invoke(this, button);
        }
    }
}