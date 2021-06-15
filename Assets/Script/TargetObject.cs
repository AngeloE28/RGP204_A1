using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public float health = 40f;
    
    
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Invoke(nameof(DestroyTarget), 0.5f);
        }
    }
    private void DestroyTarget()
    {
        Destroy(gameObject);
    }
}

