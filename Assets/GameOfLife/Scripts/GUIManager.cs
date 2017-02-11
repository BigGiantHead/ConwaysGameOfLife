using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {	
	private LifeManager manager = null;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Scripts").GetComponent<LifeManager> ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		if (GUI.Button (new Rect (10, 10, 150, 100), manager.Paused ? "Play" : "Pause")) {
			manager.Paused = !manager.Paused;
		}

		if (manager.Paused && GUI.Button (new Rect (10, 110, 150, 100), "Step")) {
			manager.ExecuteStep ();
		}
	}
}
