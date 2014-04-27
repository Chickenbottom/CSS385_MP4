using UnityEngine;
using System.Collections;

public class WorldBehavior : MonoBehaviour
{
	const int total_shields = 100;
	const int deathray = 10;
	int shield = total_shields;
	
	public GUIStyle progress_empty;
	public GUIStyle progress_full;
	
	//current progress
	public float barDisplay;
	
	Vector2 pos = new Vector2(10f,50f);
	Vector2 size = new Vector2(50f,60f);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	
	void Start()
	{
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y), emptyTex, progress_empty);
		
		GUI.Box(new Rect(pos.x, pos.y, size.x, size.y), fullTex, progress_full);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
		
		GUI.Box(new Rect(0, 0, size.x, size.y), fullTex, progress_full);
		
		GUI.EndGroup();
		GUI.EndGroup();
	}
	
	void Update()
	{
		
		//the player's health
		barDisplay = shield/total_shields;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "laser(Clone)") 
			Destroy(other.gameObject);
		if (other.gameObject.name == "alienLaser(Clone)") 
		{
			Destroy(other.gameObject);
			shield--;
		}
		if(other.gameObject.name == "deathRay(Clone)")
		{
			Destroy(other.gameObject);
			shield -= deathray;
		}
		if(shield <= total_shields/2)
		{
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			if (null != renderer) {
				Sprite s = Resources.Load("Textures/EarthShield50", typeof(Sprite)) as Sprite;
				renderer.sprite = s;
			}
		}
		if(shield <= 0)
		{
			renderer.enabled = false;	
			EnemyBehavior invade = GameObject.Find ("UFO(Clone)").GetComponent<EnemyBehavior>();
			if(invade != null){
				SpriteRenderer myWorld = GameObject.Find("mWorld").GetComponent<SpriteRenderer>();
				//SpriteRenderer renderer = GetComponent<SpriteRenderer>();
				Sprite s = Resources.Load("Textures/World_Invaded", typeof(Sprite)) as Sprite;
				myWorld.sprite = s;
			}
			
		}	
	}
	public int getShieldStatus()
	{
		return shield;
	}
	
}

