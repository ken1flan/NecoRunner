using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogManager : MonoBehaviour {
	private const float TERRITORY_WIDTH = 2.0f;
	private const float VEROCITY = 1;
	public enum MoveDirection { Right = 1, Left = -1 }
	public MoveDirection moveDirection = MoveDirection.Left;
	public enum Statuses { Walking = 1, Barking = 2 }
	public Statuses status = Statuses.Walking;
	private DateTime barkingStartTime;
	private const float BARKING_TIME = 1.0f;

	private Rigidbody2D rbody;
	private float startX;
	private float territoryRight;
	private float territoryLeft;
	private GameObject player;
	private Animator animator;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
		startX = transform.position.x;
		territoryLeft = startX - TERRITORY_WIDTH;
		territoryRight = startX + TERRITORY_WIDTH;
		animator = GetComponent<Animator> ();

		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void Update () {

		if (needsBark()) {
			barkingStartTime = DateTime.Now;
			status = Statuses.Barking;
		} else {
			if (status == Statuses.Barking && (DateTime.Now - barkingStartTime).TotalSeconds > BARKING_TIME) {
				status = Statuses.Walking;
			}
		}

		var currentPosX = transform.position.x;
		var velocity = rbody.velocity;
		switch(status){
			case Statuses.Barking:
				rbody.velocity = new Vector2 (0.0f, velocity.y);
			  break;
			case Statuses.Walking:
				if (territoryLeft > currentPosX) {
					moveDirection = MoveDirection.Right;
					transform.localScale = new Vector2 (-1, 1);
				} else if (territoryRight < currentPosX) {
					moveDirection = MoveDirection.Left;
					transform.localScale = new Vector2 (1, 1);
				}
				rbody.velocity = new Vector2 ((float)moveDirection * VEROCITY, velocity.y);
				break;
			default:
				break;
		}
		Debug.Log(status);
		animator.SetInteger("status", (int)status);
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
