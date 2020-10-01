using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks {

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;

    private void setCamera () {
        GameObject camera = GameObject.Find("Main Camera");

        if (camera != null) {
            CameraController cameraController = camera.GetComponent<CameraController>();

            Transform target = this.gameObject.GetComponent<Transform>();
            MeshRenderer targetRenderer = this.gameObject.GetComponent<MeshRenderer>();

            if (target != null && targetRenderer != null) {
                if (photonView.IsMine) {
                    cameraController.target = target;
                    cameraController.targetRenderer = targetRenderer;
                    Debug.Log("Camera correctly setted to <Color=Blue>current player</Color>", this);
                }
                else {
                    // Assigns camera to player even if not my player, dev purposes
                    // Debug.Log("Development case, delete this statement when we want to play in server");
                    // cameraController.target = target;
                    // cameraController.targetRenderer = targetRenderer;
                }
            }
            else {
                Debug.LogError("Target not found!");
            }

        }
        else {
            Debug.LogError("Camera not found!");
        }
    }

    // Start is called before the first frame update
    void Start() {

        setCamera();

        if (PlayerUiPrefab != null) {
            GameObject _uiGo =  Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else {
            Debug.LogWarning("<Color=Red>Missing</Color> PlayerUiPrefab reference on player Prefab.");
        }

        #if UNITY_5_4_OR_NEWER
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        #endif
    }

    void Awake() {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine) {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        
    }

    #if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode) {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
    #endif

    #if !UNITY_5_4_OR_NEWER
    /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
    void OnLevelWasLoaded(int level) {
        this.CalledOnLevelWasLoaded(level);
    }
    #endif


    void CalledOnLevelWasLoaded(int level) {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
            transform.position = new Vector3(125f, 5f, 125f);
        }

        // Instantiate player UI
        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
        setCamera();
        // Debug.Log("Camera updated!");
    }

    #if UNITY_5_4_OR_NEWER
    public override void OnDisable() {
        // Always call the base to remove callbacks
        base.OnDisable ();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endif

}
