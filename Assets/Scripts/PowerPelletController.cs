using UnityEngine;
using System.Collections;

public class PowerPelletController : MonoBehaviour {

	public AudioClip pelletEatAudio;

	private MeshRenderer pelletRenderer;
	private SphereCollider pelletCollider;
	private AudioSource pelletAudio;

	// Use this for initialization
	void Start () {
		
		pelletRenderer = GetComponent<MeshRenderer> ();
		pelletCollider = GetComponent<SphereCollider> ();
		pelletAudio = GetComponent<AudioSource> ();

	}

	void FixedUpdate() {

	}
	
	void OnTriggerEnter(Collider other) {

		// play pellet eat audio
		pelletAudio.clip = pelletEatAudio;
		pelletAudio.Play();

		// trigger game state change
		GameObject.Find ("Game Controller").GetComponent<GameController> ().gameState = GameController.GameStates.POWERUP;

		// remove pellet
		pelletRenderer.enabled = false;
		pelletCollider.enabled = false;
		
	}

}
