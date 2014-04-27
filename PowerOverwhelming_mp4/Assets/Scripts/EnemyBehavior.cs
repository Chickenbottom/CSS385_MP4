using System;
using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	
	private const float kReferenceSpeed = 20f;
	private const float BLAST_INTERVAL = 0.30f;
	private int rotation_speed = 9;
	public float mSpeed = kReferenceSpeed;
	public float mInvasionSpeed = 100.0f;
	public float mTowardsCenter = 0.5f;  
	private GameObject mblast = null;
	private bool shieldsDown = false;
	private float previous_blast = 0;
	private float previous_laser = 0;
	public GameObject mProjectile = null;
	private bool initial = true;
	private bool paused = true;
	private bool show_blast = false;
	private bool hit = false;
	private bool hit2 = false;
	private bool hit3 = false;
	private float STUN_INTERVAL = 5.0f;
	private float previous_stun = 0.0f;
	private EnemyState myState;

	// James's Variables
	private Vector3 away;
	private float rotateSpeed = 90 / 2f;
	// End of James's Variables

			bool turned = false;
	GlobalBehavior globalBehavior = null;
		// what is the change of enemy flying towards the world center after colliding with world bound
		// 0: no control
		// 1: always towards the world center, no randomness
		

	public enum EnemyState{
		StunnedState,
		FreeState,
		RunState,
		InvadeState
	};

	// Use this for initialization
	void Start () {
		if(globalBehavior == null)
			globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior> ();
		myState = EnemyState.FreeState;
		if (null == mProjectile)
			mProjectile = Resources.Load ("Prefabs/alienLaser") as GameObject;
		if (null == mblast)
			mblast = Resources.Load ("Prefabs/blast") as GameObject;

		NewDirection();	
		float tempX = UnityEngine.Random.Range (-Screen.width*2, Screen.width*2);
		if(tempX > -55 &&  tempX < 55)
			tempX += 10*Math.Sign(tempX);
		float tempY = UnityEngine.Random.Range (-Screen.height*2, Screen.height*2);
		if(tempY > -55 &&  tempY < 55)
			tempY += 10*Math.Sign(tempY);
		transform.position = new Vector3 (UnityEngine.Random.Range (-415, 415), UnityEngine.Random.Range (-185, 185), 0.0f);
		initial = false;
		away = new Vector3 ();
	}

	// Update is called once per frame
	void Update () {

		
		WorldBehavior worldSheild = GameObject.Find ("mShield").GetComponent<WorldBehavior>();
		GameObject hero = GameObject.Find("mHero");
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		Sprite s;

		if (!globalBehavior.getPaused()){
			

			float difX = Math.Abs(transform.position.x - hero.transform.position.x);
			float difY = Math.Abs(transform.position.y - hero.transform.position.y);


			switch(myState){
			case EnemyState.FreeState:
				transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;

				if (null != renderer) {
					s = Resources.Load("Textures/UFO", typeof(Sprite)) as Sprite;
					renderer.sprite = s;
				}


				GlobalBehavior.WorldBoundStatus status =
					globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);

				if (status != GlobalBehavior.WorldBoundStatus.Inside)
					NewDirection ();

				if (Run ())
					myState = EnemyState.RunState;
				
				break;
			case EnemyState.RunState:
				away = this.transform.position - GameObject.Find ("mHero").GetComponent<Transform> ().position;
				if (Vector3.Cross(away, this.transform.up).magnitude > 0.001f) {
					float dir = Vector3.Cross(this.transform.up, away).normalized.z;
					transform.Rotate(Vector3.forward, dir * rotateSpeed * Time.smoothDeltaTime);
				}
				transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
				if (null != renderer) {
					s = Resources.Load("Textures/UFO_Run", typeof(Sprite)) as Sprite;
					renderer.sprite = s;
				}
				if (!Run ())
					myState = EnemyState.FreeState;
				break;
			case EnemyState.StunnedState:

				s = Resources.Load("Textures/UFO_hit1", typeof(Sprite)) as Sprite;
				renderer.sprite = s;
				transform.Rotate(Vector3.forward * -rotation_speed);
				if(Time.realtimeSinceStartup - previous_stun > STUN_INTERVAL){
					myState = EnemyState.FreeState;
				}
				break;
			case EnemyState.InvadeState:
				if (null != renderer) {
					s = Resources.Load("Textures/UFO", typeof(Sprite)) as Sprite;
					renderer.sprite = s;
				
				shieldsDown = true;
				mSpeed = mInvasionSpeed;
				invade ();
				transform.position += (mInvasionSpeed * Time.smoothDeltaTime) * transform.up;
				if(Math.Abs(transform.position.y) < 10 && 
				   Math.Abs(transform.position.x) < 10)
					Destroy(gameObject);
					globalBehavior.destroyShip();
				}
				break;
				
			}

					
				if (worldSheild.getShieldStatus () <= 0) {
						myState = EnemyState.InvadeState;
				}

				if(Input.GetAxis("Fire2") > 0)
					if(Time.realtimeSinceStartup - previous_blast > BLAST_INTERVAL){
						show_blast = !show_blast;
						previous_blast = Time.realtimeSinceStartup;
					}
			}
	}

	// New direction will be something randomly within +- 45-degrees away from the direction
	// towards the center of the world
	//
	// To find an angle within +-45 degree of a direction: 
	//     1. consider the simplist case of 45-degree above or below the x-direction
	//	   2. we compute random.X: a randomly generate x-value between +1 and -1
	//     3. To ensure within 45 degrees, we simply need to make sure generating a y-value that is within the (-random.X to +random.X) range
	//     4. Now a direction towards the (random.X, random.Y) is guaranteed to be within 45-degrees from x-direction
	// Apply the above logic, only now:
	//		X-direciton is V (from current direciton towards the world center)
	//		Y-direciton is (V.y, -V.x)
	//
	// Lastly, 45-degree is nice because X=Y, we can do this for any angle that is less than 90-degree
	private void NewDirection() {
		GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();

		// we want to move towards the center of the world
		Vector2 v = globalBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);  
				// this is vector that will take us back to world center
		v.Normalize();
		Vector2 vn = new Vector2(v.y, -v.x); // this is a direciotn that is perpendicular to V

		float useV = 1.0f - Mathf.Clamp(mTowardsCenter, 0.01f, 1.0f);
		float tanSpread = Mathf.Tan( useV * Mathf.PI / 2.0f );

		//float randomX = Random.Range(0f, 1f);
		float randomX = 0.5f;
		float yRange = tanSpread * randomX;
		float randomY = UnityEngine.Random.Range (-yRange, yRange);

		Vector2 newDir = randomX * v + randomY * vn;
		newDir.Normalize();
		transform.up = newDir;

		if(!initial){
			GameObject e = Instantiate(mProjectile) as GameObject;
			LaserBehavior alienLaser = e.GetComponent<LaserBehavior>(); // Shows how to get the script from GameObject
			previous_laser = Time.realtimeSinceStartup;
			if(alienLaser != null){
				e.transform.position = transform.position;
				alienLaser.SetForwardDirection(transform.up);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Hit!");
		GlobalBehavior gbehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
		
		if (other.gameObject.name == "mShield" && !shieldsDown) 
		{
			transform.up = -transform.up;
		}
		if (other.gameObject.name == "mShield" && shieldsDown) 
		{
			Destroy(gameObject);
			globalBehavior.destroyShip();
		}
		if (other.gameObject.name == "laser(Clone)") {
			Destroy(other.gameObject);
			if(hit){
				if(hit2)
					hit3 = true;
				else
					hit2 = true;
			}
			// Destroy(this.gameObject);
			if(hit3){
				//if(show_blast){
				//	mblast.transform.position = transform.position;
				//	GameObject b = (GameObject) Instantiate(mblast);
				//}
				Instantiate(Resources.Load("Prefabs/explosion"), transform.position, new Quaternion());
				Destroy(gameObject);
				globalBehavior.destroyShip();
				return;
			}
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			if (null != renderer) {
				myState = EnemyState.StunnedState;
				previous_stun = Time.realtimeSinceStartup;	
				hit = true;
			}
		}
	}
	private void invade()
	{
		//GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();
		// we want to move towards the center of the world
		Vector2 v = globalBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);  
		// this is vector that will take us back to world center
		v.Normalize();
		Vector2 vn = new Vector2(v.y, -v.x); // this is a direciotn that is perpendicular to V
		Vector2 newDir = v + vn;
		newDir.Normalize();
		transform.up = newDir;
	
	}

	public void toggle_pause(){
		paused = !paused;
	}

	private bool Run() {
		Transform heroTransform = GameObject.Find ("mHero").GetComponent<Transform>();
		Vector3 heroPos = heroTransform.position;
		Vector3 heroDir = heroTransform.up;
		Vector3 heroToEnemy = this.transform.position - heroPos;
		float dist = Vector3.Magnitude (heroToEnemy);
		heroDir.Normalize ();
		heroToEnemy.Normalize ();
		float theta = Mathf.Acos (Vector3.Dot (heroDir, heroToEnemy)) * Mathf.Rad2Deg;
		return theta < 50f && dist < 100f;
	}

}
