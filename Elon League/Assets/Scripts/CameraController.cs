using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - transform.position; //Diferencia actual entre la camara y el tarjet
        rotateSpeed = 5;
        //tarjet = GameObject.Find("Capsule").transform;

    }

    // Update is called once per frame
    void Update()
    {

        //Obtenemos la posicion X del raton y rotamos el tarjet
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, horizontal, 0);

        //Movemos la camara en base a la rotacion actual del tarjet y respecto al offset 
        float desireYAngle = target.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desireYAngle, 0);
        transform.position = target.position - (rotation * offset);

        //transform.position = target.position - offset; //Posicion camara = objeto - offset (fijado en Start)

        transform.LookAt(target);
    }
}
