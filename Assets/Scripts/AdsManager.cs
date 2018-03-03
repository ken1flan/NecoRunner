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

	// Use this for initialization
	void Start () {
		Advertisement.Initialize(gameId);
		GetComponent<Button> ().onClick.AddListener(OnStartAds);
	}

	// Update is called once per frame
	void Update () {
	}

	public void OnStartAds () {
		Advertisement.Show();
	}
}
