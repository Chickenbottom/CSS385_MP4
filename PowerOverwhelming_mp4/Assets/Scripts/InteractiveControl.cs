using UnityEngine;	
using System.Collections;

public class InteractiveControl : MonoBehaviour {
	
	public GameObject mProjectile = null;
	private const float LASER_INTERVAL = 0.10f;
	private float previous_laser = 0;
	private float kHeroSpeed = 20f;
	private float speed_interval = 10f;
	private const float SPEED_MAX = 60f;
	private const float SPEED_MIN = 20f;
	private float kHeroRotateSpeed = 90/2f; // 90-degrees in 2 seconds
	public int laser_total = 0;
	private float radiusX;
	private float radiusY;
	Vector3 move;
	// Use this for initialization
	void Start () {
		if (null == mProjectile)
			mProjectile = Resources.Load ("Prefabs/laser") as GameObject;
		radiusX = this.transform.localScale.x/2;
		radiusY = this.transform.localScale.y/2;
	}
	
	// Update is called once per fram
	void Update () {
		GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior> ();
		if (!globalBehavior.getPaused()){
			move = Input.GetAxis ("Vertical")  * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
			transform.position += move;
			transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (kHeroRotateSpeed * Time.smoothDeltaTime));
		
			GlobalBehavior.WorldBoundStatus status =
				globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
			if (status != GlobalBehavior.WorldBoundStatus.Inside) {
				//transform.position -= move;
				keep_within(status, move);
			}
			if (Input.GetAxis ("Fire1") > 0f) { // this is Left-Control
				if(Time.realtimeSinceStartup - previous_laser > LASER_INTERVAL){
					GameObject e = Instantiate(mProjectile) as GameObject;
					LaserBehavior laser = e.GetComponent<LaserBehavior>(); // Shows how to get the script from GameObject
					previous_laser = Time.realtimeSinceStartup;
					if(laser != null){
						e.transform.position = transform.position;
						laser.SetForwardDirection(transform.up);
						laser_total++;
					}
				}
			}
			if(Input.GetKeyUp("z"))
				if(kHeroSpeed > SPEED_MIN)
					kHeroSpeed -= speed_interval;
			if(Input.GetKeyUp("a"))
				if(kHeroSpeed < SPEED_MAX)
					kHeroSpeed += speed_interval;

			GameObject speedText = GameObject.Find("SpeedGUIText");
			GUIText gui = speedText.GetComponent<GUIText>();
			gui.text = "Current Speed: " +kHeroSpeed.ToString();

		}
	}
	public void destroy_laser(){
		laser_total--;
	}
	public void keep_within(GlobalBehavior.WorldBoundStatus status, Vector3 move){
		transform.position -= move;
		switch(status){
		case GlobalBehavior.WorldBoundStatus.CollideBottom :
			transform.position.Set(transform.position.x, -(Screen.height + radiusY), 0.0f);
			break;
		case GlobalBehavior.WorldBoundStatus.CollideLeft:
			transform.position.Set(Screen.width + radiusX, transform.position.y, 0.0f);
			break;
		case GlobalBehavior.WorldBoundStatus.CollideRight:
			transform.position.Set(Screen.width-radiusX, transform.position.y,0.0f);
			break;
		case GlobalBehavior.WorldBoundStatus.CollideTop:
			transform.position.Set(transform.position.x, (Screen.height - radiusY), 0.0f);
			break;
		}
	}
}
