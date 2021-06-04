using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform player; // Ref to player

    private void LateUpdate()
    {
        // Move and rotate the camera pivot to be the same as the player
        transform.position = player.position;
        transform.rotation = player.rotation;

        // Only follow the rotation of the y-axis and ignore the x and z axis
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f));
    }
}
