using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour {
	private const float START_X_OFFSET = 8.6f;
	private const float END_X_OFFSET = 25.5f;

	private GameObject player;		// プレイヤー

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			float x = player.transform.position.x;
			if (x < START_X_OFFSET) {
				x = START_X_OFFSET;
			} else if (x > END_X_OFFSET) {
				x = END_X_OFFSET;
			}
			transform.position = new Vector3 (x, transform.position.y, transform.position.z);
		}
	}
}
