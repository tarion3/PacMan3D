using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	private Vector3 halfVector;
	
	// Use this for initialization
	void Start () {
		
		offset = transform.position - player.transform.position;
		halfVector = new Vector3(0.5f, 1.0f, 0.5f);

	}
	
	// Update is called once per frame
	void LateUpdate () {

		// move the camera at half the speed of pacman
		// in order to get a more smooth feeling game board movement
		transform.position = Vector3.Scale(player.transform.position + offset, halfVector);

	}

}
