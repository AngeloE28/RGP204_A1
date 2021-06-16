using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Car_Controller car;  // The car controller script
    public Transform attackPoint; // Where the bullets will come out from
    public ParticleSystem bullets; // Particle system to simulate gun fire
    public float maxEmmision; // Max particles being emitted
    private float emissionRate; // Controls how many particles to emit

    // Gun statistics
    public float gunRange = 100.0f; // Maximum range of the hitscan
    public bool allowGunToSpray;
    public float damage = 20;
    public bool shooting; // How does player shoot?

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        // Player can hold left click to spray bullets
        if (allowGunToSpray)
            shooting = Input.GetKey(KeyCode.Mouse1);
        else // Player needs to tap to shoot
            shooting = Input.GetKeyDown(KeyCode.Mouse1);

        // Player fires gun
        if (shooting)
            Shoot();
        else
            emissionRate = 0.0f;

        // Controls the emission that simulates bullets being fired
        var emissionModule = bullets.emission;
        emissionModule.rateOverTime = emissionRate;
    }

    // Controls the shooting of the gun
    private void Shoot()
    {
        emissionRate = maxEmmision; // Start playing particles

        RaycastHit hitInfo;
        if (Physics.Raycast(attackPoint.position, transform.forward, out hitInfo, gunRange))
        {
            EnemyBehaviour enemy = hitInfo.transform.GetComponent<EnemyBehaviour>();
            TargetObject target = hitInfo.transform.GetComponent<TargetObject>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }        
    }
}
