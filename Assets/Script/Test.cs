using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	// Use this for initialization
	[SerializeField] Scrollbar mMusicBar;
	void Start () {

		mMusicBar.transform.GetChild(1).GetComponent<Image>().fillAmount =	mMusicBar.value;
	}
	
	// Update is called once per frame
	void Update () {
		mMusicBar.transform.GetChild(1).GetComponent<Image>().fillAmount = 	mMusicBar.value;
	}
	public void Onclick()
	{
		

	}
}
