using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 5);

	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, SharedVariables.speed);
	}
	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Obstacle") || other.CompareTag("Coin") || other.CompareTag("Laser")){
		Destroy (other);
		}
	}
}
