using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Animator anim;
    public bool isPlayer;
    public bool canAttack = true;
    public float recoveryTime = 2f; //Cooldown on swing after being parried

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (canAttack) {
            anim.SetTrigger("attack");
        }
    }

    public bool IsAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Base.attack");
    }

    public void Parry()
    {
        anim.SetTrigger("parry");
    }

    public bool IsParrying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Base.parry");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Sword" && other.GetComponent<SwordController>().IsParrying()) 
        {   
            Debug.Log("Parry!");
            anim.Play("Base.idle");
            StartCoroutine(ParryRecovery());
        } else if (other.tag == "Enemy" && IsAttacking()) 
        {
            Debug.Log("Enemy Hit");
            Destroy(other.gameObject);
        } else if (other.tag == "Player") {
            Debug.Log("PLAYER HIT" + Time.time.ToString());
        }    
    }

    IEnumerator ParryRecovery() 
    {
        canAttack = false;
        yield return new WaitForSeconds(recoveryTime);
        canAttack = true;
    }
}
