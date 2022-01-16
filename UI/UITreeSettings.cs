using Microsoft.Xna.Framework.Graphics;

namespace GameCore.UI
{
    /// <summary>
    /// The settings for any particular tree of UI elements.
    /// </summary>
    public class UITreeSettings
    {
        public bool PixelPerfect { get; protected set; }
        public SamplerState SamplerState { get; set; }

        public UITreeSettings(bool pixelPerfect)
        {
            PixelPerfect = pixelPerfect;
            SamplerState = SamplerState.PointWrap;
        }
    }
}