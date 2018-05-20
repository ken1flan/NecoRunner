using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogManager : MonoBehaviour {
	private const float TERRITORY_WIDTH = 1.5f;
	private const float VEROCITY = 1;
	public enum MoveDirection { Right = 1, Left = -1 }
	public MoveDirection moveDirection = MoveDirection.Left;

	private Rigidbody2D rbody;
	private float startX;
	private float territoryRight;
	private float territoryLeft;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
		startX = transform.position.x;
		territoryLeft = startX - TERRITORY_WIDTH;
		territoryRight = startX + TERRITORY_WIDTH;
	}

	// Update is called once per frame
	void Update () {
		// 左右にうろうろする
		var currentPosX = transform.position.x;
		if (territoryLeft > currentPosX) {
			moveDirection = MoveDirection.Right;
			transform.localScale = new Vector2 (-1, 1);
		} else if (territoryRight < currentPosX) {
			moveDirection = MoveDirection.Left;
			transform.localScale = new Vector2 (1, 1);
		}

		var velocity = rbody.velocity;
		rbody.velocity = new Vector2 ((float)moveDirection * VEROCITY, velocity.y);
	}
}
