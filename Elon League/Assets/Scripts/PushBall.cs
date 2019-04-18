using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBall : MonoBehaviour
{

    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
      //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
   {
       //rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
   }
}
