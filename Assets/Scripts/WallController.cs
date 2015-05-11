using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

	public bool normalInitComplete;
	public bool powerUpInitComplete;

	private GameController gameController;

	private Vector3 origScale;
	private Vector3 newScale;

	private bool isSelectedWall;
	private float scaleFactor;

	// Use this for initialization
	void Start () {

		normalInitComplete = false;
		powerUpInitComplete = false;

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();

		origScale = transform.localScale;
		newScale = origScale;

		isSelectedWall = false;
		scaleFactor = 0;

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

				isSelectedWall = Random.Range(0, 5) == 3;
				scaleFactor = 9.0f / gameController.powerUpMaxDuration;

				normalInitComplete = false;
				powerUpInitComplete = true;
				
			}

			if (isSelectedWall && gameController.secondHasPassed) {

				if (gameController.powerUpDuration < gameController.powerUpMaxDuration / 2.0f) {

					if (origScale.x > origScale.z) {
						if (newScale.x < scaleFactor * gameController.powerUpMaxDuration) {
							newScale.x += scaleFactor;
							transform.localScale = newScale;
						}
					} else {
						if (newScale.z < scaleFactor * gameController.powerUpMaxDuration) {
							newScale.z += scaleFactor;
							transform.localScale = newScale;
						}
					}

				} else {

					if (origScale.x > origScale.z) {
						if (newScale.x > origScale.x) {
							newScale.x -= scaleFactor;
							transform.localScale = newScale;
						}
					} else {
						if (newScale.z > origScale.z) {
							newScale.z -= scaleFactor;
							transform.localScale = newScale;
						}
					}

				}

			}
			
			break;
			
		}
		
	}
	
}
