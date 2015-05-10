﻿using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour {

	public Transform pacmanTransform;
	public Material ghostAfraidMaterial;
	public Material ghostRisingMaterial;
	public Material ghostDeadMaterial;

	private GameController gameController;

	private MeshRenderer ghostRenderer;
	private BoxCollider ghostCollider;
	private NavMeshAgent ghostAgent;
	private Material origGhostMaterial;

	private Vector3 warp1Translation;
	private Vector3 warp2Translation;

	private Vector3 homePosition;
	private Vector3 ghostDestination;

	private float normalSpeed, afraidSpeed, deadSpeed;
	private float distanceFromPacman;
	private float distanceFromDestination;
	private bool reachedGhostDestination;

	public enum GhostState { ALIVE, DEAD };
	private GhostState ghostState;

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
		
		ghostRenderer = GetComponent<MeshRenderer> ();
		ghostCollider = GetComponent<BoxCollider> ();
		ghostAgent = GetComponent<NavMeshAgent> ();
		origGhostMaterial = ghostRenderer.material;

		warp1Translation = new Vector3 (28,0,0);
		warp2Translation = new Vector3 (-28,0,0);

		homePosition = transform.position;

		if (name == "Inky") ghostDestination = new Vector3(10, 1, -15);
		if (name == "Blinky") ghostDestination = new Vector3 (10, 1, 13);
		if (name == "Pinky") ghostDestination = new Vector3 (-10, 1, 13);
		if (name == "Clyde") ghostDestination = new Vector3 (-10, 1, -15);

		normalSpeed = 4.0f;
		afraidSpeed = 1.0f;
		deadSpeed = 10.0f;
		distanceFromPacman = 0;
		distanceFromDestination = 0;
		reachedGhostDestination = false;

		ghostState = GhostState.ALIVE;

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

					normalSpeed += (normalSpeed / 4.0f);
					afraidSpeed += (afraidSpeed / 4.0f);
					deadSpeed += (deadSpeed / 4.0f);

				}

				readyInitComplete = false;
				preGameInitComplete = true;

			}

			goto case GameController.GameStates.READY;

		case GameController.GameStates.READY:

			if (!readyInitComplete) {
			
				ghostAgent.enabled = false;
				transform.position = homePosition;
				ghostState = GhostState.ALIVE;
				ghostAgent.enabled = true;

				if (ghostRenderer.material != origGhostMaterial) {
					ghostRenderer.material = origGhostMaterial;
				}

				normalInitComplete = false;
				readyInitComplete = true;

			}
			
			break;
			
		case GameController.GameStates.NORMAL:

			if (!normalInitComplete) {

				reachedGhostDestination = false;

				dieInitComplete = false;
				powerUpInitComplete = false;
				winInitComplete = false;
				gameOverInitComplete = false;
				normalInitComplete = true;

			}

			if (ghostState == GhostState.ALIVE) {

				if (ghostRenderer.material != origGhostMaterial) {
					ghostRenderer.material = origGhostMaterial;
				}

				if (name == "Blinky" && gameController.pelletCount > gameController.pelletCountMax * 3.0f/4.0f) {
					ghostAgent.speed = normalSpeed * 2;
				} else {
					ghostAgent.speed = normalSpeed;
				}

				distanceFromDestination = (transform.position - ghostDestination).magnitude;
				distanceFromPacman = (transform.position - pacmanTransform.position).magnitude;

				if (distanceFromPacman < 5.0f || reachedGhostDestination) {
					if (ghostAgent.destination != pacmanTransform.position) {
						ghostAgent.SetDestination(pacmanTransform.position);
					}
				} else {
					if (ghostAgent.destination != ghostDestination) {
						ghostAgent.SetDestination (ghostDestination);
					}
					if (distanceFromDestination < 2.0f) {
						reachedGhostDestination = true;
					}
				}

			}
			else if (ghostState == GhostState.DEAD) {

				if (ghostRenderer.material != ghostDeadMaterial) {
					ghostRenderer.material = ghostDeadMaterial;
				}
				ghostAgent.speed = deadSpeed;
				ghostAgent.SetDestination (homePosition);
				
				if (ghostAgent.remainingDistance < 1.0f) {

					ghostCollider.enabled = true;
					ghostState = GhostState.ALIVE;

					if (ghostRenderer.material != origGhostMaterial) {
						ghostRenderer.material = origGhostMaterial;
					}

				}

			}

			break;

		case GameController.GameStates.POWERUP:

			if (!powerUpInitComplete) {

				normalInitComplete = false;
				winInitComplete = false;
				gameOverInitComplete = false;
				powerUpInitComplete = true;
				
			}

			eatGhostInitComplete = false;

			if (ghostState == GhostState.ALIVE) {

				float durationFraction = gameController.powerUpDuration - (int)gameController.powerUpDuration;

				if (gameController.powerUpDuration > gameController.powerUpMaxDuration / 2.0f && durationFraction > 0.50) {

					if (ghostRenderer.material != ghostRisingMaterial) {
						ghostRenderer.material = ghostRisingMaterial;
					}

				} else {

					if (ghostRenderer.material != ghostAfraidMaterial) {
						ghostRenderer.material = ghostAfraidMaterial;
					}

				}

				ghostAgent.speed = afraidSpeed;
				ghostAgent.SetDestination (-pacmanTransform.position);

			}
			else if (ghostState == GhostState.DEAD) {

				if (ghostRenderer.material != ghostDeadMaterial) {
					ghostRenderer.material = ghostDeadMaterial;
				}
				ghostAgent.speed = deadSpeed;
				ghostAgent.SetDestination (homePosition);

				if (ghostAgent.remainingDistance < 1.0f) {
					
					ghostCollider.enabled = true;
					ghostState = GhostState.ALIVE;
					
					if (ghostRenderer.material != origGhostMaterial) {
						ghostRenderer.material = origGhostMaterial;
					}
					
				}

			}

			break;

		case GameController.GameStates.EATGHOST:
			
			if (!eatGhostInitComplete) {
				
				eatGhostInitComplete = true;
				
			}
			
			break;

		case GameController.GameStates.DIE:
			
			if (!dieInitComplete) {
				
				readyInitComplete = false;
				gameOverInitComplete = false;
				dieInitComplete = true;
				
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

		if (other.gameObject.CompareTag ("WarpTunnel1")) {
			
			transform.Translate (warp1Translation, Space.World);
			
		} else if (other.gameObject.CompareTag ("WarpTunnel2")) {
			
			transform.Translate (warp2Translation, Space.World);
			
		} else if (other.gameObject.CompareTag ("PacMan")) {

			if (gameController.gameState == GameController.GameStates.POWERUP) {

				ghostState = GhostState.DEAD;
				ghostCollider.enabled = false;

			}

		}
		
	}

}
