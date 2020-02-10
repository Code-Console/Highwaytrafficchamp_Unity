using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Splash : MonoBehaviour {
	public GameObject mShopCars,mShopCars2;
	public GameObject mLock;
	Animator mAniDoNot,mAniSet;
	public Text mPlay_Buy;
	int div = 10;
	// Use this for initialization
	AudioSource MusicClick;
	void Start () {
		GameShop.IS.InitShop ();
		SelectCar ();
		mAniDoNot = transform.Find ("Menu/Donot").GetComponent<Animator> ();
		mAniSet = transform.Find ("Menu/setScr").GetComponent<Animator> ();
		Debug.Log ("Start");



		mAniSet.transform.Find ("GO/Music").GetComponent<Toggle> ().isOn=GameShop.IS.isMusic;
		mAniSet.transform.Find ("GO/SOUND").GetComponent<Toggle> ().isOn=GameShop.IS.isSound;
		mAniSet.transform.Find ("GO/STEERING/Text").GetComponent<Text> ().text = GameShop.IS.isSenser ? "TILT":"TOUCH";
		mAniSet.transform.Find ("GO/ACCELARATION/Text").GetComponent<Text> ().text = GameShop.IS.isAuto ? "AUTO":"MANUAL";

		MusicClick = gameObject.AddComponent<AudioSource> ();
		MusicClick.clip = (AudioClip)Resources.Load ("sound/click");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnClick(int Val){
		switch(Val){
		case 0://Play
			if (GameShop.IS.CarPrize [GameShop.IS.CarSel] > 0) {
				if(GameShop.IS.CarPrize [GameShop.IS.CarSel]/div <= GameShop.IS.mTotalCash) {
					GameShop.IS.mTotalCash -= (GameShop.IS.CarPrize [GameShop.IS.CarSel] / div);
					GameShop.IS.CarPrize [GameShop.IS.CarSel] = 0;

					SelectCar ();
				} else {
					mAniDoNot.SetBool ("isOpen", true);//Donot
				}
			} else {
				GameShop.IS.mGameLoc = Random.Range (0, 4);
				GameShop.IS.Save ();
				SceneManager.LoadScene ("GamePlay");
			}
			break;
		case 1://Setting
			mAniSet.SetBool ("isOpen", true);
			break;
		case 2://Left
			GameShop.IS.CarSel--;
			if (GameShop.IS.CarSel < 0)
				GameShop.IS.CarSel = mShopCars.transform.childCount - 1;
			SelectCar ();
			break;
		case 3://Right
			GameShop.IS.CarSel++;
			GameShop.IS.CarSel %= mShopCars.transform.childCount;
			SelectCar ();
			break;
		case 4://Ok
			mAniDoNot.SetBool ("isOpen", false);//Donot
			mAniSet.SetBool ("isOpen", false);
			break;
		case 5:
			Debug.Log ("One");
			GameShop.IS.isMusic =  mAniSet.transform.Find ("GO/Music").GetComponent<Toggle> ().isOn;
			break;
		case 6:
			GameShop.IS.isSound =  mAniSet.transform.Find ("GO/SOUND").GetComponent<Toggle> ().isOn;
			break;
		case 7:
			GameShop.IS.isSenser = !GameShop.IS.isSenser;
			mAniSet.transform.Find ("GO/STEERING/Text").GetComponent<Text> ().text = GameShop.IS.isSenser ? "TILT":"TOUCH";
			break;
		case 8:
			GameShop.IS.isAuto = !GameShop.IS.isAuto;
			mAniSet.transform.Find ("GO/ACCELARATION/Text").GetComponent<Text> ().text = GameShop.IS.isAuto ? "AUTO":"MANUAL";
			break;
		}
		if (GameShop.IS.isSound) {
			MusicClick.Play ();
		}
	}

	public void SelectCar()
	{
		for (int i = 0; i < mShopCars.transform.childCount; i++) {
			mShopCars.transform.GetChild (i).gameObject.SetActive (GameShop.IS.CarSel == i);
			mShopCars2.transform.GetChild (i).gameObject.SetActive (GameShop.IS.CarSel == i);
		}
		mLock.SetActive (GameShop.IS.CarPrize [GameShop.IS.CarSel] > 0);
		mLock.transform.GetChild (0).GetComponent<Text> ().text = "" + (GameShop.IS.CarPrize [GameShop.IS.CarSel]/div);
		mPlay_Buy.text = GameShop.IS.CarPrize [GameShop.IS.CarSel] == 0 ? "PLAY":"BUY";
		transform.Find("Menu/Coin/Text").GetComponent<Text>().text = ""+GameShop.IS.mTotalCash;

	}
}
