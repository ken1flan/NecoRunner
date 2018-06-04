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
	private float velocityX = 0;		// 現在のスピード
	private float velocityY = 0;		// 現在のスピード

	public enum MoveDirection{
		STOP,
		LEFT,
		RIGHT,
	};
	private bool canMove = false;
	private MoveDirection moveDirection = MoveDirection.STOP;	// 移動方向
	private float jumpPower = 300;			// ジャンプ力
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
		// ジャンプ可能か
		canJump = Physics2D.Linecast (transform.position - (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer)
			|| Physics2D.Linecast (transform.position + (transform.right * 0.2f), transform.position - (transform.up * 0.1f), blockLayer);

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

		if (!usingButtons) {
			float x = Input.GetAxisRaw ("Horizontal");

			if (x == 0) {
				moveDirection = MoveDirection.STOP;
			} else {
				if (x > 0) {
					moveDirection = MoveDirection.RIGHT;
				} else {
					moveDirection = MoveDirection.LEFT;
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
		velocityX = rbody.velocity.x;
		velocityY = rbody.velocity.y;

		// 移動処理
		if (canJump) {
			switch (moveDirection) {
			case MoveDirection.LEFT:
				velocityX = -MOVE_SPEED;
				transform.localScale = new Vector2 (-1, 1);
				status = Statuses.Running;
				break;
			case MoveDirection.RIGHT:
				velocityX = MOVE_SPEED;
				transform.localScale = new Vector2 (1, 1);
				status = Statuses.Running;
				break;
			default:
				velocityX = 0;
				status = Statuses.Standing;
				break;
			}
		}

		// ジャンプ処理
		if (goJump) {
			audioSource.PlayOneShot (jumpSe);
			velocityY = 0;
			rbody.AddForce (Vector2.up * jumpPower);
			goJump = false;
			status = Statuses.Jumping;
		} else if (goWallRightJump) {
			audioSource.PlayOneShot (jumpSe);
			velocityY = 0;
			rbody.AddForce (Vector2.up * jumpPower);
			goWallRightJump = false;
			velocityX = -MOVE_SPEED;
			transform.localScale = new Vector2 (-1, 1);
			status = Statuses.Jumping;
		} else if (goWallLeftJump) {
			audioSource.PlayOneShot (jumpSe);
			velocityY = 0;
			rbody.AddForce (Vector2.up * jumpPower);
			goWallLeftJump = false;
			velocityX = MOVE_SPEED;
			transform.localScale = new Vector2 (1, 1);
			status = Statuses.Jumping;
		}

		// 移動速度設定
		rbody.velocity = new Vector2 (velocityX, velocityY);

		animator.SetInteger("status", (int)status);
	}

	void OnTriggerEnter2D (Collider2D col) {
		var gameObject = col.gameObject;

		if (gameObject.tag == "Bullet") {
			Debug.Log("hit");
			Destroy(gameObject);
			BePushedBack ();
		}

		if (gameObject.tag == "Trap") {
			gameManager.GetComponent<GameManager> ().GameOver ();
			DestroyPlayer ();
		}

		if (gameObject.tag == "Goal") {
			gameManager.GetComponent<GameManager> ().GameClear ();
			DestroyPlayer ();
		}
	}
	void DestroyPlayer () {
		Destroy (this.gameObject);
	}

	public void StartGame () {
		canMove = true;
	}

	public void PushLeftButton () {
		moveDirection = MoveDirection.LEFT;
		usingButtons = true;
	}

	public void PushRightButton () {
		moveDirection = MoveDirection.RIGHT;
		usingButtons = true;
	}

	public void ReleaseMoveButton () {
		moveDirection = MoveDirection.STOP;
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

	public void BePushedBack () {
		// 処理
	}
}
