using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestBall : MonoBehaviourPun
{

    // this script pushes all rigidbodies that the character touches
    public float pushPower = 40.0f;
    public float bounciness = 0.2f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, bounciness, hit.moveDirection.z);

        

        if (!hit.gameObject.GetComponent<PhotonView>().IsMine) {
            hit.gameObject.GetComponent<PhotonView>().TransferOwnership( PhotonNetwork.LocalPlayer );
        }

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
