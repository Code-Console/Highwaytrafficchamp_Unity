using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour {

	// Use this for initialization
	[SerializeField] float mDelay=0;
	void Start()
	{
	}
	void OnEnable () 
	{
		StartCoroutine (PlayAnim());
	}
	void OnDisable () 
	{
    	this.GetComponent<Animation> ().gameObject.transform.localScale= new Vector3(0,0,0);
	}
	IEnumerator PlayAnim()
	{
		yield return new WaitForSeconds (mDelay);
		this.GetComponent<Animation>().Play();
	}
	// Update is called once per frame
	void Update ()
	{

	}

}
