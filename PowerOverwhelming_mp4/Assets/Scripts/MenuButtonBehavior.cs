using UnityEngine;
using System.Collections;

public class MenuButtonBehavior : MonoBehaviour {

	public enum MenuButtonState {
		kMenu,
		kResume
	}
	
	public MenuButtonState MenuButton;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUp() 
	{
		switch (MenuButton) {
		case (MenuButtonState.kMenu):
			LoadMenu ();
			break;
			
		case (MenuButtonState.kResume):
			ResumeGame ();
			break;
		}
	}
	
	public void ResumeGame()
	{
		GlobalBehavior currentLevel = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();
		currentLevel.ResumeGame();
	}
	
	public void LoadMenu()
	{
		GameState gameState = GameManager.TheGameState();
		gameState.LoadGameLevel(GameState.GameLevel.kMenu);
	}
}
