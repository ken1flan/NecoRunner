using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditor {
	[MenuItem("Tools/PlayerPrefs/DeletAll")]
	static void DeleteAll () {
		PlayerPrefs.DeleteAll ();
		Debug.Log("Delete All Data of PlayerPrefs.");
	}

}
