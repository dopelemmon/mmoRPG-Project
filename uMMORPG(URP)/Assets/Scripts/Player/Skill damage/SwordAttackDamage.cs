using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackDamage : MonoBehaviour
{
    public ParticleSystem skill;
    public LayerMask EnemyLayer;
    public float damageAmount = 30f;
    
    public List<GameObject> enemyGameObjects = new List<GameObject>();
    public List<EnemyAi> enemies = new List<EnemyAi>();

    public bool isInCollider;

    Collider enemyCollider;

    public GameObject targetMarkerPosition;
    public GameObject originalTransform;

    public Collider thisGameObjectCollider;
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
        enemyGameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        

        
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
            if(isInCollider)
            {   
                foreach (var enemy in enemies)
                {
                    enemy.takeDamage(30);
                }                
                    
            }
            else{
                
            }
            
                        
        }
            
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isInCollider = true;
            Debug.Log("Enemy entered collider");
        }
    }

        
        
        
       
        
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isInCollider = true;
            Debug.Log("Enemy entered collider");
        }
    

    }

}
//COMPARE TAG HINDI PA NAGANA 