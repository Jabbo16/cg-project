using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static int score_player1;
    public static int score_player2;

    public Text scoreText;
    public Text goalText;

    private Animator anim;

    private GameObject soccer_ball;

    private Rigidbody soccer_ball_RB;

    public static bool goalEnable;

    private static bool played = false;

    private float timer = 0.0f;

    public GameObject scaleObject;
    public GameObject velocityObject;
    

    // Start is called before the first frame update
    void Start()
    {
      score_player1 = 0;
      score_player2 = 0;

      goalEnable = true;

      anim = goalText.GetComponent<Animator>();

      soccer_ball = GameObject.Find("Soccer Ball");
      soccer_ball_RB = soccer_ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

       if (played) timer -= Time.deltaTime;

       if (timer <= 0 && played) {
            soccer_ball.transform.position = new Vector3(125, 13, 125);
            soccer_ball_RB.velocity = Vector3.zero;
            played = false;
            setTrueGoalEnable();
       }

        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Default Empty") && played){
        //   soccer_ball.transform.position = new Vector3(125, 13, 125);
        //   soccer_ball_RB.velocity = Vector3.zero;
        //   played = false;
        //   setTrueGoalEnable();
        //}
    }

    public void increaseGoalP1(){
        score_player1 ++;
        updateScore();
    }

    public void increaseGoalP2(){
        score_player2 ++;
        updateScore();
    }

    public void updateScore(){

      anim.Play("goalAnimation");
      //anim.SetTrigger("Play Goal Animation");
      timer = 2.3f;
      played = true;      

      scoreText.text = score_player1.ToString() + " - " + score_player2.ToString();

      //soccer_ball.transform.position = new Vector3(125, 13, 125);
      //soccer_ball_RB.velocity = Vector3.zero;
    }

    public void generateScale(){
        Instantiate(scaleObject, new Vector3(125, 0, 125), Quaternion.identity);
    }

    public void generateVelocity(){
        Instantiate(velocityObject, new Vector3(125, 0, 125), Quaternion.identity);
    }

    public void setFalseGoalEnable(){
      goalEnable = false;
    }

    public void setTrueGoalEnable(){
      goalEnable = true;
    }

    public bool getGoalEnable(){
      return goalEnable;
    }
}
