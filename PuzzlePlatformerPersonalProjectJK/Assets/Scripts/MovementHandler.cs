using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    private bool isHorizontalMovement = true;              // flag for horizontal movement
    private bool isVerticalMovement = true;                // flag for vertical movement

    private CharacterController characterController;       // variable to store the XROrigin's CharacterController
    private Vector3 playerVelocity = Vector3.zero;         // vector to keep track of velocity to apply to CharacterController
    private float gravitationalAcceleration = -9.81f;      // acceleration due to gravity

    private Vector3 currentPosition = Vector3.zero;
    private Vector3 previousPosition = Vector3.zero;
    private float movementThreshold = 0.02f;
    private bool lateralMovmementFlag = false;


    private void Awake() { characterController = GetComponent<CharacterController>(); }

    // handle player falling due to gravity in Update()
    private void Update()
    {
        if (!isVerticalMovement)                                         // if vertical movement is toggled off, exit function
            return;

        if (characterController.isGrounded && playerVelocity.y < 0)      // prevent the player from building negative vertical velocity when grounded
            playerVelocity.y = 0;

        playerVelocity.y += gravitationalAcceleration * Time.deltaTime;  // increment the playerVelocity.y by g every second

        characterController.Move(playerVelocity * Time.deltaTime);       // apply playerVelocity to the characterController

        
        // update the lateralMovementFlag based on whether or not the player is moving in the x or z directions
        currentPosition = characterController.transform.position;
        lateralMovmementFlag = isMovingLaterally();
    }

    // method for determining whether or not the player is moving laterally
    private bool isMovingLaterally()
    {
        bool result = Mathf.Abs(currentPosition.z - previousPosition.z) > movementThreshold   // the boolean to return
                   || Mathf.Abs(currentPosition.x - previousPosition.x) > movementThreshold;

        previousPosition = currentPosition;                                                   // update the previous position
        return result;
    }

    // public method to toggle whether or not the player can move horizontally
    public void toggleHorizontalMovement()
    {
        isHorizontalMovement = !isHorizontalMovement;                           // toggle the horizontal movement flag
        characterController.slopeLimit = (isHorizontalMovement) ? 45.0f : 0.0f; // set the slope limit based off of the current status of the flag
    }

    // public method to toggle whether or not the player can move vertically
    public void toggleVerticalMovement() { isVerticalMovement = !isVerticalMovement; }

    // public method for setting the gravitational acceleration
    public void setGravity(float newGravity) { gravitationalAcceleration = newGravity; }

    // public method for setting player's velocity
    public void setPlayerVelocity(Vector3 newPlayerVelocity) { playerVelocity = newPlayerVelocity; }

    // public method for getting player's velocity
    public Vector3 getPlayerVelocity() { return playerVelocity; }

    // public method that returns a boolean based on whether or not the player has moved laterally since the previous call to Update
    public bool getLateralMovement() { return lateralMovmementFlag; }
}
