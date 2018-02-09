using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject textGameOver;	// ゲームオーバーテキスト
	public GameObject buttons;		// 操作ボタン
	public GameObject textTime;		// タイム表示

	private float time = 0;			// 現在の経過時間
	public enum STATUS {
		PLAYING,
		GAMEOVER,
	};
	private STATUS status = STATUS.PLAYING;

	// Use this for initialization
	void Start () {
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
}
