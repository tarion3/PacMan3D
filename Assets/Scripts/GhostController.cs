using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour {

	public bool preGameInitComplete;
	public bool readyInitComplete;
	public bool normalInitComplete;
	public bool powerUpInitComplete;
	public bool eatGhostInitComplete;
	public bool dieInitComplete;
	public bool winInitComplete;
	public bool gameOverInitComplete;

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
	private float chaseTimer;

	public enum GhostState { ALIVE, DEAD };
	private GhostState ghostState;

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
		
		ghostRenderer = GetComponentInChildren<MeshRenderer> ();
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
		afraidSpeed = 2.0f;
		deadSpeed = 20.0f;
		distanceFromPacman = 0;
		distanceFromDestination = 0;
		reachedGhostDestination = false;
		chaseTimer = 0;

		ghostState = GhostState.ALIVE;

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
				chaseTimer = 0;

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
					ghostAgent.speed = normalSpeed * 1.5f;
				} else {
					ghostAgent.speed = normalSpeed;
				}

				distanceFromDestination = (transform.position - ghostDestination).magnitude;
				distanceFromPacman = (transform.position - pacmanTransform.position).magnitude;

				if (distanceFromPacman < 5.0f || reachedGhostDestination) {

					if (ghostAgent.destination != pacmanTransform.position) {
						ghostAgent.SetDestination(pacmanTransform.position);
					}

					chaseTimer += Time.deltaTime;

					if (chaseTimer >= 10.0f) {
						chaseTimer = 0;
						reachedGhostDestination = false;
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

			if (ghostAgent.velocity.magnitude == 0) {
				ghostAgent.Resume();
			}

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

			}

			break;

		case GameController.GameStates.EATGHOST:
			
			if (!eatGhostInitComplete) {

				ghostAgent.Stop();

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

		//transform.LookAt (ghostAgent.destination);

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
