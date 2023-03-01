using UnityEngine.SceneManagement;
using UnityEngine;

namespace DredPack.Load
{
    public class AssyncLoadSceneManager : GeneralSingleton<AssyncLoadSceneManager>
    {
        private string SceneID;

        public void AddSceneToLoad(string SceneName)
        {
            SceneID = SceneName;
        }

        public AsyncOperation LoadScene()
        {
            AsyncOperation asyncOperation;

            if (SceneID != "")
                asyncOperation = SceneManager.LoadSceneAsync(SceneID);
            else
                return null;

            return asyncOperation;
        }
    }

}