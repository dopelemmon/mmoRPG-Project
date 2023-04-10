using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public GameObject mainPlayer;

    public PlayerStatsAndAttributes playerStats;

    public LayerMask whatIsGround, whatIsPlayer;

    //Animation
    public Animator animator;

    bool isWalking;
    bool isAttacking;
    bool isRunning;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackrange;

    ///Enemy Stats
    public float enemyHealth;
    public float enemyArmor;

    int attackSelector;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerStats = mainPlayer.GetComponent<PlayerStatsAndAttributes>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackrange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackrange)Patrolling();
        if(playerInSightRange && !playerInAttackrange)ChasePlayer();
        if(playerInSightRange && playerInAttackrange)AttackPlayer();

        
    }

    private void Patrolling()
    {
        
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", false);
        if(!walkPointSet) SearchWalkPoint();

        if(!isWalking)
        {
            isWalking = true;
            
        }
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetBool("isWalking",true);
        }
        


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walk Point Reached
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        isAttacking = false;
        animator.SetBool("isRunning", true);
        agent.SetDestination(player.position);
        animator.SetBool("isAttacking", false);
    }

    private void AttackPlayer()
    {
        animator.SetBool("isRunning",false);
        animator.SetBool("isWalking", false);
        
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        animator.SetBool("isAttacking", true);
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            attackSelector = Random.Range(0, 3);
            animator.SetInteger("Attack Selector", attackSelector);
            if(attackSelector != 0)
            {
                playerStats.takeDamage(25f);
            }

        }


    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        
        
        isAttacking = true;
        
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public void takeDamage(float damageAmmount)
    {
        if(enemyArmor < 0.0f)
        {
            enemyHealth -= damageAmmount;
        
        }
        else{
            enemyArmor -= damageAmmount;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        takeDamage(10);
    }
}
