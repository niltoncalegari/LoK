using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {
    
	public float lifeTime;
	
	// Update is called once per frame
	void Update () {

		lifeTime -= Time.deltaTime;
		if(lifeTime < 0)
		{
			Destroy(gameObject);
		}
	}
}