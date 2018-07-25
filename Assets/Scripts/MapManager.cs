using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {
    public Vector3Int mapSize;
    public Vector3 mapRightTopPosition;
    public Vector3Int mapOrigin;
    public Vector3 mapLeftBottomPosition;
    public Vector3 cellSize;

    private Grid grid;
	private Tilemap keepoutTilemap;

	// Use this for initialization
	void Start () {
		SetGroundTilemap ();
		SetKeepoutTilemap ();
        SetTrapTilemap ();

        SetGridInformation();
        SetMapInformation ();
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
		keepoutTilemap = tilemap.GetComponent<Tilemap> ();
	}

	private void SetTrapTilemap () {
		var tilemap = GameObject.Find("TrapTilemap");
		tilemap.tag = "Trap";
		var collider = tilemap.AddComponent<TilemapCollider2D> ();
		collider.isTrigger = true;
	}

    private void SetGridInformation () {
        grid = gameObject.GetComponent<Grid> ();
        cellSize = grid.cellSize;
    }

    private void SetMapInformation () {
        var keepoutOrigin = keepoutTilemap.origin;
        mapOrigin = new Vector3Int(keepoutOrigin.x - 1, keepoutOrigin.y - 1, keepoutOrigin.z);
        mapLeftBottomPosition = keepoutTilemap.GetCellCenterWorld(mapOrigin);

        var keepoutSize = keepoutTilemap.size;
        mapSize = new Vector3Int(keepoutSize.x - 2, keepoutSize.y - 2, keepoutSize.z);
        var mapTopRight = new Vector3Int(mapSize.x + mapOrigin.x, mapSize.y + mapOrigin.y, mapSize.z);
        mapRightTopPosition = keepoutTilemap.GetCellCenterWorld(mapTopRight);
    }
}
