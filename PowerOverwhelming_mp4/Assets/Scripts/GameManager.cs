using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{	
	private static GameState sGameState = null;
	
		
	public void Awake()
	{
		InitializeGameState();
	}
	
	public static GameState TheGameState()
	{
		InitializeGameState();
		return sGameState;
	}
	
	private static void InitializeGameState()
	{
		if (sGameState != null)
			return;
		
		GameObject newGameState = new GameObject();
		newGameState.AddComponent<GameState>();
		sGameState = newGameState.GetComponent<GameState>();
	}
}
