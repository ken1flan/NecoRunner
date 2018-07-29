using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	private LayerMask blockLayer;	// ブロックレイヤ
	private GameObject gameManager;	// ゲームマネージャ
	private Rigidbody2D rbody;		// プレイヤー制御用Ridgebody2D
	private AudioSource audioSource;		// オーディオソース
	private Animator animator;
	public AudioClip jumpSe;				// ジャンプSE

	public enum Statuses {
		WaitingStart = 0,
		Standing = 1,
		Running = 2,
		Jumping = 3,
		StepingBack = 4
	}
	public Statuses status = Statuses.WaitingStart;

	public enum MoveDirection { Right = 1, Stop = 0, Left = -1 };
	private const float MOVE_SPEED = 3;	// スピード
	private const float JUMP_POWER = 300;			// ジャンプ力
	private Vector2 stepBackDir = new Vector2(1.0f, 0.5f);
	private const float STEP_BACK_POWER = 150.0f; // 後ずさる力
	private MoveDirection goStepBack = MoveDirection.Stop;	// 飛び退ったか否か
	private bool onGround = false;			// 地面に触れているか
	private bool touchingLeftWall = false;	// 左壁に触れているか
	private bool touchingRightWall = false;	// 右壁に触れているか
	private MoveDirection moveDirection = MoveDirection.Stop;	// 移動方向
	private bool goJump = false;			// ジャンプしたか否か

	// Use this for initialization
	void Start () {
		status = Statuses.WaitingStart;
		rbody = GetComponent<Rigidbody2D> ();

		gameManager = GameObject.Find("GameManager");
		// オーディオソースの設定
		audioSource = gameManager.GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();

		blockLayer = LayerMask.GetMask("Block");
	}

	// Update is called once per frame
	void Update () {
		CheckJumpAvailablity();
	}

	// 固定更新処理
	void FixedUpdate () {
		if (status == Statuses.WaitingStart) {
			return;
		}

		// 飛び退り処理
		if (status == Statuses.StepingBack) {
			if (onGround) {
				status = Statuses.Standing;
			} else {
				return;
			}
		}
		if (goStepBack != MoveDirection.Stop) {
			StepBack(goStepBack);
			return;
		}

		// 現在のスピード
		var newVelocity = rbody.velocity;

		// 移動処理
		if (onGround) {
			switch (moveDirection) {
			case MoveDirection.Left:
			case MoveDirection.Right:
				Walk(moveDirection);
				break;
			default:
				Stand();
				break;
			}
		}

		// ジャンプ処理
		if (goJump) {
			if (onGround) {
				Jump ();
			} else if (touchingRightWall) {
				WallJump(MoveDirection.Left);
			} else if (touchingLeftWall) {
				WallJump(MoveDirection.Right);
			} else {
				// なにもしない
			}
			goJump = false;
		}

		animator.SetInteger("status", (int)status);
	}

	void OnTriggerEnter2D (Collider2D col) {
		var collidedGameObject = col.gameObject;

		switch(collidedGameObject.tag) {
			case "Bullet":
				goStepBack = collidedGameObject.transform.position.x - transform.position.x <= 0 ? MoveDirection.Right : MoveDirection.Left;
				Destroy(collidedGameObject);
				break;
			case "Trap":
				gameManager.GetComponent<GameManager> ().GameOver ();
				DestroyPlayer ();
				break;
			case "Goal":
				gameManager.GetComponent<GameManager> ().GameClear ();
				DestroyPlayer ();
				break;
		}
	}

	void DestroyPlayer () {
		Destroy (this.gameObject);
	}

	public void StartGame () {
		status = Statuses.Standing;
	}

	public void GoLeft () {
		moveDirection = MoveDirection.Left;
	}

	public void GoRight () {
		moveDirection = MoveDirection.Right;
	}

	public void StopMoving () {
		moveDirection = MoveDirection.Stop;
	}

	public void GoJump () {
		goJump = true;
	}

	public void Stand () {
		var newVelocity = rbody.velocity;
		newVelocity.x = 0;
		status = Statuses.Standing;
		rbody.velocity = newVelocity;
	}

	public void Walk (MoveDirection dir) {
		var newVelocity = rbody.velocity;
		newVelocity.x = (int)dir * MOVE_SPEED;
		transform.localScale = new Vector2 ((int)dir, 1);
		status = Statuses.Running;
		rbody.velocity = newVelocity;
	}

	public void Jump () {
		var newVelocity = rbody.velocity;
		audioSource.PlayOneShot (jumpSe);
		newVelocity.y = 0;
		rbody.AddForce (Vector2.up * JUMP_POWER);
		status = Statuses.Jumping;

		rbody.velocity = newVelocity;
	}

	public void WallJump (MoveDirection dir) {
		audioSource.PlayOneShot (jumpSe);
		var newVelocity = rbody.velocity;
		newVelocity.y = 0;
		rbody.AddForce (Vector2.up * JUMP_POWER);
		// FIX
		newVelocity.x = (int)dir * MOVE_SPEED;
		transform.localScale = new Vector2 ((int)dir, 1);
		status = Statuses.Jumping;
		rbody.velocity = newVelocity;
	}

	public void StepBack (MoveDirection direction) {
	// おどろいて後ろに飛び退る
		transform.localScale = new Vector2 ((int)direction * -1, 1);

		if (CheckOnGround()) {
			// 少し浮かせないと摩擦で後ろに飛ばない
			var currentPosition = transform.position;
			transform.position = new Vector2(currentPosition.x, currentPosition.y + 0.1f);

			var v = new Vector2((float)direction * stepBackDir.x, stepBackDir.y);
			rbody.AddForce(v * STEP_BACK_POWER);
		} else {
			var currentPosition = transform.position;
			transform.position = new Vector2(currentPosition.x / 2, currentPosition.y);
		}
		status = Statuses.StepingBack;
		animator.SetInteger("status", (int)status);

		goStepBack = MoveDirection.Stop;
	}

	private void CheckJumpAvailablity () {
		// ジャンプ可能か
		onGround = CheckOnGround();

		// 壁ジャンプ可能か
		Vector3 startOffset = transform.right * 0.6f;
		Vector3 endOffset = transform.up * 1.5f;
		touchingRightWall = !onGround && Physics2D.Linecast (
			transform.position + startOffset,
			transform.position + startOffset + endOffset,
			blockLayer);

		touchingLeftWall = !onGround && Physics2D.Linecast (
			transform.position - startOffset,
			transform.position - startOffset + endOffset,
			blockLayer);
	}

	private bool CheckOnGround () {
		return Physics2D.Linecast (transform.position - (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer)
			|| Physics2D.Linecast (transform.position + (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer);
	}
}
