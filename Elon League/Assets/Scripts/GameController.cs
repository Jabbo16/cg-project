using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static int score_player1;
    public static int score_player2;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
      score_player1 = 0;
      score_player2 = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void increaseGoalP1(){
      score_player1 ++;
      Debug.Log("score_player1: " + score_player1);
      updateScore();
    }

    public void increaseGoalP2(){
      score_player2 ++;
      Debug.Log("score_player2: " + score_player2);
      updateScore();
    }

    public void updateScore(){
      scoreText.text = score_player1.ToString() + " - " + score_player2.ToString();
    }
}
