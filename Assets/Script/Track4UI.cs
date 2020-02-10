using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track4UI : MonoBehaviour
{
	public float spd = .1f;
	const int trackLenth = 60;
	int count = 0;
	int Noroad = 2;
	public Transform mShopCars;
	public Transform mShopTrack;
	GameObject mCemra;
	//int rotationcount = 0;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < mShopTrack.childCount; i++) {
			mShopTrack.GetChild (i).transform.position = new Vector3 (0, mShopTrack.position.y, i * trackLenth);
			for (int j = 0; j < mShopTrack.GetChild (i).childCount; j++) {
				mShopTrack.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
			}
		}
		mCemra =  GameObject.Find ("Main Camera");
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (GameUi.GameScr == GameUi.Scr.MenuScr);
		}
		//mShopCars.GetChild (mShopCars.childCount - 1).gameObject.SetActive (GameUi.GameScr == GameUi.Scr.ShopScr);
		if (GameUi.GameScr == GameUi.Scr.MenuScr) {
			for (int i = 0; i < mShopCars.GetChild (GameShop.IS.CarSel).GetChild(1).childCount; i++) {
				mShopCars.GetChild (GameShop.IS.CarSel).GetChild(1).GetChild (i).transform.rotation *= Quaternion.AngleAxis (3, Vector3.right);
			}
			DrawBG ();
			mCemra.transform.position = new Vector3 (12,15,0);
			mCemra.transform.rotation = Quaternion.Euler (50,-60,0);
		}

	}

	void DrawBG ()
	{
		for (int i = 0; i < mShopTrack.childCount; i++) {
			mShopTrack.GetChild (i).transform.position -= Vector3.forward*spd;
		}
		for (int i = 0; i < mShopTrack.childCount; i++) {
			if (mShopTrack.GetChild (i).transform.position.z < -60) {
				mShopTrack.GetChild (i).transform.position = new Vector3 (0, mShopTrack.position.y, mShopTrack.GetChild ((i == 0 ? mShopTrack.childCount : i) - 1).transform.position.z + trackLenth);
				count++;
				if (count > 1 && i == 0) {
					Noroad++;
					Noroad %= 4;
					if (Noroad == 3) {
						for (int j = 0; j < mShopTrack.GetChild (i).childCount; j++) {
							mShopTrack.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
						}
						count = 1005;
					} else {
						int rnd = Random.Range (4, 6);
						for (int j = 0; j < mShopTrack.GetChild (i).childCount; j++) {
							mShopTrack.GetChild (i).GetChild (j).gameObject.SetActive (rnd == j);
						}
						count = 0;
					}

				} else {
					for (int j = 0; j < mShopTrack.GetChild (i).childCount; j++) {
						mShopTrack.GetChild (i).GetChild (j).gameObject.SetActive (Noroad == j);
					}
				}
			}
		}
	}
}
