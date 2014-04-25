using UnityEngine;
using System.Collections;

public class LoadLevelOnMouse : MonoBehaviour {

	public string levelName = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp() {
		Debug.Log (levelName);
		Application.LoadLevel (levelName);
	}

}
