using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour {

	public Transform pacmanTransform;
	public Material ghostAfraidMaterial;
	public Material ghostDeadMaterial;

	private GameController gameController;

	private MeshRenderer ghostRenderer;
	private BoxCollider ghostCollider;
	private NavMeshAgent ghostAgent;
	private Material origGhostMaterial;

	private Vector3 warp1Translation;
	private Vector3 warp2Translation;

	private Vector3 homePosition;

	public enum GhostState { ALIVE, DEAD };
	private GhostState ghostState;


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
		ghostState = GhostState.ALIVE;

	}

	void FixedUpdate() {

		switch (gameController.gameState) {

		case GameController.GameStates.READY:
			
			transform.position = homePosition;
			ghostState = GhostState.ALIVE;
			
			break;
			
		case GameController.GameStates.NORMAL:

			if (ghostState == GhostState.ALIVE) {

				if (ghostRenderer.material != origGhostMaterial) {
					ghostRenderer.material = origGhostMaterial;
				}
				ghostAgent.speed = 8.0f;
				ghostAgent.SetDestination (pacmanTransform.position);

			}
			else if (ghostState == GhostState.DEAD) {

				ghostAgent.speed = 30.0f;
				ghostAgent.SetDestination (homePosition);
				
				if (ghostAgent.remainingDistance < 0.2f) {
					ghostCollider.enabled = true;
					ghostState = GhostState.ALIVE;
				}

			}

			break;

		case GameController.GameStates.POWERUP:

			if (ghostState == GhostState.ALIVE) {

				if (ghostRenderer.material != ghostAfraidMaterial) {
					ghostRenderer.material = ghostAfraidMaterial;
				}
				ghostAgent.speed = 3.0f;
				ghostAgent.SetDestination (homePosition);

			}
			else if (ghostState == GhostState.DEAD) {

				if (ghostRenderer.material != ghostDeadMaterial) {
					ghostRenderer.material = ghostDeadMaterial;
				}
				ghostAgent.speed = 30.0f;
				ghostAgent.SetDestination (homePosition);
				
				if (ghostAgent.remainingDistance < 0.2f) {
					ghostCollider.enabled = true;
					ghostState = GhostState.ALIVE;
				}
				
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
