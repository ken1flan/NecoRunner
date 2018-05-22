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
	private GameObject player;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
		startX = transform.position.x;
		territoryLeft = startX - TERRITORY_WIDTH;
		territoryRight = startX + TERRITORY_WIDTH;

		player = GameObject.Find("Player");
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

		if (needsBark()) {
			Debug.Log("FOUND");
		} else {
			Debug.Log("NOT FOUND");
		}

		var velocity = rbody.velocity;
		rbody.velocity = new Vector2 ((float)moveDirection * VEROCITY, velocity.y);
	}

	private bool needsBark () {
		var currentPosX = transform.position.x;
		var playerPosX = player.transform.position.x;

		if (moveDirection == MoveDirection.Right && playerPosX < currentPosX + TERRITORY_WIDTH && playerPosX >= currentPosX) {
			return true;
		} else if (moveDirection == MoveDirection.Left && playerPosX > currentPosX - TERRITORY_WIDTH && playerPosX <= currentPosX) {
			return true;
		}
		return false;
	}
}
