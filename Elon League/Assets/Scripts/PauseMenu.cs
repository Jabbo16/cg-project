using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PauseMenu : MonoBehaviour {

    public bool isPaused = false;
    public GameObject menuPanel;
    public GameObject centralPanel;

    // Start is called before the first frame update
    void Start() {
        menuPanel.SetActive(isPaused);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {        
            setPause();
            // SetCountText(); 
        }
    }

    public void setPause() {
        // only pause when singleplayer
        if (SceneManagerHelper.ActiveSceneName == "Room for 1") {
            // if (!isPaused) {
            //     Time.timeScale = 0;
            // }
            // else {
            //     Time.timeScale = 1;
            // }
        }

        isPaused = !isPaused;
        
        // toggle menu
        menuPanel.SetActive(isPaused);
        centralPanel.SetActive(!isPaused);
        
        // toggle camera follow
        GameObject camera = GameObject.Find("Main Camera");
        if (camera != null) {
            CameraController cameraController = camera.GetComponent<CameraController>();
            cameraController.stopForPause = isPaused;
            cameraController.setLockCursor(!isPaused);
        }
    }

    public void onContinuePressed() {
        setPause();
    }

    public void onQuitPressed() {
        Application.Quit();
    }

}
