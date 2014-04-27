using UnityEngine;
using System.Collections;

public class ContinueMenuBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void Update() {}
	
	void OnMouseUp() 
	{
		GameState gameState = GameManager.TheGameState();
		gameState.AdvanceLevel ();
	}
}
