using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PanelGameEndManager :MonoBehaviour {

	public GameObject gameManager;
	public GameObject textGameEnd;
	public AudioClip clearSe;
	public AudioClip gameOverSe;
	public AudioClip buttonDownSe;

	private Text text;
	private const string GAME_OVER_TEXT = "Game Over";
	private const string GAME_CLEAR_TEXT = "Game Clear";
	private Color textColorGameOver = new Color(1.0f,  0.0f, 0.0f, 1.0f);
	private Color textColorGameClear = new Color(0.1f,  0.6f, 0.8f, 1.0f);

	private AudioSource audioSource;

	// Use this for initialization
	void Awake () {
	}
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void GameClear () {
		SetMembers ();
		text.text = GAME_CLEAR_TEXT;
		text.color = textColorGameClear;
		this.gameObject.SetActive (true);
		audioSource.PlayOneShot(clearSe);
	}

	public void GameOver () {
		SetMembers ();
		text.text = GAME_OVER_TEXT;
		text.color = textColorGameOver;
		this.gameObject.SetActive (true);
		audioSource.PlayOneShot(gameOverSe);
	}

	public void BackToTitle () {
		audioSource.PlayOneShot(buttonDownSe);

		Invoke("ChangeToTitleScene", 1.0f);
	}

	void ChangeToTitleScene () {
		SceneManager.LoadScene ("TitleScene");
	}

	void SetMembers () {
		audioSource = gameManager.GetComponent<AudioSource> ();
		text = textGameEnd.GetComponent<Text> ();
	}
}
