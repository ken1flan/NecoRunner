using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour {
	private PlayerManager playerManager;

	// Use this for initialization
	void Start () {
		playerManager = GameObject.Find("Player").GetComponent<PlayerManager> ();

		SetLeftButton ();
		SetRightButton ();
		SetJumpButton ();
	}

	// Update is called once per frame
	void Update () {
		float x = Input.GetAxisRaw ("Horizontal");

		if (x == 0) {
			playerManager.StopMoving ();
		} else {
			if (x > 0) {
				playerManager.GoRight ();
			} else {
				playerManager.GoLeft ();
			}
		}

		if (Input.GetKeyDown ("space")){
			playerManager.GoJump ();
		}
	}

	private void SetLeftButton () {
		var button = transform.Find("ButtonLeft").gameObject;
		EventTrigger eventTrigger = button.AddComponent(typeof(EventTrigger)) as EventTrigger;

		EventTrigger.Entry pushButtonEntry = new EventTrigger.Entry();
		pushButtonEntry.eventID = EventTriggerType.PointerDown;
		pushButtonEntry.callback.AddListener((data) => { playerManager.GoLeft(); });
		eventTrigger.triggers.Add(pushButtonEntry);

		EventTrigger.Entry releaseButtonEntry = new EventTrigger.Entry();
		releaseButtonEntry.eventID = EventTriggerType.PointerUp;
		releaseButtonEntry.callback.AddListener((data) => { playerManager.StopMoving(); });
		eventTrigger.triggers.Add(releaseButtonEntry);
	}

	private void SetRightButton () {
		var button = transform.Find("ButtonRight").gameObject;
		EventTrigger eventTrigger = button.AddComponent(typeof(EventTrigger)) as EventTrigger;

		EventTrigger.Entry pushButtonEntry = new EventTrigger.Entry();
		pushButtonEntry.eventID = EventTriggerType.PointerDown;
		pushButtonEntry.callback.AddListener((data) => { playerManager.GoRight(); });
		eventTrigger.triggers.Add(pushButtonEntry);

		EventTrigger.Entry releaseButtonEntry = new EventTrigger.Entry();
		releaseButtonEntry.eventID = EventTriggerType.PointerUp;
		releaseButtonEntry.callback.AddListener((data) => { playerManager.StopMoving(); });
		eventTrigger.triggers.Add(releaseButtonEntry);
	}

	private void SetJumpButton () {
		var button = transform.Find("ButtonJump").gameObject;
		EventTrigger eventTrigger = button.AddComponent(typeof(EventTrigger)) as EventTrigger;

		EventTrigger.Entry pushButtonEntry = new EventTrigger.Entry();
		pushButtonEntry.eventID = EventTriggerType.PointerDown;
		pushButtonEntry.callback.AddListener((data) => { playerManager.GoJump(); });
		eventTrigger.triggers.Add(pushButtonEntry);
	}
}
