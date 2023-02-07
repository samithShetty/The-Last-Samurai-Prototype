using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private int maxHealth;
    [SerializeField] private CameraController camController;

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
        rb.velocity = Quaternion.Euler(0,camController.GetAngle(),0) * new Vector3(move.x,0,move.y).normalized * moveSpeed; // Rotate move input by camera
        anim.SetBool("isMoving", IsMoving());
    }

    private void Look()
    {
        playerTransform.LookAt(playerTransform.position + rb.velocity);
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        camController.OnLook(value);
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
