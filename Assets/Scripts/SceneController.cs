using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

	public void loadScene(string sceneName) {

		Application.LoadLevel (sceneName);

	}

	public void exitGame() {

		Application.Quit ();

	}

}
