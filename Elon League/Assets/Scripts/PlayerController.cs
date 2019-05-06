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
    
    private System.Random ran;

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

        ran = new System.Random();  

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
                
                GameObject velocity = GameObject.Find("Velocity Wrapper - PUN(Clone)");

                if (velocity == null) {
                    Debug.Log ("Cannot find 'Velocity' object");
                }
                else {
                    if (!velocity.GetComponent<PhotonView>().IsMine) {
                        velocity.GetComponent<PhotonView>().TransferOwnership( PhotonNetwork.LocalPlayer );
                    }
                    velocity.transform.position = new Vector3(ran.Next(105, 145), 4, ran.Next(105, 145));
                    velocity.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                
                respawnVelocity = false;
            }
        }

        if (respawnScale) {
            timerScale -= Time.deltaTime;
            if (timerScale <= 0) {

            GameObject scale = GameObject.Find ("Scale Wrapper - PUN(Clone)");

            if (scale == null) {
                Debug.Log ("Cannot find 'Scale' object");
            }
            else {
                if (!scale.GetComponent<PhotonView>().IsMine) {
                    scale.GetComponent<PhotonView>().TransferOwnership( PhotonNetwork.LocalPlayer );
                }
                scale.transform.position = new Vector3(ran.Next(105, 145), 4, ran.Next(105, 145));
                scale.transform.localScale = new Vector3(1f, 1f, 1f);
            }

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
                else {
                    Debug.Log("<Another>");
                    jumpTimer = .5f;                    
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
                else {
                    Debug.Log("<Another>");
                    catchTimer = .5f; 
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
    }

    void LateUpdate() {

    }

    private void OnTriggerEnter(Collider colidedObj) {

        Debug.Log ("Colision detectada con" + colidedObj.name);

        if (colidedObj.name == "Boots") {
            
            timePowerSpeed = 10f;
            powerSpeed = true;

            this.moveSpeed = this.moveSpeed * 2;

            GameObject velocity = GameObject.Find("Velocity Wrapper - PUN(Clone)");

            if (velocity == null) {
                Debug.Log ("Cannot find 'Velocity' object");
            }
            else {
                // Destroy(velocity);
                if (!velocity.GetComponent<PhotonView>().IsMine) {
                    velocity.GetComponent<PhotonView>().TransferOwnership( PhotonNetwork.LocalPlayer );
                }
                velocity.transform.localScale = new Vector3(0f, 0f, 0f);
                velocity.transform.position += Vector3.up * 20f;
                timerVelocity = 30f;
                respawnVelocity = true;
            }
        }

        if (colidedObj.name == "Mushroom"){

            timePowerSize = 10f;
            powerSize = true;

            this.transform.localScale += new Vector3(3F, 3F, 3F);

            GameObject scale = GameObject.Find ("Scale Wrapper - PUN(Clone)");

            if (scale == null) {
                Debug.Log ("Cannot find 'Scale' object");
            }
            else {
                // Destroy(scale);
                // Debug.Log( scale.GetComponent<PhotonView>().IsMine );
                if (!scale.GetComponent<PhotonView>().IsMine) {
                    scale.GetComponent<PhotonView>().TransferOwnership( PhotonNetwork.LocalPlayer );
                }

                scale.transform.localScale = new Vector3(0f, 0f, 0f);
                scale.transform.position += Vector3.up * 20f;
                timerScale = 30f;
                respawnScale = true;
            }
        }
    }
}
