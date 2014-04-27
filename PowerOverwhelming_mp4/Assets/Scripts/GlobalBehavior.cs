using UnityEngine;
using System.Collections;

public class GlobalBehavior : MonoBehaviour {
	
	#region World Bound support
	private Bounds mWorldBound;  // this is the world bound
	private Vector2 mWorldMin;	// Better support 2D interactions
	private Vector2 mWorldMax;
	private Vector2 mWorldCenter;
	private Camera mMainCamera;
	
	private GameState mGameState;
	
	private int mSpawnedShips;
	private int mDestroyedShips;
	
	private int MAX_SHIPS = 150;
	private int cur_ships = 50;		//start with 50
	public bool paused = true;
	private const float PAUSE_INTERVAL = 0.5f;
	private float previous_pause = 0f;
	private string curGame;

//	GameObject background = null;
	#endregion

	#region  support runtime enemy creation
	// to support time ...
	private float mPreEnemySpawnTime = -1f; // 
	private const float kEnemySpawnInterval = 1.0f; // in seconds
	// spwaning enemy ...
	public GameObject mEnemyToSpawn = null;
	#endregion
	
	// Use this for initialization
	void Start () {
		
		
		#region world bound support
		mMainCamera = Camera.main;
		mWorldBound = new Bounds(Vector3.zero, Vector3.one);
		UpdateWorldWindowBound();

		#endregion

		mGameState = GameManager.TheGameState();
		
		#region initialize enemy spawning
		if (null == mEnemyToSpawn) 
			mEnemyToSpawn = Resources.Load("Prefabs/UFO") as GameObject;
		
		mSpawnedShips = mGameState.NumEnemiesInLevel();
		for(int i = 0; i < mSpawnedShips; i++) {
			GameObject e = (GameObject) Instantiate(mEnemyToSpawn);
		}
		#endregion
	}
	
	// Update is called once per frame
	void Update () {
		WorldBehavior worldSheild = GameObject.Find ("mShield").GetComponent<WorldBehavior>();
		
		if (worldSheild.getShieldStatus() > 0 && cur_ships < MAX_SHIPS && !paused) 
		{
			//SpawnAnEnemy();	
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			paused = !paused;
			previous_pause = Time.realtimeSinceStartup;
			}
		GameObject remainingEnemyText = GameObject.Find("EnemyGUIText");
		GUIText gui = remainingEnemyText.GetComponent<GUIText>();
		gui.text = "Remaining Enemy Ships: " + (mSpawnedShips - mDestroyedShips).ToString();

		if (mDestroyedShips >= mSpawnedShips) { // win condition, no remaining enemies
			mGameState.AdvanceLevel();
		}
	}

	
	#region Game Window World size bound support
	public enum WorldBoundStatus {
		CollideTop,
		CollideLeft,
		CollideRight,
		CollideBottom,
		Outside,
		Inside
	};
	
	/// <summary>
	/// This function must be called anytime the MainCamera is moved, or changed in size
	/// </summary>
	public void UpdateWorldWindowBound()
	{
		// get the main 
		if (null != mMainCamera) {
			float maxY = mMainCamera.orthographicSize;
			float maxX = mMainCamera.orthographicSize * mMainCamera.aspect;
			float sizeX = 2 * maxX;
			float sizeY = 2 * maxY;
			float sizeZ = Mathf.Abs(mMainCamera.farClipPlane - mMainCamera.nearClipPlane);
			
			// Make sure z-component is always zero
			Vector3 c = mMainCamera.transform.position;
			c.z = 0.0f;
			mWorldBound.center = c;
			mWorldBound.size = new Vector3(sizeX, sizeY, sizeZ);

			mWorldCenter = new Vector2(c.x, c.y);
			mWorldMin = new Vector2(mWorldBound.min.x, mWorldBound.min.y);
			mWorldMax = new Vector2(mWorldBound.max.x, mWorldBound.max.y);
		}
	}
	
	public Vector2 WorldCenter { get { return mWorldCenter; } }
	public Vector2 WorldMin { get { return mWorldMin; }} 
	public Vector2 WorldMax { get { return mWorldMax; }}
	
	public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
	{
		WorldBoundStatus status = WorldBoundStatus.Inside;
		
		if (mWorldBound.Intersects(objBound)) {
			if (objBound.max.x > mWorldBound.max.x)
				status = WorldBoundStatus.CollideRight;
			else if (objBound.min.x < mWorldBound.min.x)
				status = WorldBoundStatus.CollideLeft;
			else if (objBound.max.y > mWorldBound.max.y)
				status = WorldBoundStatus.CollideTop;
			else if (objBound.min.y < mWorldBound.min.y)
				status = WorldBoundStatus.CollideBottom;
			else if ( (objBound.min.z < mWorldBound.min.z) || (objBound.max.z > mWorldBound.max.z))
				status = WorldBoundStatus.Outside;
		} else 
			status = WorldBoundStatus.Outside;
		return status;
		
	}
	#endregion 

	#region enemy spawning support
	private void SpawnAnEnemy()
	{
		if ((Time.realtimeSinceStartup - mPreEnemySpawnTime) > kEnemySpawnInterval) {
			GameObject e = (GameObject) Instantiate(mEnemyToSpawn);
			mPreEnemySpawnTime = Time.realtimeSinceStartup;
			cur_ships++;
			
			// Debug.Log("New enemy at: " + mPreEnemySpawnTime.ToString());
		}
	}
	#endregion
	public void destroyShip(){
		mDestroyedShips++;	
	}
	public bool getPaused(){
		return paused;
	}
}
