using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{
    public Transform playerCam;
    public Transform portalEntrance;
    public Transform portalExit;

    // Update is called once per frame
    void LateUpdate()
    {
        // Limit the offset to only the x and y axis
        Vector3 playerOffsetFromPortal = new Vector3(playerCam.position.x, playerCam.position.y, 0.0f) - 
                                         new Vector3(portalExit.position.x, portalExit.position.y, 0.0f);

        // Moves the camera depending  on the player's position
        transform.position = portalEntrance.position + playerOffsetFromPortal;

        // Rotates the camera depending on the player's rotation
        // Get the angular difference between portals
        float angularDiffBetweenPortalRot = Quaternion.Angle(portalEntrance.rotation, portalExit.rotation);

        Quaternion portalRotDifference = Quaternion.AngleAxis(angularDiffBetweenPortalRot, Vector3.up);
        
        // Get the new vector for the camera with the rotation difference
        Vector3 newCamDir = portalRotDifference * -playerCam.forward;

        // Gets the new look direction
        Quaternion lookDir = Quaternion.LookRotation(newCamDir, Vector3.up);

        // Get the target rotation
        Quaternion targetCamDir;

        // Get the euler angles
        Vector3 eulerRotation = lookDir.eulerAngles;

        targetCamDir = Quaternion.Euler(15.0f, eulerRotation.y, eulerRotation.z);

        // Applies the target rotation
        transform.rotation = targetCamDir;
    }
}
