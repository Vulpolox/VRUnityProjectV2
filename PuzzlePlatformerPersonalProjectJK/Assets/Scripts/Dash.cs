using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    // editor-adjustable variables for dash metrics
    [SerializeField] private float dashCooldownTime; // the time it takes after dashing to be able to dash again
    [SerializeField] private float dashDuration;     // the time of the dash
    [SerializeField] private float dashForce;        // the strength of the dash

    // private variables
    private Vector2 dashDirection;       // variable for holding the normalized vector containing the x and z components of the player's current movement direction
    private bool dashOnCooldown = false; // flag for whether or not dash is on cooldown
    private bool isDashing = false;      // flag for whether or not the player is currently dashing

    // for mappable dash button
    [SerializeField] private InputActionReference dashButton;

    // reference of CharacterController
    private CharacterController player;

    void Start() {player = GetComponent<CharacterController>();}

    private void OnEnable() {dashButton.action.performed += Dashing;}
    private void OnDisable() {dashButton.action.performed -= Dashing;}

    // method called when the dash button is pressed
    private void Dashing(InputAction.CallbackContext obj)
    {
        // if the dash is not on cooldown and the player isn't currently dashing
        if (!dashOnCooldown && !isDashing)
        {
            dashDirection = this.transform.forward;

            StartCoroutine(performDash(dashDirection));
            StartCoroutine(dashCooldownCoroutine());

        }
        
    }

    // coroutine that handles the logic for the dash itself
    IEnumerator performDash(Vector2 dashDirection)
    {
        if (dashOnCooldown)
            yield break;

        isDashing = true;                               // set the isDashing flag to true

        float startTime = Time.time;                    // the time at which the dash is initiated
        Vector2 dashVector = dashDirection * dashForce; // the vector representing the number of units to move per second in the x and z directions

        while (Time.time < startTime + dashDuration)
        {
            player.Move(dashVector * Time.deltaTime);

            yield return null;
        }

        isDashing = false;                               // reset the isDashing flag
    }
    
    // coroutine that handles the logic for the dash cooldown
    IEnumerator dashCooldownCoroutine()
    {
        float currentTime = Time.time;                    // the starting time of the cooldown timer
        float endTime = currentTime + dashCooldownTime;   // the ending time of the cooldown timer

        dashOnCooldown = true;                            // set the dashOnCooldown flag to true

        while (currentTime < endTime)
        {
            // only progress the cooldown if the player is grounded; goal is to only have one dash available per time in air
            if (player.isGrounded)
                currentTime += Time.deltaTime;

            yield return null;
        }

        dashOnCooldown = false;                           // after the cooldown is complete reset the flag
    }
}
