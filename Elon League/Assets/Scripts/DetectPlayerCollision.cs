using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug-draw all contact points and normals
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Debug.DrawRay(contact.point, contact.normal, Color.white);
        // }

        // // Play a sound if the colliding objects had a big impact.
        // if (collision.relativeVelocity.magnitude > 2)
        //     audioSource.Play();

        Debug.Log("Ball was hit by " + collision.collider.name);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        Debug.Log("Ball was hit by " + hit.collider.gameObject.name);
        Rigidbody body = hit.collider.attachedRigidbody;
    }
}
