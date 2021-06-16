using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The transparancy code was made by following the tutorial by
// How Can I Create A “See Behind Walls” Effect In Unity?
// Romeo Violini (n.d.), at http://gamedevelopertips.com/how-can-i-create-a-see-behind-walls-effect-in-unity/

public class Camera_Follow : MonoBehaviour
{
    public Transform player; // Ref to player
    public Camera mainCam; // Ref to the camera
    public LayerMask groundMask; // Most of the objects will have the layer ground
    //public LayerMask treeMask; // The tree's will have this layermask
    private Turn_Transparent currentTransparentObj; // Ref to the current transparent object

    private void FixedUpdate()
    {
        // Raycast direction
        Vector3 dir = player.position - mainCam.transform.position;

        // Raycast Length
        float rayLength = Vector3.Distance(player.position, mainCam.transform.position);

        Debug.DrawRay(mainCam.transform.position, dir.normalized * rayLength, Color.red);

        // Checks for objects with the ground layermask
        CheckForObjectsBetweenCamAndPlayer(mainCam.transform, dir, rayLength, groundMask);
    }

    private void LateUpdate()
    {
        // Move and rotate the camera pivot to be the same as the player
        transform.position = player.position;
        transform.rotation = player.rotation;

        // Only follow the rotation of the y-axis and ignore the x and z axis
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f));
    }

    // Raycast to check for objects between player and camera
    private void CheckForObjectsBetweenCamAndPlayer(Transform cam, Vector3 dir, float length, LayerMask mask)
    {
        RaycastHit hit;
        // Cast the ray to make the object transparent
        if (Physics.Raycast(cam.position, dir, out hit, length, mask))
        {
            // Get the Turn_Transparent script, to trigger the transparency
            Turn_Transparent obj = hit.transform.GetComponent<Turn_Transparent>();

            // Check to see if the object is not null
            if (obj)
            {
                // Check to see if this object was hit before and
                // its different to the new transparent object
                if (currentTransparentObj && currentTransparentObj.gameObject != obj.gameObject)
                {
                    // Reset the transparency
                    currentTransparentObj.ChangeTransparency(false);
                }
                // Make the object transparent
                obj.ChangeTransparency(true);
                currentTransparentObj = obj;
            }
        }
        else
        {
            // If there is nothing inbetween the cam and the player
            // and there was an object turned transparent
            if (currentTransparentObj)
            {
                // Reset its transparency
                currentTransparentObj.ChangeTransparency(false);
            }
        }
    }
}
