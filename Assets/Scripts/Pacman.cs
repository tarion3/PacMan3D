using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pacman : MonoBehaviour {

	public Text countText;
	public Text scoreText;
	public Text winText;

	private Vector3 direction;
	private Vector3 warp1Translation;
	private Vector3 warp2Translation;

	private float speed;
	private int count;
	private int score;
	
	// Use this for initialization
	void Start () {
	
		speed = 0.18f;
		direction = new Vector3(0,0,0);
		warp1Translation = new Vector3 (28,0,0);
		warp2Translation = new Vector3 (-28,0,0);

		count = 0;
		score = 0;

		SetCountText();
		SetScoreText ();
		winText.text = "";

	}

	void FixedUpdate() {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		if (moveHorizontal > 0) direction.Set(speed, 0, 0);
		else if (moveHorizontal < 0) direction.Set(-speed, 0, 0);
		else if (moveVertical > 0) direction.Set(0, 0, speed);
		else if (moveVertical < 0) direction.Set(0, 0, -speed);

		transform.LookAt (transform.position + direction);
		transform.Translate (direction, Space.World);

	}
	
	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("Pellet")) {

			Pickup (other, 5);

		} else if (other.gameObject.CompareTag ("PowerPellet")) {

			Pickup (other, 10);

		} else if (other.gameObject.CompareTag ("WarpTunnel1")) {
			transform.Translate(warp1Translation, Space.World);
		} else if (other.gameObject.CompareTag ("WarpTunnel2")) {
			transform.Translate(warp2Translation, Space.World);
		}

	}

	void Pickup(Collider other, int points) {
		count++;
		SetCountText();
		score += points;
		SetScoreText();
	}
	
	void SetCountText() {
		countText.text = "Count: " + count.ToString();
		if (count >= 156) {
			winText.text = "You Win!";
		}
	}

	void SetScoreText() {
		scoreText.text = "Score: " + score.ToString();
	}
	
}
