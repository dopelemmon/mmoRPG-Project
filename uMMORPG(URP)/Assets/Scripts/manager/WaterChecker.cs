using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterChecker : MonoBehaviour
{
    public GameObject playerMovementController;
    // Start is called before the first frame update
    void Start()
    {
        playerMovementController = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        other = playerMovementController.GetComponent<CapsuleCollider>();
        
        Debug.Log("You cannot walk here! This is water");
        
    }
}
