using UnityEngine;
using System.Collections;

public class PacmanController : MonoBehaviour {

	public AudioClip pacmanDieAudio;
	public AudioClip pelletEatAudio;
	public AudioClip powerPelletEatAudio;
	public AudioClip ghostEatAudio;
	
	private GameController gameController;

	private AudioSource pacmanAudio;

	private Vector3 homePosition;
	private Vector3 direction;
	private Vector3 warp1Translation;
	private Vector3 warp2Translation;

	private float speed;

	// Use this for initialization
	void Start () {

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();

		pacmanAudio = GetComponent<AudioSource> ();

		homePosition = transform.position;
		speed = 0.16f;
		direction = new Vector3(0,0,0);

		warp1Translation = new Vector3 (28,0,0);
		warp2Translation = new Vector3 (-28,0,0);

	}

	void FixedUpdate() {

		switch (gameController.gameState) {

		case GameController.GameStates.READY:

			transform.position = homePosition;
			direction = Vector3.zero;

			break;

		case GameController.GameStates.NORMAL:
		case GameController.GameStates.POWERUP:

			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			
			if (moveHorizontal > 0)
				direction.Set (speed, 0, 0);
			else if (moveHorizontal < 0)
				direction.Set (-speed, 0, 0);
			else if (moveVertical > 0)
				direction.Set (0, 0, speed);
			else if (moveVertical < 0)
				direction.Set (0, 0, -speed);
			
			// only move if there are no obstacles in the new direction
			//if (!Physics.Raycast(transform.position, direction, 0.3f)) {
			
			transform.LookAt (transform.position + direction);
			transform.Translate (direction, Space.World);
			
			//}

			break;

		}

	}
	
	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("Pellet")) {

			pacmanAudio.clip = pelletEatAudio;
			pacmanAudio.Play ();
			gameController.count++;
			gameController.score += 5;


		} else if (other.gameObject.CompareTag ("PowerPellet")) {

			pacmanAudio.clip = powerPelletEatAudio;
			pacmanAudio.Play ();
			gameController.count++;
			gameController.score += 10;

		} else if (other.gameObject.CompareTag ("WarpTunnel1")) {

			transform.Translate (warp1Translation, Space.World);

		} else if (other.gameObject.CompareTag ("WarpTunnel2")) {

			transform.Translate (warp2Translation, Space.World);

		} else if (other.gameObject.CompareTag ("Ghost")) {

			// if we are in normal mode, pacman just dies
			if (gameController.gameState == GameController.GameStates.NORMAL) {

				gameController.gameAudio.Stop();
				pacmanAudio.clip = pacmanDieAudio;
				pacmanAudio.Play();

				gameController.lives--;
				
				if (gameController.lives > 0) {
					gameController.gameState = GameController.GameStates.READY;
				}
				else {
					gameController.gameState = GameController.GameStates.GAMEOVER;
				}

			}

			// otherwise, pacman eats the ghost
			else if (gameController.gameState == GameController.GameStates.POWERUP) {

				pacmanAudio.clip = ghostEatAudio;
				pacmanAudio.Play();
				gameController.score += 100;

			}

		}

	}

}