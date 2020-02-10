using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject mPlayer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = Vector3.Slerp (transform.position,  new Vector3 (0,10,mPlayer.transform.position.z-10) ,Time.deltaTime*M.TimeDelt);
		//transform.position = new Vector3 (0, 10, mPlayer.transform.position.z - 10);
	}
}
