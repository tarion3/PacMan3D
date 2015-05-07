using UnityEngine;
using System.Collections;

public class PelletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("PacMan")) {

			this.gameObject.SetActive(false);

		}

	}

}
