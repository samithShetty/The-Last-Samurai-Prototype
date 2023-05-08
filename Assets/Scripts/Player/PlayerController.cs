using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathCreation;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dirAirJumpForce;

    [Header("Grapple")]
    [SerializeField] private float grappleRange;
    [SerializeField] private float grappleStrength;
    private bool grapple = false;
    private Vector3 grapplePoint;
    
    [Header("Player Specs")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float maxSpeedBonus;

    [Header("Scripts/Objects")]
    [SerializeField] private CameraController camController;
    [SerializeField] private GUIManager gui;
    public Transform playerTransform;
    public Rigidbody rb;
    private Animator anim;
    public CombatManager combatManager;
    private SphereCollider groundDetector;
    private GrindController gc;
    public PathCreator nearestRail;
    private Transform camTransform;
    [SerializeField] private GameObject playerCorpsePrefab;

    public Vector2 moveInput;
    private int health;
    public float speedBonus; 
    private int groundCounter;
    private bool hasAirJump = true;
    bool grinding = false;
    

    void Awake()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        gc = GetComponent<GrindController>(); 
        health = maxHealth;
        camTransform = Camera.main.transform;
    }

    void FixedUpdate()
    {
        UpdateSpeedBonus();
        if (grapple) {
            rb.velocity += (grapplePoint - playerTransform.position).normalized*grappleStrength;
            if (Vector3.Distance(playerTransform.position, grapplePoint) < 5f) {
                grapple = false;
            }
        }

        if (IsGrounded()) { 
            MovePlayer();
            anim.SetBool("isJumping", false);
        }
    }

    private void Look() {
        playerTransform.eulerAngles = new Vector3(0,camController.GetAngle(), 0);
    }

    private void MovePlayer() 
    {
        playerTransform.Rotate(new Vector3(0,moveInput.x*turnSpeed,0));
        rb.velocity = Quaternion.Euler(0,moveInput.x*turnSpeed,0) * rb.velocity;

        if (rb.velocity.magnitude < maxSpeed*(1+speedBonus)) {
            rb.velocity += playerTransform.forward*moveInput.y*moveSpeed;
        }
        anim.SetBool("isMoving", IsMoving());
    }

    public bool IsMoving() 
    {
        return moveInput != Vector2.zero;
    }

    public void TryJump() {
        if (grinding) {
            grinding = false;
            gc.enabled = false;
        }
        
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
        anim.SetBool("isJumping", true);
    }

    public void Grind() {
        if (nearestRail != null) {
            gc.pathCreator = nearestRail;
            gc.speed = rb.velocity.magnitude;
            gc.enabled = true;
            grinding = true;
            UpdateSpeedBonus(0.1f);
        }
    }

    public void Grapple() {
        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, grappleRange,~(1<<3))){
            grapple = true;
            grapplePoint = hit.point;
            Debug.DrawLine(playerTransform.position,hit.point,Color.red, 5f);

            //Kinematic Equation to Launch to Point
            float gravity = Physics.gravity.y;
            float displacementY = grapplePoint.y - playerTransform.position.y;
            Vector3 displacementXZ = new Vector3(grapplePoint.x - playerTransform.position.x, 0f, grapplePoint.z - playerTransform.position.z);
            print(displacementXZ);     
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * grappleStrength);
            print(velocityY);     

            Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * grappleStrength / gravity) 
                + Mathf.Sqrt(2 * (displacementY - grappleStrength) / gravity));  
            print(velocityXZ); 
            rb.velocity = velocityXZ + velocityY;
        }
    }

    public void EndGrapple() {
        grapple = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ground" || other.tag == "Grindable") {
            ++groundCounter;
            hasAirJump = true;
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.isTrigger && other.tag == "Grindable"){
            nearestRail = other.GetComponent<PathCreator>();
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Ground") {
            --groundCounter;
        } else if (other.tag == "Grindable"){
            --groundCounter;
            nearestRail = null;
        }
    }

    private bool IsGrounded() {
        return groundCounter > 0;
    }

    ///<summary>
    /// Adds value to the player's current health and updates healthbar. Pass in negative value to deal damage. 
    ///</summary>
    public void UpdateHealth(int value) {
        if (value < 0) {
            anim.SetTrigger("hit"); //Player is taking damage -> trigger the stagger animation
        }
        health = Mathf.Min(health + value, maxHealth);
        if (health <=0) {
            KillPlayer();
        }
        gui.UpdateHealthBar((float)health/maxHealth);
    }

    ///<summary>
    /// Adds value to the player's current speed bonus, or if no value is passed in (will happen in Fixed Update) it will decay. Also Updates the UI Speedbar
    ///</summary>
    public void UpdateSpeedBonus(float value = 0f) {
        if (value != 0f) {
            speedBonus += value;
        } else { // Decay towards 1
            speedBonus *= 0.999f;  
        }

        speedBonus = Mathf.Clamp(speedBonus, 0f, maxSpeedBonus);
        anim.speed = 1 + 0.25f*speedBonus;
        gui.UpdateSpeedBar(speedBonus/maxSpeedBonus);
    }

    void KillPlayer() // Replace player with corpse, then after a few seconds, change the scene
    {
        // Disable All EnemyController Scripts
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            enemy.GetComponent<EnemyController>().enabled = false;
        }

        GameObject corpse = Instantiate(playerCorpsePrefab, transform.position, Quaternion.identity);
        camController.targetTransform = corpse.transform; //Reassign Camera target to corpse 

        Destroy(gameObject);
    }

}
