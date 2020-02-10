using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppObj : MonoBehaviour {
	public float spd = 0.1f;
	public int isCollide = 0;
	public bool getscore = false;
	public float takeside = 0;


	int counter = 0;

	void Update () {
		if (GameUi.GameScr == GameUi.Scr.GamePauseScr) {
			return;
		}

		if(M.IsGO)
			transform.position += Vector3.forward * spd;

		if (isCollide > 1) {
			isCollide++;
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild(i).gameObject.SetActive (isCollide % 4 < 2);
			}
			if (isCollide > 20) {
				Debug.Log (isCollide + "   ");
				transform.position -= Vector3.forward * 50;
				for (int i = 0; i < transform.childCount; i++) {
					transform.GetChild(i).gameObject.SetActive (true);
				}
				isCollide = 0;
			}
		}

		if (takeside != transform.position.x) {
			counter++;


			if (takeside < transform.position.x) {
				transform.position -= Vector3.right * .04f;
				if (takeside > transform.position.x) {
					transform.position = new Vector3(takeside,0,transform.position.z)  ;
				}
			}
			if (takeside > transform.position.x) {
				transform.position += Vector3.right * .04f;
				if (takeside < transform.position.x) {
					transform.position = new Vector3(takeside,0,transform.position.z)  ;
				}
			}
			transform.GetChild (0).GetChild (0).gameObject.SetActive (counter % 10 < 5 && takeside < transform.position.x);
			transform.GetChild (0).GetChild (1).gameObject.SetActive (counter % 10 < 5  && takeside > transform.position.x);
		}
	}
	public void setCollide(float _spd){
		transform.position += Vector3.forward * 5;
		isCollide++;
		getscore = true;
	}

	public void setOpp(float _spd,Vector3 vec){
		Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
		if (rigidbody) {
			rigidbody.velocity = new Vector3 (0f, 0f, 0f); 
			rigidbody.angularVelocity = new Vector3 (0f, 0f, 0f);
		}
		spd = _spd;
		if(spd < 0)
			transform.rotation = Quaternion.Euler (0, 180, 0);
		else
			transform.rotation = Quaternion.Euler (0, 0, 0);
		
		transform.position = vec;
		takeside = vec.x;
		isCollide = 0;
		getscore = false;




	}
	public void GiveSide(float val){
		if (takeside == transform.position.x) {
			takeside += val;
			if (takeside < transform.position.x) {
				transform.position -= Vector3.right * .1f;
			}
			if (takeside > transform.position.x) {
				transform.position += Vector3.right * .1f;
			}
		}
		
	}


	void OnTriggerEnter(Collider collision) {
		//Debug.Log (collision.transform.name + "  OnTriggerEnter  " + transform.name);


		if (collision.transform.tag == "Opp") {
			if(spd > collision.GetComponent<OppObj> ().spd)
				spd = collision.GetComponent<OppObj> ().spd;
			else
				collision.GetComponent<OppObj> ().spd = spd;
			if (collision.transform.position.z > transform.position.z) {
				spd -= .05f;
			} else {
				collision.GetComponent<OppObj> ().spd -=.05f;
			}
		}

	}
	void OnCollisionEnter(Collision collision)
	{
		//Debug.Log (collision.transform.name + "  OnCollisionEnter  " + transform.name);
		
	}
}
