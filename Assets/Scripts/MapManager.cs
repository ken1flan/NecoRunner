using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SetGroundTilemap();
		SetKeepoutTilemap ();
		SetTrapTilemap ();
	}

	// Update is called once per frame
	void Update () {

	}

	private void SetGroundTilemap () {
		var tilemap = GameObject.Find("GroundTilemap");
		tilemap.layer = LayerMask.NameToLayer("Block");
		tilemap.AddComponent<TilemapCollider2D> ();
	}

	private void SetKeepoutTilemap () {
		var tilemap = GameObject.Find("KeepoutTilemap");
		tilemap.layer = LayerMask.NameToLayer("Keepout");
		tilemap.AddComponent<TilemapCollider2D> ();
	}

	private void SetTrapTilemap () {
		var tilemap = GameObject.Find("TrapTilemap");
		tilemap.tag = "Trap";
		var collider = tilemap.AddComponent<TilemapCollider2D> ();
		collider.isTrigger = true;
	}
}
