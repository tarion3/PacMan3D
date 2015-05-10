using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private GameController gameController;
	private Vector3 offset;
	private Vector3 halfVector;

	private Vector3 origPosition;
	private Vector3 playPosition;

	private float interpolationAlpha;

	private bool preGameInitComplete;
	private bool winInitComplete;
	private bool gameOverInitComplete;

	// Use this for initialization
	void Start () {

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();

		offset = transform.position - player.transform.position;
		halfVector = new Vector3(0.5f, 1.0f, 0.5f);

		origPosition = transform.position;
		playPosition = new Vector3 (0.0f, 30.0f, -15.0f);

		interpolationAlpha = 0;
		
		preGameInitComplete = false;
		winInitComplete = false;
		gameOverInitComplete = false;

	}
	
	// Update is called once per frame
	void LateUpdate () {

		switch (gameController.gameState) {
			
		case GameController.GameStates.PREGAME:
			
			if (!preGameInitComplete) {

				gameOverInitComplete = false;
				preGameInitComplete = true;
				
			} else {

				interpolationAlpha = gameController.preGameDuration / gameController.preGameMaxDuration;
				offset = ((1 - interpolationAlpha) * origPosition + interpolationAlpha * playPosition) - player.transform.position;

			}

			break;

		case GameController.GameStates.WIN:
			
			if (!winInitComplete) {
				
				preGameInitComplete = false;
				winInitComplete = true;
				
			} else {
				
				interpolationAlpha = gameController.gameOverDuration / gameController.gameOverMaxDuration;
				offset = ((1 - interpolationAlpha) * playPosition + interpolationAlpha * origPosition) - player.transform.position;
				
			}
			
			break;

		case GameController.GameStates.GAMEOVER:
			
			if (!gameOverInitComplete) {

				preGameInitComplete = false;
				gameOverInitComplete = true;
				
			} else {
				
				interpolationAlpha = gameController.gameOverDuration / gameController.gameOverMaxDuration;
				offset = ((1 - interpolationAlpha) * playPosition + interpolationAlpha * origPosition) - player.transform.position;
				
			}

			break;
			
		}

		transform.position = Vector3.Scale(player.transform.position + offset, halfVector);

	}

}
