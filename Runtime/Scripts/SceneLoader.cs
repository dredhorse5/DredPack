using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string loadingScreenID = "LoadingScreen";
    
    public static void LoadNewScene(string ID, bool assync = true)
    {
        if (assync)
        {
            AssyncLoadSceneManager.Instance.AddSceneToLoad(ID);
            SceneManager.LoadScene(loadingScreenID);
        }
        else
        {
            SceneManager.LoadScene(ID);
        }
    }

    public static void ReloadScene(bool assync = true)
    {
        LoadNewScene(SceneManager.GetActiveScene().name, assync);
    }
    
}
