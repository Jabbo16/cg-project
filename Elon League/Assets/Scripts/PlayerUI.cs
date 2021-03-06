﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUI : MonoBehaviourPun
{

    #region Public Fields
    
        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        public Vector3 screenOffset = new Vector3(0f,80f,0f);

    #endregion

    #region Private Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;    

        [Tooltip("Player target")]
        private PlayerManager target;

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        Vector3 targetPosition;

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

        void LateUpdate() {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null) {
                // Debug.Log("Taget renderer: " + targetRenderer.isVisible.ToString());
                // this.gameObject.SetActive(targetRenderer.isVisible);
                this.gameObject.SetActive(true);
            }

            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null) {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint (targetPosition) + screenOffset;
            }
        }

    #endregion

    #region Public Methods

    public void SetTarget(PlayerManager _target) {
        // Debug.Log("SetTarget called");

        if (_target == null) {
            Debug.LogError("<Color=Red>Missing</Color> PlayMakerManager target for PlayerUI.SetTarget.");
            return;
        }
        // Cache references for efficiency
        target = _target;
        if (playerNameText != null) {
            playerNameText.text = target.photonView.Owner.NickName;
            if (PhotonNetwork.IsMasterClient){
                if (target.photonView.IsMine){
                    // red
                    playerNameText.color = new Color(130.0f/255.0f, 47.0f/255.0f, 47.0f/255.0f);
                }
                else {
                    // blue
                    playerNameText.color = new Color(53.0f/255.0f, 123.0f/255.0f, 154.0f/255.0f);
                }
            }    
            else{
                if (target.photonView.IsMine) {
                    // blue
                    playerNameText.color = new Color(53.0f/255.0f, 123.0f/255.0f, 154.0f/255.0f);
                }
                else {
                    // red
                    playerNameText.color = new Color(130.0f/255.0f, 47.0f/255.0f, 47.0f/255.0f);
                    
                }
            }
                
                    
        }

        targetTransform = this.target.GetComponent<Transform>();
        targetRenderer = this.target.GetComponent<Renderer>();
        CharacterController characterController = _target.GetComponent<CharacterController> ();
        // Get data from the Player that won't change during the lifetime of this Component
        if (characterController != null) {
            characterControllerHeight = characterController.height;
        }
    }

    #endregion

}
