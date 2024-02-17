using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Where the player starts the level")]
    public GameObject startPoint;

    [Tooltip("The player")]
    public XROrigin player;

    private Vector3 startCoords;
    private float playerHeight = 1.3f;

    private void Start()
    {
        startCoords = startPoint.transform.position;
        startCoords.y += playerHeight;

        player.transform.position = startCoords;
        player.transform.rotation = startPoint.transform.rotation;
    }

}
