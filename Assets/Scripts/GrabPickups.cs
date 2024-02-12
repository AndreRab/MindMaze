using System.Runtime.Serialization;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GrabPickups : MonoBehaviour {

	private AudioSource[] allAudioSources;
	public GameObject diedPanel;
    public GameObject audioD;
	public GameObject loaderUI;
    public Slider progressSlider;

    public GameObject otherMenu;

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


	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag == "Pickup" && Statics.startLoad == true) {
			Statics.level = Statics.level + 1;
			Statics.saveProgress();
			Statics.startLoad = false; 
			StopAllAudio();
			LoadSceneLol("Play");
		}
	}

	public void StopAllAudio() {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach( AudioSource audioS in allAudioSources) {
            if (audioS.tag != "diedMusic" ) {
                audioS.Stop();
            }
        }
    }
	
	void OnTriggerEnter(Collider info) {
		if (info.gameObject.tag == "enemy") {
			StopAllAudio();
            //Statics.prevLevel = Statics.level;
            Statics.level = 1;
			Statics.saveProgress();
            diedPanel.SetActive(true);
            audioD.SetActive(true);
        }
    }
}