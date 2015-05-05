using UnityEngine;
using System.Collections;

public class Pacman : MonoBehaviour {

	public float speed;
	private Vector3 direction;
	private GameObject camera;

	// Use this for initialization
	void Start () {
	
		speed = 0.2f;
		direction = new Vector3(0,0,0);
		camera = GameObject.Find ("Main Camera");

	}

	void FixedUpdate() {

		// move Pac-Man along a velocity vector
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		if (moveHorizontal > 0) direction.Set(speed, 0, 0);
		else if (moveHorizontal < 0) direction.Set(-speed, 0, 0);
		else if (moveVertical > 0) direction.Set(0, 0, speed);
		else if (moveVertical < 0) direction.Set(0, 0, -speed);

		transform.LookAt (transform.position + direction);
		transform.Translate (direction, Space.World);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
