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

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f,30f,0f);

    #endregion

    #region MonoBehaviour Callbacks
    
        void Start() {
        }

        void Awake() {
            this.transform.SetParent(GameObject.Find("UI Canvas").GetComponent<Transform>(), false);
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
