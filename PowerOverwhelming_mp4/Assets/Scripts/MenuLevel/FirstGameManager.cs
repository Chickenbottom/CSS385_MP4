using UnityEngine;
using System.Collections;

// Copied from http://forum.unity3d.com/threads/58609-DontDestroyOnLoad-Leading-to-Double-Objects
// scroll down to posting from JRavey
//
public class FirstGameManager : MonoBehaviour {

	#region support the creation and accesssing of a GlobalGameManager
	// Trick is to create a GlobalGameManager, only once!
	// Since this "GlobalGameManager" will persist over all scenes, it cannot be part of a scene (everything gets destroyed!)
	// So, we create this object the first time and re-use all over
	// 
	private static GlobalGameManager sTheGameState = null;

	private static void CreateGlobalManager()
	{
		GameObject newGameState = new GameObject();
		newGameState.name = "GlobalStateManager";
		newGameState.AddComponent<GlobalGameManager>();
		sTheGameState = newGameState.GetComponent<GlobalGameManager>();
	}

	public static GlobalGameManager TheGameState { get {
		if (null == sTheGameState)
			CreateGlobalManager();
			return sTheGameState;
		} }
	#endregion 	

	void Awake() {
		if (null == sTheGameState) { // not here yet
			CreateGlobalManager();
		}
	}

	// from this point on, ALL objects from all levels should be able to access
	// FirstGameManager.TheGameState

	void Start()
	{
		FirstGameManager.TheGameState.PrintCurrentLevel();
	}
}
