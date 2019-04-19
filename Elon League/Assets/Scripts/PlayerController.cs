using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    //public Rigidbody theRB;
    public float jumpForce;

    public CharacterController controller;
    private Vector3 moveDirection;

    public Transform relativeTransform;
    private Vector3 vector_blue;
    private Vector3 vector_red;

    public bool fixedCamera = false;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent <GameController>();
        }
        if (gameController == null)
        {
            Debug.Log ("Cannot find 'GameController' script");
        }

        moveSpeed = 20;
        jumpForce = 200;
        //theRB = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        vector_blue = relativeTransform.forward;
        vector_red = relativeTransform.right;
    }

    // Update is called once per frame
    void Update()
    {
        /*theRB.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, theRB.velocity.y, Input.GetAxis("Vertical") * moveSpeed);

        if (Input.GetButtonDown("Jump"))
        {
            theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
        }*/

        if (Input.GetMouseButtonDown(1)) fixedCamera = true;

        if (Input.GetMouseButtonUp(1)){
            fixedCamera = false;
            vector_blue = relativeTransform.forward;
            vector_red = relativeTransform.right;
        }

        if (fixedCamera){
          if(Input.GetKey(KeyCode.W)) moveDirection = relativeTransform.forward * moveSpeed;
          if(Input.GetKey(KeyCode.S)) moveDirection = -relativeTransform.forward * moveSpeed;
          if(Input.GetKey(KeyCode.A)) moveDirection = -relativeTransform.right * moveSpeed;
          if(Input.GetKey(KeyCode.D)) moveDirection = relativeTransform.right * moveSpeed;
        }
        else {
          //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);
          if(Input.GetKey(KeyCode.W)) moveDirection = vector_blue * moveSpeed;
          if(Input.GetKey(KeyCode.S)) moveDirection = -vector_blue * moveSpeed;
          if(Input.GetKey(KeyCode.A)) moveDirection = -vector_red * moveSpeed;
          if(Input.GetKey(KeyCode.D)) moveDirection = vector_red * moveSpeed;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) moveDirection = new Vector3(0, 0, 0);

        //moveDirection.y = 0f;

        if (controller.isGrounded) {

            moveDirection.y = 0f;

            if (Input.GetButtonDown("Jump")){

                gameController.increaseGoalP1();

                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);
    }
}
