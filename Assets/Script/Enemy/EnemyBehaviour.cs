using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Code reference https://www.youtube.com/watch?v=UjkSFoLxesw

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform attackPoint;
    public LayerMask whatIsGround, whatIsPlayer;
    public float maxEnemyHealth = 100f;
    private float currentEnemyHealth;
    public float shootForce;
    public float groundRayLength;
    public float smoothCarRotationVal;

    // Respawning
    private Vector3 originalPos;

    //EnemyRoaming
    public Vector3 walkpoint;
    bool walkPointSet;
    public float walkPointRange;


    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //states
    public float sightRange, attackRange;
    public bool inSightRange, inAttackRange;


    private void Awake()
    {
        originalPos = transform.position;
        currentEnemyHealth = maxEnemyHealth;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patroling()
    {
        if(!walkPointSet)
        {
            LookForWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkpoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint; // calculating distance to walk point

        //WalkPoint  Reached
        if(distanceToWalkPoint.magnitude < 1.0f) //if distance is less than 1, walkpoint reached
        {
            walkPointSet = false; 
        }
            
    }

    private void ChasePlayer()
    {
        //transform.position = player.position;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //stop enemy from moving
        agent.SetDestination(transform.position);

        transform.LookAt(player);        

        if (!alreadyAttacked)
        {
            //Attack
            Rigidbody rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            Destroy(rb.gameObject, 2.0f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;

    }
    public void TakeDamage(float damage)
    {
        currentEnemyHealth -= damage;

        if (currentEnemyHealth <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        // Disable the game object for a set amount of time to replicate respawning
        this.gameObject.SetActive(false);
        Invoke(nameof(Respawn), 5.0f);
    }

    private void Respawn()
    {
        // Enable the game object replicate respawning
        // Reset all original values
        // Re-activate at original position
        this.gameObject.SetActive(true);

        currentEnemyHealth = maxEnemyHealth;
        transform.position = originalPos;
    }

    private void LookForWalkPoint()
    {
        //calculating random point
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkpoint, -transform.up, groundRayLength, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checking if the player is in attack or sight range
        float distBetweenEnemyAndPlayer = Vector3.Distance(transform.position, player.position);

        if (distBetweenEnemyAndPlayer <= sightRange)
            inSightRange = true;
        else
            inSightRange = false;

        if (distBetweenEnemyAndPlayer <= attackRange)
            inAttackRange = true;
        else
            inAttackRange = false;

        if (!inSightRange && !inAttackRange)
        {
          Patroling();
        }
        
        if(inSightRange && !inAttackRange)
        {
            ChasePlayer();
        }

        if (inSightRange && inAttackRange)
        {
            AttackPlayer();
        }
    }
}
