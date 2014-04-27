using UnityEngine;
using System.Collections;

public class LoadLevelOnMouse : MonoBehaviour {
	
	public GameState.GameLevel mGameLevel;
	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp() {
		GameState gameState = GameManager.TheGameState();
		gameState.LoadGameLevel(mGameLevel);
	}

}
