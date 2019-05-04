using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    #region Private Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;

        [Tooltip("Player target")]
        private PlayerManager target;

    #endregion

    #region MonoBehaviour Callbacks
    
        void Start() {
        }

        void Awake() {
            this.transform.SetParent(GameObject.Find("My Robot Kyle PUN(Clone)").GetComponent<Transform>(), false);
        }

        void Update() {
            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (target == null) {
                Destroy(this.gameObject);
                return;
            }
        }

    #endregion

    #region Public Methods

    public void SetTarget(PlayerManager _target) {

        if (_target == null) {
            Debug.LogError("<Color=Red>Missing</Color> PlayMakerManager target for PlayerUI.SetTarget.");
            return;
        }
        // Cache references for efficiency
        target = _target;
        if (playerNameText != null) {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }

    #endregion

}
