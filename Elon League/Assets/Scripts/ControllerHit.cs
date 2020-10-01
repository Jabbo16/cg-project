using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHit : MonoBehaviour
{

  public float pushStrength = 6.0f;
  //public ThirdPersonController tpc;

  void OnControllerColliderHit(ControllerColliderHit hit){

      Debug.Log ("Hola");
      Rigidbody body = hit.collider.attachedRigidbody;

      if (body == null || body.isKinematic) return;

      if (hit.moveDirection.y < - 0.3f) return;

     // pushStrength = tpc.GetSpeed();

      Vector3 direction = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);

      body.velocity = direction * pushStrength;
  }

  private void Start() {
      Debug.Log ("Start");
      //tpc = gameObject.GetComponent<ThirdPersonController>();
  }
}
