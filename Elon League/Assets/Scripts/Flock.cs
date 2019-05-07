using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
	
    public FlockManager myManager;
    float speed;
    // Use this for initialization
    void Start () {
        speed = Random.Range(myManager.minSpeed,
            myManager.maxSpeed);
    }
	
    // Update is called once per frame
    void Update () {
        transform.Translate(0, 0, Time.deltaTime * speed);
        ApplyRules();
		
    }
    void ApplyRules()
    {
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        float bDistance = float.MaxValue;
        GameObject closestBird = null;
        foreach (var bird in myManager.allBird) 
        {
            if(bird != this.gameObject)
            {
                nDistance = Vector3.Distance(bird.transform.position,this.transform.position); 
                if(nDistance <= myManager.neighbourDistance)
                {
                    vcentre += bird.transform.position;	
                    groupSize++;	
					
                    if(nDistance < 4.0f)		
                    {
                        vavoid = vavoid + (this.transform.position - bird.transform.position);
                    }
					
                    var anotherFlock = bird.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                } else if (nDistance < bDistance)
                {
                    bDistance = nDistance;
                    closestBird = bird;
                }
            }
        } 
		
        if(groupSize > 0)
        {
            vcentre = vcentre/groupSize;
            speed = gSpeed/groupSize;
			
            Vector3 direction = (vcentre + vavoid) - transform.position;
            direction += (myManager.target - transform.position) * myManager.forceToTarget;
            var distToTarget = Vector3.Distance(transform.position,myManager.target); 
            if (distToTarget > 100)
            {
                direction = myManager.target;
            }
            if(direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), 
                    myManager.rotationSpeed * Time.deltaTime);
		
        }
        else if(closestBird)
        {
            vcentre = closestBird.transform.position;
            speed = gSpeed/groupSize;
			
            Vector3 direction = vcentre - transform.position;
            if(direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), 
                    myManager.rotationSpeed * Time.deltaTime);
        }
    }
}