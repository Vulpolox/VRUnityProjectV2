using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // === Variables ==================================================================================================================================

        // GameObject References
            [Tooltip("Where the player starts the level")]
            public GameObject startPoint;                   // to hold reference to the startPoint; where the player spawns in the level

            [Tooltip("The player")]
            public XROrigin player;                         // to hold reference to the player

            private Renderer leftRenderer, rightRenderer;   // to hold reference to the left and right hands

            private GameObject[] colorBlocks;               // to hold reference to all ColorBlocks in the scene

            private AudioSource audioSource;                // to hold reference to the audio source on the XR Origin

            private MovementHandler movementHandler;        // to hold reference to MovementHandler script attached to the XR Origin

    // StartPoint Variables
            private Vector3 startCoords;             // to hold the x and z coordinates of the start point
            private float playerHeight = 1.3f;       // y offset of the player to prevent spawning in the ground

        // Color Switcher Variables
            private Color currentPlayerColor;
    
            [Tooltip("The color the player starts as")]
            public Color startingColor = Color.black;

        // Powerup Variables
            public enum PowerupState { LowGravity, SuperJump, SuperDash, None };
            private PowerupState currentPowerupState = PowerupState.None;
            [SerializeField] private InputActionReference powerupButton;
            [SerializeField] private AudioClip superJumpSound, superDashSound, noPowerupSound, lowGravitySound;

    private void Start()
    {
        // get references
        player = GameObject.FindWithTag("Player").GetComponent<XROrigin>();            // automatically set the reference to the player if it exists
        startPoint = GameObject.FindWithTag("StartPoint");                             // automatically set the reference to the startPoint if it exists
        leftRenderer = GameObject.FindWithTag("LeftHand").GetComponent<Renderer>();    // do the same for the left hand renderer
        rightRenderer = GameObject.FindWithTag("RightHand").GetComponent<Renderer>();  // do the same for the right hand renderer
        colorBlocks = GameObject.FindGameObjectsWithTag("ColorBlock");                 // do the same for all ColorBlocks
        audioSource = player.GetComponent<AudioSource>();                              // do the same for the audio source
        movementHandler = player.GetComponent<MovementHandler>();                      // do the same for the MovementHandler script reference


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

    // === Miscellaneous ===========================================================================================================================

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


    // === Color Changing ===========================================================================================================================

        // public method for changing the player's color state
        public void ChangeColor(Color newColor)
        {
            // change the color of the player's hands to signify the current color state
            leftRenderer.material.color = newColor;
            rightRenderer.material.color = newColor;

            // change the color state
            currentPlayerColor = newColor;
        
            // update the state of all color blocks
            StartCoroutine(UpdateColorBlocks());
        }

        // method for getting the player's current color
        public Color GetCurrentColor() { return currentPlayerColor; }

        // static method for comparing RGB of colors
        public static bool IsColorEqual(Color c1, Color c2)
        {
            return (c1.r == c2.r) && (c1.g == c2.g) && (c1.b == c2.b);
        }

        // method for updating the state of all ColorBlocks in the scene; if player
        // is the same color as the block, it becomes transparent and is able to be passed through
        IEnumerator UpdateColorBlocks()
        {
            foreach (var block in colorBlocks)
            {
                ColorCube scriptRef = block.GetComponent<ColorCube>();
                Color blockColor = scriptRef.blockColor;

                bool isMakeSolid = (!IsColorEqual(blockColor, currentPlayerColor));
                scriptRef.SetState(isMakeSolid);

                yield return null;
            }
        }


    // === Powerups ===========================================================================================================================

    // methods for setting up the powerup button
    private void OnEnable() { powerupButton.action.performed += UsePowerup; }
    private void OnDisable() { powerupButton.action.performed -= UsePowerup; }

    // methods for setting powerup state (will be called on pickup)
    public void SetSuperJump() { currentPowerupState = PowerupState.SuperJump; }
    public void SetLowGravity() { currentPowerupState = PowerupState.LowGravity; }
    public void SetSuperDash() { currentPowerupState = PowerupState.SuperDash; }
    
    // method for using powerup when powerupButton is pressed
    private void UsePowerup(InputAction.CallbackContext obj)
    {
        // play the corresponding sound and call the corresponding method pertaining to the currentPowerupState
        switch(currentPowerupState)
        {
            case PowerupState.LowGravity:
                audioSource.PlayOneShot(lowGravitySound);
                StartCoroutine(UseLowGravity());
                break;
            case PowerupState.SuperDash:
                break; // TODO
            case PowerupState.SuperJump:
                audioSource.PlayOneShot(superJumpSound);
                UseSuperJump();
                break;
            case PowerupState.None:
                audioSource.PlayOneShot(noPowerupSound);
                break;

        }

        currentPowerupState = PowerupState.None;
    }

    // method for using SuperJump powerup
    private void UseSuperJump()
    {
        bool isMoving = movementHandler.getLateralMovement();                            // a boolean that specifies whether or not the player has moved laterally since last frame
        float velocityToApply = Mathf.Sqrt(15.0f * -2.0f * -9.81f);                      // the jump velocity to apply to the player

        Vector3 superJumpVelocity = Vector3.zero;
        superJumpVelocity.y = (isMoving) ? velocityToApply : (velocityToApply / 1.35f);  // b/c of jankiness from the ContinuousMoveProvider, jump velocity needs to be modified when
                                                                                         // the player isn't moving to prevent jumping higher than expected

        movementHandler.setPlayerVelocity(superJumpVelocity);                            // set the player's updated velocity
    }

    // coroutine for using LowGravity powerup
    private IEnumerator UseLowGravity()
    {
        float defaultGravitationalAcceleration = movementHandler.getGravity();           // store the gravitational acceleration before modification in a variable
        float startTime = Time.time;
        movementHandler.setGravity(-3.0f);                                               // set the gravity to a lower amount

        yield return new WaitForSeconds(5.0f);                                           // wait for 5 seconds

        movementHandler.setGravity(defaultGravitationalAcceleration);                    // reset the gravitational acceleration to its previous amount
    }
}