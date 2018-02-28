using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour {
	private AudioSource audioSource;
	public AudioClip startSe;
	private string sceneName;

	// Use this for initialization
	void Start () {
		// audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource = this.gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

	}

	// Stage 1 が押された
	public void PushStage1 () {
		sceneName = "GameScene1";
		StartCoroutine (ChangeScene ());
	}

	// Stage 2 が押された
	public void PushStage2 () {
		sceneName = "Stage1Scene";
		StartCoroutine (ChangeScene ());
	}

	IEnumerator ChangeScene () {
		audioSource.Stop();
		audioSource.PlayOneShot(startSe);
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene (sceneName);
	}
}
