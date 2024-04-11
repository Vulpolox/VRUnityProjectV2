using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType { LowGravity, SuperJump, SuperDash }; // enumeration definition for the type of powerup
    public PowerupType powerupType;                               // instance of the enumeration to be set in the inspector

    private Collider powerupPickupZone;                           // the trigger collider for powerup pickup
    private Renderer powerupRenderer;                             // renderer for the powerup

    private GameManager gameManager;                              // script reference of the game manager to handle powerup pickup

    private AudioSource audioSource;                              // the AudioSource component of the XR Origin
    public AudioClip powerupPickupSound;                          // the sound to be played when the player picks up a powerup

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        powerupPickupZone = this.GetComponent<Collider>();
        powerupRenderer = this.GetComponent<Renderer>();
    }

    // method for game logic when picking up powerup
    private void OnTriggerEnter(Collider other)
    {
        // if the player collides with the powerup
        if (other.CompareTag("Player"))
        {
            // set the player's currently held powerup based on the set PowerupType
            switch (powerupType)
            {
                case PowerupType.LowGravity:
                    gameManager.SetLowGravity();
                    break;
                case PowerupType.SuperJump:
                    gameManager.SetSuperJump();
                    break;
                case PowerupType.SuperDash:
                    gameManager.SetSuperDash();
                    break;
            }
        }

        audioSource.PlayOneShot(powerupPickupSound); // play the powerup pickup sound
        StartCoroutine(PowerupRespawnTimer());       // temporarily disable the powerup
    }

    // Coroutine for powerup respawning
    private IEnumerator PowerupRespawnTimer()
    {
        powerupRenderer.enabled = false;
        powerupPickupZone.enabled = false;

        yield return new WaitForSeconds(3.0f);

        powerupRenderer.enabled = true;
        powerupPickupZone.enabled = true;
    }

}
