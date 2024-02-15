using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class DeathHandler : MonoBehaviour
{
    [Tooltip("The height at which the player dies and the level resets")]
    public float killHeight = -50.0f;

    [Tooltip("The sound to play when the player dies")]
    public AudioClip deathSound = null;

    [Tooltip("The XR Rig; this is the only object whose movement is tracked")]
    public XROrigin xrRig = null;

    private AudioSource audioSource = null;   // The thing that emits sounds
    private float soundHeight;                // The height at which the death sound plays
    private bool soundFlag = false;
    



    // Start is called before the first frame update
    void Start()
    {
        audioSource = xrRig.GetComponent<AudioSource>();
        soundHeight = killHeight + 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (xrRig.transform.position.y <= killHeight)
        {
            KillPlayer();
        }

        else if (xrRig.transform.position.y <= soundHeight && soundFlag == false)
        {
            PlayDeathSound();
        }
    }

    public void KillPlayer()
    {
        // TODO: add UI menu where user chooses to restart or to quit to level select //
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload the scene (temporary)
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
        soundFlag = true;
    }
}
