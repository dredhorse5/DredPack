using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : MonoBehaviour
{
    public Image LoadingBar;
    public Text LoadingText;

    private AsyncOperation asyncOperation;

    void Start()
    {
        StartCoroutine(LoadSceneCor());
    }

    IEnumerator LoadSceneCor()
    {
        yield return new WaitForSeconds(1f);
        asyncOperation = AssyncLoadSceneManager.Instance.LoadScene();

        if (asyncOperation == null)
        {
            Debug.LogError("No Scene Selected");
            yield return null;
        }

        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;
            LoadingBar.fillAmount = progress;
            LoadingText.text = "Loading: " + string.Format("{0:0}%", progress * 100f);
            yield return null;
        }
    }
}
