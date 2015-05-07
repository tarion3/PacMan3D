using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public AudioClip preGameAudio;
	public AudioClip normalAudio;
	public AudioClip powerUpAudio;

	public enum GameStates {PREGAME, READY, NORMAL, POWERUP, GAMEOVER};
	public GameStates gameState;

	public Text countText;
	public Text scoreText;
	public Text livesText;
	public Text winText;

	public int count;
	public int score;
	public int lives;
	private int oldCount, oldScore, oldLives, maxCount;

	public AudioSource gameAudio;
	private float audioMaxLength;
	private float audioDuration;

	private bool preGameInitComplete;
	private bool readyInitComplete;
	private bool normalInitComplete;
	private bool powerUpInitComplete;
	private bool gameOverInitComplete;

	private float elapsedTime;
	private bool skipReadyWait;

	// Use this for initialization
	void Start () {

		gameAudio = GetComponent<AudioSource> ();
		gameState = GameStates.PREGAME;

		oldCount = 0;
		oldScore = 0;
		oldLives = 3;
		count = 0;
		score = 0;
		lives = 3;
		maxCount = 156;

		SetCountText("0");
		SetScoreText("0");
		SetLivesText("3");
		SetWinText("");

		audioMaxLength = 8;
		audioDuration = 0;

		preGameInitComplete = false;
		readyInitComplete = false;
		normalInitComplete = false;
		powerUpInitComplete = false;
		gameOverInitComplete = false;

		elapsedTime = 0;
		skipReadyWait = false;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (count != oldCount) {
			SetCountText (count.ToString ());
		}

		if (score != oldScore) {
			SetScoreText (score.ToString ());
		}

		if (lives != oldLives) {
			SetLivesText (lives.ToString ());
		}
		
		switch (gameState) {

		case GameStates.PREGAME:

			if (!preGameInitComplete) {

				ResetGameBoard();
				lives = 3;

				SetWinText("Ready?");
				gameAudio.clip = preGameAudio;
				gameAudio.loop = false;
				gameAudio.Play();

				preGameInitComplete = true;

			} else if (!gameAudio.isPlaying) {

				skipReadyWait = true;

				preGameInitComplete = false;
				readyInitComplete = false;
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
					SetWinText("Ready?");
				}

				readyInitComplete = true;

			}

			elapsedTime += Time.deltaTime;

			if (elapsedTime > 2) {

				elapsedTime = 0;
				readyInitComplete = false;
				normalInitComplete = false;
				gameState = GameStates.NORMAL;

			}
			
			break;

		case GameStates.NORMAL:

			if (!normalInitComplete) {

				SetWinText("");
				gameAudio.clip = normalAudio;
				gameAudio.loop = true;
				gameAudio.Play();
				normalInitComplete = true;
				
			}

			break;

		case GameStates.POWERUP:

			if (!powerUpInitComplete) {

				gameAudio.clip = powerUpAudio;
				gameAudio.loop = true;
				gameAudio.Play();
				powerUpInitComplete = true;

			} else {

				audioDuration += Time.deltaTime;
				
				// stop audio and trigger game state change
				if (audioDuration >= audioMaxLength) {
					
					// stop audio
					audioDuration = 0;
					gameAudio.Stop ();
					
					// trigger game state change
					powerUpInitComplete = false;
					normalInitComplete = false;
					readyInitComplete = false;
					gameState = GameStates.NORMAL;
					
				}
			
			}

			break;

		case GameStates.GAMEOVER:

			if (!gameOverInitComplete) {

				if (count >= maxCount) {
					SetWinText ("You Win!");
				}
				else {
					SetWinText("GAME OVER");
				}

				gameAudio.Stop ();
				elapsedTime = 0;
				gameOverInitComplete = true;

			}

			elapsedTime += Time.deltaTime;

			if (elapsedTime > 5) {
				gameOverInitComplete = false;
				preGameInitComplete = false;
				gameState = GameStates.PREGAME;
			}

			break;

		}

	}

	void SetCountText(string text) {
		countText.text = "Count: " + text + " / " + maxCount.ToString();
		if (count >= maxCount) {
			gameState = GameStates.GAMEOVER;
		}
	}
	
	void SetScoreText(string text) {
		scoreText.text = "Score: " + text;
	}

	void SetLivesText(string text) {
		livesText.text = "Lives: " + text;
	}
	
	void SetWinText(string text) {
		winText.text = text;
	}

	void ResetGameBoard() {

		// redisplay all pellets
		GameObject[] pellets = GameObject.FindGameObjectsWithTag ("Pellet");
		foreach (GameObject pellet in pellets) {
			pellet.SetActive(true);
		}

		// redisplay all power pellets
		GameObject[] powerPellets = GameObject.FindGameObjectsWithTag ("PowerPellet");
		foreach (GameObject powerPellet in powerPellets) {
			powerPellet.SetActive(true);
		}

	}

}
