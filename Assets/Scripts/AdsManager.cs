using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {

	#if UNITY_IOS
	private string gameId = "1721184";
	#elif UNITY_ANDROID
	private string gameId = "1721183";
	#endif

	private Rigidbody2D rbody;
	private const float SPEED = -50f;
	private float currentSpeed = SPEED;
	private const float LEFT_SIDE = -160f;
	private const float RIGHT_SIDE = 1440f;

	// Use this for initialization
	void Start () {
		Advertisement.Initialize(gameId);
		GetComponent<Button> ().onClick.AddListener(OnStartAds);

		rbody = GetComponent<Rigidbody2D> ();
		rbody.velocity = new Vector2 (SPEED, 0);
	}

	// Update is called once per frame
	void Update () {
		// 左端まで行ってたら右端から出てくる
		if (transform.position.x < LEFT_SIDE) {
			transform.position = new Vector2 (RIGHT_SIDE, transform.position.y);
		}
	}

	public void OnStartAds () {
		Advertisement.Show();
	}
}
