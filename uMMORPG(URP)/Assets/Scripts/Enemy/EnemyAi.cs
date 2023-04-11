using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    HS_MovementInput hS_MovementInput;

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
    public bool isDead;

    int attackSelector;
    int currentAttackMode;
    bool hasDealtDamage = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerStats = mainPlayer.GetComponent<PlayerStatsAndAttributes>();
        hS_MovementInput = mainPlayer.GetComponent<HS_MovementInput>();
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
        animator.SetInteger("Attack Selector", 0);
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

            Attack();


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
        
        if(enemyArmor <= 0.0f)
        {
            enemyHealth -= damageAmmount;
            animator.SetTrigger("isGettingHit");
            if(enemyHealth < damageAmmount || enemyHealth < 0f) // checking if the damage ammount is greater than current enemyhealth
            {
                enemyHealth = 0; // setting the health ammount to 0
                

            }
            if(enemyHealth == 0)
            {
                StartCoroutine(Dead(gameObject));
                isDead = true;
                hS_MovementInput.screenTargets.Remove(gameObject);
            }
        
        }
        else {
            enemyArmor -= damageAmmount;
            animator.SetTrigger("isGettingHit");
            if(enemyArmor < damageAmmount)
            {
                enemyArmor = 0f;
            }
        }

        

    }
    // animation attack 01 damage dealer
    public void Attack01(float damageAmmount)
    {
        if(playerStats.playerArmor <=0f)
        {
            playerStats.playerHealth -= damageAmmount;
        }
        else{
            playerStats.playerArmor -= damageAmmount;
        }
    }
    // animation attack 02 damage dealer
    public void Attack02(float damageAmmount)
    {
        if(playerStats.playerArmor <=0f)
        {
            playerStats.playerHealth -= damageAmmount;
        }
        else{
            playerStats.playerArmor -= damageAmmount;
        }
    }
    public void Attack()
    {
        attackSelector = Random.Range(1, 10);
        animator.SetInteger("Attack Selector", attackSelector);
    }

    IEnumerator Dead(GameObject gameObject)
    {
        hS_MovementInput.activeTarger = false;
        animator.SetBool("isDead", true);
        float sinkGameObject = gameObject.transform.position.y;
        yield return new WaitForSeconds(2f);
        sinkGameObject -= 2 * Time.deltaTime;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        Destroy(gameObject);

    }


        
        

        
}

