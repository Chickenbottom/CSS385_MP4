using UnityEngine;
using System.Collections;

public class SpriteCharacterBehavior : MonoBehaviour {

	// we will assume this element is defined 
	private SpriteSheetManager mSpriteManager;

	// per action sprite animation information 
	private const float kWalkPeriod = 0.25f; // 0.25 second
	private const float kRunPeriod = 0.1f;  // 0.1f second update interveral
	private SpriteActionDefinition mWalkRight = new SpriteActionDefinition(7, 9, 0, 9, 0.05f, false); // Row, BeginColumn, EndColumn
	//private SpriteActionDefinition mWalkLeft = new SpriteActionDefinition(0, 9, 3, kWalkPeriod, true);
	//private SpriteActionDefinition mRightStop = new SpriteActionDefinition(1, 0, 0, kWalkPeriod, false); // to stop
	//private SpriteActionDefinition mLeftStop = new SpriteActionDefinition(0, 0, 0, kWalkPeriod, false); // to stop
		
	void Start () {
		mSpriteManager = GetComponent<SpriteSheetManager>();
		if (null != mSpriteManager) 
			mSpriteManager.SetSpriteAnimationAciton(mWalkRight);
	}
	
	// Update is called once per frame
	const float kWalkSpeed = 10f;
	const float kRunSpeed = 20f;
	float mSpeed = kWalkSpeed;
	void Update () {
		
		Vector3 p = transform.forward * Input.GetAxis("Vertical") * mSpeed * Time.smoothDeltaTime + 
					transform.right * Input.GetAxis("Horizontal") * mSpeed * Time.smoothDeltaTime;
		transform.Translate(p);

		if (null == mSpriteManager)
			return;

		// change speed, notice, the AnimationAction object is local, and yet
		// changing these value will cause the animation behavior to change
		// SpriteManager creates a reference to our copy of the action
		/*if (Input.GetKeyDown(KeyCode.UpArrow)) {
			mWalkRight.mUpdatePeriod = kRunPeriod;
			mWalkLeft.mUpdatePeriod = kRunPeriod;
			mSpeed = kRunSpeed;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			mWalkRight.mUpdatePeriod = kWalkPeriod;
			mWalkLeft.mUpdatePeriod = kWalkPeriod;
			mSpeed = kWalkSpeed;
		}
		
		// Determine which animation to invoke
		if (Input.GetKeyDown(KeyCode.RightArrow)) 			
			mSpriteManager.SetSpriteAnimationAciton(mWalkRight);
		if (Input.GetKeyUp (KeyCode.RightArrow))
			mSpriteManager.SetSpriteAnimationAciton(mRightStop);
		
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			mSpriteManager.SetSpriteAnimationAciton(mWalkLeft);
		if (Input.GetKeyUp(KeyCode.LeftArrow))
			mSpriteManager.SetSpriteAnimationAciton(mLeftStop);
		*/
		// verify intersection with camera window bound still works
		/*CameraBehavior cameraBehavior = GameObject.Find("Main Camera").GetComponent<CameraBehavior>();
		CameraBehavior.WorldBoundStatus status = 
			cameraBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
		if (status != CameraBehavior.WorldBoundStatus.Inside)
			Debug.Log ("Touching World:" + status);
		*/
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Collide with:" + other.name);
	}
}
