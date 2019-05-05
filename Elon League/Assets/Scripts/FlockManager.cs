using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlockManager : MonoBehaviour {
	public GameObject Birdie;
	public int numBird = 20;
	public GameObject[] allBird;
	public Vector3 flyLimits = new Vector3(5,5,5);
	
	[Header("Bird Settings")]
	[Range(0.0f, 5.0f)]
	public float minSpeed;
	[Range(0.0f, 5.0f)]
	public float maxSpeed;
	[Range(1.0f, 10.0f)]
	public float neighbourDistance;
	[Range(0.0f, 5.0f)]
	public float rotationSpeed;
	
	void Start () {
		allBird = new GameObject[numBird];
		for(int i = 0; i < numBird; i++)
		{
			Vector3 pos = this.transform.position + new Vector3(Random.Range(-flyLimits.x,flyLimits.x),
				              Random.Range(-flyLimits.y,flyLimits.y),
				              Random.Range(-flyLimits.z,flyLimits.z));
			allBird[i] = (GameObject) Instantiate(Birdie, pos, Quaternion.identity);
			allBird[i].transform.localScale += new Vector3(100F, 100F, 100F);
			allBird[i].AddComponent<Flock>();
			allBird[i].GetComponent<Flock>().myManager = this;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}