using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
	private AudioSource audioSource;	// オーディオソース
	public AudioClip startSe;			// スタートボタン押し下げ音

	// Use this for initialization
	void Start () {
		audioSource = this.gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

	}

	// ゲーム開始
	public void StartGame () {
		// BGMを止める
		audioSource.Stop();

		// ピロンと音を鳴らす
		audioSource.PlayOneShot(startSe);

		// なり終わるのを待って、ゲームにシーン変更
		Invoke("ChangeToGameScene", 2.0f);
	}

	// ゲームシーン切り替え
	void ChangeToGameScene () {
		SceneManager.LoadScene ("StageSelectScene");
	}
}
