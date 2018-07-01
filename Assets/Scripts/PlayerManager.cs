using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public LayerMask blockLayer;	// ブロックレイヤ
	public GameObject gameManager;	// ゲームマネージャ
	private Rigidbody2D rbody;		// プレイヤー制御用Ridgebody2D

	public enum Statuses {
		Standing = 1,
		Running = 2,
		Jumping = 3,
		BeingPushedBack = 4
	}
	public Statuses status = Statuses.Standing;
	private const float MOVE_SPEED = 3;	// スピード

	public enum MoveDirection { Right = 1, Stop = 0, Left = -1 };
	private bool canMove = false;
	private MoveDirection moveDirection = MoveDirection.Stop;	// 移動方向
	private const float JUMP_POWER = 300;			// ジャンプ力
	private Vector2 stepBackDir = new Vector2(1.0f, 0.5f);
	private const float STEP_BACK_POWER = 150.0f; // 後ずさる力
	private const float STEP_BACK_SPEED = 3.0f; // 後ずさる速さ
	private MoveDirection goStepBack = MoveDirection.Stop;	// 飛び退ったか否か
	private bool goJump = false;			// ジャンプしたか否か
	private bool canJump = false;			// ジャンプが可能か
	private bool goWallRightJump = false;	// 右壁ジャンプしたか否か
	private bool canWallRightJump = false;	// 右壁ジャンプが可能か
	private bool goWallLeftJump = false;	// 左壁ジャンプしたか否か
	private bool canWallLeftJump = false;	// 左壁ジャンプが可能か
	private bool usingButtons = false;		// ボタンを利用中か

	private AudioSource audioSource;		// オーディオソース
	public AudioClip jumpSe;				// ジャンプSE
	private Animator animator;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();

		// オーディオソースの設定
		audioSource = gameManager.GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		CheckJumpAvailablity();

		if (!usingButtons) {
			float x = Input.GetAxisRaw ("Horizontal");

			if (x == 0) {
				moveDirection = MoveDirection.Stop;
			} else {
				if (x > 0) {
					moveDirection = MoveDirection.Right;
				} else {
					moveDirection = MoveDirection.Left;
				}
			}

			if (Input.GetKeyDown ("space")){
				PushJumpButton ();
			}
		}
	}

	// 固定更新処理
	void FixedUpdate () {
		if (!canMove) {
			return;
		}

		// 現在のスピード
		var newVelocity = rbody.velocity;

		// 移動処理
		if (canJump) {
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
			Jump ();
		} else if (goWallRightJump) {
			WallJump(MoveDirection.Left);
		} else if (goWallLeftJump) {
			WallJump(MoveDirection.Right);
		}

		// 飛び退り処理
		if (goStepBack != MoveDirection.Stop) {
			StepBack(goStepBack);

			goStepBack = MoveDirection.Stop;
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
		canMove = true;
	}

	public void PushLeftButton () {
		moveDirection = MoveDirection.Left;
		usingButtons = true;
	}

	public void PushRightButton () {
		moveDirection = MoveDirection.Right;
		usingButtons = true;
	}

	public void ReleaseMoveButton () {
		moveDirection = MoveDirection.Stop;
	}

	public void PushJumpButton () {
		if (canJump) {
			goJump = true;
		} else if (canWallRightJump) {
			goWallRightJump = true;
		} else if (canWallLeftJump) {
			goWallLeftJump = true;
		}
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
		goJump = false;
		status = Statuses.Jumping;

		rbody.velocity = newVelocity;
	}

	public void WallJump (MoveDirection dir) {
		audioSource.PlayOneShot (jumpSe);
		var newVelocity = rbody.velocity;
		newVelocity.y = 0;
		rbody.AddForce (Vector2.up * JUMP_POWER);
		// FIX
		goWallRightJump = false;
		goWallLeftJump = false;
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
		}
		status = Statuses.BeingPushedBack;

		animator.SetInteger("status", (int)status);
	}

	private void CheckJumpAvailablity () {
		// ジャンプ可能か
		canJump = CheckOnGround();

		// 壁ジャンプ可能か
		Vector3 startOffset = transform.right * 0.6f;
		Vector3 endOffset = transform.up * 1.5f;
		canWallRightJump = !canJump && Physics2D.Linecast (
			transform.position + startOffset,
			transform.position + startOffset + endOffset,
			blockLayer);

		canWallLeftJump = !canJump && Physics2D.Linecast (
			transform.position - startOffset,
			transform.position - startOffset + endOffset,
			blockLayer);
	}

	private bool CheckOnGround () {
		return Physics2D.Linecast (transform.position - (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer)
			|| Physics2D.Linecast (transform.position + (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer);
	}
}
