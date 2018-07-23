using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private string sceneName;	// シーン名

	private GameObject buttons	;			// 操作ボタン
	private Text textTime;						// タイム表示
	private Text textBestTime;				// ベストタイム

	private PlayerManager player;			// プレイヤー
	private AudioSource audioSource;	// オーディオソース

	private PanelGameStartManager panelGameStartManager;
	private PanelGameEndManager panelGameEndManager;
	private float time = 0;			// 現在の経過時間
	public enum Statuses {
		Starting,
		Playing,
		GameOver,
		GameClear,
	};
	private Statuses status = Statuses.Starting;

	// 保存用キー
	private const string SAVE_KEY_FORMAT_BEST_TIME = "BEST_TIME_{0}";	// ベストタイム
	private const float BEST_TIME_DEFAULT = 9999.99f;	// 初期ベストタイム

	// Use this for initialization
	void Start () {
		// シーン名の取得
		sceneName = SceneManager.GetActiveScene().name;

		// オーディオソースの設定
		audioSource = this.gameObject.GetComponent<AudioSource> ();

		// BGMは止めて開始
		audioSource.Stop ();

		// キャンバスUI設定
		var canvasUi = transform.Find("CanvasUI").gameObject;
		canvasUi.SetActive(true);

		// ベストタイム読み込み
		textTime = GameObject.Find("TextTime").GetComponent<Text> ();
		textBestTime = GameObject.Find("TextBestTime").GetComponent<Text> ();

		var bestTime = BestTime ();
		textBestTime.text = bestTime.ToString ("Best ###0.00 Sec");

		// 開始パネル設定
		panelGameStartManager = GameObject.Find("PanelGameStart").GetComponent<PanelGameStartManager> ();
		panelGameStartManager.SetConfigurations(audioSource, OnCompleteGameStartPanel);

		// 終了パネル設定
		panelGameEndManager = canvasUi.transform.Find("PanelGameEnd").gameObject.GetComponent<PanelGameEndManager> ();

		// プレイヤーの取得
		player =  GameObject.Find("Player").GetComponent<PlayerManager> ();

		// ボタンの取得
		buttons = GameObject.Find("Buttons");
	}

	// Update is called once per frame
	void Update () {
		if (status == Statuses.Playing) {
			time += Time.deltaTime;
			string newTimeString = time.ToString ("###0.00 Sec");
			if (textTime.text != newTimeString) {
				textTime.text = newTimeString;
			}
		}
	}

	// ゲーム開始パネルの表示完了後処理
	public void OnCompleteGameStartPanel () {
		status = Statuses.Playing;
		time = 0;
		audioSource.Play();
		player.StartGame ();
	}

	// PanelGameEndManagerに移動
	// ゲームオーバー処理
	public void GameOver () {
		status = Statuses.GameOver;
		// BGMを止める
		audioSource.Stop ();

		panelGameEndManager.GameOver ();
	}

	// PanelGameEndManagerに移動
	// ゲームクリア処理
	public void GameClear () {
		status = Statuses.GameClear;
		audioSource.Stop ();
		panelGameEndManager.GameClear ();

		// ベストタイム更新
		UpdateBestTime ();
	}

	// ベストタイム保存キー取得
	string SaveKeyBestTime () {
		return string.Format(SAVE_KEY_FORMAT_BEST_TIME, sceneName.ToUpper ());
	}

	// ベストタイム読み込み
	float BestTime () {
		return PlayerPrefs.GetFloat (SaveKeyBestTime (), BEST_TIME_DEFAULT);
	}

	// ベストタイム更新
	void UpdateBestTime () {
		var bestTime = BestTime ();
		if (time < bestTime) {
			PlayerPrefs.SetFloat (SaveKeyBestTime (), time);

			string newTimeString = time.ToString ("Best ###0.00 Sec");
			textBestTime.GetComponent<Text> ().text = newTimeString;
		}
	}
}
