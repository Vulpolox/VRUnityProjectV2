using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GameObject References
        [Tooltip("Where the player starts the level")]
        public GameObject startPoint;            // to hold reference to the startPoint

        [Tooltip("The player")]
        public XROrigin player;                  // to hold reference to the player

        private GameObject leftHand, rightHand;  // to hold references to the hands of the XROrigin

    // StartPoint Variables
        private Vector3 startCoords;             // to hold the x and z coordinates of the start point
        private float playerHeight = 1.3f;       // y offset of the player to prevent spawning in the ground

    // Color Switcher Variables
        private Color currentPlayerColor = Color.white;
    
        [Tooltip("The color the player starts as")]
        public Color startingColor;

    private void Start()
    {
        // get references
        player = GameObject.FindWithTag("Player").GetComponent<XROrigin>(); // automatically set the reference to the player if it exists
        startPoint = GameObject.FindWithTag("StartPoint");                  // automatically set the reference to the start point if it exists
        leftHand = GameObject.FindWithTag("LeftHand");                      // do the same for the left hand
        rightHand = GameObject.FindWithTag("RightHand");                    // do the same for the right hand


        // exit the function if references aren't set properly
        if (startPoint == null || player == null)
            return;                                                         

        // get the location for the player to spawn in
        startCoords = startPoint.transform.position;
        startCoords.y += playerHeight;

        // spawn the player in at said location
        player.transform.position = startCoords;
        player.transform.rotation = startPoint.transform.rotation;
    }

    public void ChangeColor(Color newColor)
    {

    }

}
