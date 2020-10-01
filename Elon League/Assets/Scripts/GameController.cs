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
    public Text resultText;

    public GameObject scaleObject;
    public GameObject velocityObject;

    public static bool goalEnable;

    // PRIVATE
    private Animator anim;
    private GameObject soccer_ball;
    private Rigidbody soccer_ball_RB;
    private static bool played = false;
    private float timer = 0.0f;
    private float resultTimer = 0.0f;
    System.Random ran;
    private CameraController cameraController;
    private Ball gCounter;


    // Start is called before the first frame update
    void Start() {
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
      

      if (gCounter.scorePlayer1 == 5 || gCounter.scorePlayer2 == 5) resultTimer -= Time.deltaTime;

      if (resultTimer <= 0) {
        if (PhotonNetwork.IsMasterClient){
          if (gCounter.scorePlayer1 == 5) {
            resultText.text = "You won!!!";
          } else if  (gCounter.scorePlayer2 == 5) {
            resultText.text = "You lost...";
          }
        } else {
          if (gCounter.scorePlayer2 == 5) {
            resultText.text = "You won!!!";
          } else if  (gCounter.scorePlayer1 == 5) {
            resultText.text = "You lost...";
          }
        }

      }

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
        soccer_ball.transform.position = new Vector3(125, 29, 125);
        soccer_ball_RB.velocity = Vector3.zero;
        
        played = false;
        setTrueGoalEnable();

        if (gCounter.scorePlayer1 == 5 || gCounter.scorePlayer2 == 5) {
          PhotonNetwork.LeaveRoom();
          cameraController.setLockCursor(false);
        } else {
          // if (PhotonNetwork.IsMasterClient) {
          //   PlayerManager.LocalPlayerInstance.transform.position = new Vector3(125f, 5f, 105f);
          //   PlayerManager.LocalPlayerInstance.transform.rotation = Quaternion.identity;
          //   Debug.LogFormat("Re-Setting player after goal: Position for P1", SceneManagerHelper.ActiveSceneName);
          // }
          // else {
          //   PlayerManager.LocalPlayerInstance.transform.position = new Vector3(125f, 5f, 145f);
          //   PlayerManager.LocalPlayerInstance.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
          //   Debug.LogFormat("Re-Setting player after goal: Position for P2", SceneManagerHelper.ActiveSceneName);
          // }
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

        if (gCounter.scorePlayer1 == 5 || gCounter.scorePlayer2 == 5) resultTimer = .5f;
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
