using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]

// Script made using help from this: https://www.youtube.com/watch?v=-jh-YlRXuyk&ab_channel=BlackWhaleStudio-XRTutorials
public class Jump : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpButton;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravityAcceleration = -9.81f;
    
    [SerializeField] private AudioClip jumpSound;

    private CharacterController characterController;
    private Vector3 playerVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable() // no clue what these do
    {
        jumpButton.action.performed += Jumping;
    }

    private void OnDisable()
    {
        jumpButton.action.performed -= Jumping;
    }

    private void Jumping(InputAction.CallbackContext obj)
    {
        if (!characterController.isGrounded) // if the player isn't grounded, exit the function
            return;

        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityAcceleration); // manipulates kinematic equation to find the velocity
                                                                                 // required to reach jumpHeight and sets playerVelocity's y component to that


        
    }

    public void Update()
    {
        if (characterController.isGrounded && playerVelocity.y < 0) // prevents large negative acceleration while on the ground
        {
            playerVelocity.y = 0.0f;
        }

        playerVelocity.y += gravityAcceleration * Time.deltaTime; // decrease player velocity by 9.81 meters per second
        characterController.Move(playerVelocity * Time.deltaTime); // changes the player's y position based on the current y velocity
    }
}
