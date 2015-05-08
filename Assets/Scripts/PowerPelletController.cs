using UnityEngine;
using System.Collections;

public class PowerPelletController : MonoBehaviour {

	private GameController gameController;
	private MeshRenderer powerPelletRenderer;
	private SphereCollider powerPelletCollider;

	private bool preGameInitComplete;
	private bool gameOverInitComplete;

	// Use this for initialization
	void Start () {
		
		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();
		powerPelletRenderer = GetComponent<MeshRenderer> ();
		powerPelletCollider = GetComponent<SphereCollider> ();

		preGameInitComplete = false;
		gameOverInitComplete = false;

	}
	
	void FixedUpdate() {
		
		switch (gameController.gameState) {
			
		case GameController.GameStates.PREGAME:
			
			if (!preGameInitComplete) {
				
				powerPelletRenderer.enabled = true;
				powerPelletCollider.enabled = true;

				gameOverInitComplete = false;
				preGameInitComplete = true;

			}
			
			break;


		case GameController.GameStates.GAMEOVER:
			
			if (!gameOverInitComplete) {
				
				preGameInitComplete = false;
				gameOverInitComplete = true;
				
			}
			
			break;

		}
		
	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("PacMan")) {

			// remove pellet
			powerPelletRenderer.enabled = false;
			powerPelletCollider.enabled = false;

			gameController.pelletCount++;
			gameController.score += 50;

			// trigger game state change
			gameController.gameState = GameController.GameStates.POWERUP;
			gameController.powerUpDuration = 0;
		
		}
		
	}

}
