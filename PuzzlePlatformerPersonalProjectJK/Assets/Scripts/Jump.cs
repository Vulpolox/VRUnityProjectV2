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
        if (!characterController.isGrounded) // if the player isn't grounded, exit the function
            return;

        playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityAcceleration); // manipulates kinematic equation to find the velocity
                                                                                 // required to reach jumpHeight and sets playerVelocity's y component to that
        
        movementHandler.setPlayerVelocity(playerVelocity);                       // set the player's updated velocity

        audioSource.PlayOneShot(jumpSound);                                      // play the jumping sound effect when Jumping() is called

    }
}
