using UnityEngine;
using System.Collections;

public class PacmanController : MonoBehaviour {

	public bool preGameInitComplete;
	public bool readyInitComplete;
	public bool normalInitComplete;
	public bool powerUpInitComplete;
	public bool eatGhostInitComplete;
	public bool dieInitComplete;
	public bool winInitComplete;
	public bool gameOverInitComplete;

	public AudioClip eatPelletAudio;
	public AudioClip eatPowerPelletAudio;
	public AudioClip eatGhostAudio;
	public AudioClip deathAudio;
	
	private GameController gameController;
	private MeshRenderer[] pacmanRenderers;

	private AudioSource pacmanAudio;

	private Vector3 homePosition;
	private Vector3 curDirection;
	private Vector3 newDirection;
	private RaycastHit hit;
	private Vector3 warp1Translation;
	private Vector3 warp2Translation;

	private float speed;

	// Use this for initialization
	void Start () {

		preGameInitComplete = false;
		readyInitComplete = false;
		normalInitComplete = false;
		powerUpInitComplete = false;
		eatGhostInitComplete = false;
		dieInitComplete = false;
		winInitComplete = false;
		gameOverInitComplete = false;

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();
		pacmanRenderers = GetComponentsInChildren<MeshRenderer> ();

		pacmanAudio = GetComponent<AudioSource> ();

		homePosition = transform.position;
		curDirection = new Vector3(0,0,0);
		newDirection = new Vector3(0,0,0);
		warp1Translation = new Vector3 (28,0,0);
		warp2Translation = new Vector3 (-28,0,0);

		speed = 0.14f;

	}

	void FixedUpdate() {

		switch (gameController.gameState) {

		case GameController.GameStates.PREGAME:

			if (!preGameInitComplete) {

				if (gameController.level > 1) {

					speed += (speed/4.0f);

				}
				
				readyInitComplete = false;
				preGameInitComplete = true;
				
			}

			goto case GameController.GameStates.READY;

		case GameController.GameStates.READY:

			if (!readyInitComplete) {

				transform.position = homePosition;
				foreach(MeshRenderer renderer in pacmanRenderers) {
					renderer.enabled = true;
				}
				curDirection = Vector3.zero;

				normalInitComplete = false;
				readyInitComplete = true;

			}

			break;

		case GameController.GameStates.NORMAL:

			if (!normalInitComplete) {

				dieInitComplete = false;
				powerUpInitComplete = false;
				winInitComplete = false;
				gameOverInitComplete = false;
				normalInitComplete = true;
				
			}

			goto case GameController.GameStates.POWERUP;

		case GameController.GameStates.POWERUP:

			if (!powerUpInitComplete) {
				
				normalInitComplete = false;
				winInitComplete = false;
				gameOverInitComplete = false;
				powerUpInitComplete = true;
				
			}

			eatGhostInitComplete = false;
			
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			if (moveHorizontal != 0 || moveVertical != 0) {
			
				if (moveHorizontal > 0)
					newDirection.Set (speed, 0, 0);
				else if (moveHorizontal < 0)
					newDirection.Set (-speed, 0, 0);
				else if (moveVertical > 0)
					newDirection.Set (0, 0, speed);
				else if (moveVertical < 0)
					newDirection.Set (0, 0, -speed);
				
				// only move in a new direction if there are no obstacles
				if (!Physics.Raycast(transform.position, newDirection, out hit, 1.0f) || hit.collider.tag != "Wall") {
					curDirection = newDirection;
					transform.LookAt (transform.position + curDirection);
				}

			}

			transform.Translate (curDirection, Space.World);

			break;

		case GameController.GameStates.EATGHOST:
			
			if (!eatGhostInitComplete) {

				eatGhostInitComplete = true;

			}

			break;

		case GameController.GameStates.DIE:

			if (!dieInitComplete) {

				// play death animation

				readyInitComplete = false;
				gameOverInitComplete = false;
				dieInitComplete = true;

			}

			break;

		case GameController.GameStates.WIN:
			
			if (!winInitComplete) {
				
				transform.position = homePosition;
				foreach(MeshRenderer renderer in pacmanRenderers) {
					renderer.enabled = false;
				}
				
				preGameInitComplete = false;
				winInitComplete = true;
				
			}
			
			break;

		case GameController.GameStates.GAMEOVER:
			
			if (!gameOverInitComplete) {

				transform.position = homePosition;
				foreach(MeshRenderer renderer in pacmanRenderers) {
					renderer.enabled = false;
				}
				
				preGameInitComplete = false;
				gameOverInitComplete = true;
				
			}
			
			break;

		}

	}
	
	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("Pellet")) {

			gameController.pelletCount++;
			gameController.score += 10;
		
			if (!pacmanAudio.isPlaying || pacmanAudio.clip == eatPelletAudio) {

				pacmanAudio.clip = eatPelletAudio;
				pacmanAudio.Play ();

			}

		} else if (other.gameObject.CompareTag ("PowerPellet")) {

			gameController.pelletCount++;
			gameController.score += 50;

			pacmanAudio.clip = eatPowerPelletAudio;
			pacmanAudio.Play ();

		} else if (other.gameObject.CompareTag ("WarpTunnel1")) {

			transform.Translate (warp1Translation, Space.World);

		} else if (other.gameObject.CompareTag ("WarpTunnel2")) {

			transform.Translate (warp2Translation, Space.World);

		} else if (other.gameObject.CompareTag ("Ghost")) {

			// if we are in normal mode, pacman just dies
			if (gameController.gameState == GameController.GameStates.NORMAL) {

				pacmanAudio.clip = deathAudio;
				pacmanAudio.Play ();

				gameController.gameState = GameController.GameStates.DIE;
			}

		// otherwise, pacman eats the ghost
		else if (gameController.gameState == GameController.GameStates.POWERUP) {

				gameController.numGhostsEaten++;
				gameController.score += (int)((Mathf.Pow (2.0f, gameController.numGhostsEaten)) * 100.0f);

				pacmanAudio.clip = eatGhostAudio;
				pacmanAudio.Play ();

				gameController.gameState = GameController.GameStates.EATGHOST;

			}

		}

	}

}