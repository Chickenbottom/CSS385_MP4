using UnityEngine;
using System.Collections;
public class MotherBehavior : MonoBehaviour
{
	GlobalBehavior globalBehavior = null;
	float x_dir = 1;
	float shields = 25;
	public void Start(){
		globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior> ();
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

