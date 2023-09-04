using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

namespace Peg
{
    /// <summary>
    /// Something is causing a crash in builds when quitting the app. This is a bandaide to fix that by removing
    /// all other GameObjects from the scene before quitting.
    /// </summary>
    public class QuitCleanup : MonoBehaviour
    {
        bool Allow;

        private void Awake()
        {
            Application.wantsToQuit += HandleQuit;
        }

        bool HandleQuit()
        {
            if (!Allow)
            {
                StartCoroutine(CleanupApp());
                return false;
            }
            else return true;
        }

        private void OnApplicationQuit()
        {
            
        }

        List<Scene> GetAllScenes()
        {
            List<Scene> scenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
                scenes.Add(SceneManager.GetSceneAt(i));

            return scenes;
        }

        IEnumerator CleanupApp()
        {
            KillAll();
            yield return null;
            KillAll(); //this ensures we killed anything spawned during the first genocide
            yield return null;

            Allow = true;
            Application.Quit();
        }
        
        void KillAll()
        {
            var roots = GetAllScenes().SelectMany(s => s.GetRootGameObjects());

            foreach (var root in roots)
            {
                if (root != null && root != this.gameObject)
                    Destroy(root);
            }
        }
    }
}
