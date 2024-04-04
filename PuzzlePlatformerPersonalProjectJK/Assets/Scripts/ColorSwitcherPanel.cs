using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ColorSwitcherPanel : MonoBehaviour
{
    public Color switcherColor = Color.red;
    public BoxCollider triggerCollider;

    private Renderer panelRenderer, colorOrbRenderer;
    private Transform parentTransform;
    private GameManager gameManager;


    void Start()
    {
        // get references
        parentTransform = this.transform.parent;
        colorOrbRenderer = parentTransform.Find("ColorOrb").GetComponent<Renderer>();
        panelRenderer = this.GetComponent<Renderer>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // update the color of the ColorSwitcherPanel's components
        panelRenderer.material.color = switcherColor;
        foreach (var mat in colorOrbRenderer.materials)
        {
            mat.color = switcherColor;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.ChangeColor(switcherColor); // change the player's color state and update all ColorBlocks
            StartCoroutine(OrbCooldown());          // initiate a cooldown for the next use of the ColorSwitcherPanel
        }
    }

    // coroutine for cooldown between uses of ColorSwitcherPanel
    IEnumerator OrbCooldown()
    {
        colorOrbRenderer.enabled = false;
        triggerCollider.enabled = false;

        yield return new WaitForSeconds(3.0f);

        colorOrbRenderer.enabled = true;
        triggerCollider.enabled = true;
    }
}
