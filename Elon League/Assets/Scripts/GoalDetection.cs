using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour
{

    public GameController gameController;

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
    }

    private void OnTriggerEnter(Collider colidedObj) {
        if (colidedObj.name == "Soccer Ball")
            gameController.increaseGoalP1();        
    }
}
