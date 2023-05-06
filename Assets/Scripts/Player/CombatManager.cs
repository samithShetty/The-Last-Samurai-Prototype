using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Transform footTransform;
    //public bool isPlayer;
    [SerializeField] private float kickSize;
    [SerializeField] private float parrySize;
    [SerializeField] private int damage;
    [SerializeField] private float recoveryTime; //Cooldown on swing after being parried
    [SerializeField] private float parryInvulnerabilityTime; //Seconds of player invulnerability after successfully parrying
    [SerializeField] private ParticleSystem particleSystem;
    private bool canAttack = true;
    private Animator anim;
    public bool isKicking;
    public int blockState; // Keeps track of current defense state: 2 = Parry, 1 = Blocking, 0 = Not Blocking 
    public bool isInvuln; //Keeps track of player invulnerability after parries/kills

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update() 
    {
        if (isKicking) {
            Kick();
        }
    }

    public void Attack()
    {
        if (canAttack) {
            anim.SetTrigger("attack");
        }
    }

    public void startKick(){
        isKicking = true;
    }

    public void endKick(){
        isKicking = false;
    }

    public void startParrying(){
        blockState = 2;
    }
    
    public void endParry(){
        blockState = 1;
    }

    public void endBlock(){
        blockState = 0;
    }

    public void Kick() 
    {
        Collider[] hitColliders = Physics.OverlapSphere(footTransform.position, kickSize);
        foreach (Collider collider in hitColliders) 
        {
            if (collider.tag == "Enemy") {
                kickEnemy(collider.gameObject);
            }
        }
    }


    private void OnDrawGizmosSelected() { //Draws Kick + Parry Hitbox when GameObject is Selected
        // Kick
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (footTransform.position, kickSize);

        // Parry 
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere (footTransform.position, parrySize);

    }

    private void kickEnemy(GameObject enemy) 
    {
        enemy.GetComponent<EnemyController>().Die();
    }

    public bool IsAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Standing Kick") || anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Running Spin Kick");
    }

    public void Parry()
    {
        anim.SetTrigger("parry");
    }

    public bool IsParrying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Parry");
    }

    public void ParrySuccess() 
    {
        anim.SetTrigger("idle");
        StartCoroutine(PlayerInvulnerability(parryInvulnerabilityTime));
        particleSystem.Play();

    }

    public void BlockSuccess() 
    {
        anim.SetTrigger("idle");
    }

    IEnumerator PlayerInvulnerability(float invulnTime) 
    {
        isInvuln = true;
        yield return new WaitForSeconds(invulnTime);
        isInvuln = false;

    }
}
