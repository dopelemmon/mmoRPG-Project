using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    Animator animator;

    FrontAttack frontAttack;

    public Transform target;

    bool isCasting1stSpell,isCasting2ndSpell, isCasting3rdSpell, isCasting4thSpell, isCasting5thSpell, isCasting6thSpell;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        frontAttack = GetComponentInChildren<FrontAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        isCasting1stSpell = Input.GetKeyDown(KeyCode.Alpha1);
        isCasting2ndSpell = Input.GetKeyDown(KeyCode.Alpha2);
        isCasting3rdSpell = Input.GetKeyDown(KeyCode.Alpha3);
        isCasting4thSpell = Input.GetKeyDown(KeyCode.Alpha4);
        isCasting5thSpell = Input.GetKeyDown(KeyCode.Alpha5);
        isCasting6thSpell = Input.GetKeyDown(KeyCode.Alpha6);

        if(isCasting1stSpell)
        {
            animator.Play("attack1");
        }
        if(isCasting2ndSpell)
        {
            animator.Play("attack2");
        }
        if(isCasting3rdSpell)
        {
            animator.Play("attack3");
        }
        if(isCasting4thSpell)
        {
            animator.Play("attack4");
        }
        if(isCasting5thSpell)
        {
            animator.Play("attack5");
        }
        if(isCasting6thSpell)
        {
            animator.Play("attack6");
        }
        
    }

    void Cast3rdSpell()
    {
        
        frontAttack.PrepeareAttack(target.position);
    }
}
