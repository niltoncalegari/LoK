using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    
	public Transform target;

	public float cameraSpeed;

	public float moveAhead;

	private Vector3 screenShakeActive;

	private float screenShakeAmount;
	public float screenShakeDecay;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3(target.position.x,target.position.y, transform.position.z);

		transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + (moveAhead * target.localScale.x), target.position.y, transform.position.z), cameraSpeed * Time.deltaTime);

		if(screenShakeAmount > 0)
		{
			screenShakeActive = new Vector3(Random.Range(-screenShakeAmount, screenShakeAmount), Random.Range(-screenShakeAmount, screenShakeAmount), 0f);

			screenShakeAmount -= Time.deltaTime * screenShakeDecay;
		} else {
			screenShakeActive = Vector3.zero;
		}

		transform.position += screenShakeActive;
	}

	public void ScreenShake(float toShake)
	{
		if(toShake > screenShakeAmount)
		{
			screenShakeAmount = toShake;
		}
	}
}