using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// using Photon.Realtime;​

public class PlayerController : MonoBehaviourPun
{

    public float moveSpeed;
    //public Rigidbody theRB;
    public float jumpForce;
    private bool isMoving;

    private Vector3 moveDirection;

    public CharacterController controller;

    public Transform relativeTransform;
    private Vector3 vector_blue;
    private Vector3 vector_red;

    public bool fixedCamera = false;

    public GameController gameController;

    private Animator animator;

    public float directionDampTime = 0.4f;
    
    public float timePowerSpeed = 0f;
    public float timePowerSize = 0f;

    private bool powerSpeed = false;
    private bool powerSize = false;

    private float timerVelocity = 0f;
    private float timerScale = 0f;

    private bool respawnVelocity = false;
    private bool respawnScale = false;

    private bool playerJump;
    private float jumpTimer = 0;
    private float catchTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator) {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }

        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent <GameController>();
        }
        if (gameController == null) {
            Debug.Log ("Cannot find 'GameController' script");
        }

        moveSpeed = 1;
        jumpForce = 7;
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        moveDirection = new Vector3(0,0,0);
        playerJump = false;

        //vector_blue = relativeTransform.forward;
        //vector_red = relativeTransform.right;

         //animator.Play("Idle");
        // animator.SetFloat("Speed", 5f);
    }

    // Update is called once per frame
    void Update() {

        // Only update animation if we're the local player
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
           return;
        }

        // Respawn powerUps
        if (respawnVelocity) {
            timerVelocity -= Time.deltaTime;
            if (timerVelocity <= 0) {
                gameController.generateVelocity();
                respawnVelocity = false;
            }
        }

        if (respawnScale) {
            timerScale -= Time.deltaTime;
            if (timerScale <= 0) {
                gameController.generateScale();
                respawnScale = false;
            }
        }

        // Get user input (W,A,S,D)
        //  Horizontal -S, +W
        //  Vertical -A, +D
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Deactivate running backwards
        if (v <= 0.1) {
            v = 0;
            h = 0;
        }

        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running Jump")) {
        //     Debug.Log("Animator on <Running Jump>");
        // }
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slow down")) {
        //     Debug.Log("Animator on <Slow down>");
        // }
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
        //     Debug.Log("Animator on <Run>");
        // }

        jumpTimer -= Time.deltaTime;
        catchTimer -= Time.deltaTime;

        if (jumpTimer < 0 && jumpTimer > -10) {
            jumpTimer = -20f;
            Debug.Log("End Jump");
            animator.SetBool("Jumping", false);    
        }

        if (catchTimer < 0 && catchTimer > -10) {
            catchTimer = -20f;
            Debug.Log("End Catch");
            animator.SetBool("BallCatch", false);
        }

        // If is grounded...
        if (Physics.Raycast(transform.position, -Vector3.up, 1f)) {  
            
            if (Input.GetKeyDown("space")) {
                moveDirection.y = jumpForce;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                    Debug.Log("Idle");
                    jumpTimer = .6f;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
                    Debug.Log("Run");
                    jumpTimer = 1.1f;                    
                }
                animator.SetBool("Jumping", true);

                Debug.Log("Spacebar pressed");
            }

            if (Input.GetKeyDown("c")) {
                moveDirection.y = jumpForce;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                    Debug.Log("Idle");
                    catchTimer = 1f;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
                    Debug.Log("Run");
                    catchTimer = 1.2f;                    
                }
                animator.SetBool("BallCatch", true);

                Debug.Log("C pressed");
            }
        }

        animator.SetFloat("Speed", v * moveSpeed);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        // moveDirection = new Vector3(h, moveDirection.y + (Physics.gravity.y * Time.deltaTime), v);
        moveDirection.y = moveDirection.y + (Physics.gravity.y * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

        if (powerSpeed) {
            if (timePowerSpeed > 0) timePowerSpeed -= Time.deltaTime;
            else {
                this.moveSpeed = this.moveSpeed / 2;
                powerSpeed = false;
            }
        }

        if (powerSize) {
            if (timePowerSize > 0)  timePowerSize -= Time.deltaTime;
            else {
                this.transform.localScale -= new Vector3(3F, 3F, 3F);
                powerSize = false;
            } 
        }
        
        

        //if (Input.GetKey(KeyCode.W)){
            // float v = Input.GetAxis("Vertical");
            //
            // if (v > 0) animator.SetFloat("Speed", moveSpeed);
            // else {
            //     //animator.CrossFade("Idle", 0.05f);
            //     animator.SetFloat("Speed", 0);
            // }

            // if (v <= 0) {
            //     v = 0;
            //     animator.CrossFade("Idle", 0.15f);
            // }

        //}
        //else animator.SetFloat("Speed", 0);

        /* if (!fixedCamera && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) {
            //float h = Input.GetAxis("Horizontal");
            //animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
        else animator.SetFloat("Direction", 0, directionDampTime, Time.deltaTime);
        */
        /*if (Input.GetMouseButtonDown(1)) fixedCamera = true;

        if (Input.GetMouseButtonUp(1)){
            fixedCamera = false;
            // vector_blue = relativeTransform.forward;
            // vector_red = relativeTransform.right;
        }

        /*if (fixedCamera){
          if(Input.GetKey(KeyCode.W)){
              moveDirection = relativeTransform.forward * moveSpeed;
              isMoving = true;
          }
          if(Input.GetKey(KeyCode.S)) {
              moveDirection = -relativeTransform.forward * moveSpeed;
              isMoving = true;
          }
          if(Input.GetKey(KeyCode.A)) {
              moveDirection = -relativeTransform.right * moveSpeed;
              isMoving = true;
          }
          if(Input.GetKey(KeyCode.D)) {
              moveDirection = relativeTransform.right * moveSpeed;
              isMoving = true;
          }
        }
        else {
          //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);
          if(Input.GetKey(KeyCode.W)) {
              moveDirection = vector_blue * moveSpeed;
              isMoving = true;
          }
          if(Input.GetKey(KeyCode.S)) {
              moveDirection = -vector_blue * moveSpeed;
              isMoving = true;
          }
          if(Input.GetKey(KeyCode.A)) {
              moveDirection = -vector_red * moveSpeed;
              isMoving = true;
          }
          if(Input.GetKey(KeyCode.D)) {
              moveDirection = vector_red * moveSpeed;
              isMoving = true;
          }
        }

        //moveDirection.y = 0f;

        if (controller.isGrounded) {

            moveDirection.y = 0f;

            if (Input.GetButtonDown("Jump")){

                gameController.increaseGoalP1();

                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * Time.deltaTime);*/

        //controller.Move(moveDirection * Time.deltaTime);
        //if (isMoving) {
            //controller.Move(moveDirection * Time.deltaTime);
            //animator.SetFloat("Speed", moveSpeed);
            //float h = Input.GetAxis("Horizontal");
            //animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
            //controller.Move(moveDirection * Time.deltaTime);
        //}
        //else animator.SetFloat("Speed", 0);

        //animator.SetFloat("Direction", moveDirection.right, directionDampTime, Time.deltaTime);
    }

    void LateUpdate() {

    }

    private void OnTriggerEnter(Collider colidedObj) {

        Debug.Log ("Colision detectada con" + colidedObj.name);

        if (colidedObj.name == "Boots") {
            
            timePowerSpeed = 10.0f;
            powerSpeed = true;

            this.moveSpeed = this.moveSpeed * 2;

            GameObject velocity = GameObject.Find ("Velocity Wrapper(Clone)");

            if (velocity == null)
            {
                Debug.Log ("Cannot find 'Velocity' object");
            }
            else {
                Destroy(velocity);
                timerVelocity = 30.0f;
                respawnVelocity = true;
            }
        }

        if (colidedObj.name == "Mushroom"){

            timePowerSize = 10.0f;
            powerSize = true;

            this.transform.localScale += new Vector3(3F, 3F, 3F);

            GameObject scale = GameObject.Find ("Scale Wrapper(Clone)");

            if (scale == null)
            {
                Debug.Log ("Cannot find 'Scale' object");
            }
            else {
                Destroy(scale);
                timerScale = 30.0f;
                respawnScale = true;
            }
        }
    }
}
