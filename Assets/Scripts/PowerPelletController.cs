using UnityEngine;
using System.Collections;

public class PowerPelletController : MonoBehaviour {

	public bool preGameInitComplete;
	public bool winInitComplete;
	public bool gameOverInitComplete;

	private GameController gameController;
	private MeshRenderer powerPelletRenderer;
	private SphereCollider powerPelletCollider;
	
	// Use this for initialization
	void Start () {

		preGameInitComplete = false;
		winInitComplete = false;
		gameOverInitComplete = false;

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();
		powerPelletRenderer = GetComponent<MeshRenderer> ();
		powerPelletCollider = GetComponent<SphereCollider> ();

	}
	
	void FixedUpdate() {
		
		switch (gameController.gameState) {
			
		case GameController.GameStates.PREGAME:
			
			if (!preGameInitComplete) {
				
				powerPelletRenderer.enabled = true;
				powerPelletCollider.enabled = true;

				gameOverInitComplete = false;
				winInitComplete = false;
				preGameInitComplete = true;

			}
			
			break;

		case GameController.GameStates.WIN:
			
			if (!winInitComplete) {
				
				preGameInitComplete = false;
				winInitComplete = true;
				
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

			powerPelletRenderer.enabled = false;
			powerPelletCollider.enabled = false;

			gameController.gameState = GameController.GameStates.POWERUP;
			gameController.powerUpInitComplete = false;
		
		}
		
	}

}
