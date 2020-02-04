using UnityEngine;
using System.Collections;

public class ShakeOnStart : MonoBehaviour {
    
	public float shakeAmount;
	private CameraController theCamera;

	// Use this for initialization
	void Start () {
		theCamera = FindObjectOfType<CameraController>();
		theCamera.ScreenShake(shakeAmount);
	}
	

}