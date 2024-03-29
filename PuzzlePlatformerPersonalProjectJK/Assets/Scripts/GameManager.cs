using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GameObject References
        [Tooltip("Where the player starts the level")]
        public GameObject startPoint;            // to hold reference to the startPoint; where the player spawns in the level

        [Tooltip("The player")]
        public XROrigin player;                  // to hold reference to the player

        private Renderer leftRenderer, rightRenderer;

    // StartPoint Variables
        private Vector3 startCoords;             // to hold the x and z coordinates of the start point
        private float playerHeight = 1.3f;       // y offset of the player to prevent spawning in the ground

    // Color Switcher Variables
        private Color currentPlayerColor = Color.black;
    
        [Tooltip("The color the player starts as")]
        public Color startingColor;

    private void Start()
    {
        // get references
        player = GameObject.FindWithTag("Player").GetComponent<XROrigin>();            // automatically set the reference to the player if it exists
        startPoint = GameObject.FindWithTag("StartPoint");                             // automatically set the reference to the startPoint if it exists
        leftRenderer = GameObject.FindWithTag("LeftHand").GetComponent<Renderer>();    // do the same for the left hand renderer
        rightRenderer = GameObject.FindWithTag("RightHand").GetComponent<Renderer>();  // do the same for the right hand renderer


        // exit the function if references aren't set properly
        if (startPoint == null || player == null)
        {
            Debug.Log("References not set properly");
            return;
        }
            
        // spawn the player at the startPoint
        SpawnPlayer();

        // change player's currentColor to the startingColor
        ChangeColor(startingColor);
        
    }

    // method for setting the player's position and rotation to that of the startPoint
    private void SpawnPlayer()
    {
        // get the location for the player to spawn in
        startCoords = startPoint.transform.position;
        startCoords.y += playerHeight;

        // spawn the player in at said location
        player.transform.position = startCoords;
        player.transform.rotation = startPoint.transform.rotation;
    }

    // method for changing the player's color state
    public void ChangeColor(Color newColor)
    {
        // change the color of the player's hands to signify the current color state
        leftRenderer.material.color = newColor;
        rightRenderer.material.color = newColor;

        // change the color state
        currentPlayerColor = newColor;
    }

    // method for getting the player's current color
    public Color GetCurrentColor() { return currentPlayerColor; }

    // static method for comparing RGB of colors
    public static bool isColorEqual(Color c1, Color c2)
    {
        return (c1.r == c2.r) && (c1.g == c2.g) && (c1.b == c2.b);
    }

}
