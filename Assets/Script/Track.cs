using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 if(GameScr == Scr.GameLocScr) 
		{
			mGameLoc.transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = GameShop.IS.mTotalCash + "";
			for(int i = 0; i < mGameLoc.transform.GetChild (5).GetChild (0).GetChild (0).childCount; i++) 
			{
				mGameLoc.transform.GetChild (5).GetChild (0).GetChild (0).GetChild (i).GetChild (0).gameObject.SetActive (GameShop.IS.LocPrize[i]>0);
				mGameLoc.transform.GetChild (5).GetChild (0).GetChild (0).GetChild (i).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.LocPrize [i] + "";
			}

		}
 */
//using UnityEngine.Advertisements;
public class Track : MonoBehaviour
{
	const int trackLenth = 60;
	int Noroad = 0;
	int count = 0;
	//GameObject mPlayer;
	GameObject mCemra;



	void Start () {
		
		//mPlayer = GameObject.Find ("Car");
		mCemra =  GameObject.Find ("Main Camera");
		ResetGame ();
	}
	public void ResetGame(){
		
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).transform.position = new Vector3 (0, 0, i * trackLenth);
			for (int j = 0; j < transform.GetChild (i).childCount; j++) {
				transform.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
			}
		}
	}
	void Update () {
		if (GameUi.GameScr == GameUi.Scr.ShopScr) {
			return;
		}
		if(mCemra)
			DrawBG ();
	}
	void DrawBG ()
	{
		
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).transform.position.z + trackLenth * .4f < mCemra.transform.position.z) {
				transform.GetChild (i).transform.position = new Vector3 (0, 0, transform.GetChild ((i == 0 ? transform.childCount : i) - 1).transform.position.z + trackLenth);
				count++;
				if (count > 15 && i == 0) {
					
					Noroad++;
					if (GameShop.IS.mGameLoc == 0) {
						Noroad %= 5;
						if (Noroad == 4) {
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
							}
							count = 1005;
						} else {
							int rnd = Random.Range (5, 7);
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (rnd == j);
							}
							count = 0;
						}
					}
					if (GameShop.IS.mGameLoc == 1 || GameShop.IS.mGameLoc == 2) {
						Noroad %= 4;
						if (Noroad == 3) {
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
							}
							count = 1005;
						} else {
							int rnd = Random.Range (4, 6);
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (rnd == j);
							}
							count = 0;
						}
					}
					if (GameShop.IS.mGameLoc == 3) {
						Noroad %= 4;
						count = 0;
						if (Noroad == 3 ) {
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (6 == j);
							}

						} else if (Noroad == 0 ) {
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (7 == j);
							}

						} else {
							int rnd = Random.Range (4, 6);
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (rnd == j);
							}
						}
					}
				} else {
					if (GameShop.IS.mGameLoc == 3) {
						Debug.Log (Noroad+"    "+count +"   i =  "+i);
						if (Noroad == 3 && count == 1 && i==1) {
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (4 == j);
							}

						} else {
							for (int j = 0; j < transform.GetChild (i).childCount; j++) {
								transform.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
							}
						}
					} else {
						for (int j = 0; j < transform.GetChild (i).childCount; j++) {
							transform.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
						}
					}
				}
			}
		}
	}
}
