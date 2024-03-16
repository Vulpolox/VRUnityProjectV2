using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Goal : MonoBehaviour
{
    
    private XROrigin player; // variable for holding reference to the CharacterController attached to the XROrigin

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<XROrigin>();
    }

    // if the player tocuhes the collider on the goal
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            win();
        }
    }

    // method for handling the logic of level completion
    private void win()
    {
        
    }
}
