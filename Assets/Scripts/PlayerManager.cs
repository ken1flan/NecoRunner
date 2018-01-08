using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	private Rigidbody2D rbody;		// プレイヤー制御用Ridgebody2D

	private const float SPEED_MAX = 100;	// 最大スピード
	private const float SPEED_ACCELERATION = 0.4f; // ボタン一回あたりの加速度
	private const float SPEED_RESISTANCE = 0.03f;	// 抵抗

	private float speed = 0;				// 現在のスピード
	private int runButtonCount = 0;			// Runボタンを押した数

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {

	}

	// 固定更新処理
	void FixedUpdate () {
		speed = speed + SPEED_ACCELERATION * runButtonCount - SPEED_RESISTANCE;
		speed = speed < 0 ? 0 : speed;
		rbody.velocity = new Vector2 (speed, rbody.velocity.y);

		runButtonCount = 0;
	}

	// Runボタンを押した
	public void PushRunButton () {
		runButtonCount++;
	}
}
