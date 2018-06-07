using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkWaveManager : MonoBehaviour {
	private float startX;
	private float RANGE_DISTANCE = 5.0f;
	private Rigidbody2D rbody;
	private float VELOCITY = 5.0f;
	public enum MoveDirection { Right = 1, Left = -1 }
	public MoveDirection moveDirection = MoveDirection.Left;

	// Use this for initialization
	void Start () {
		startX = transform.position.x;
		rbody = GetComponent<Rigidbody2D>();
		rbody.velocity = new Vector2 (VELOCITY * (float)moveDirection, 0);
	}

	// Update is called once per frame
	void Update () {
		var currentX = transform.position.x;
		var difference = Math.Abs(currentX - startX);
		var progress = difference / RANGE_DISTANCE;
		transform.localScale = new Vector2 ((float)moveDirection * progress, progress);

		if (difference > RANGE_DISTANCE) {
			Destroy (this.gameObject);
			return;
		}
	}
}
