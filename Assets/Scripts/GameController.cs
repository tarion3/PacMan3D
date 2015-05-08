﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public AudioClip preGameAudio;
	public AudioClip normalAudio;
	public AudioClip powerUpAudio;
	public AudioSource gameAudio;

	public enum GameStates {PREGAME, READY, NORMAL, POWERUP, DIE, GAMEOVER};
	public GameStates gameState;

	public Text pelletCountText;
	public Text scoreText;
	public Text livesText;
	public Text winText;

	public int pelletCount;
	public int pelletCountMax;
	public int score;
	public int lives;
	public int level;

	private int oldPelletCount, oldScore, oldLives;

	public float powerUpMaxLength;
	public float powerUpDuration;

	private bool preGameInitComplete;
	private bool readyInitComplete;
	private bool normalInitComplete;
	private bool powerUpInitComplete;
	private bool dieInitComplete;
	private bool gameOverInitComplete;

	private float elapsedTime;
	private bool skipReadyWait;

	public int numGhostsEaten;

	// Use this for initialization
	void Start () {

		gameAudio = GetComponent<AudioSource> ();
		gameState = GameStates.PREGAME;

		oldPelletCount = 0;
		oldScore = 0;
		oldLives = 3;
		pelletCount = 0;
		score = 0;
		lives = 3;
		level = 1;
		pelletCountMax = 156;

		SetPelletCountText("0");
		SetScoreText("0");
		SetLivesText("3");
		SetWinText("");

		powerUpMaxLength = 8;
		powerUpDuration = 0;

		preGameInitComplete = false;
		readyInitComplete = false;
		normalInitComplete = false;
		powerUpInitComplete = false;
		dieInitComplete = false;
		gameOverInitComplete = false;

		elapsedTime = 0;
		skipReadyWait = false;

		numGhostsEaten = 0;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (pelletCount != oldPelletCount) {
			oldPelletCount = pelletCount;
			SetPelletCountText (pelletCount.ToString ());
		}

		if (score != oldScore) {
			oldScore = score;
			SetScoreText (score.ToString ());
		}

		if (lives != oldLives) {

			oldLives = lives;
			SetLivesText (lives.ToString ());

			if (lives == 0) {
				gameOverInitComplete = false;
				gameState = GameStates.GAMEOVER;
			} else if (lives < 3) {
				readyInitComplete = false;
				gameState = GameStates.READY;
			}

		}
		
		switch (gameState) {

		case GameStates.PREGAME:

			if (!preGameInitComplete) {

				pelletCount = 0;

				if (level == 1) {
					lives = 3;
					score = 0;
				}

				SetWinText("Level " + level.ToString() + "\nGet Ready!");
				SetScoreText (score.ToString ());

				gameAudio.clip = preGameAudio;
				gameAudio.loop = false;
				gameAudio.Play();

				readyInitComplete = false;
				skipReadyWait = true;

				preGameInitComplete = true;

			} else if (!gameAudio.isPlaying) {

				gameState = GameStates.READY;

			}

			break;

		case GameStates.READY:

			if (!readyInitComplete) {

				if (skipReadyWait) {
					elapsedTime = 2;
					skipReadyWait = false;
				} else {
					elapsedTime = 0;
					SetWinText(lives.ToString() + " Lives Left\nGet Ready!");
				}

				normalInitComplete = false;
				readyInitComplete = true;

			}

			elapsedTime += Time.deltaTime;

			if (elapsedTime > 2) {

				elapsedTime = 0;
				gameState = GameStates.NORMAL;

			}
			
			break;

		case GameStates.NORMAL:

			if (!normalInitComplete) {

				SetWinText("");
				gameAudio.clip = normalAudio;
				gameAudio.loop = true;
				gameAudio.Play();

				powerUpInitComplete = false;
				dieInitComplete = false;
				gameOverInitComplete = false;
				normalInitComplete = true;
				
			}

			break;

		case GameStates.POWERUP:

			if (!powerUpInitComplete) {

				gameAudio.clip = powerUpAudio;
				gameAudio.loop = true;
				gameAudio.Play();

				powerUpDuration = 0;
				numGhostsEaten = 0;

				normalInitComplete = false;
				gameOverInitComplete = false;
				powerUpInitComplete = true;

			} else {

				powerUpDuration += Time.deltaTime;
				
				// stop audio and trigger game state change
				if (powerUpDuration >= powerUpMaxLength) {
					
					// stop audio
					gameAudio.Stop ();
					
					// trigger game state change
					gameState = GameStates.NORMAL;
					
				}
			
			}

			break;

		case GameStates.DIE:

			if (!dieInitComplete) {

				readyInitComplete = false;
				gameOverInitComplete = false;

				dieInitComplete = true;

			}

			break;

		case GameStates.GAMEOVER:

			if (!gameOverInitComplete) {

				if (pelletCount >= pelletCountMax) {
					SetWinText ("Level " + level.ToString() + "\nCleared!");
					level++;
				}
				else {
					SetWinText("GAME OVER\nScore: " + score.ToString());
					level = 1;
				}

				gameAudio.Stop ();
				elapsedTime = 0;

				preGameInitComplete = false;
				gameOverInitComplete = true;

			} else {

				elapsedTime += Time.deltaTime;

				if (elapsedTime > 5) {
					gameState = GameStates.PREGAME;
				}

			}

			break;

		}

	}

	void SetPelletCountText(string text) {
		pelletCountText.text = "Pellets: " + text + " / " + pelletCountMax.ToString();
		if (pelletCount >= pelletCountMax) {
			gameState = GameStates.GAMEOVER;
		}
	}
	
	void SetScoreText(string text) {
		scoreText.text = "Level " + level + ", Score: " + text;
	}

	void SetLivesText(string text) {
		livesText.text = "Lives: " + text;
	}
	
	void SetWinText(string text) {
		winText.text = text;
	}

}
