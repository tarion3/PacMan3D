using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	private Vector3 halfVector;
	
	// Use this for initialization
	void Start () {
		
		offset = transform.position - player.transform.position;
		halfVector = new Vector3(0.5f, 1.0f, 1.0f);

	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		transform.position = Vector3.Scale(player.transform.position + offset, halfVector);

	}

}
