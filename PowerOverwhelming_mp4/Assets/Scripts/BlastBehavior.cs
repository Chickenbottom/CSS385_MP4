using UnityEngine;
using System.Collections;
public class BlastBehavior : MonoBehaviour
{
	GameObject mBlast = null;
	bool show_blast = false;
	private float previous_blast = 0;
	private float BLAST_INTERVAL = 0.30f;
	public void Start()
	{
		if(mBlast == null){
			mBlast = Resources.Load ("Prefabs/blast") as GameObject;
			mBlast.particleSystem.startColor = getRandColor();
		}
		BlastFunction();
	}

	public void Update()
	{
//		if(mBlast != null)
//			if(!particleSystem.IsAlive())
//				Destroy(this.gameObject);
	}
	private Color getRandColor()
	{
		float rand = UnityEngine.Random.value;
		if (rand < 0.1f)
			return Color.blue;
		else if (rand < 0.2f)
			return Color.red;
		else if (rand < 0.3f)
			return Color.yellow;
		else if (rand < 0.4f)
			return Color.green;
		else if (rand < 0.5f)
			return Color.cyan;
		else if (rand < 0.6f)
			return Color.blue;
		else if (rand < 0.7f)
			return Color.magenta;
		else if (rand < 0.8f)
			return Color.white;
		else if (rand < 0.9f)
			return Color.gray;
		return Color.yellow;
	}

	IEnumerator BlastFunction() {
		if(mBlast != null){
			yield return new WaitForSeconds(3.5f);
			Destroy(mBlast);
		}
	}
}
