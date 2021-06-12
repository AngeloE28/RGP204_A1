using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script was made by following the tutorial by
// How Can I Create A “See Behind Walls” Effect In Unity?
// Romeo Violini (n.d.), at http://gamedevelopertips.com/how-can-i-create-a-see-behind-walls-effect-in-unity/

public class Turn_Transparent : MonoBehaviour
{
    private GameObject obj;
    private Renderer objRend;
    private Material mat;
    private Material originalMat;
    public Material transparentMat;
    private bool transparent = false;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the gameobject
        obj = this.gameObject;

        // Get the renderer of the object
        objRend = GetComponent<Renderer>();

        // Get the material color
        mat = objRend.material;
        originalMat = mat;
    }

    public void ChangeTransparency(bool transparent)
    {
        // Checks to avoid  setting the transparency twice
        if (this.transparent == transparent)
            return;

        // Set the new configuration
        this.transparent = transparent;

        // Check if the gameobject should be transparent
        if (transparent)
        {
            // Change the render mode to make it transparent
            mat = transparentMat;
        }
        else
        {
            // Resets the alpha
            mat = originalMat;
        }
        objRend.material = mat;
    }
}
