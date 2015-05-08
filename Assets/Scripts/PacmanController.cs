using UnityEngine;
using System.Collections;

public class PacmanController : MonoBehaviour {

	public AudioClip pacmanDieAudio;
	public AudioClip pelletEatAudio;
	public AudioClip powerPelletEatAudio;
	public AudioClip ghostEatAudio;

	private GameController gameController;
	private MeshRenderer pacmanRenderer;

	private AudioSource pacmanAudio;

	private Vector3 homePosition;
	private Vector3 direction;
	private Vector3 warp1Translation;
	private Vector3 warp2Translation;

	private float speed;

	private bool preGameInitComplete;
	private bool readyInitComplete;
	private bool normalInitComplete;
	private bool powerUpInitComplete;
	private bool dieInitComplete;
	private bool gameOverInitComplete;

	// Use this for initialization
	void Start () {

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();
		pacmanRenderer = GetComponent<MeshRenderer> ();

		pacmanAudio = GetComponent<AudioSource> ();

		homePosition = transform.position;
		speed = 0.16f;
		direction = new Vector3(0,0,0);

		warp1Translation = new Vector3 (28,0,0);
		warp2Translation = new Vector3 (-28,0,0);

		preGameInitComplete = false;
		readyInitComplete = false;
		normalInitComplete = false;
		powerUpInitComplete = false;
		dieInitComplete = false;
		gameOverInitComplete = false;

	}

	void FixedUpdate() {

		switch (gameController.gameState) {

		case GameController.GameStates.PREGAME:

			if (!preGameInitComplete) {

				speed *= (gameController.level + 1)/2;
				
				readyInitComplete = false;
				preGameInitComplete = true;
				
			}

			goto case GameController.GameStates.READY;

		case GameController.GameStates.READY:

			if (!readyInitComplete) {

				transform.position = homePosition;
				pacmanRenderer.enabled = true;
				direction = Vector3.zero;

				normalInitComplete = false;
				readyInitComplete = true;

			}

			break;

		case GameController.GameStates.NORMAL:

			if (!normalInitComplete) {

				dieInitComplete = false;
				powerUpInitComplete = false;
				gameOverInitComplete = false;
				normalInitComplete = true;
				
			}

			goto case GameController.GameStates.POWERUP;

		case GameController.GameStates.POWERUP:

			if (!powerUpInitComplete) {
				
				normalInitComplete = false;
				gameOverInitComplete = false;
				powerUpInitComplete = true;
				
			}

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

		case GameController.GameStates.DIE:

			if (!dieInitComplete) {

				gameController.gameAudio.Stop();
				pacmanAudio.clip = pacmanDieAudio;
				pacmanAudio.Play();

				readyInitComplete = false;
				gameOverInitComplete = false;
				dieInitComplete = true;

			} else if (!pacmanAudio.isPlaying) {

				gameController.lives--;

			}

			break;

		case GameController.GameStates.GAMEOVER:
			
			if (!gameOverInitComplete) {

				transform.position = homePosition;
				pacmanRenderer.enabled = false;
				
				preGameInitComplete = false;
				gameOverInitComplete = true;
				
			}
			
			break;

		}

	}
	
	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("Pellet")) {

			pacmanAudio.clip = pelletEatAudio;
			pacmanAudio.Play ();


		} else if (other.gameObject.CompareTag ("PowerPellet")) {

			pacmanAudio.clip = powerPelletEatAudio;
			pacmanAudio.Play ();

		} else if (other.gameObject.CompareTag ("WarpTunnel1")) {

			transform.Translate (warp1Translation, Space.World);

		} else if (other.gameObject.CompareTag ("WarpTunnel2")) {

			transform.Translate (warp2Translation, Space.World);

		} else if (other.gameObject.CompareTag ("Ghost")) {

			// if we are in normal mode, pacman just dies
			if (gameController.gameState == GameController.GameStates.NORMAL) {
				gameController.gameState = GameController.GameStates.DIE;
			}

			// otherwise, pacman eats the ghost
			else if (gameController.gameState == GameController.GameStates.POWERUP) {

				pacmanAudio.clip = ghostEatAudio;
				pacmanAudio.volume = 1.0f;
				pacmanAudio.Play();
				pacmanAudio.volume = 0.5f;

			}

		}

	}

}