using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionRadius;
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
    private bool aggro;
    private bool circling;

    
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
        float playerDistance = GetPlayerDistance();
        if (playerDistance < attackRadius) {
            anim.SetBool("isMoving", false);
            if (CanAttack)
            {
                agent.destination = player.transform.position;
                trans.LookAt(player.transform.position);
                Attack();
                CanAttack = false;
                StartCoroutine(ResetAttackCooldown());
            }
        } else if (playerDistance < circlingRadius && !aggro) 
        {
            trans.LookAt(player.transform.position);
            trans.Rotate(new Vector3(0,80,0));
            agent.destination = trans.position + trans.forward*circlingStepSize;
            
            if (!circling) {
                StartCoroutine(GoAggro());
                circling = true;
            }
            anim.SetBool("isMoving", true);
        } else if (playerDistance < detectionRadius) {
            circling = false;
            anim.SetBool("isMoving", true);
            ChasePlayer();
        } else {
            StartCoroutine(Roam());
        }
        
    }

    private void ChasePlayer() 
    {
        agent.destination = player.transform.position;
        if (pc.IsMoving())
        {
            agent.destination += player.transform.forward * GetPlayerDistance() * 0.8f;
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

    IEnumerator GoAggro() 
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        aggro = true;
    }

    IEnumerator Roam() 
    {
        Vector2 random = Random.insideUnitCircle * 10;
        agent.destination = new Vector3(random.x, trans.position.y,random.y);
        yield return new WaitForSeconds(Random.Range(3f, 6f));

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
