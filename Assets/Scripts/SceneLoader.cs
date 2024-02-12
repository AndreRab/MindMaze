using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ToDyToScAnO {

public class SceneLoader : MonoBehaviour
{
    public GameObject loaderUI;
    public Slider progressSlider;

    public GameObject otherMenu;
    public VideoPlayer videoPlayer;

    private bool firstLoad = true; 

    void Update() {
        if (videoPlayer != null && videoPlayer.isPaused && firstLoad)  {
            LoadSceneLol("play");
            firstLoad = false;
        } 
    }

    public void LoadSceneLol(string name)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadScene_CoroutineLol(name));
    }

    public IEnumerator LoadScene_CoroutineLol(string name){
        progressSlider.value = 0;
        otherMenu.SetActive(false);
        loaderUI.SetActive(true);

        UnityEngine.AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        } 
    }
}
}
