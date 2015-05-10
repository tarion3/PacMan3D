﻿using UnityEngine;
using System.Collections;

public class PelletController : MonoBehaviour {

	private GameController gameController;
	private MeshRenderer pelletRenderer;
	private SphereCollider pelletCollider;

	private bool preGameInitComplete;
	private bool winInitComplete;
	private bool gameOverInitComplete;

	// Use this for initialization
	void Start () {
	
		gameController = GameObject.Find ("Game Controller").GetComponent<GameController> ();
		pelletRenderer = GetComponent<MeshRenderer> ();
		pelletCollider = GetComponent<SphereCollider> ();

		preGameInitComplete = false;
		winInitComplete = false;
		gameOverInitComplete = false;

	}

	void FixedUpdate() {

		switch (gameController.gameState) {
			
		case GameController.GameStates.PREGAME:

			if (!preGameInitComplete) {

				pelletRenderer.enabled = true;
				pelletCollider.enabled = true;

				gameOverInitComplete = false;
				preGameInitComplete = true;

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

		if (other.gameObject.CompareTag ("PacMan")) {

			pelletRenderer.enabled = false;
			pelletCollider.enabled = false;

		}

	}

}
