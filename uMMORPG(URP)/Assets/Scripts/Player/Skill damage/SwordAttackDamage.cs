using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackDamage : MonoBehaviour
{
    public ParticleSystem skill;
    public LayerMask EnemyLayer;
    public float damageAmount = 30f;
    GameObject enemyGameObject;
    public List<GameObject> enemyGameObjects = new List<GameObject>();
    public List<EnemyAi> enemies = new List<EnemyAi>();

    public bool isInCollider;

    Collider enemyCollider;

    public GameObject targetMarkerPosition;
    public GameObject originalTransform;

    public Collider thisGameObjectCollider;

    int index;
    //public BoxCollider damageArea;

    // Start is called before the first frame update
    void Start()
    {
        skill = GetComponent<ParticleSystem>();
        var collision = skill.collision;
        collision.collidesWith = EnemyLayer;
        // enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyAi>();
        // enemyCollider = enemy.GetComponent<CapsuleCollider>();
        thisGameObjectCollider = this.gameObject.GetComponent<CapsuleCollider>();
        enemyGameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        

        Collider[] allColliders = FindObjectsOfType<Collider>();
        foreach (Collider col in allColliders)
        {
            if (col != enemyCollider)
            {
                Physics.IgnoreCollision(col, thisGameObjectCollider);
            }
        }
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            enemies.Add(enemy.GetComponent<EnemyAi>());
        }

        
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        thisGameObjectCollider.transform.position = targetMarkerPosition.transform.position;
        
  
        
    }

     private void OnParticleCollision(GameObject other)
    {
        //Debug.Log("OnParticleCollision called with: " + other.name);
        
        
        if(other.CompareTag("Terrain"))
        {
            //BUG DITO NADADAMAGE LAHAT NG ENEMY
            
              
            foreach (var enemy in enemyGameObjects)
            {
                if (enemy.GetComponent<EnemyAi>().isInCollider)
                {
                    enemy.GetComponent<EnemyAi>().takeDamage(30);
                }
            }                
                    
            

            
                        
        }
            
        
    }
    //HINDI NANAMAN TO GUMAGANA PUTANGINA
    void OnTriggerEnter(Collider other)
    {
        foreach (var enemy in enemyGameObjects)
        {

            if (enemy.gameObject == other.gameObject)
            {
                if(other.gameObject.CompareTag("Enemy"))
                {
                    enemy.GetComponent<EnemyAi>().isInCollider = true;
                    Debug.Log("Enemy entered collider");

                }
                
            }
        }


    }

        
        
        
       
        
    
    void OnTriggerExit(Collider other)
    {
        if (other != null && enemyGameObjects.Contains(other.gameObject))
        {
            isInCollider = false;
            Debug.Log("Enemy exits collider");

        }
    }

}
//COMPARE TAG HINDI PA NAGANA 
//NADADAMAGE LAHAT NG ENEMY
