using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Code reference https://www.youtube.com/watch?v=UjkSFoLxesw

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health = 100f;

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
        if(distanceToWalkPoint.magnitude < 1f) //if distance is less than 1, walkpoint reached
        {
            walkPointSet = false; 
        }
            
    }

    private void ChasePlayer()
    {
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
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

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
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void LookForWalkPoint()
    {
        //calculating random point
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;

        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checking if the player is in attack or sight range
        inSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


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
