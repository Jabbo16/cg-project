using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
{

    // PUBLIC
    public static int score_player1;
    public static int score_player2;

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

    // Start is called before the first frame update
    void Start() {
      score_player1 = 0;
      score_player2 = 0;

      goalEnable = true;

      anim = goalText.GetComponent<Animator>();

      soccer_ball = GameObject.Find("Soccer Ball");
      soccer_ball_RB = soccer_ball.GetComponent<Rigidbody>();

      GameObject camera = GameObject.Find("Main Camera");
      if (camera != null) {
          cameraController = camera.GetComponent<CameraController>();
          // cameraController.stopForPause = isPaused;
      }

      ran = new System.Random();  

      generateScale();
      generateVelocity();
    }

    // Update is called once per frame
    void Update() {

       if (played) timer -= Time.deltaTime;

       if (timer <= 0 && played) {
            soccer_ball.transform.position = new Vector3(125, 13, 125);
            soccer_ball_RB.velocity = Vector3.zero;
            played = false;
            setTrueGoalEnable();

            if (score_player1 == 5 || score_player2 == 5) {
              PhotonNetwork.LeaveRoom();
              cameraController.setLockCursor(false);
            } 
       }
    }

    public void increaseGoalP1() {
        score_player1 ++;
        updateScore();
    }

    public void increaseGoalP2() {
        score_player2 ++;
        updateScore();
    }

    public void updateScore() {

      anim.Play("goalAnimation");
      timer = 2.3f;
      played = true;      

      scoreText.text = score_player1.ToString() + " - " + score_player2.ToString();

      //soccer_ball.transform.position = new Vector3(125, 13, 125);
      //soccer_ball_RB.velocity = Vector3.zero;
    }

    public void generateScale() {
        // Instantiate(scaleObject, new Vector3(generateRandom(105, 145), 0, generateRandom(105,145)), Quaternion.identity);
    }

    public void generateVelocity() {
        // Instantiate(velocityObject, new Vector3(generateRandom(105, 145), 0, generateRandom(105,145)), Quaternion.identity);
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
