using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class Goal : MonoBehaviour
{
    
    private XROrigin player;                 // variable for holding reference to the CharacterController attached to the XROrigin
    private MovementHandler movementHandler; // variable for a reference to the MovementHandler script attached to the XROrigin

    public Canvas goalCanvas;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<XROrigin>();
        movementHandler = player.GetComponent<MovementHandler>();

        goalCanvas.enabled = false;
    }

    // if the player tocuhes the collider on the goal
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            win();
        }
    }

    // method for handling the logic of level completion
    private void win()
    {
        movementHandler.toggleHorizontalMovement();
        goalCanvas.enabled = true;
    }

    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
    }
}
