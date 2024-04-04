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
        blockRenderer = this.GetComponent<Renderer>();
        blockCollider = this.GetComponent<Collider>();

        blockRenderer.material.color = blockColor;
    }

    // method for setting the ColorBlock's state
    public void SetState(bool isSolid)
    {
        Color tempColor = new Color();

        if (isSolid)
        {
            tempColor = blockRenderer.material.color;
            tempColor.a = 1;
            blockRenderer.material.color = tempColor;

        }

        else
        {
            tempColor = blockRenderer.material.color;
            tempColor.a = 0.5f;
            blockRenderer.material.color = tempColor;
        }

        blockCollider.enabled = isSolid;
    }

}
