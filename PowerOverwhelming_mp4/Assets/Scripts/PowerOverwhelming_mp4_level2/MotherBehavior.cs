using UnityEngine;
using System.Collections;
public class MotherBehavior : MonoBehaviour
{
	GlobalBehavior globalBehavior = null;
	float x_dir = 1;
	float shields = 25;
	public void Start(){
	}
	public void Update(){

		transform.position += new Vector3(1, 0.0f,0.0f);
		
		GlobalBehavior.WorldBoundStatus status =
			globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
		if (status != GlobalBehavior.WorldBoundStatus.Inside) 
			x_dir = -x_dir;
		

	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "alienLaser(Clone)")
		{
			Destroy(other);
		}
		else if(other.name == "laser(Clone)")
		{
			Destroy(other);
			shields--;
		}
	}
}

