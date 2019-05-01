using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumetaController : MonoBehaviour
{

    float angle;
    Vector2 input;

    public float velocity = 5;
    public float turnSpeed = 10;

    Quaternion targetRotation;

    Transform cam;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetInput();
        //if (Mathf.Abs(input.x) < 1  && Mathf.Abs(input.y) < 1 ) return;

        //CalculateDirection();
        //Rotate();
        //Move();

        anim.SetFloat("BlendX", 1);
        anim.SetFloat("BlendY", 1);
    }

    void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.x = Input.GetAxis("Vertical");
        //anim.SetFloat("BlendX", input.x);
        //anim.SetFloat("BlendY", input.y);
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
    
    void Move ()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
    }
}
