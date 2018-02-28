using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	// Stage 1 が押された
	public void PushStage1 () {
		SceneManager.LoadScene ("GameScene1");
	}

	// Stage 2 が押された
	public void PushStage2 () {
		SceneManager.LoadScene ("Stage1Scene");
	}
}
