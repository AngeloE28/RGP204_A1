using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public float health = 40f;
    private float originalHealth;

    private void Start()
    {
        originalHealth = health;
    }
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
        // Disable the game object for a set amount of time to replicate respawning
        this.gameObject.SetActive(false);
        Invoke(nameof(Respawn), 5.0f);
    }

    private  void Respawn()
    {    
        // Enable the game object replicate respawning
        // Reset all original values
        this.gameObject.SetActive(true);

        health = originalHealth;
    }
}

