using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.UI
{
    public struct AnchorPoint
    {
        /// <summary>
        /// The anchor along the axis. From minimum to maximum interpolated by this value.
        /// <para>For example, a value of <c>0.5f</c> sets the anchor to be halfway between the left and right points horizontally.</para>
        /// </summary>
        public float AxisInterpolation { get; set; }
        public float PixelOffset { get; set; }

        public AnchorPoint(float axisInterp, float pixelOffset)
        {
            AxisInterpolation = axisInterp;
            PixelOffset = pixelOffset;
        }

        public static AnchorPoint Lerp(AnchorPoint p1, AnchorPoint p2, float amount)
        {
            return new AnchorPoint(
                MathHelper.Lerp(p1.AxisInterpolation, p2.AxisInterpolation, amount),
                MathHelper.Lerp(p1.PixelOffset, p2.PixelOffset, amount));
        }
    }
}