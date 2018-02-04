using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject textGameOver;	// ゲームオーバーテキスト
	public GameObject buttons;		// 操作ボタン

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// ゲームオーバー処理
	public void GameOver () {
		textGameOver.SetActive (true);
		buttons.SetActive (false);
	}
}
