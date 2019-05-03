using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator) {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }

        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent <GameController>();
        }
        if (gameController == null)
        {
            Debug.Log ("Cannot find 'GameController' script");
        }

        moveSpeed = 1;
        jumpForce = 5;
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        moveDirection = new Vector3(0,0,0);

        //vector_blue = relativeTransform.forward;
        //vector_red = relativeTransform.right;

         //animator.Play("Idle");
        // animator.SetFloat("Speed", 5f);
    }

    // Update is called once per frame
    void Update()
    {

        /*if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            //moveDirection = new Vector3(0, 0, 0);
            isMoving = false;
        }
        else isMoving = true;*/

        // if (Input.GetMouseButtonDown(1)) fixedCamera = true;
        //
        // if (Input.GetMouseButtonUp(1)) fixedCamera = false;
        //

        //
        // if (v > 0) v = 1;
        // moveDirection = new Vector3 (0, 0, 10);
        // controller.Move(moveDirection * Time.deltaTime);
        //
        // float v = Input.GetAxis("Vertical");
        //animator.SetFloat("Speed", v);
        //animator.SetBool("Speed", true);

        // if (v < 0) {
        //     v = 0;
        //     // animator.SetFloat("Speed", 0);
        //     // animator.Play("Idle");
        //     // animator.CrossFade("Idle", 0.01f);
        // }
        // else {
        //     animator.SetFloat("Speed", 1);
        //     animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        // }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Debug.Log(v);

        if (v <= 0) {
            v = 0;
            h = 0;
            //animator.CrossFade("Idle", 0.05f);
            animator.Play("Idle");
            
        }
        animator.SetFloat("Speed", v * moveSpeed);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        if (Input.GetKeyDown("space"))
        {
            //Debug.Log ("Se ha pulsado la barra espaciadora");
            moveDirection.y = jumpForce;
            animator.Play("Running Jump");
        }

       if (Input.GetKeyDown("c"))
        {
            moveDirection.y = jumpForce;
            animator.Play("GoalKeeper Jump");
        }

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

    private void OnTriggerEnter(Collider colidedObj) {

        Debug.Log ("Colision detectada con" + colidedObj.name);

        if (colidedObj.name == "Boots") {
            
            timePowerSpeed = 10.0f;
            powerSpeed = true;

            this.moveSpeed = this.moveSpeed * 2;

            GameObject velocity = GameObject.Find ("Velocity");

            if (velocity == null)
            {
                Debug.Log ("Cannot find 'Velocity' script");
            }
            else {
                Destroy(velocity);
            }
        }

        if (colidedObj.name == "Mushroom"){

            timePowerSize = 10.0f;
            powerSize = true;

            this.transform.localScale += new Vector3(3F, 3F, 3F);

            GameObject velocity = GameObject.Find ("Scale");

            if (velocity == null)
            {
                Debug.Log ("Cannot find 'Scale' script");
            }
            else {
                Destroy(velocity);
            }
        }
    }
}
