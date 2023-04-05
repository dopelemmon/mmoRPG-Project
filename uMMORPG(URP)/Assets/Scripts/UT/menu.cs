using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    // Start is called before the first frame update
   public rope menurope;
    Animator hand;
    bool start = false;

    void Start()
    {
        hand = GameObject.Find("Hand").GetComponent<Animator>();
        hand.enabled = false;
        Invoke("startanim", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!start)
        {
            if (menurope.line == true)
            {
                start = true;
                Invoke("nextlevel", 1f);

            }
        }
       
    }

    void nextlevel()
    {
        gamemanager.Instance.Menu_Next();

    }
    void startanim()
    {
        hand.enabled = true;

    }
}
