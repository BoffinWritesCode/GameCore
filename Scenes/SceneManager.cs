using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Scenes
{
    public class SceneManager
    {
        public LinkedList<IScene> SceneList { get; }
        public IScene CurrentDrawingScene { get; private set; }

        public SceneManager()
        {
            SceneList = new LinkedList<IScene>();
        }
        
        public void AddSceneToFront(IScene scene)
        {
            scene.Load();
            SceneList.AddLast(scene);
        }

        public void AddSceneToBack(IScene scene)
        {
            scene.Load();
            SceneList.AddFirst(scene);
        }

        public void RemoveScene(IScene scene)
        {
            scene.Unload();
            SceneList.Remove(scene);
        }

        public void BringToFront(IScene scene)
        {
            var node = SceneList.Find(scene);
            if (node == null) return;

            SceneList.Remove(node);
            SceneList.AddLast(node);
        }

        public T GetScene<T>() where T : class, IScene
        {
            foreach (var scene in SceneList)
            {
                if (scene is T conv) return conv;
            }
            return null;
        }

        public void Update() { foreach (var scene in SceneList) scene.Update(); }
        public void Draw() 
        {
            foreach (var scene in SceneList)
            {
                CurrentDrawingScene = scene;
                scene.Draw();
            }
        }
    }
}
