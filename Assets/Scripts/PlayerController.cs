using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dirAirJumpForce;
    [SerializeField] private int maxHealth;
    [SerializeField] private CameraController camController;

    private Transform playerTransform;
    public Rigidbody rb;
    private Animator anim;
    public SwordController sc;
    private SphereCollider groundDetector;

    public Vector2 moveInput;
    private int health;
    private bool CanAttack = true;
    private int groundCounter;
    private bool hasAirJump = true;

    void Awake()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsGrounded()) { 
            MovePlayer();
        }
    }

    private void Look() {
        //playerTransform.eulerAngles = new Vector3(0,camController.GetAngle(), 0);
    }

    private void MovePlayer() 
    {
        playerTransform.Rotate(new Vector3(0,moveInput.x*turnSpeed,0));
        rb.velocity = Quaternion.Euler(0,moveInput.x*turnSpeed,0) * rb.velocity;

        if (rb.velocity.magnitude < maxSpeed) {
            rb.velocity += playerTransform.forward*moveInput.y*moveSpeed;
        }
        anim.SetBool("isMoving", IsMoving());
    }

    public bool IsMoving() 
    {
        return moveInput != Vector2.zero;
    }

    public void TryJump() {
        if (IsGrounded()) {
            Jump(Vector2.zero);
        } else if (hasAirJump) {
            Jump(moveInput);
            hasAirJump = false;
        }
    }

    private void Jump(Vector2 directionalInput) {
        Vector3 jumpVel = rb.velocity;
        jumpVel.y = jumpForce;
        if (directionalInput != Vector2.zero) {
            jumpVel = Quaternion.Euler(0,directionalInput.x*dirAirJumpForce,0) * jumpVel;
        }

        rb.velocity = jumpVel;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ground") {
            ++groundCounter;
            hasAirJump = true;
        }  
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Ground") {
            --groundCounter;
        }
    }

    private bool IsGrounded() {
        return groundCounter > 0;
    }

    public void Attack()
    {
        sc.Attack();
    }

    public void Parry()
    {
        sc.Parry();
    }



    ///<summary>
    /// Adds value to the player's current health and updates healthbar. Pass in negative value to deal damage. 
    ///</summary>
    public void UpdateHealth(int value) {
        health = Mathf.Min(health + value, maxHealth);
        if (health <=0) {
            print("Player Died!");
            health = 100;
        }
        GUIManager.instance.UpdateHealthBar((float)health/maxHealth);
    }
}
