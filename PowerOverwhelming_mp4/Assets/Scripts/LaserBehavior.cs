using UnityEngine;
using System.Collections;

public class LaserBehavior : MonoBehaviour {
	
	private float mSpeed = 100f;

	void Start()
	{
	}

	// Update is called once per frame
	void Update () {
		GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior> ();
		if (!globalBehavior.getPaused()){

			transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;

			GlobalBehavior.WorldBoundStatus status =
				globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);

			
			if (status != GlobalBehavior.WorldBoundStatus.Inside) {
				Destroy(gameObject);			
				}
		}
	}
	public void SetForwardDirection(Vector3 f)
	{
		transform.up = f;
	}
	
}
