using System.Collections;
using System.Collections.Generic;

using GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace GameCore.Utility.Coroutines
{
    public static class UtilityCoroutines
    {
        public static IEnumerator UpdateColor(IColorable colorable, float time, Color start, Color end)
        {
            float current = 0f;
            while (current < time)
            {
                colorable.Color = Color.Lerp(start, end, current / time);

                current += Time.DeltaTime;

                yield return null;
            }

            colorable.Color = end;
        }

        public static IEnumerator JustWait(float time)
        {
            yield return new YieldForSeconds(time);
        }

        public static IEnumerator ActionList(List<IEnumerator> coroutines)
        {
            foreach (IEnumerator c in coroutines) yield return c;
        }
    }
}