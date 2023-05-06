using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float circlingRadius;
    [SerializeField] private float circlingStepSize;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject enemyCorpsePrefab;
    [SerializeField] private SwordController swordController;

    private Transform trans;
    private Rigidbody rb;
    public Animator anim;
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerController pc;

    private Vector3 velocity;
    private bool CanAttack = true;

    
    void Start()
    {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); 
        agent.stoppingDistance = attackRadius;
        agent.speed = moveSpeed;

        player = GameObject.Find("Player"); 
        pc = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GetPlayerDistance() < attackRadius) {
            anim.SetBool("isMoving", false);
            if (CanAttack)
            {
                agent.destination = player.transform.position;
                trans.LookAt(player.transform.position);
                Attack();
                CanAttack = false;
                StartCoroutine(ResetAttackCooldown());
            }
        } else if (GetPlayerDistance() < circlingRadius) 
        {
            trans.LookAt(player.transform.position);
            trans.Rotate(new Vector3(0,80,0));
            agent.destination = trans.position + trans.forward*circlingStepSize;
            anim.SetBool("isMoving", true);
        } else
        {
            anim.SetBool("isMoving", true);
            ChasePlayer();
        }
        
    }

    private void ChasePlayer() 
    {
        agent.destination = player.transform.position;
        if (pc.IsMoving())
        {
            agent.destination += player.transform.forward * GetPlayerDistance();
        }
    }

    private float GetPlayerDistance()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    IEnumerator ResetAttackCooldown() 
    {
        yield return new WaitForSeconds(attackCooldown);
        CanAttack = true;
    }

    public void Die() 
    {
        Destroy(gameObject);
        Instantiate(enemyCorpsePrefab, transform.position, Quaternion.identity);
    }

    private void Attack() 
    {
        anim.SetBool("attack", true);
    }

    public void activateSword(){
        swordController.activateHitbox();
    }
    public void deactivateSword(){
        swordController.deactivateHitbox();
    }
}
