using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public string sceneName;	// シーン名

	public GameObject panelGameStart;		// ゲーム開始パネル
	public GameObject panelGameEnd;		// ゲームエンドパネル
	public GameObject textGameOver;		// ゲームオーバーテキスト
	public GameObject textGameClear;	// ゲームクリアテキスト
	public GameObject buttons;			// 操作ボタン
	public GameObject textTime;			// タイム表示
	public GameObject textBestTime;		// ベストタイム

	private AudioSource audioSource;	// オーディオソース
	public AudioClip clearSe;			// クリアSE
	public AudioClip gameOverSe;		// ゲームオーバーSE
	public AudioClip buttonDownSe;		// ボタン押下SE

	private float time = 0;			// 現在の経過時間
	public enum STATUS {
		PLAYING,
		GAMEOVER,
		GAMECLEAR,
	};
	private STATUS status = STATUS.PLAYING;

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

		// ベストタイム読み込み
		var bestTime = BestTime ();
		textBestTime.GetComponent<Text> ().text = bestTime.ToString ("Best ###0.00 Sec");

		// 開始パネル設定
		panelGameStart.GetComponent<PanelGameStartManager> ().SetConfigurations(audioSource, OnCompleteGameStartPanel);
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

	// ゲーム開始パネルの表示完了後処理
	public void OnCompleteGameStartPanel () {
		time = 0;
		audioSource.Play();
	}

	// ゲームオーバー処理
	public void GameOver () {
		// BGMを止める
		audioSource.Stop ();

		// ゲームオーバーSEを鳴らす
		audioSource.PlayOneShot (gameOverSe);

		// ゲームオーバー画面の表示
		panelGameEnd.SetActive (true);
		textGameOver.SetActive (true);
		buttons.SetActive (false);
		status = STATUS.GAMEOVER;
	}

	// ゲームクリア処理
	public void GameClear () {
		// BGMを止める
		audioSource.Stop ();

		// クリアSEを鳴らす
		audioSource.PlayOneShot(clearSe);

		// クリア画面表示
		panelGameEnd.SetActive (true);
		textGameClear.SetActive (true);
		buttons.SetActive (false);

		// ゲームのステータスをクリアに変更
		status = STATUS.GAMECLEAR;

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

	// タイトルに戻る
	public void BackToTitle () {
		// ボタン押下音を鳴らす
		audioSource.PlayOneShot(buttonDownSe);

		// なり終わるのを待って、ゲームにシーン変更
		Invoke("ChangeToTitleScene", 1.0f);

	}

	// タイトルシーンに切り替え
	void ChangeToTitleScene () {
		SceneManager.LoadScene ("TitleScene");
	}
}
