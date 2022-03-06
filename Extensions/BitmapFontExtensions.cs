using System;

using MonoGame.Extended.BitmapFonts;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;

namespace GameCore
{
    public static class BitmapFontExtensions
    {
        private static string SPACE = " ";
        private static string EMPTY = "";
        private static Regex _findNewLines = new Regex("/n+");

        public static float GetScaleFromLineHeight(this BitmapFont font, float targetLineHeight)
        {
            return targetLineHeight / font.LineHeight;
        }

        public static string WrapText(this BitmapFont font, string text, float maxLineWidth)
        {
            if (string.IsNullOrEmpty(text)) return "";

            string newText = EMPTY;
            string[] lines = text.Split('\n');
            float spaceWidth = font.MeasureString(SPACE).Width;
            for (int j = 0; j < lines.Length; j++)
            {
                string[] words = lines[j].Split(' ');
                float currentWidth = 0f;
                for (int i = 0; i < words.Length; i++)
                {
                    bool last = i == words.Length - 1;
                    string word = words[i];
                    Vector2 wordSize = font.MeasureString(word);

                    if (currentWidth + wordSize.X < maxLineWidth)
                    {
                        newText += word + (last ? EMPTY : SPACE);
                        currentWidth += wordSize.X + spaceWidth;
                        continue;
                    }

                    newText += Environment.NewLine + word + (last ? EMPTY : SPACE);
                    currentWidth = wordSize.X + spaceWidth;
                }

                if (j < lines.Length - 1)
                {
                    newText += Environment.NewLine;
                }
            }

            return newText;
        }
    }
}