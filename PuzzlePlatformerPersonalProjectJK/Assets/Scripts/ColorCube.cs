using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCube : MonoBehaviour
{
    public Color blockColor = Color.red;        // reference to the color of the ColorBlock
    private Renderer blockRenderer;             // reference to the renderer to manipulate the ColorBlock's color
    private Collider blockCollider;             // reference to collider


    private void Awake()
    {
        blockRenderer = this.GetComponent<Renderer>();  // set reference of the renderer
        blockCollider = this.GetComponent<Collider>();  // set reference of the collider

        blockRenderer.material.color = blockColor;      // change the color of the block
    }

    // method for setting the ColorBlock's state
    public void SetState(bool isSolid)
    {
        Color tempColor = new Color(); // temporary Color object because Unity is finnicky with assigning new alpha values

        // if the block's state is specified to be changed to solid
        if (isSolid)
        {
            tempColor = blockRenderer.material.color;
            tempColor.a = 1;
            blockRenderer.material.color = tempColor;

        }

        // if the block's state is specified to be changed to passable
        else
        {
            tempColor = blockRenderer.material.color;
            tempColor.a = 0.5f;
            blockRenderer.material.color = tempColor;
        }

        blockCollider.enabled = isSolid; // toggle the collider
    }

}
