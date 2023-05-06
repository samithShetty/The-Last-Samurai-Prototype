using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float recoveryTime; //Cooldown on swing after being parried
    private Collider swordCollider;
    private Animator enemyAnim;
    private bool canAttack = true;

    private void Start() {
        swordCollider = GetComponent<MeshCollider>();
        enemyAnim = GetComponentInParent<Animator>();
    }
    public bool IsAttacking()
    {
        return enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Sword Attack 1");
    }

    public bool IsParrying()
    {
        return enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Parry");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            //Check if the player is Parrying/Blocking in the right direction
            float enemyAngle = Vector3.Angle(enemyAnim.gameObject.transform.position - other.transform.position, other.transform.forward); // Angle between enemy and Player forward direction
            CombatManager cm = other.gameObject.GetComponent<CombatManager>();

            if (cm.blockState == 2 && enemyAngle < 30) { //Parry
                parriedByPlayer();
                cm.ParrySuccess();
            } else if (cm.blockState == 1 && enemyAngle < 30 ){
                blockedByPlayer();
                cm.BlockSuccess();
            } else if (!cm.isInvuln) {
                other.GetComponent<PlayerController>().UpdateHealth(-damage);
            }


        }    
    }

    public void parriedByPlayer()
    {
        print("Parry");
        enemyAnim.SetTrigger("parried");
    }

    public void blockedByPlayer()
    {
        print("blocked");
        enemyAnim.SetTrigger("blocked");
    }

    public void activateHitbox() {
        swordCollider.enabled = true;
    }
    public void deactivateHitbox() {
        swordCollider.enabled = false;
    }
}
