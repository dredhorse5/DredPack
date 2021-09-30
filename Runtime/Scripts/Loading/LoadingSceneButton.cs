using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneButton : MonoBehaviour
{
    public enum LoadTypes
    {
        Assync,
        Linear
    }
    public enum SceneTypes
    {
        LoadNewScene,
        ReloadThisScene
    }

    public LoadTypes LoadType;
    public SceneTypes SceneType;
    public string SceneName = "";
    
    
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            switch (SceneType)
            {
                case SceneTypes.LoadNewScene:
                    if(SceneName != null && SceneName != "")
                        SceneLoader.LoadNewScene(SceneName, LoadType == LoadTypes.Assync);
                    else
                        Debug.LogError("No scene selected in the button : " + gameObject.name);
                    break;
                case SceneTypes.ReloadThisScene:
                    SceneLoader.ReloadScene(LoadType == LoadTypes.Assync);
                    break;
            }
        });
    }
}
