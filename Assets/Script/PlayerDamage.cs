using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{

    public float playerHealth = 100f;
    public float damage;

    

    // Start is called before the first frame update
    void Start()
    {
        print( playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            playerHealth -= 10f;
            print(playerHealth);
        }
    }
}
