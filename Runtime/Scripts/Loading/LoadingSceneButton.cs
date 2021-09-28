using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneButton : MonoBehaviour
{
    public enum SceneTypes
    {
        LoadNewScene,
        ReloadThisScene
    }
    public string SceneName = "";
    public SceneTypes SceneType;
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if(SceneName != null && SceneName != "")
                SceneLoader.LoadNewScene(SceneName);
            else
                Debug.LogError("No scene selected in the button : " + gameObject.name);
        });
    }
}
