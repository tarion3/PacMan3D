using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public AudioClip preGameAudio;
	public AudioClip normalAudio;
	public AudioClip powerUpAudio;
	public AudioClip powerUpEndingAudio;
	public AudioSource gameAudio;

	public enum GameStates {PREGAME, READY, NORMAL, POWERUP, EATGHOST, DIE, WIN, GAMEOVER};
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

	public float preGameDuration;
	public float preGameMaxDuration;

	public float readyDuration;
	public float readyMaxDuration;

	public float powerUpDuration;
	public float powerUpMaxDuration;

	public float eatGhostDuration;
	public float eatGhostMaxDuration;

	public float dieDuration;
	public float dieMaxDuration;

	public float winDuration;
	public float winMaxDuration;

	public float gameOverDuration;
	public float gameOverMaxDuration;
	
	private bool preGameInitComplete;
	private bool readyInitComplete;
	private bool normalInitComplete;
	private bool powerUpInitComplete;
	private bool eatGhostInitComplete;
	private bool dieInitComplete;
	private bool winInitComplete;
	private bool gameOverInitComplete;

	private bool skipReadyWait;

	public int numGhostsEaten;

	// Use this for initialization
	void Start () {

		gameAudio = GetComponent<AudioSource> ();
		gameState = GameStates.PREGAME;

		level = 1;

		score = 0;
		lives = 3;

		pelletCount = 0;
		pelletCountMax = 156;

		oldScore = 0;
		oldLives = 3;
		oldPelletCount = 0;
		
		SetPelletCountText(pelletCount.ToString());
		SetScoreText(score.ToString());
		SetLivesText(lives.ToString());
		SetWinText("");

		preGameDuration = 0;
		preGameMaxDuration = 4.3f;

		readyDuration = 0;
		readyMaxDuration = 2.0f;
		
		powerUpDuration = 0;
		powerUpMaxDuration = 10.0f;

		eatGhostDuration = 0;
		eatGhostMaxDuration = 1.0f;

		dieDuration = 0;
		dieMaxDuration = 1.7f;

		winDuration = 0;
		winMaxDuration = 2.0f;

		gameOverDuration = 0;
		gameOverMaxDuration = 5.0f;

		preGameInitComplete = false;
		readyInitComplete = false;
		normalInitComplete = false;
		powerUpInitComplete = false;
		eatGhostInitComplete = false;
		dieInitComplete = false;
		winInitComplete = false;
		gameOverInitComplete = false;

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
		}

		if (pelletCount >= pelletCountMax && gameState != GameStates.PREGAME) {
			gameState = GameStates.WIN;
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
				preGameDuration = 0;

				preGameInitComplete = true;

			}

			preGameDuration += Time.deltaTime;

			if (preGameDuration >= preGameMaxDuration) {
				SetWinText("");
				gameState = GameStates.READY;
			}

			break;

		case GameStates.READY:

			if (!readyInitComplete) {

				if (!skipReadyWait) {
					readyDuration = 0;
					SetWinText(lives.ToString() + " Lives Left\nGet Ready!");
				}

				normalInitComplete = false;
				readyInitComplete = true;

			}

			readyDuration += Time.deltaTime;

			if (skipReadyWait || readyDuration >= readyMaxDuration) {
				SetWinText("");
				skipReadyWait = false;
				gameState = GameStates.NORMAL;
			}
			
			break;

		case GameStates.NORMAL:

			if (!normalInitComplete) {

				gameAudio.clip = normalAudio;
				gameAudio.loop = true;
				gameAudio.Play();

				powerUpInitComplete = false;
				dieInitComplete = false;
				winInitComplete = false;
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
				winInitComplete = false;
				gameOverInitComplete = false;
				powerUpInitComplete = true;

			}

			eatGhostInitComplete = false;
			powerUpDuration += Time.deltaTime;

			if (powerUpDuration >= powerUpMaxDuration / 2.0f && gameAudio.clip != powerUpEndingAudio) {

				gameAudio.clip = powerUpEndingAudio;
				gameAudio.loop = true;
				gameAudio.Play ();

			}

			if (powerUpDuration >= powerUpMaxDuration) {

				gameAudio.Stop ();
				gameState = GameStates.NORMAL;
				
			}
			
			break;

		case GameStates.EATGHOST:

			if (!eatGhostInitComplete) {

				eatGhostDuration = 0;
				eatGhostInitComplete = true;

			}

			eatGhostDuration += Time.deltaTime;

			if (eatGhostDuration >= eatGhostMaxDuration) {
				gameState = GameStates.POWERUP;
			}

			break;

		case GameStates.DIE:

			if (!dieInitComplete) {

				gameAudio.Stop ();

				dieDuration = 0;

				readyInitComplete = false;
				gameOverInitComplete = false;
				dieInitComplete = true;

			}

			dieDuration += Time.deltaTime;

			if (dieDuration >= dieMaxDuration) {

				lives--;

				if (lives == 0) gameState = GameStates.GAMEOVER;
				else gameState = GameStates.READY;

			}
			
			break;

		case GameStates.WIN:
			
			if (!winInitComplete) {

				SetWinText ("Level " + level.ToString() + "\nCleared!");
				gameAudio.Stop ();

				winDuration = 0;

				preGameInitComplete = false;
				winInitComplete = true;
				
			}
			
			winDuration += Time.deltaTime;
			
			if (winDuration >= winMaxDuration) {
				level++;
				SetWinText("");
				gameState = GameStates.PREGAME;
			}
			
			break;

		case GameStates.GAMEOVER:

			if (!gameOverInitComplete) {

				SetWinText("GAME OVER\nScore: " + score.ToString());
				gameAudio.Stop ();

				gameOverDuration = 0;

				preGameInitComplete = false;
				gameOverInitComplete = true;

			}

			gameOverDuration += Time.deltaTime;

			if (gameOverDuration >= gameOverMaxDuration) {
				level = 1;
				SetWinText("");
				gameState = GameStates.PREGAME;
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
