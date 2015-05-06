using UnityEngine;
using System.Collections;

public class PelletController : MonoBehaviour {

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
	
	void OnTriggerEnter(Collider other) {

		pelletAudio.clip = pelletEatAudio;
		pelletAudio.Play();

		pelletRenderer.enabled = false;
		pelletCollider.enabled = false;

	}

}
