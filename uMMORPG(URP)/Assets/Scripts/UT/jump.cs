using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    public int force;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject)
        {
            print(collision.gameObject.name);
            Rigidbody2D collsrigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            collsrigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        }
     

    }
}
