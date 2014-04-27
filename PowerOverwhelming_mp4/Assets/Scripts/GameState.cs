using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour{
	private GameLevel mGameLevel;
	private static int[] mNumEnemies;

	public int Score { get { return mScore; } }
	private int mScore = 0;
	
	public enum GameLevel {
		kMenu = 0,
		kLevel1 = 1,
		kLevel2 = 2
	}

	public void AddToScore(int value)
	{
		mScore += value;
	}

	public int NumEnemiesInLevel()
	{
		return mNumEnemies[(int)mGameLevel];
	}
	
	public void LoadGameLevel(GameLevel level)
	{
		mGameLevel = level;
				
		switch (level) {
		case(GameLevel.kMenu):
			Application.LoadLevel("MenuLevel");
			break;
		case(GameLevel.kLevel1):
			Application.LoadLevel("PowerOverwhelming_mp4_level1");
			break;
		case(GameLevel.kLevel2):
			Application.LoadLevel("PowerOverwhelming_mp4_level2");
			break;
		}
	}
	
	public string GetLevelName()
	{
		switch (mGameLevel) {
		case(GameLevel.kLevel1):
			return "Level 1";
			break;
		case(GameLevel.kLevel2):
			return "Level 2";
			break;
		default:
			return "";
			break;
		}
	}
	
	public void AdvanceLevel()
	{
		GameLevel level;
		
		switch (mGameLevel) {
		case(GameLevel.kMenu):
			level = GameLevel.kLevel1;
			break;
		case(GameLevel.kLevel1):
			level = GameLevel.kLevel2;
			break;
		case(GameLevel.kLevel2):
			level = GameLevel.kMenu;
			break;
		default:
			level = GameLevel.kMenu;
			break;
		}
	
		LoadGameLevel(level);
	}
	
	void Start()
	{
		DontDestroyOnLoad(this);
		
		if (mNumEnemies == null) {
			mNumEnemies = new int[3];
			mNumEnemies[(int)GameLevel.kMenu] = 0;
			mNumEnemies[(int)GameLevel.kLevel1] = 3;
			mNumEnemies[(int)GameLevel.kLevel2] = 20;
		}
	}
}

