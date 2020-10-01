using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPunCallbacks, IPunObservable {

    public int scorePlayer1;
    public int scorePlayer2;

    Vector3 _networkPosition;
    Quaternion _networkRotation;
    Rigidbody _rb; 

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.rotation);
            stream.SendNext(_rb.velocity);
            stream.SendNext(scorePlayer1);
            stream.SendNext(scorePlayer2);
        }
        else
        {
            _networkPosition = (Vector3)stream.ReceiveNext();
            _networkRotation = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector3)stream.ReceiveNext();
            scorePlayer1 = (int)stream.ReceiveNext();
            scorePlayer2 = (int)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            _networkPosition += (_rb.velocity * lag);
        }
    }

    void Start() {
        _rb = GetComponent<Rigidbody>();
        scorePlayer1 = 0;
        scorePlayer2 = 0;
    }

    public void FixedUpdate() {
        if (!photonView.IsMine) {
            _rb.position = Vector3.MoveTowards(_rb.position, _networkPosition, Time.fixedDeltaTime);
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, _networkRotation, Time.fixedDeltaTime * 100.0f);
        }
    }

}