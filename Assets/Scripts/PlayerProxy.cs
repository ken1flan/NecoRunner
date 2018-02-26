using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProxy : MonoBehaviour {
	public GameObject player;
	PlayerManager playerManager;

	// Use this for initialization
	void Start () {
		playerManager = player.GetComponent<PlayerManager> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void PushLeftButton () {
		playerManager.PushLeftButton ();
	}

	public void PushRightButton () {
		playerManager.PushRightButton ();
	}

	public void ReleaseMoveButton () {
		playerManager.ReleaseMoveButton ();
	}

	public void PushJumpButton () {
		playerManager.PushJumpButton ();
	}
}
