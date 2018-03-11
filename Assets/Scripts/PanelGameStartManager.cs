using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelGameStartManager : MonoBehaviour {
	public GameObject textSignal;
	private Text signal;
	private Image background;

	const int READY_TIME = 0;
	const int SET_TIME = 1;
	const int GO_TIME = 2;
	const float DESTROY_TIME = 2.5f;
	private const string READY_TEXT = "Ready..";
	private const string SET_TEXT = "Set..";
	private const string GO_TEXT = "Go!!";

	private Color textColorReady = new Color(1.0f,  0.5f, 0.5f, 1.0f);
	private Color textColorSet = new Color(1.0f,  1.0f, 0.5f, 1.0f);
	private Color textColorGo = new Color(0.5f,  0.5f, 1.0f, 1.0f);

	private Color backgroundlColorReady = new Color(0.0f, 0.0f, 0.0f, 0.9f);
	private Color backgroundColorSet = new Color(0.0f, 0.0f, 0.0f, 0.5f);
	private Color backgroundColorGo = new Color(0.0f, 0.0f, 0.0f, 0.0f);

	public enum Statuses {
		Ready,
		Set,
		Go
	}
	public Statuses status = Statuses.Ready;

	// Use this for initialization
	void Start () {
		signal = textSignal.GetComponent<Text> ();
		background = this.GetComponent<Image> ();
	}

	// Update is called once per frame
	void Update () {
		var time = Time.time;
		if (time > SET_TIME && status == Statuses.Ready) {
			status = Statuses.Set;
			signal.text = SET_TEXT;
			signal.color = textColorSet;
			background.color = backgroundColorSet;
		} else if (time > GO_TIME && status == Statuses.Set) {
			status = Statuses.Go;
			signal.text = GO_TEXT;
			signal.color = textColorGo;
			background.color = backgroundColorGo;
		} else if ( time > DESTROY_TIME ) {
			this.hideFlags = HideFlags.DontSave;
			Destroy (this.gameObject);
		}
	}
}
