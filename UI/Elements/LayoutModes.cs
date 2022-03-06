using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore.UI.Elements
{
    public enum AxisLayoutMode
    {
        /// <summary>
        /// The children will be resized to fit into the parent. No scrolling would be needed.
        /// </summary>
        StretchToFitAllInParentsSize,
        /// <summary>
        /// The children will all be a set size (they will stretch in the alternative axis, the ChildSize value only represents one axis.
        /// </summary>
        SetChildSize,
        /// <summary>
        /// The children themselves control their size. They must be anchored to the top left and have their size specified for this to work.
        /// </summary>
        ChildrenControlTheirSize
    }

    public enum GridLayoutMode
    {
        /// <summary>
        /// The grid has a specific amount of rows.
        /// <br>New columns will be created to accomodate. Columns flow from left to right.</br>
        /// </summary>
        SetNumberOfRows,
        /// <summary>
        /// The grid has a specific amount of columns.
        /// <br>New rows will be created to accomodate. Rows flow from top to bottom.</br>
        /// </summary>
        SetNumberOfColumns,
        /// <summary>
        /// The grid will expand children slightly to try and fit as many children of the specified size in the grid.
        /// <br>New rows will be created to accomodate. Rows flow from top to bottom.</br>
        /// </summary>
        TryFitChildSizeFlowDownVertical,
        /// <summary>
        /// The grid will expand children slightly to try and fit as many children of the specified size in the grid.
        /// <br>New columns will be created to accomodate. Columns flow from left to right.</br>
        /// </summary>
        TryFitChildSizeFlowDownHorizontal
    }
}