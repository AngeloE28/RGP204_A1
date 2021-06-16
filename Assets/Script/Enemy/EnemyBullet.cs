using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Car_Controller player;

    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy gameobject on impact
        if (collision.gameObject.tag == "ground")
            Destroy(this.gameObject);

        if (collision.gameObject.tag == "player")
        {
            player = collision.transform.GetComponent<Car_Controller>();
            player.PlayerTakeDamage(damage);
           
            Destroy(this.gameObject);
        }
    }
}
