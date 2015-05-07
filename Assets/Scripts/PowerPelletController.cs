using UnityEngine;
using System.Collections;

public class PowerPelletController : MonoBehaviour {

	private GameController gameController;

	// Use this for initialization
	void Start () {
		
		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();

	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("PacMan")) {

			// trigger game state change
			gameController.gameState = GameController.GameStates.POWERUP;

			// remove pellet
			this.gameObject.SetActive(false);

		}
		
	}

}
