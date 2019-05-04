using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour
{

    public GameController gameController;

    //private GameObject soccer_ball;

    //private Rigidbody soucer_ball_RB;

    AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent <GameController>();
        }
        if (gameController == null)
        {
            Debug.Log ("Cannot find 'GameController' script");
        }

        //soccer_ball = GameObject.Find("Soccer Ball");

        //soucer_ball_RB = soccer_ball.GetComponent<Rigidbody>();

        audioData = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider colidedObj) {

        //Debug.Log ("El valor de goalEnable es: " + gameController.getGoalEnable());

        if (this.gameObject.name == "Porteria 2" && colidedObj.name == "Soccer Ball" && gameController.getGoalEnable()){
            gameController.setFalseGoalEnable();
            gameController.increaseGoalP1();
            audioData.Play(0);
            //soccer_ball.transform.position = new Vector3(125, 13, 125);
            //soucer_ball_RB.velocity = Vector3.zero;
        }
        if (this.gameObject.name == "Porteria 1" && colidedObj.name == "Soccer Ball" && gameController.getGoalEnable()){
            gameController.setFalseGoalEnable();
            gameController.increaseGoalP2();
            audioData.Play(0);
            //soccer_ball.transform.position = new Vector3(125, 13, 125);
            //soucer_ball_RB.velocity = Vector3.zero;
        }
    }
}
