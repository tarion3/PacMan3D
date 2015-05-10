using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {
	
	private GameController gameController;

	private bool normalInitComplete;
	private bool powerUpInitComplete;

	private Vector3 origScale;
	private float scaleTimer;

	// Use this for initialization
	void Start () {
		
		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();

		normalInitComplete = false;
		powerUpInitComplete = false;

		origScale = transform.localScale;

	}
	
	void FixedUpdate() {
		
		switch (gameController.gameState) {
			
		case GameController.GameStates.NORMAL:
			
			if (!normalInitComplete) {

				transform.localScale = origScale;

				powerUpInitComplete = false;
				normalInitComplete = true;
				
			}
			
			break;
			
		case GameController.GameStates.POWERUP:
			
			if (!powerUpInitComplete) {

				scaleTimer = 0;

				normalInitComplete = false;
				powerUpInitComplete = true;
				
			}

			scaleTimer += Time.deltaTime;

			if (scaleTimer < 2.0f) {

				if (origScale.x > origScale.z) {
					transform.localScale.Set(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z);
				} else {
					transform.localScale.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z * 2);
				}

			} else if (scaleTimer > 4.0f) {

				transform.localScale = origScale;
				scaleTimer = 0;

			}
			
			break;
			
		}
		
	}
	
}
