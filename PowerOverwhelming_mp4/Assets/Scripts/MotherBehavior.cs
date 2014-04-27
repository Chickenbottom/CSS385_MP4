using UnityEngine;
using System.Collections;
public class MotherBehavior : MonoBehaviour
{
	GlobalBehavior globalBehavior = null;
	public GameObject mProjectile = null;
	float x_dir = 1;
	float shields = 25;
	float previous_shot = 0.0f;
	float SHOT_INTERVAL = 5.0f;
	Vector3 earth;
	public void Start(){
		globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior> ();
		if(mProjectile == null)
			mProjectile = Resources.Load ("Prefabs/deathRay") as GameObject;
		earth = new Vector3(0.01f,0.01f,0.0f);
	}
	public void Update(){

		transform.position += new Vector3(x_dir, 0.0f,0.0f);
		
		GlobalBehavior.WorldBoundStatus status =
			globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
		if (status != GlobalBehavior.WorldBoundStatus.Inside) 
			x_dir = -x_dir;
		
		if(shields <= 0.0f)
		{
			this.renderer.enabled = false;
		}

		if(Time.realtimeSinceStartup - previous_shot > SHOT_INTERVAL){
			GameObject e = Instantiate(mProjectile) as GameObject;
			LaserBehavior laser = e.GetComponent<LaserBehavior>(); // Shows how to get the script from GameObject
			previous_shot = Time.realtimeSinceStartup;
			if(laser != null){
				e.transform.position = transform.position;
				Vector3 newUP = earth - transform.position;
				laser.SetForwardDirection(newUP);
			}
			
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "alienLaser(Clone)")
		{
			Destroy(other.gameObject);
		}
		else if(other.name == "laser(Clone)")
		{
			Destroy(other.gameObject);
			shields--;
		}
	}
}

