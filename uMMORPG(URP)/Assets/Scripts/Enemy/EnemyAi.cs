using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
    [Header("Behavior")]
    public bool isCampingBehavior;
    public NavMeshAgent agent;
    public GameObject player;
    public Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;

    //*****************CAMPING BEHAVIOR ********************
    public Transform campLocation;
    public Vector3 randomCampPosition;
    bool campingWalkPointSet;
    public float campingWalkPointRange;

    //*************PLAYER STATS**************
    PlayerStatsAndAttributes playerStats;

    //****************** ANIMATION ********************
    //HASH VARIABLES (TO BE CONVERTED TO BOOL)
    int isWalkingHash;
    int isRunningHash;
    int isAttackingHash;
    int isDeadHash;
    int attackSelectorHash;

    int attackSelector;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

     void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator.GetComponent<Animator>();
        playerStats = player.GetComponent<PlayerStatsAndAttributes>();
    
    }

     void Update()
    {
        
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isDeadHash = Animator.StringToHash("isDead");
        attackSelectorHash = Animator.StringToHash("Attack Selector");

        

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!isCampingBehavior && !playerInSightRange && !playerInAttackRange)Patroling();
        if(isCampingBehavior && !playerInSightRange && !playerInAttackRange)Camping();
        if(playerInSightRange && !playerInAttackRange)ChasePlayer();
        if(playerInSightRange && playerInAttackRange)AttackPlayer();

       //SETTING LOCAL VARIABLE TO ANIMATOR PARAMETERS
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isAttacking = animator.GetBool(isAttackingHash);
        bool isDead = animator.GetBool(isDeadHash);
        
        
    }
     void Patroling()
    {
        
        animator.SetBool(isWalkingHash, true); //plays walking animation
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            
        }

        Vector3 distanceTowalkPoint = transform.position - walkPoint;

        //Walkpoint reached!
        if(distanceTowalkPoint.magnitude < 5)
        {
            walkPointSet = false;
        }
    }
    void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround));
        {
            walkPointSet = true;
        }
    }

    void Camping()
    {
        animator.SetBool(isWalkingHash, true);
        animator.SetBool(isRunningHash, false);
        
        float randomX = Random.Range(-campingWalkPointRange, campingWalkPointRange);
        float randomZ = Random.Range(-campingWalkPointRange, campingWalkPointRange);

        randomCampPosition = new Vector3(campLocation.position.x + randomX, campLocation.position.y, campLocation.position.z + randomZ);

        Vector3 distancetoPosition = transform.position - randomCampPosition;

        if(Physics.Raycast(randomCampPosition, -transform.up, whatIsGround))
        {
            agent.SetDestination(randomCampPosition);
            if(gameObject.transform.position.magnitude == 0)
            {
                animator.SetBool(isWalkingHash, false);
            }
        }

    }
    
     void ChasePlayer()
    {
        //CHANGE FROM WALKING ANIMATION TO RUNNING ANIMATION
        animator.SetBool(isRunningHash, true);
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isAttackingHash, false); //ENEMY WILL CHASE THE PLAYER SO ATTACK ANIMATION WILL NOT PLAY
        agent.SetDestination(player.transform.position);

    }
     void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);

        if(!alreadyAttacked)
        {
            //STOPS RUNNING ANIMATION AND STARTS ATTACKING ANIMATION
            animator.SetBool(isAttackingHash, true);
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isWalkingHash, false);
            attackSelector = (Random.Range(1, 11)); //RANDOMIZE RANGE WHAT ATTACK ANIMATION WILL PLAY
            animator.SetInteger(attackSelectorHash, attackSelector);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

     void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void Attack(float damage)
    {
        if(playerStats.playerArmor <= 0f)
        {
            playerStats.playerHealth -= damage;
            if(playerStats.playerHealth < 0f)
            {
                playerStats.playerHealth = 0f;
            }
        }
        else{
            playerStats.playerArmor -= damage;
            if(playerStats.playerArmor < 0f)
            {
                playerStats.playerArmor = 0f;
            }
        }
    }

    //Visualize Attack and Sight Range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(campLocation.position, campingWalkPointRange);
    }

}
