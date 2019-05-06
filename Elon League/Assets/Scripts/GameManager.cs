using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Es.Alumnos.Uc3m
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        #endregion
        
        #region  Public Fields

        public static GameManager Instance;

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        
        [Tooltip("The prefab to use for representing the scale powerUp")]
        public GameObject powerUpScale;

        [Tooltip("The prefab to use for representing the speed powerUp")]
        public GameObject powerUpSpeed;

        #endregion

        #region Private Methods

        void Start() {
            Instance = this;

            if (playerPrefab == null) {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else {
                if (PlayerManager.LocalPlayerInstance == null) {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // Debug.LogFormat("players : {0}", this.playerPrefab.name);

                    if (SceneManagerHelper.ActiveSceneName == "Room for 1") {
                        Debug.Log("Setting player for <Room for 1>");
                        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(125f, 5f, 105f), Quaternion.identity, 0);
                    }
                    // Never reached because always start in Room for 1
                    else if (SceneManagerHelper.ActiveSceneName == "Room for 2") {
                        Debug.Log("Setting player for Room for 2");

                        // Different position for each player
                        if (PhotonNetwork.CurrentRoom.PlayerCount == 1){
                            Debug.Log("First player in the room");
                            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(125f, 5f, 145f), Quaternion.AngleAxis(180, Vector3.up), 0);
                        }
                        else {
                            Debug.Log("Second player in the room");
                            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(125f, 5f, 105f), Quaternion.identity, 0);
                        }
                        
                    }
                    
                }
                else {

                    // Reubicate player to start place
                    if (PhotonNetwork.IsMasterClient) {
                        PlayerManager.LocalPlayerInstance.transform.position = new Vector3(125f, 5f, 105f);
                        PlayerManager.LocalPlayerInstance.transform.rotation = Quaternion.identity;
                        Debug.LogFormat("Re-Setting player: Position for P1", SceneManagerHelper.ActiveSceneName);

                        // Instantiate powerups
                        Debug.Log ("Instantiating Speed PowerUp");
                        PhotonNetwork.Instantiate(this.powerUpSpeed.name, new Vector3(135f, 4f, 125f), Quaternion.identity, 0);
                        
                        Debug.Log ("Instantiating Scale PowerUp");
                        PhotonNetwork.Instantiate(this.powerUpScale.name, new Vector3(115f, 4f, 125f), Quaternion.identity, 0);
                    }
                    else {
                        PlayerManager.LocalPlayerInstance.transform.position = new Vector3(125f, 5f, 145f);
                        PlayerManager.LocalPlayerInstance.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                        Debug.LogFormat("Re-Setting player: Position for P1", SceneManagerHelper.ActiveSceneName);
                    }

                    // Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        void LoadArena() {
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion


        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player other) {
            // not seen if you're the player connecting
            Debug.LogFormat("A new player ({0}) has entered the room", other.NickName); 

            if (PhotonNetwork.IsMasterClient) {
                // called before OnPlayerLeftRoom
                Debug.LogFormat("Is a MasterClient: {0}", PhotonNetwork.IsMasterClient); 
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other) {
            // seen when other disconnects
            Debug.LogFormat("A player ({0}) has left the room", other.NickName); 

            if (PhotonNetwork.IsMasterClient) {
                // called before OnPlayerLeftRoom
                Debug.LogFormat("Is a MasterClient: {0}", PhotonNetwork.IsMasterClient); 
                LoadArena();
            }
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion


        #region Public Methods


        public void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }


        #endregion
    }
}