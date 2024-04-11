using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    // editor-adjustable variables for dash metrics
    [SerializeField] private float dashCooldownTime = 2.0f; // the time it takes after dashing to be able to dash again
    [SerializeField] private float dashDuration = 1.0f;     // the time of the dash
    [SerializeField] private float dashForce = 5.0f;        // the strength of the dash

    // private variables
    private Vector3 dashDirection;       // variable for holding the normalized vector containing the x and z components of the player's current movement direction
    private bool dashOnCooldown = false; // flag for whether or not dash is on cooldown
    private bool isDashing = false;      // flag for whether or not the player is currently dashing

    // for mappable dash button
    [SerializeField] private InputActionReference dashButton;

    // references to components
    private CharacterController player;
    private XROrigin xrRig;
    private MovementHandler movementHandler;
    private AudioSource audioSource;
    [SerializeField] private AudioClip dashSound;

    void Start() 
    {
        player = GetComponent<CharacterController>();
        xrRig = GetComponent<XROrigin>();
        movementHandler = GetComponent<MovementHandler>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {dashButton.action.performed += Dashing;}
    private void OnDisable() {dashButton.action.performed -= Dashing;}

    // method called when the dash button is pressed
    private void Dashing(InputAction.CallbackContext obj)
    {
        // if the dash is not on cooldown and the player isn't currently dashing
        if (!dashOnCooldown && !isDashing && !player.isGrounded)
        {
            dashDirection = xrRig.Camera.transform.forward;
            dashDirection.y = 0;

            StartCoroutine(performDash(dashDirection));
            StartCoroutine(dashCooldownCoroutine());
        }
        
    }

    // coroutine that handles the logic for the dash itself
    IEnumerator performDash(Vector3 dashDirection)
    {
        audioSource.PlayOneShot(dashSound);

        if (dashOnCooldown)
            yield break;

        isDashing = true;                                // set the isDashing flag to true

        float startTime = Time.time;                     // the time at which the dash is initiated
        Vector3 dashVector = dashDirection * dashForce;  // the vector representing the number of units to move per second in the x and z directions

        while (Time.time < startTime + dashDuration)
        {
            // only move the player if they're in the air so as to prevent ackward sliding on the ground
            if (!player.isGrounded)
                player.Move(dashVector * Time.deltaTime);

            yield return null;
        }

        isDashing = false;                               // reset the isDashing flag
    }
    
    // coroutine that handles the logic for the dash cooldown
    IEnumerator dashCooldownCoroutine()
    {
        float startTime = Time.time;                      // the starting time of the cooldown timer
        bool regrounded = false;                          // flag for tracking whether the player has grounded since dashing

        dashOnCooldown = true;                            // set the dashOnCooldown flag to true

        while (Time.time < (startTime + dashCooldownTime) && !regrounded)
        {
            if (player.isGrounded)
                regrounded = true;

            yield return null;
        }

        dashOnCooldown = false;                           // after the cooldown is complete reset the flag
    }
}
