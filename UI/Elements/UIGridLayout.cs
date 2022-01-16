using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;
using GameCore.Input;

namespace GameCore.UI.Elements
{
    public class UIGridLayout : UILayoutElement
    {
        private GridLayoutMode _layoutMode;
        public GridLayoutMode LayoutMode { get => _layoutMode; set { _layoutMode = value; UpdateChildLayouts(); } }

        /// <summary>
        /// The amount of pixel spacing between each child.
        /// </summary>
        public Vector2 Spacing { get; set; }

        /// <summary>
        /// The size of the children. Only applies when the layout mode is set to <see cref="GridLayoutMode.SetNumberOfColumns"/> or <see cref="GridLayoutMode.SetNumberOfRows"/>.
        /// <br>Specifically, it applies to certain axes when necessary. When column count is set, only the vertical height is used.</br>
        /// </summary>
        public Vector2 ChildSize { get; set; }

        /// <summary>
        /// The number of columns. Only uses this when the layout mode is set to <see cref="GridLayoutMode.SetNumberOfColumns"/>
        /// </summary>
        /// <value></value>
        public int ColumnCount { get; set; }

        /// <summary>
        /// The number of columns. Only uses this when the layout mode is set to <see cref="GridLayoutMode.SetNumberOfColumns"/>
        /// </summary>
        /// <value></value>
        public int RowCount { get; set; }

        public UIGridLayout(GridLayoutMode mode = GridLayoutMode.SetNumberOfColumns) : base()
        {
            _layoutMode = mode;
        }

        public override void UpdateChildLayouts()
        {
            switch (_layoutMode)
            {
                case GridLayoutMode.SetNumberOfColumns:
                    SetNumber(CalculateRect(), Axis.Vertical, ColumnCount);
                    break;
                case GridLayoutMode.SetNumberOfRows:
                    SetNumber(CalculateRect(), Axis.Horizontal, RowCount);
                    break;
                case GridLayoutMode.TryFitChildSizeFlowDownVertical:
                    TryFitChild(Axis.Vertical);
                    break;
                case GridLayoutMode.TryFitChildSizeFlowDownHorizontal:
                    TryFitChild(Axis.Horizontal);
                    break;
            }

            base.UpdateChildLayouts();
        }

        // extension axis is the direction in which new children are placed.
        protected void SetNumber(RectangleF availableSpace, Axis extensionAxis, int count)
        {
            bool vertical = extensionAxis == Axis.Vertical;

            // calculate space available on appropriate axis
            float space = vertical ? availableSpace.Width - Padding.Left - Padding.Right : availableSpace.Height - Padding.Top - Padding.Bottom;
            space -= (count - 1) * (vertical ? Spacing.X : Spacing.Y);

            int line = 0;
            int lineItem = 0;
            Vector2 perItem = vertical ? new Vector2(space / count, ChildSize.Y) : new Vector2(ChildSize.X, space / count);
            for (int i = 0; i < Children.Count; i++)
            {
                if (vertical) Children[i].Rect.AnchorTopLeft(perItem.X, perItem.Y, Padding.Left + lineItem * (perItem.X + Spacing.X), Padding.Top + line * (perItem.Y + Spacing.Y));
                else Children[i].Rect.AnchorTopLeft(perItem.X, perItem.Y, Padding.Left + line * (perItem.X + Spacing.X), Padding.Top + lineItem * (perItem.Y + Spacing.Y));

                if (i == Children.Count - 1) break;

                // move to the next item
                lineItem++;
                if (lineItem >= count)
                {
                    lineItem = 0;
                    line++;
                }
            }

            ChildArea = vertical ? new Vector2(availableSpace.Width, Padding.Top + Padding.Bottom + line * (perItem.Y + Spacing.Y) + perItem.Y) : new Vector2(Padding.Left + Padding.Right + line * (perItem.X + Spacing.X) + perItem.X, availableSpace.Height);
        }

        protected void TryFitChild(Axis extensionAxis)
        {
            bool vertical = extensionAxis == Axis.Vertical;
            RectangleF availableSpace = CalculateRect();

            int count = 
                vertical ? 
                (int)MathF.Floor((availableSpace.Width - Padding.Left - Padding.Right) / ChildSize.X) : 
                (int)MathF.Floor((availableSpace.Height - Padding.Top - Padding.Bottom) / ChildSize.Y);

            if (count <= 0) count = 1;
            
            SetNumber(availableSpace, extensionAxis, count);
        }
    }
}