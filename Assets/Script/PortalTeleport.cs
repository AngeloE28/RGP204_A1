using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{

    public Transform player;
    public Transform portalExit;
    private bool isPlayerOVerlapping = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlayerOVerlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;

            float rotationDiff = -Quaternion.Angle(transform.rotation, portalExit.rotation);
            rotationDiff += 180;

            player.Rotate(Vector3.up, rotationDiff);

            // Calculate the player's offset when teleporting
            Vector3 positionOffset = Quaternion.Euler(0.0f, rotationDiff, 0.0f) * portalToPlayer;
            Vector3 finalOffset = new Vector3(positionOffset.x, (positionOffset.y + 3.0f), positionOffset.z);

            // Teleport the player
            player.position = portalExit.position + finalOffset;

            isPlayerOVerlapping = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            isPlayerOVerlapping = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
            isPlayerOVerlapping = false;
    }
}
