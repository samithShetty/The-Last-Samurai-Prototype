using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackCooldown;

    private Rigidbody rb;
    private Animator anim;
    private NavMeshAgent agent;
    public SwordController sc;
    private GameObject player;
    private PlayerController pc;

    private Vector3 velocity;
    private bool CanAttack = true;

    
    void Start()
    {
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
                sc.Attack();
                CanAttack = false;
                StartCoroutine(ResetAttackCooldown());
            }
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

}
