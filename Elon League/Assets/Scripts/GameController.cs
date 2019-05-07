using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
{

    // PUBLIC
    public Text scoreText;
    public Text goalText;

    public GameObject scaleObject;
    public GameObject velocityObject;

    public static bool goalEnable;

    // PRIVATE
    private Animator anim;
    private GameObject soccer_ball;
    private Rigidbody soccer_ball_RB;
    private static bool played = false;
    private float timer = 0.0f;
    System.Random ran;
    private CameraController cameraController;
    private Ball gCounter;
    private static int scorePlayer1;
    private static int scorePlayer2;

    // Start is called before the first frame update
    void Start() {
      scorePlayer1 = 0;
      scorePlayer2 = 0;

      goalEnable = true;

      anim = goalText.GetComponent<Animator>();

      soccer_ball = GameObject.Find("Soccer Ball");
      soccer_ball_RB = soccer_ball.GetComponent<Rigidbody>();      
      gCounter = soccer_ball.GetComponent<Ball>();

      GameObject camera = GameObject.Find("Main Camera");
      if (camera != null) {
          cameraController = camera.GetComponent<CameraController>();
          // cameraController.stopForPause = isPaused;
      }

      ran = new System.Random();  
    }

    // Update is called once per frame
    void Update() {
      updateScore();
      
      if (played) timer -= Time.deltaTime;

      if (timer <= 0 && played) {
        // only update if MasterClient sees the 
        // if (PhotonNetwork.IsMasterClient) {
        //   if (!soccer_ball.GetComponent<PhotonView>().IsMine) {
        //     soccer_ball.GetComponent<PhotonView>().TransferOwnership( PhotonNetwork.LocalPlayer );
        //   }
        //   soccer_ball.transform.position = new Vector3(125, 13, 125);
        //   soccer_ball_RB.velocity = Vector3.zero;
        // }
        
        soccer_ball.transform.position = new Vector3(125, 13, 125);
        soccer_ball_RB.velocity = Vector3.zero;
        
        played = false;
        setTrueGoalEnable();

        if (gCounter.scorePlayer1 == 5 || gCounter.scorePlayer2 == 5) {
          PhotonNetwork.LeaveRoom();
          cameraController.setLockCursor(false);
        } 
      }
    }

    public void increaseGoalP1() {
      // if (PhotonNetwork.IsMasterClient) {
      //   gCounter.scorePlayer1++;        
      // }
      gCounter.scorePlayer1++;  
    }

    public void increaseGoalP2() {
      // if (PhotonNetwork.IsMasterClient) {
      //   gCounter.scorePlayer2++;        
      // }
      gCounter.scorePlayer2++;         
    }

    public void updateScore() {
      string prev = scoreText.text;
      scoreText.text = gCounter.scorePlayer1.ToString() + " - " + gCounter.scorePlayer2.ToString();
      if (prev != scoreText.text) {
        anim.Play("goalAnimation");
        timer = 2.3f;
        played = true;   
      }
    }

    // Generate a random number between two numbers  
    public int generateRandom(int min, int max)  
    {  
        return ran.Next(min, max);  
    }  

    public void setFalseGoalEnable() {
      goalEnable = false;
    }

    public void setTrueGoalEnable() {
      goalEnable = true;
    }

    public bool getGoalEnable() {
      return goalEnable;
    }
}
