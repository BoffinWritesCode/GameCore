using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameCore.Utility.Coroutines
{
    /// <summary>
    /// Manages coroutines, allowing you to run functions over a period of time yielding based on time or frame counts.
    /// </summary>
    public class CoroutineManager
    {
        private List<Coroutine> activeCoroutines = new List<Coroutine>();
        private List<Coroutine> incomingCoroutines = new List<Coroutine>();
        //private ObjectPool<Coroutine> coroutinePool = new ObjectPool<Coroutine>(() => new Coroutine());

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = new Coroutine();
            coroutine.Reset(routine);
            incomingCoroutines.Add(coroutine);
            return coroutine;
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            int index = activeCoroutines.FindIndex((c) => c == coroutine);

            if (index != -1)
            {
                //coroutinePool.ReturnItem(activeCoroutines[index]);
                activeCoroutines.RemoveAt(index);
            }
        }

        public void UpdateCoroutines()
        {
            activeCoroutines.AddRange(incomingCoroutines);
            incomingCoroutines.Clear();

            for (int i = 0; i < activeCoroutines.Count; i++)
            {
                if (!activeCoroutines[i].Update())
                {
                    //coroutinePool.ReturnItem(activeCoroutines[i]);
                    activeCoroutines[i] = null;
                }
            }

            activeCoroutines.RemoveAll((x) => { return x == null; });
        }
    }
}
