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
		// Do nothing
	}

	private void SetLeftButton () {
		EventTrigger eventTrigger = transform.Find("ButtonLeft").GetComponent<EventTrigger> ();

		EventTrigger.Entry pushButtonEntry = new EventTrigger.Entry();
		pushButtonEntry.eventID = EventTriggerType.PointerDown;
		pushButtonEntry.callback.AddListener((data) => { playerManager.PushLeftButton(); });
		eventTrigger.triggers.Add(pushButtonEntry);

		EventTrigger.Entry releaseButtonEntry = new EventTrigger.Entry();
		releaseButtonEntry.eventID = EventTriggerType.PointerUp;
		releaseButtonEntry.callback.AddListener((data) => { playerManager.ReleaseMoveButton(); });
		eventTrigger.triggers.Add(releaseButtonEntry);
	}

	private void SetRightButton () {
		EventTrigger eventTrigger = transform.Find("ButtonRight").GetComponent<EventTrigger> ();

		EventTrigger.Entry pushButtonEntry = new EventTrigger.Entry();
		pushButtonEntry.eventID = EventTriggerType.PointerDown;
		pushButtonEntry.callback.AddListener((data) => { playerManager.PushRightButton(); });
		eventTrigger.triggers.Add(pushButtonEntry);

		EventTrigger.Entry releaseButtonEntry = new EventTrigger.Entry();
		releaseButtonEntry.eventID = EventTriggerType.PointerUp;
		releaseButtonEntry.callback.AddListener((data) => { playerManager.ReleaseMoveButton(); });
		eventTrigger.triggers.Add(releaseButtonEntry);
	}

	private void SetJumpButton () {
		EventTrigger eventTrigger = transform.Find("ButtonJump").GetComponent<EventTrigger> ();

		EventTrigger.Entry pushButtonEntry = new EventTrigger.Entry();
		pushButtonEntry.eventID = EventTriggerType.PointerDown;
		pushButtonEntry.callback.AddListener((data) => { playerManager.PushJumpButton(); });
		eventTrigger.triggers.Add(pushButtonEntry);
	}
}
