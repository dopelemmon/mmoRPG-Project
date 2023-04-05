using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parade : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;
   public  bool turnr,turnl;
    void Start()
    {
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!turnr&&turnl)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if(!turnl&&turnr)
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "turn")
        {
            turnr = false;
            turnl = true;
        }
           
            if (collision.gameObject.tag == "turnl")
        {
            turnl = false;
            turnr = true;
        }
              

    }
}
