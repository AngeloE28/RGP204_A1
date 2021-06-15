using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{

    public Camera cameraTemple;
    public Camera cameraTable;

    public Material cameraMatTemple;
    public Material cameraMatTable;
    // Start is called before the first frame update
    void Start()
    {

        // Remove the texture
        if (cameraTemple.targetTexture != null)
            cameraTemple.targetTexture.Release();

        // Create a new texture
        cameraTemple.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatTemple.mainTexture = cameraTemple.targetTexture;

        // Remove the texture
        if (cameraTable.targetTexture != null)
            cameraTable.targetTexture.Release();

        // Create a new texture
        cameraTable.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatTable.mainTexture = cameraTable.targetTexture;
    }
}
