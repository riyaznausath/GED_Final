using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // plugin dll imports
    [DllImport("Plugin")]
    private static extern int GetID();

    [DllImport("Plugin")]
    private static extern void SetID(int id);

    [DllImport("Plugin")]
    private static extern Vector3 GetPosition();

    [DllImport("Plugin")]
    private static extern void SetPosition(float x, float y, float z);

    [DllImport("Plugin")]
    private static extern float SetJump();




    // Player controller variables
    GameControls inputAction;
    Vector2 move;
    Vector2 rotate;
    Rigidbody rb;

    // PLayer jump, walk and look around variables
    private float distanceToGround;
    bool isGrounded = true;
    public float jump = 5f;
    public float walkSpeed = 5f;
    public Camera playerCamera;
    Vector3 cameraRotation;
    PlayerSword sword;

    // Player animator variables
    private Animator animator;
    private bool isWalking = false;
    private bool isAttacking = false;
    [SerializeField] 
    private float animationFinishTime = 0.9f;

    private void Start() {

        // Input action for player movement
        inputAction = PlayerInputController.controller.inputAction;

        // Space to jump
        inputAction.Player.Jump.performed += cntxt => Jump();

        // WASD to move
        inputAction.Player.Move.performed += cntxt => move = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => move = Vector2.zero;

        // Use mouse to look around
        inputAction.Player.Look.performed += cntxt => rotate = cntxt.ReadValue<Vector2>();
        inputAction.Player.Look.canceled += cntxt => rotate = Vector2.zero;

        // LMB to attack
        inputAction.Player.Attack.performed += cntxt => Attack(); 

        // ESC to quit game     
        inputAction.Player.Quit.performed += cntxt => QuitGame();  

        // Player components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sword = GetComponentInChildren<PlayerSword>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;

        // Main camera rotation values
        cameraRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);       
    }

    private void Jump()
    {
        // If player is on the ground, player can jump
        if(isGrounded)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {

                rb.velocity = new Vector2(rb.velocity.x, SetJump());
                isGrounded = false;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                isGrounded = false;
            }

           
        }

    }

    private void Attack()
    {
        // If player is not attacking, player can initialize the attack
        if(!isAttacking)
        {
            animator.SetTrigger("isAttacking");
            StartCoroutine(InitialiseAttack());
        }
    }

    IEnumerator InitialiseAttack()
    {
        yield return new WaitForSeconds(.1f);
        sword.OnAttack();
        isAttacking = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Camera and player rotation with mouse
        cameraRotation = new Vector3(cameraRotation.x + rotate.y, cameraRotation.y + rotate.x, cameraRotation.z);
        
        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        transform.eulerAngles = new Vector3(transform.rotation.x, cameraRotation.y, transform.rotation.z);

        transform.Translate(Vector3.right * Time.deltaTime * move.x * walkSpeed, Space.Self);
        transform.Translate(Vector3.forward * Time.deltaTime * move.y * walkSpeed, Space.Self);

        // Checking if player is on ground
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
        // Drawing the raycast
        Debug.DrawRay(transform.position, -Vector3.up * distanceToGround, Color.red);

        // Updating player's position on x and z axis according to the input
        Vector3 m = new Vector3(move.x, 0, move.y);
        // Trigger walk animation
        AnimateRun(m);

        // Checking if player is attacking
        if(isAttacking && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= animationFinishTime)
        {
            isAttacking = false;
        }

        // Check if player is falling down
        if(rb.velocity.y < -10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Player's walking animation
    void AnimateRun(Vector3 m)
    {
        isWalking = (m.x > 0.1f || m.x < -0.1f) || (m.z > 0.1f || m.z < -0.1f) ? true : false;
        animator.SetBool("isWalking", isWalking);
    }

    // Quit the game
    void QuitGame()
    {
        Application.Quit();
    }
}
