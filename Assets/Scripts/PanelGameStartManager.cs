using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelGameStartManager : MonoBehaviour {
	public GameObject textSignal;
	public AudioClip readySe;
	public AudioClip setSe;
	public AudioClip goSe;

	public delegate void OnComplete ();
	protected OnComplete onCompleteCallback;

	private Text signal;
	private Image background;

	const int READY_TIME = 1;
	const int SET_TIME = 2;
	const int GO_TIME = 3;
	const float DESTROY_TIME = 3.5f;
	private const string READY_TEXT = "Ready..";
	private const string SET_TEXT = "Set..";
	private const string GO_TEXT = "Go!!";

	private Color textColorReady = new Color(1.0f,  0.5f, 0.5f, 1.0f);
	private Color textColorSet = new Color(1.0f,  1.0f, 0.5f, 1.0f);
	private Color textColorGo = new Color(0.5f,  0.5f, 1.0f, 1.0f);

	private Color backgroundColorReady = new Color(0.0f, 0.0f, 0.0f, 0.9f);
	private Color backgroundColorSet = new Color(0.0f, 0.0f, 0.0f, 0.5f);
	private Color backgroundColorGo = new Color(0.0f, 0.0f, 0.0f, 0.0f);

	public enum Statuses {
		BeforeStart,
		Ready,
		Set,
		Go
	}
	public Statuses status = Statuses.Ready;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		signal = textSignal.GetComponent<Text> ();
		background = this.GetComponent<Image> ();
	}

	// Update is called once per frame
	void Update () {
		var time = Time.time;
		if (time > READY_TIME && status == Statuses.BeforeStart) {
			status = Statuses.Ready;
			signal.text = READY_TEXT;
			signal.color = textColorReady;
			background.color = backgroundColorReady;
			audioSource.PlayOneShot(setSe);
		} else if (time > SET_TIME && status == Statuses.Ready) {
			status = Statuses.Set;
			signal.text = SET_TEXT;
			signal.color = textColorSet;
			background.color = backgroundColorSet;
			audioSource.PlayOneShot(setSe);
		} else if (time > GO_TIME && status == Statuses.Set) {
			status = Statuses.Go;
			signal.text = GO_TEXT;
			signal.color = textColorGo;
			background.color = backgroundColorGo;
			audioSource.PlayOneShot(goSe);
		} else if ( time > DESTROY_TIME ) {
			this.hideFlags = HideFlags.DontSave;
			onCompleteCallback ();
			Destroy (this.gameObject);
		}
	}

	public void SetConfigurations (AudioSource audioSource, OnComplete onCompleteCallback) {
		this.audioSource = audioSource;
		this.onCompleteCallback = onCompleteCallback;
	}
}
