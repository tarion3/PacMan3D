﻿using UnityEngine;
using System.Collections;

public class PacmanController : MonoBehaviour {

	public AudioClip eatPelletAudio;
	public AudioClip eatPowerPelletAudio;
	public AudioClip eatGhostAudio;
	public AudioClip deathAudio;
	
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
	private bool eatGhostInitComplete;
	private bool dieInitComplete;
	private bool winInitComplete;
	private bool gameOverInitComplete;

	// Use this for initialization
	void Start () {

		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();
		pacmanRenderer = GetComponent<MeshRenderer> ();

		pacmanAudio = GetComponent<AudioSource> ();

		homePosition = transform.position;
		direction = new Vector3(0,0,0);
		warp1Translation = new Vector3 (28,0,0);
		warp2Translation = new Vector3 (-28,0,0);

		speed = 0.14f;

		preGameInitComplete = false;
		readyInitComplete = false;
		normalInitComplete = false;
		powerUpInitComplete = false;
		eatGhostInitComplete = false;
		dieInitComplete = false;
		winInitComplete = false;
		gameOverInitComplete = false;

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
				pacmanRenderer.enabled = false;
				
				preGameInitComplete = false;
				winInitComplete = true;
				
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
				pacmanAudio.Play();

				gameController.gameState = GameController.GameStates.DIE;
			}

			// otherwise, pacman eats the ghost
			else if (gameController.gameState == GameController.GameStates.POWERUP) {

				gameController.numGhostsEaten++;
				gameController.score += (int)((Mathf.Pow(2.0f, gameController.numGhostsEaten)) * 100.0f);

				pacmanAudio.clip = eatGhostAudio;
				pacmanAudio.Play();

				gameController.gameState = GameController.GameStates.EATGHOST;

			}

		}

	}

}