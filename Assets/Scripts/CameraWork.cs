using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    public float startXOffset;
    public float endXOffset;
    public float startYOffset;
    public float endYOffset;

    private GameObject player;		// プレイヤー
    private MapManager mapManager;  // マップ
    private bool isSetRange = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        mapManager = GameObject.Find("MapGrid").GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            float x = player.transform.position.x;
            if (x < startXOffset)
            {
                x = startXOffset;
            }
            else if (x > endXOffset)
            {
                x = endXOffset;
            }

            float y = player.transform.position.y;
            if (y < startYOffset)
            {
                y = startYOffset;
            }
            else if (y > endYOffset)
            {
                y = endYOffset;
            }

            transform.position = new Vector3(x, y, transform.position.z);
        }

        if (!isSetRange)
        {
            setRange();
            isSetRange = true;
        }
    }

    private void setRange()
    {
        startXOffset = mapManager.mapLeftBottomPosition.x + 8.0f;
        endXOffset = mapManager.mapRightTopPosition.x - 8.0f;
        startYOffset = mapManager.mapLeftBottomPosition.y + 10.0f;
        endYOffset = mapManager.mapRightTopPosition.y - 10.0f;
    }
}