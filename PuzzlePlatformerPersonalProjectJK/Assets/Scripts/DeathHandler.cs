using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent (typeof(AudioSource))]

public class DeathHandler : MonoBehaviour
{
    [Tooltip("The height at which the player dies and the level resets")]
    public float killHeight = -50.0f;

    [Tooltip("The sound to play when the player dies")]
    public AudioClip deathSound = null;

    private AudioSource audioSource = null;          // The AudioSource the emit sounds
    public XROrigin xrRig = null;                    // The player


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (xrRig.transform.position.y <= killHeight)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        audioSource.PlayOneShot(deathSound);                        // play the death sound
        StartCoroutine(waitSecond(1f));                             // wait 1 second for the death sound to play
        // TODO: add UI menu where user chooses to restart or to quit to level select //
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload the scene

    }

    IEnumerator waitSecond(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
