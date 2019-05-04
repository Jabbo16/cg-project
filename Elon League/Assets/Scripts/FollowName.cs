using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class FollowName : MonoBehaviour {

    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text nameLabel;
    
    [Tooltip("Player target")]
    private PlayerManager target;

    private string name;

    // Start is called before the first frame update
    void Start() { 
        nameLabel = GameObject.Find("UI Canvas/NameLabel").GetComponent<Text>();
        // nameLabel.text = PlayerManager.LocalPlayerInstance.GetComponent<Transform>().photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update() {
        Vector3 namePosition = Camera.main.WorldToScreenPoint(this.transform.position);
        nameLabel.transform.position = namePosition;

        if (name == null) {
            nameLabel.text = "Name";
        }
        else {
            nameLabel.text = name;
        }

        if (target == null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public void SetTarget(PlayerManager _target) {

        if (_target == null) {
            Debug.LogError("<Color=Red>Missing</Color> PlayMakerManager target for FollowName.SetTarget.");
            return;
        }

        // Cache references for efficiency
        target = _target;
        if (nameLabel != null) {
            name = target.photonView.Owner.NickName;
            Debug.LogFormat("Target set to {0}", target.photonView.Owner.NickName);
        }
            Debug.Log("Target set!");
        
    }

}
