using UnityEngine;
using System.Collections;

public class MenuButtonBehavior : MonoBehaviour {

	public enum MenuButtonState {
		kMenu,
		kResume,
		kQuit
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
			
		case (MenuButtonState.kQuit):
			QuitGame ();
			break;
		}
	}
	
	public void QuitGame()
	{
		Debug.Log ("Quit");
	}
	
	public void ResumeGame()
	{
		Debug.Log ("Resume");
		GlobalBehavior currentLevel = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();
		if (currentLevel == null)
			Debug.Log ("null level");
		currentLevel.ResumeGame();
	}
	
	public void LoadMenu()
	{
		Debug.Log ("Load");
		GameState gameState = GameManager.TheGameState();
		gameState.LoadGameLevel(GameState.GameLevel.kMenu);
	}
}
