using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]

// Script made using help from this: https://www.youtube.com/watch?v=-jh-YlRXuyk&ab_channel=BlackWhaleStudio-XRTutorials
public class Jump : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpButton;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravityAcceleration = -9.81f;
    
    // Stuff for the jump sound effect
    [SerializeField] private AudioClip jumpSound;
    private AudioSource audioSource;

    private CharacterController characterController;
    private Vector3 playerVelocity;

    private MovementHandler movementHandler; // holds a reference to the PlayerMovmentHandler script attached to the XROrigin

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        movementHandler = GetComponent<MovementHandler>();
    }

    // no clue what these do
    private void OnEnable() {jumpButton.action.performed += Jumping;}
    private void OnDisable() {jumpButton.action.performed -= Jumping;}

    private void Jumping(InputAction.CallbackContext obj)
    {
        // if the player isn't grounded, exit the function
        if (!characterController.isGrounded)
            return;

        bool isMoving = movementHandler.getLateralMovement();                         // a boolean that specifies whether or not the player has moved laterally since last frame
        float velocityToApply = Mathf.Sqrt(jumpHeight * -3.0f * gravityAcceleration); // the jump velocity to apply to the player

        playerVelocity.y = (isMoving) ? velocityToApply : (velocityToApply / 1.35f);  // b/c of jankiness from the ContinuousMoveProvider, jump velocity needs to be modified when
                                                                                      // the player isn't moving to prevent jumping higher than expected

        movementHandler.setPlayerVelocity(playerVelocity);                            // set the player's updated velocity

        audioSource.PlayOneShot(jumpSound);                                           // play the jumping sound effect when Jumping() is called

    }
}
