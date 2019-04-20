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
        #region Private Methods

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


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion
    }
}