using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject textGameOver;		// ゲームオーバーテキスト
	public GameObject textGameClear;	// ゲームクリアテキスト
	public GameObject buttons;			// 操作ボタン
	public GameObject textTime;			// タイム表示
	public GameObject textBestTime;		// ベストタイム

	private float time = 0;			// 現在の経過時間
	public enum STATUS {
		PLAYING,
		GAMEOVER,
		GAMECLEAR,
	};
	private STATUS status = STATUS.PLAYING;

	// 保存用キー
	private const string SAVE_KEY_BEST_TIME = "SAVE_KEY_BEST_TIME";	// ベストタイム
	private const float BEST_TIME_DEFAULT = 9999.99f;	// 初期ベストタイム

	// Use this for initialization
	void Start () {
		// ベストタイム読み込み
		var bestTime = PlayerPrefs.GetFloat (SAVE_KEY_BEST_TIME, BEST_TIME_DEFAULT);
		textBestTime.GetComponent<Text> ().text = bestTime.ToString ("Best ###0.00 Sec");

	}
	
	// Update is called once per frame
	void Update () {
		if (status == STATUS.PLAYING) {
			time += Time.deltaTime;
			string newTimeString = time.ToString ("###0.00 Sec");
			var textComponent = textTime.GetComponent<Text> ();
			if (textComponent.text != newTimeString) {
				textComponent.text = newTimeString;
			}
		}
	}

	// ゲームオーバー処理
	public void GameOver () {
		textGameOver.SetActive (true);
		buttons.SetActive (false);
		status = STATUS.GAMEOVER;
	}

	// ゲームクリア処理
	public void GameClear () {
		textGameClear.SetActive (true);
		buttons.SetActive (false);
		status = STATUS.GAMECLEAR;

		// ベストタイム更新
		UpdateBestTime ();
	}

	// ベストタイム更新
	void UpdateBestTime () {
		var bestTime = PlayerPrefs.GetFloat (SAVE_KEY_BEST_TIME, BEST_TIME_DEFAULT);
		if (time < bestTime) {
			PlayerPrefs.SetFloat (SAVE_KEY_BEST_TIME, time);

			string newTimeString = time.ToString ("Best ###0.00 Sec");
			textBestTime.GetComponent<Text> ().text = newTimeString;
		}
	}
}
