using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

	public bool normalInitComplete;
	public bool powerUpInitComplete;

	private GameController gameController;

	private Vector3 origScale;
	private Vector3 newScale;

	private bool isSelectedWall;
	private float interpolationAlpha;

	// Use this for initialization
	void Start () {

		normalInitComplete = false;
		powerUpInitComplete = false;

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();

		origScale = transform.localScale;
		newScale = origScale;

		isSelectedWall = false;
		interpolationAlpha = 0;

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

				isSelectedWall = Random.Range(0, 2) == 1;

				normalInitComplete = false;
				powerUpInitComplete = true;
				
			}

			if (isSelectedWall) {

				interpolationAlpha = gameController.powerUpDuration / gameController.powerUpMaxDuration;

				if (gameController.powerUpDuration < gameController.powerUpMaxDuration / 2.0f) {

					if (origScale.x > origScale.z) {

						newScale.x = (1 - interpolationAlpha) * origScale.x + interpolationAlpha * 14.0f;
						transform.localScale = newScale;

					} else {

						newScale.z = (1 - interpolationAlpha) * origScale.z + interpolationAlpha * 14.0f;
						transform.localScale = newScale;

					}
					
				} else {
					
					if (origScale.x > origScale.z) {
						
						newScale.x = (1 - interpolationAlpha) * 14.0f + interpolationAlpha * origScale.x;
						transform.localScale = newScale;
						
					} else {
						
						newScale.z = (1 - interpolationAlpha) * 14.0f + interpolationAlpha * origScale.z;
						transform.localScale = newScale;
						
					}

				}

			}


			break;
			
		}
		
	}
	
}
