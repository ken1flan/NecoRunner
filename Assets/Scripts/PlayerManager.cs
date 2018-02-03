using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public LayerMask blockLayer;	// ブロックレイヤ
	private Rigidbody2D rbody;		// プレイヤー制御用Ridgebody2D

	private const float MOVE_SPEED = 3;	// スピード
	private float moveSpeed = 0;			// 現在のスピード
	public enum MOVE_DIR{
		STOP,
		LEFT,
		RIGHT,
	};
	private MOVE_DIR moveDirection = MOVE_DIR.STOP;	// 移動方向
	private float jumpPower = 300;			// ジャンプ力
	private bool goJump = false;			// ジャンプしたか否か
	private bool canJump = false;			// ジャンプが可能か

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		canJump = Physics2D.Linecast (transform.position - (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer)
			|| Physics2D.Linecast (transform.position + (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer);

	}

	// 固定更新処理
	void FixedUpdate () {
		switch (moveDirection) {
		case MOVE_DIR.LEFT:
			moveSpeed = -MOVE_SPEED;
			transform.localScale = new Vector2 (-1, 1);
			break;
		case MOVE_DIR.RIGHT:
			moveSpeed = MOVE_SPEED;
			transform.localScale = new Vector2 (1, 1);
			break;
		default:
			moveSpeed = 0;
			break;
		}

		rbody.velocity = new Vector2 (moveSpeed, rbody.velocity.y);

		// ジャンプ処理
		if (goJump) {
			rbody.AddForce (Vector2.up * jumpPower);
			goJump = false;
		}
	}

	public void PushLeftButton () {
		moveDirection = MOVE_DIR.LEFT;
	}

	public void PushRightButton () {
		moveDirection = MOVE_DIR.RIGHT;
	}

	public void ReleaseMoveButton () {
		moveDirection = MOVE_DIR.STOP;
	}

	public void PushJumpButton () {
		Debug.Log ("canJump:" + canJump);
		if (canJump) {
			goJump = true;
		}
	}
}
