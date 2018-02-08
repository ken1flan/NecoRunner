using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour {
	private const float START_X_OFFSET = 8.6f;
	private const float END_X_OFFSET = 25.5f;
	private const float START_Y_OFFSET = 4.0f;
	private const float END_Y_OFFSET = 12.0f;

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

			float y = player.transform.position.y;
			if (y < START_Y_OFFSET) {
				y = START_Y_OFFSET;
			} else if (y > END_Y_OFFSET) {
				y = END_Y_OFFSET;
			}




			transform.position = new Vector3 (x, y, transform.position.z);
		}
	}
}
