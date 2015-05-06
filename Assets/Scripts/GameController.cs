using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public AudioClip preGameAudio;
	public AudioClip normalAudio;
	public AudioClip powerUpAudio;

	public enum GameStates {PREGAME, NORMAL, POWERUP, WIN, GAMEOVER};
	public GameStates gameState;

	private AudioSource gameAudio;
	private float audioMaxLength;
	private float audioDuration;
	private bool preGameInitComplete;
	private bool normalInitComplete;
	private bool powerUpInitComplete;

	// Use this for initialization
	void Start () {

		gameAudio = GetComponent<AudioSource> ();
		gameState = GameStates.PREGAME;
		audioMaxLength = 5;
		audioDuration = 0;
		preGameInitComplete = false;
		normalInitComplete = false;
		powerUpInitComplete = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		switch (gameState) {

		case GameStates.PREGAME:

			if (!preGameInitComplete) {

				gameAudio.clip = preGameAudio;
				gameAudio.Play();
				preGameInitComplete = true;

			} else if (!gameAudio.isPlaying) {

				normalInitComplete = false;
				gameState = GameStates.NORMAL;

			}

			break;

		case GameStates.NORMAL:

			if (!normalInitComplete) {
				
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
					gameState = GameStates.NORMAL;
					
				}
			
			}

			break;

		case GameStates.WIN:

			break;

		case GameStates.GAMEOVER:

			break;

		}

	}
}
