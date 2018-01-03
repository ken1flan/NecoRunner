using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour {
	private const float START_OFFSET = -4.4f;
	private const float END_OFFSET = 4.4f;

	private GameObject player;		// プレイヤー

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		float x = player.transform.position.x;
		if (x < START_OFFSET) {
			x = START_OFFSET;
		} else if (x > END_OFFSET) {
			x = END_OFFSET;
		}
		transform.position = new Vector3 (x, transform.position.y, transform.position.z);
	}
}
