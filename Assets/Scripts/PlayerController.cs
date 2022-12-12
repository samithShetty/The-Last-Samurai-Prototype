using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int maxHealth;

    private Rigidbody rb;
    private Animator anim;
    public SwordController sc;

    private Vector2 move;
    private int health;
    private bool CanAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        health = maxHealth;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        Look(); 
    }


    private void MovePlayer()
    {
        rb.velocity = (Quaternion.Euler(0, 45, 0) * new Vector3(move.x, 0f, move.y / Mathf.Sin(Mathf.PI/6))) * moveSpeed;
    }

    private void Look()
    {
        if (!IsMoving()) { // If standing still, orient player to face mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Camera.main.farClipPlane, 1 << 6)) {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            } 

            anim.SetBool("isMoving", false);
        }
        else { // Otherwise, face move direction
            transform.LookAt(transform.position + rb.velocity);
            anim.SetBool("isMoving", true);

        }
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnAttack()
    {
        sc.Attack();
    }

    public void OnParry()
    {
        sc.Parry();
    }

    public bool IsMoving() 
    {
        return move != Vector2.zero;
    }
}
