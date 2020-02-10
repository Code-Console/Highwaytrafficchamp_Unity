using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUi : MonoBehaviour {

	// Use this for initialization
	public enum Scr  {MenuScr,ShopScr,SettingScr,GameInAppScr,GameFreeCashScr,GameModeScr,GameLocScr,GamePauseScr,GameOverScr,GamePlayScr,GameLoading,GameBitCoin};
	public static Scr GameScr = Scr.MenuScr;
	int mPrevScr;
	AsyncOperation asysc;
	GameObject mGameShop,mGameMenu,mGameSetting,mGameInApp,mGameFreeCash,mGameMode,mGameLoc,mGameNotEnough,mGameLoading,mGameBitCoin;
	[SerializeField] GameObject mGameShopList=null,mShopCars=null,mShopCars2=null,mModeVal=null;
	Texture[] mWheelTex = new Texture[14],mBodyTex = new Texture[10]; 


	[SerializeField] Slider mMusicBar=null,mSoundBar=null;
	bool isNotEnough =false;

	AudioSource MusicClick,MusicScene;
	public static  AudioSource MusicBG;



	Animator _Animator;
	void Awake()
	{
		AdsManager.Instance.Init();
	}
	void Start () 
	{
		mGameMenu      = transform.GetChild (0).gameObject;
		mGameSetting   = transform.GetChild (1).gameObject;
		mGameShop      = transform.GetChild (2).gameObject;
		mGameInApp     = transform.GetChild (3).gameObject;
		mGameFreeCash  = transform.GetChild (4).gameObject;
		mGameMode      = transform.GetChild (5).gameObject;
		mGameLoc       = transform.GetChild (6).gameObject;
		mGameNotEnough = transform.GetChild (7).gameObject;
		mGameLoading	= transform.GetChild (8).gameObject;
		mGameBitCoin	= transform.GetChild (9).gameObject;


		GameShop.IS.InitShop ();
		GameShop.IS.UpgradeType = GameShop.COLOR;
		for(int i = 0; i < mShopCars.transform.childCount; i++)
		{
			mShopCars.transform.GetChild (i).gameObject.SetActive (i==GameShop.IS.CarSel);
			mShopCars2.transform.GetChild (i).gameObject.SetActive (i==GameShop.IS.CarSel);
		}
		for(int i = 0;i<mGameShopList.transform.GetChild(1).GetChild (2).transform.childCount;i++) //Wheel
		{
			Sprite img   = Resources.Load<Sprite>("TyreIcn/tyre"+i);
			mGameShopList.transform.GetChild (1).GetChild (2).GetChild(i).GetComponent<Image>().sprite = img;
			mWheelTex[i] = Resources.Load<Texture>("TyreTexture/Tyre_"+i);
		}
		for(int i = 0;i<mGameShopList.transform.GetChild(3).GetChild (2).transform.childCount;i++) //CarTatoo
        {
			Sprite img   = Resources.Load<Sprite>("Decals/0/decal"+i);
            mGameShopList.transform.GetChild (3).GetChild (2).GetChild(i).GetComponent<Image>().sprite = img;
            mBodyTex[i] = Resources.Load<Texture>("Decals/decal_"+i);
        } 

		GameShop.IS.WheelSel = 0;
		GameShop.IS.ColorSel = 0;

		mMusicBar.value = GameShop.IS.MusicValue;
		mSoundBar.value = GameShop.IS.SoundValue;

		isNotEnough =false;
		GameShop.IS.mColor = mGameShopList.transform.GetChild (0).GetChild(2).GetChild(0).GetComponent<Image>().color;


		MusicClick = gameObject.AddComponent<AudioSource> ();
		MusicClick.clip = (AudioClip)Resources.Load ("sound/click");
		MusicClick.volume = mSoundBar.value;

		MusicScene = gameObject.AddComponent<AudioSource> ();
		MusicScene.clip = (AudioClip)Resources.Load ("sound/play_button");
		MusicScene.volume = mSoundBar.value;


		MusicBG = gameObject.AddComponent<AudioSource> ();
		MusicBG.clip = (AudioClip)Resources.Load ("sound/ui");
		MusicBG.loop = true;
		if (GameShop.IS.isMusic) {
			MusicBG.Play ();
			MusicBG.volume = mMusicBar.value;
		}




		for (int i = 0; i < mShopCars.transform.childCount; i++) {
			GameShop.IS.mColor = mGameShopList.transform.GetChild (0).GetChild(2).GetChild(0).GetComponent<Image>().color;
			//for (int i = 0; i < mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetComponent<Renderer> ().materials.Length; i++) 
			{
				mShopCars.transform.GetChild (i).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials[0].color  = GameShop.IS.mColor;
				if(i==0)
					mShopCars.transform.GetChild (i).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials [1].color = GameShop.IS.mColor;

				mShopCars2.transform.GetChild (i).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials[0].color  = GameShop.IS.mColor;
				if(i==0)
					mShopCars2.transform.GetChild (i).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials [1].color = GameShop.IS.mColor;
			}
		}

		setBitCoin ();
		//setInAppValse ();
		setScreen (GameScr);
		mGameSetting.transform.GetChild (2).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isSenser ? "TILT" : "TOUCH";
		mGameSetting.transform.GetChild (3).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isAuto ? "AUTO" : "MANIAL";
		mGameSetting.transform.GetChild (4).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isQuality ? "HIGH" : "LOW";

		setShopValuse ();

		setFreeScr ();


		mSoundBar.transform.parent.GetChild (1).GetChild (0).GetComponent<Image> ().enabled = GameShop.IS.isSound;
		mSoundBar.gameObject.SetActive (GameShop.IS.isSound);

		mMusicBar.transform.parent.GetChild (1).GetChild (0).GetComponent<Image> ().enabled = GameShop.IS.isMusic;
		mMusicBar.gameObject.SetActive (GameShop.IS.isMusic);

		#if UNITY_IPHONE
		mGameBitCoin.transform.GetChild (9).GetChild (3).gameObject.SetActive (false);
		#endif
		if (www == null) {
			#if UNITY_IPHONE
			www = new WWW("http://hututusoftwares.com/Link/iphone.html");
			#else
			www = new WWW("http://hututusoftwares.com/Link/android.html");
			#endif

		}

	}
	
	// Update is called once per frame
	void Update () {
		mShopCars.SetActive (GameScr == Scr.ShopScr||GameScr == Scr.MenuScr);
		mShopCars2.SetActive(GameScr == Scr.ShopScr);
		mShopCars2.transform.parent.GetChild (1).gameObject.SetActive (GameScr == Scr.ShopScr);


//		mGameMenu.SetActive (GameScr == Scr.MenuScr);
//		mGameSetting.SetActive (GameScr == Scr.SettingScr);
//		mGameShop.SetActive (GameScr == Scr.ShopScr);
//		mGameInApp.SetActive (GameScr == Scr.GameInAppScr);
//		mGameFreeCash.SetActive (GameScr == Scr.GameFreeCashScr);
//		mGameMode.SetActive (GameScr == Scr.GameModeScr);
//		mGameLoc.SetActive (GameScr  == Scr.GameLocScr);
//		mGameNotEnough.SetActive (isNotEnough);
//		mGameLoading.SetActive(GameScr == Scr.GameLoading);
//		mGameBitCoin.SetActive(GameScr == Scr.GameBitCoin);
//

		mShopCars.SetActive (true);

		if (GameScr == Scr.ShopScr) {
			//Lock
			mGameShop.transform.GetChild (12).gameObject.SetActive (GameShop.IS.CarPrize[GameShop.IS.CarSel]>0);
			mGameShop.transform.GetChild (12).GetChild (1).GetComponent<Text> ().text = GameShop.IS.CarPrize[GameShop.IS.CarSel]+"";
			//PlayBtn
			mGameShop.transform.GetChild (0).gameObject.SetActive (GameShop.IS.CarPrize[GameShop.IS.CarSel]==0 
				&& GameShop.IS.TyrePrize [GameShop.IS.WheelSel, GameShop.IS.CarSel] ==0 &&
				GameShop.IS.ColorPrize [GameShop.IS.ColorSel,GameShop.IS.CarSel]==0 && 
				GameShop.IS.TexPrize[GameShop.IS.TexSel,GameShop.IS.CarSel]==0);
			//Color , Tyre Money ,Texture
			mGameShopList.gameObject.SetActive (GameShop.IS.CarPrize [GameShop.IS.CarSel] == 0);
			switch(GameShop.IS.UpgradeType) 
			{
				
				case GameShop.COLOR:
						mGameShop.transform.GetChild (7).gameObject.SetActive (GameShop.IS.ColorPrize [GameShop.IS.ColorSel, GameShop.IS.CarSel] >0);
						mGameShop.transform.GetChild (7).GetChild (0).GetChild (0).GetComponent<Text> ().text = GameShop.IS.ColorPrize [GameShop.IS.ColorSel, GameShop.IS.CarSel] + "";
						for(int i=0;i<mGameShopList.transform.GetChild(0).GetChild(2).childCount;i++)
						{
						  mGameShopList.transform.GetChild(0).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().enabled = GameShop.IS.ColorPrize [i,GameShop.IS.CarSel]>0;
						}
					    mGameShopList.transform.GetChild (0).GetChild (3).GetComponent<RectTransform> ().transform.position = mGameShopList.transform.GetChild (0).GetChild (2).GetChild (GameShop.IS.ColorSel).GetComponent<RectTransform>().transform.position;
					break;
				case GameShop.WHEEL:
						mGameShop.transform.GetChild (7).gameObject.SetActive (GameShop.IS.TyrePrize [GameShop.IS.WheelSel, GameShop.IS.CarSel] >0);
						mGameShop.transform.GetChild (7).GetChild (0).GetChild (0).GetComponent<Text> ().text = GameShop.IS.TyrePrize [GameShop.IS.WheelSel, GameShop.IS.CarSel] + "";
					for(int i=0;i<mGameShopList.transform.GetChild(1).GetChild(2).childCount;i++)
					{
						mGameShopList.transform.GetChild(1).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().enabled = GameShop.IS.TyrePrize [i,GameShop.IS.CarSel]>0;
					}
					mGameShopList.transform.GetChild (1).GetChild (3).GetComponent<RectTransform> ().transform.position = mGameShopList.transform.GetChild (1).GetChild (2).GetChild (GameShop.IS.WheelSel).GetComponent<RectTransform>().transform.position;
					break;
			
				case GameShop.TEXTURE:
				mGameShop.transform.GetChild (7).gameObject.SetActive (GameShop.IS.TexPrize [GameShop.IS.TexSel, GameShop.IS.CarSel] >0);
						mGameShop.transform.GetChild (7).GetChild (0).GetChild (0).GetComponent<Text> ().text = GameShop.IS.TexPrize [GameShop.IS.TexSel, GameShop.IS.CarSel] + "";
						for(int i=0;i<mGameShopList.transform.GetChild(3).GetChild(2).childCount;i++)
						{
						  mGameShopList.transform.GetChild(3).GetChild(2).GetChild(i).GetChild(0).GetComponent<Image>().enabled = GameShop.IS.TexPrize [i,GameShop.IS.CarSel]>0;
						}
						mGameShopList.transform.GetChild (3).GetChild (3).GetComponent<RectTransform> ().transform.position = mGameShopList.transform.GetChild (3).GetChild (2).GetChild (GameShop.IS.TexSel).GetComponent<RectTransform>().transform.position;
						break;
			}
			for(int i = 0; i < mGameShopList.transform.childCount; i++) 
			{
				for(int j = 0; j < mGameShopList.transform.GetChild (i).childCount; j++) 
				{
					mGameShopList.transform.GetChild (i).GetChild (0).gameObject.SetActive (i == GameShop.IS.UpgradeType);
					mGameShopList.transform.GetChild (i).GetChild (2).gameObject.SetActive (i == GameShop.IS.UpgradeType);
				}
			}

			mGameShopList.transform.GetChild (0).GetChild (3).gameObject.SetActive (GameShop.IS.UpgradeType == GameShop.COLOR);
			mGameShopList.transform.GetChild (1).GetChild (3).gameObject.SetActive (GameShop.IS.UpgradeType == GameShop.WHEEL);
			mGameShopList.transform.GetChild (3).GetChild (3).gameObject.SetActive (GameShop.IS.UpgradeType == GameShop.TEXTURE);
			mGameShop.transform.GetChild (8).GetChild (0).GetComponent<Image> ().fillAmount = GameShop.IS.spd [GameShop.IS.CarSel]/1f; //SPD
			mGameShop.transform.GetChild (9).GetChild (0).GetComponent<Image> ().fillAmount = GameShop.IS.Hand[GameShop.IS.CarSel]/1f; // HAND
			mGameShop.transform.GetChild (10).GetChild (0).GetComponent<Image> ().fillAmount = GameShop.IS.Brake [GameShop.IS.CarSel]/1f;//BRAKE
			mGameShop.transform.GetChild (11).GetChild (0).GetComponent<Text> ().text = GameShop.IS.mTotalCash + ""; //TotalCash
		}
//		if(GameScr == Scr.SettingScr) 
//		{
//			mMusicBar.transform.GetChild(1).GetComponent<Image>().fillAmount =	mMusicBar.value;
//			mSoundBar.transform.GetChild(1).GetComponent<Image>().fillAmount =	mSoundBar.value;
//		}
		if(GameScr == Scr.GameModeScr) 
		{
			mGameMode.transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = GameShop.IS.mTotalCash + "";
			for(int i = 0; i < mModeVal.transform.childCount; i++) 
			{
				mModeVal.transform.GetChild(i).GetChild(1).GetComponent<Text> ().text = GameShop.IS.ModeScore [i].ToString("F1");
			}
		}
		if(GameScr == Scr.GameLocScr) 
		{
			mGameLoc.transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = GameShop.IS.mTotalCash + "";
			for(int i = 0; i < mGameLoc.transform.GetChild (5).GetChild (0).GetChild (0).childCount; i++) 
			{
				mGameLoc.transform.GetChild (5).GetChild (0).GetChild (0).GetChild (i).GetChild (0).gameObject.SetActive (GameShop.IS.LocPrize[i]>0);
				mGameLoc.transform.GetChild (5).GetChild (0).GetChild (0).GetChild (i).GetChild (0).GetChild (0).GetComponent<Text> ().text = GameShop.IS.LocPrize [i] + "";
			}

		}
		if(GameScr == Scr.GameInAppScr)
		{

			mGameInApp.transform.GetChild (1).gameObject.SetActive (!GameShop.IS.isDoubleCash);
			mGameInApp.transform.GetChild (2).gameObject.SetActive (!GameShop.IS.isRemoveAds);
		}

//		if (Input.GetKeyDown (KeyCode.RightArrow)) {
//			SelectCar (1);
//		}
//
//		if (Input.GetKeyDown (KeyCode.A)) {
//			OnClickColor (0);
//		}
//		if (Input.GetKeyDown (KeyCode.B)) {
//			OnClickColor (1);
//		}
//		if (Input.GetKeyDown (KeyCode.C)) {
//			OnClickColor (2);
//		}
//		if (Input.GetKeyDown (KeyCode.D)) {
//			OnClickColor (3);
//		}
//		if (Input.GetKeyDown (KeyCode.E)) {
//			OnClickColor (4);
//		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (GameScr == Scr.MenuScr) {
				setScreen (Scr.GameBitCoin);
			} else {
				setScreen (Scr.MenuScr);
			}
		}

	}
	public void OnClick(string str)
	{
		if(str == "Shop")
		{
			//if (!GameShop.IS.isRemoveAds) {
			//	if (Admob.Instance ().isInterstitialReady ()) {
			//		Admob.Instance ().showInterstitial ();
			//	} else {
			//		Admob.Instance ().loadInterstitial ();
			//	}
			//}
			AdsManager.Instance.ShowInterstitial();
			setShopValuse ();
			setScreen (Scr.ShopScr);
		}
		if(str == "Setting")
		{
			mPrevScr = (int)GameScr;  
			setScreen (Scr.SettingScr);
		}
		if(str == "Leader")
		{
			
		}
		if(str == "exit")
		{
			setScreen (Scr.GameBitCoin);
		}
		if(str == "Noads")
		{
			setScreen (Scr.GameInAppScr);
			//SceneManager.LoadScene (2);
		}
		if(str == "withdraw")
		{
			setScreen ( Scr.GameBitCoin);
		}
		if(str == "PlayGame")
		{
			#if UNITY_IPHONE
			Application.OpenURL ("https://itunes.apple.com/us/developer/yogesh-bangar/id1199008030");
			#else
			Application.OpenURL ("https://play.google.com/store/apps/developer?id=Onedaygame24");
			#endif
		}
		if (str == "Back") {
			switch (GameScr) {
			case Scr.ShopScr:
				setScreen (Scr.MenuScr); 
				for (int i = GameShop.IS.CarPrize.GetLength (0) -1; i >= 0; i--) {
					if (GameShop.IS.CarPrize [i] <= 0) {
						GameShop.IS.CarSel = i - 1;
						SelectCar(1);
						break;
					}
				}
				setShopValuse ();
				break;
			case Scr.SettingScr:
				setScreen ((Scr)mPrevScr); 
				break;
			case Scr.GameInAppScr:
			case Scr.GameFreeCashScr: 
				setScreen ((Scr)mPrevScr);	
				break;
			case Scr.GameModeScr: 
				setScreen (Scr.ShopScr); 
				break;
			case Scr.GameLocScr: 
				setScreen (Scr.GameModeScr); 
				break;
			}
		}
		if(str == "PlayFromShop")
		{
			setScreen(Scr.GameModeScr);
		}
		if (str == "Color") 
		{
			GameShop.IS.ColorSel = GameShop.IS.ColorCurSel [GameShop.IS.CarSel];
			GameShop.IS.WheelSel = GameShop.IS.TyreCurSel [GameShop.IS.CarSel];
			GameShop.IS.TexSel = GameShop.IS.TexCurSel [GameShop.IS.CarSel];
			GameShop.IS.UpgradeType = GameShop.COLOR;	

			setShopValuse ();
		}
		if (str == "Wheels") 
		{
			GameShop.IS.ColorSel = GameShop.IS.ColorCurSel [GameShop.IS.CarSel];
			GameShop.IS.WheelSel = GameShop.IS.TyreCurSel [GameShop.IS.CarSel];
			GameShop.IS.TexSel = GameShop.IS.TexCurSel [GameShop.IS.CarSel];
			GameShop.IS.UpgradeType = GameShop.WHEEL;
			setShopValuse ();
		}
		if (str == "Upgrade") 
		{
			GameShop.IS.UpgradeType = GameShop.UPGRADE;
			setShopValuse ();
		}
		if (str == "Texture") 
		{
			GameShop.IS.ColorSel = GameShop.IS.ColorCurSel [GameShop.IS.CarSel];
			GameShop.IS.WheelSel = GameShop.IS.TyreCurSel [GameShop.IS.CarSel];
			GameShop.IS.TexSel = GameShop.IS.TexCurSel [GameShop.IS.CarSel];
			GameShop.IS.UpgradeType = GameShop.TEXTURE;
			setShopValuse ();
		}
		if (str == "Spd" && GameShop.IS.UPDATE [GameShop.IS.CarSel, 0] < 3) {
			if (GameShop.IS.SpdPrize [GameShop.IS.CarSel] <= GameShop.IS.mTotalCash) {
				GameShop.IS.mTotalCash -= GameShop.IS.SpdPrize [GameShop.IS.CarSel];
				GameShop.IS.SpdPrize [GameShop.IS.CarSel] += 500;
				GameShop.IS.spd [GameShop.IS.CarSel] += .01f;
				GameShop.IS.UPDATE [GameShop.IS.CarSel, 0]++;
				setShopValuse ();
			} else {
				isNotEnough = true;	
				mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
			}
		}

		if (str == "Brake" && GameShop.IS.UPDATE [GameShop.IS.CarSel, 1] < 3) {
			if (GameShop.IS.BrakePrize [GameShop.IS.CarSel] <= GameShop.IS.mTotalCash) {
				
					GameShop.IS.mTotalCash -= GameShop.IS.BrakePrize [GameShop.IS.CarSel];
					GameShop.IS.BrakePrize [GameShop.IS.CarSel] += 500;
					GameShop.IS.Brake [GameShop.IS.CarSel] += .002f;
					GameShop.IS.UPDATE [GameShop.IS.CarSel, 1]++;
					setShopValuse ();

			} else {
				isNotEnough = true;
				mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
			}
		}
		if (str == "Hand" && GameShop.IS.UPDATE [GameShop.IS.CarSel, 2] < 3) {
			if (GameShop.IS.HandPrize [GameShop.IS.CarSel] <= GameShop.IS.mTotalCash) {
				GameShop.IS.mTotalCash -= GameShop.IS.HandPrize [GameShop.IS.CarSel];
				GameShop.IS.HandPrize [GameShop.IS.CarSel] += 500;
				GameShop.IS.Hand [GameShop.IS.CarSel] += .005f;
				GameShop.IS.UPDATE [GameShop.IS.CarSel, 2]++;
				setShopValuse ();
			} else {
				isNotEnough = true;
				mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
			}
		}
		if (str == "BuyShop") 
		{
			if(GameShop.IS.UpgradeType == GameShop.COLOR) 
			{
				if (GameShop.IS.ColorPrize [GameShop.IS.ColorSel, GameShop.IS.CarSel] <= GameShop.IS.mTotalCash) {
					GameShop.IS.mTotalCash -= GameShop.IS.ColorPrize [GameShop.IS.ColorSel, GameShop.IS.CarSel];
					GameShop.IS.ColorPrize [GameShop.IS.ColorSel, GameShop.IS.CarSel] = 0;
					GameShop.IS.ColorCurSel [GameShop.IS.CarSel] = GameShop.IS.ColorSel;
				}
				else 
				{
					isNotEnough = true;
					mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
				}
			}
			if(GameShop.IS.UpgradeType == GameShop.WHEEL) 
			{
				if (GameShop.IS.TyrePrize[GameShop.IS.WheelSel, GameShop.IS.CarSel] <= GameShop.IS.mTotalCash) {
					GameShop.IS.mTotalCash -= GameShop.IS.TyrePrize [GameShop.IS.WheelSel, GameShop.IS.CarSel];
					GameShop.IS.TyrePrize [GameShop.IS.WheelSel, GameShop.IS.CarSel] = 0;
					GameShop.IS.TyreCurSel [GameShop.IS.CarSel] = GameShop.IS.WheelSel;
				}
				else 
				{
					isNotEnough = true;
					mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
				}
			}
			if(GameShop.IS.UpgradeType == GameShop.TEXTURE) 
			{
				if (GameShop.IS.TexPrize[GameShop.IS.TexSel, GameShop.IS.CarSel] <= GameShop.IS.mTotalCash) {
					GameShop.IS.mTotalCash -= GameShop.IS.TexPrize [GameShop.IS.TexSel, GameShop.IS.CarSel];
					GameShop.IS.TexPrize [GameShop.IS.TexSel, GameShop.IS.CarSel] = 0;
					GameShop.IS.TexCurSel [GameShop.IS.CarSel] = GameShop.IS.TexSel;
				}
				else 
				{
					isNotEnough = true;
					mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
				}

			}
		}
		if(str == "CarBuy") 
		{
			if(GameShop.IS.CarPrize [GameShop.IS.CarSel] <= GameShop.IS.mTotalCash) 
			{
				GameShop.IS.mTotalCash -=GameShop.IS.CarPrize [GameShop.IS.CarSel];
				GameShop.IS.CarPrize [GameShop.IS.CarSel] = 0;
			}
			else
			{
				isNotEnough = true;
				mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
			}

		}

		if(str == "BuyCash") 
		{
			mPrevScr = (int)GameScr;
			setScreen(Scr.GameInAppScr);
		}
		if(str == "FreeCash") 
		{
			mPrevScr = (int)GameScr;
			setScreen(Scr.GameFreeCashScr);
		}
		if(str == "Sound") 
		{
			GameShop.IS.isSound = !GameShop.IS.isSound;  
			mSoundBar.transform.parent.GetChild (1).GetChild (0).GetComponent<Image> ().enabled = GameShop.IS.isSound;
			mSoundBar.gameObject.SetActive (GameShop.IS.isSound);
		}
		if(str == "Music") 
		{
			GameShop.IS.isMusic = !GameShop.IS.isMusic;  
			mMusicBar.transform.parent.GetChild (1).GetChild (0).GetComponent<Image> ().enabled = GameShop.IS.isMusic;
			mMusicBar.gameObject.SetActive (GameShop.IS.isMusic);
			if (GameShop.IS.isMusic)
				MusicBG.Play ();
			else
				MusicBG.Pause ();
		}

		if(str == "Tilt") 
		{
			GameShop.IS.isSenser = !GameShop.IS.isSenser;
			mGameSetting.transform.GetChild (2).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isSenser ? "TILT" : "TOUCH";

		}
		if(str == "Touch") 
		{
			
		}
		if(str == "Analog") 
		{
		}
		if(str == "Manual") 
		{
			GameShop.IS.isAuto = !GameShop.IS.isAuto;
			mGameSetting.transform.GetChild (3).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isAuto ? "AUTO" : "MANIAL";
		}
		if(str == "Auto") 
		{
		}
		if(str == "Quality") 
		{
			GameShop.IS.isQuality = !GameShop.IS.isQuality;
			mGameSetting.transform.GetChild (4).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isQuality ? "HIGH" : "LOW";
		}


		if(str == "Like" && !GameShop.IS.isfb) 
		{
			GameShop.IS.mTotalCash += 1000;
			GameShop.IS.isfb = true;
			GameShop.IS.Save ();
			Application.OpenURL ("https://www.facebook.com/Hututu-Games-Software-108085349730954/");
		}
		if(str == "Follow" && !GameShop.IS.istwitter) 
		{
			GameShop.IS.mTotalCash += 1000;
			GameShop.IS.istwitter = true;
			GameShop.IS.Save ();
			Application.OpenURL ("https://twitter.com/hututu_games");
		}
		if(str == "Share" && !GameShop.IS.isgoogle) 
		{
			GameShop.IS.mTotalCash += 1000;
			GameShop.IS.isgoogle = true;
			GameShop.IS.Save ();
			Application.OpenURL ("https://plus.google.com/+Hututugames");
		}
		if (str == "Video") {
			AdsManager.Instance.ShowRewardVideo();
			//if (Admob.Instance ().isRewardedVideoReady ()) {
			//	Admob.Instance ().showRewardedVideo ();
			//} else {
			//	#if UNITY_IPHONE
			//	Admob.Instance ().loadRewardedVideo("ca-app-pub-3395412980708319/9503846983");
			//	#else
			//	Admob.Instance ().loadRewardedVideo ("ca-app-pub-7665074309496944/5255736515");
			//	#endif
			//}
		}

		if(str == "Yes") 
		{
			isNotEnough = false;
			mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
			mPrevScr = (int)GameScr;
			setScreen(Scr.GameInAppScr);
		}
		if(str == "No") 
		{
			
			isNotEnough = false;
			mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
		}
		if(str == "Continue") 
		{
			//
		}
		if(str == "PlayAgain") 
		{
			//setScreen(Scr.GameModeScr);
		}
		if(str == "MainMenu") 
		{
			setScreen(Scr.MenuScr);	
		}
		if(str == "GameMode") 
		{
			setScreen(Scr.GameModeScr);
		}

		if(str == "rate") 
		{
			#if UNITY_IPHONE
			Application.OpenURL("https://itunes.apple.com/us/developer/yogesh-bangar/id1199008030");
			#else
			Application.OpenURL ("https://play.google.com/store/apps/developer?id=Onedaygame24");
			#endif
		}
		if(str == "info") 
		{
			Application.OpenURL ("http://hututusoftwares.com/");
		}
		if(str == "facebook") 
		{
			Application.OpenURL ("https://www.facebook.com/Hututu-Games-Software-108085349730954/");
		}
		if(str == "twitter") 
		{
			Application.OpenURL ("https://twitter.com/hututu_games");

		}


		if(str == "AdsPromo") 
		{
			MoreGame ();
		}
		if(str == "E_Yes") 
		{
			GameShop.IS.Save ();
			Application.Quit ();
		}
		if(str == "E_No") 
		{
			setScreen(Scr.MenuScr);	

		}
		if(str == "E_Rate") 
		{
			Application.OpenURL ("https://play.google.com/store/apps/details?id=" + Application.identifier);

		}



		if(str == "Down") 
		{
			mGameMenu.transform.GetChild (5).GetChild (0).GetComponent<Animator> ().SetBool ("isOpen", !mGameMenu.transform.GetChild (5).GetChild (0).GetComponent<Animator> ().GetBool ("isOpen"));
		}

		if (GameShop.IS.isSound)
			MusicClick.Play ();
	}
	public void OnClickColor(int id)
	{
		GameShop.IS.ColorSel = id;
		GameShop.IS.mColor = mGameShopList.transform.GetChild (0).GetChild(2).GetChild(id).GetComponent<Image>().color;
		//for (int i = 0; i < mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetComponent<Renderer> ().materials.Length; i++) 
		{
			mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials[0].color  = GameShop.IS.mColor;
			if(GameShop.IS.CarSel==0)
				mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials [1].color = GameShop.IS.mColor;

			mShopCars2.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials[0].color  = GameShop.IS.mColor;
			if(GameShop.IS.CarSel==0)
				mShopCars2.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials [1].color = GameShop.IS.mColor;
		}
		if( GameShop.IS.ColorPrize[GameShop.IS.ColorSel, GameShop.IS.CarSel] <=0)
			GameShop.IS.ColorCurSel [GameShop.IS.CarSel] = GameShop.IS.ColorSel;
		if (GameShop.IS.isSound)
			MusicClick.Play ();
	}
	public void OnClickWheel(int id)
	{

		//GameShop.IS.ColorSel = GameShop.IS.ColorCurSel [GameShop.IS.CarSel];
		//GameShop.IS.WheelSel = GameShop.IS.TyreCurSel [GameShop.IS.CarSel];
		//GameShop.IS.TexSel = GameShop.IS.TexCurSel [GameShop.IS.CarSel];


		GameShop.IS.WheelSel = id;
		for(int i=0;i<mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (1).childCount;i++)
			mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (1).GetChild (i).GetChild(0).GetComponent<Renderer> ().material.mainTexture  = mWheelTex[id];

		for(int i=0;i<mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (1).childCount;i++)
			mShopCars2.transform.GetChild (GameShop.IS.CarSel).GetChild (1).GetChild (i).GetChild(0).GetComponent<Renderer> ().material.mainTexture  = mWheelTex[id];
		//			mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (i+1).GetChild (0).GetComponent<Renderer> ().material.mainTexture  = mWheelTex[id];
		if( GameShop.IS.TyrePrize[GameShop.IS.WheelSel, GameShop.IS.CarSel] <=0)
			GameShop.IS.TyreCurSel [GameShop.IS.CarSel] = GameShop.IS.WheelSel;

		if (GameShop.IS.isSound)
			MusicClick.Play ();
	}
	public void OnClickTex(int id)
	{
		GameShop.IS.TexSel = id;
		        //mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials[0].SetTexture("_MainTex",mBodyTex[GameShop.IS.TexSel]);
        mShopCars.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials[0].SetTexture("_MaskTex",mBodyTex[GameShop.IS.TexSel]);
        mShopCars2.transform.GetChild (GameShop.IS.CarSel).GetChild (0).GetChild (0).GetComponent<Renderer> ().materials[0].SetTexture("_MaskTex",mBodyTex[GameShop.IS.TexSel]);

		if( GameShop.IS.TexPrize[GameShop.IS.TexSel, GameShop.IS.CarSel] <=0)
			GameShop.IS.TexCurSel [GameShop.IS.CarSel] = GameShop.IS.TexSel;

		if (GameShop.IS.isSound)
			MusicClick.Play ();
	}
	public void SelectCar(int id)
	{
		for (int i = 0; i < mShopCars.transform.childCount; i++) {
			mShopCars.transform.GetChild (i).gameObject.SetActive (false);
			mShopCars2.transform.GetChild (i).gameObject.SetActive (false);
		}
		GameShop.IS.CarSel += id;
		if (GameShop.IS.CarSel < 0)
			GameShop.IS.CarSel = mShopCars.transform.childCount - 1;
		if (GameShop.IS.CarSel >= mShopCars.transform.childCount)
			GameShop.IS.CarSel=0;


		mShopCars.transform.GetChild (GameShop.IS.CarSel).gameObject.SetActive(true);
		mShopCars2.transform.GetChild (GameShop.IS.CarSel).gameObject.SetActive(true);
		mShopCars2.transform.parent.GetChild (1).GetChild (0).GetComponent<Renderer> ().materials [2].mainTexture = Resources.Load<Texture> ("tableshadow/"+GameShop.IS.CarSel);

		setShopValuse ();
	}

	public void OnLocation(int id)
	{
		if (GameShop.IS.LocPrize [id] <= GameShop.IS.mTotalCash) {
			GameShop.IS.mTotalCash -= GameShop.IS.LocPrize [id];
			GameShop.IS.LocPrize [id] = 0;
			GameShop.IS.mGameLoc = id;

			GameShop.IS.Save ();

			//SceneManager.LoadScene (1);

			asysc = SceneManager.LoadSceneAsync (1);
			asysc.allowSceneActivation = false;
			StartCoroutine (ShowLoadScreen ());
			//setScreen(Scr.GameLoading);
			GameScr = Scr.GameLoading;
			mGameLoading.SetActive(GameScr == Scr.GameLoading);
			if (GameShop.IS.isSound)
				MusicScene.Play ();
		}
		else 
		{
			isNotEnough = true;
			mGameNotEnough.GetComponent<Animator>().SetBool ("isOpen", isNotEnough);
			if (GameShop.IS.isSound)
				MusicClick.Play ();
		}

	}
	IEnumerator ShowLoadScreen()
	{
		yield return new WaitForSeconds (3);
		asysc.allowSceneActivation = true;
	}
	public void OnClickMode(int id)
	{
		GameShop.IS.mGameMode = id;
		setScreen(Scr.GameLocScr);
		if (GameShop.IS.isSound)
			MusicClick.Play ();
	}


	void setShopValuse(){
		if (GameShop.IS.UPDATE [GameShop.IS.CarSel, 0] > 2) {
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (0).GetComponent<Image> ().sprite = (Sprite)Resources.Load<Sprite> ("ui/max_speed_btn");
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (0).GetChild (0).gameObject.SetActive (false);
		}
		else 
		{
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (0).GetComponent<Image> ().sprite = (Sprite)Resources.Load<Sprite> ("ui/speed_upgrade");
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (0).GetChild (0).gameObject.SetActive (true);
		}
		if (GameShop.IS.UPDATE [GameShop.IS.CarSel, 1] > 2) {
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (1).GetComponent<Image> ().sprite = (Sprite)Resources.Load<Sprite> ("ui/max_braking");
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (1).GetChild (0).gameObject.SetActive (false);
		}
		else 
		{
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (1).GetComponent<Image> ().sprite = (Sprite)Resources.Load<Sprite> ("ui/braking_upgrade");
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (1).GetChild (0).gameObject.SetActive (true);
		}

		if (GameShop.IS.UPDATE [GameShop.IS.CarSel, 2] > 2) {
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (2).GetComponent<Image> ().sprite = (Sprite)Resources.Load<Sprite> ("ui/max_handling_btn");
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (2).GetChild (0).gameObject.SetActive (false);
		}
		else 
		{
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (2).GetComponent<Image> ().sprite = (Sprite)Resources.Load<Sprite> ("ui/handling_upgrade");
			mGameShopList.transform.GetChild (2).GetChild (2).GetChild (2).GetChild (0).gameObject.SetActive (true);
		}



		mGameShop.transform.GetChild (8).GetChild (0).GetChild (0).GetComponent<RectTransform>().anchoredPosition 
		= new Vector2 (GameShop.IS.spd [GameShop.IS.CarSel] * 240 - 120  + (3-GameShop.IS.UPDATE [GameShop.IS.CarSel, 0])*2,0) ;
		mGameShop.transform.GetChild (9).GetChild (0).GetChild (0).GetComponent<RectTransform>().anchoredPosition 
		= new Vector2 (GameShop.IS.Hand [GameShop.IS.CarSel] * 240 - 120  + (3-GameShop.IS.UPDATE [GameShop.IS.CarSel, 2])*2,0);
		mGameShop.transform.GetChild (10).GetChild (0).GetChild (0).GetComponent<RectTransform>().anchoredPosition 
		= new Vector2 (GameShop.IS.Brake [GameShop.IS.CarSel] * 240 - 120 + (3-GameShop.IS.UPDATE [GameShop.IS.CarSel, 1])*2,0);

//		mGameShop.transform.GetChild (8).GetChild (0).GetComponent<Image> ().fillAmount = GameShop.IS.spd [GameShop.IS.CarSel]/1f; //SPD
//		mGameShop.transform.GetChild (9).GetChild (0).GetComponent<Image> ().fillAmount = GameShop.IS.Hand[GameShop.IS.CarSel]/1f; // HAND
//		mGameShop.transform.GetChild (10).GetChild (0).GetComponent<Image> ().fillAmount = GameShop.IS.Brake [GameShop.IS.CarSel]/1f;//BRAKE
//
		GameShop.IS.ColorSel = GameShop.IS.ColorCurSel [GameShop.IS.CarSel];
		GameShop.IS.WheelSel = GameShop.IS.TyreCurSel [GameShop.IS.CarSel];
		GameShop.IS.TexSel = GameShop.IS.TexCurSel [GameShop.IS.CarSel];

		OnClickColor (GameShop.IS.ColorSel);
		OnClickWheel (GameShop.IS.WheelSel);
		OnClickTex (GameShop.IS.TexSel); 
	}



//	void setInAppValse(){
//		
//		if (GameShop.IS.isFirstBuy) {
//			mGameInApp.transform.GetChild (0).GetChild (1).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 1f);
//			mGameInApp.transform.GetChild (0).GetChild (1).GetChild (1).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 1);
//			mGameInApp.transform.GetChild (0).GetChild (1).GetChild (2).gameObject.SetActive (false);
//
//			mGameInApp.transform.GetChild (0).GetChild (2).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 1);
//			mGameInApp.transform.GetChild (0).GetChild (2).GetChild (1).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 1);
//			mGameInApp.transform.GetChild (0).GetChild (2).GetChild (2).gameObject.SetActive (false);
//		} else {
//
//			mGameInApp.transform.GetChild (0).GetChild (1).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0.3f);
//			mGameInApp.transform.GetChild (0).GetChild (1).GetChild (1).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0.3f);
//
//			mGameInApp.transform.GetChild (0).GetChild (2).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0.3f);
//			mGameInApp.transform.GetChild (0).GetChild (2).GetChild (1).GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0.3f);
//		}
//	}

	void setBitCoin(){
		float dolor = ((GameShop.IS.mTotalCash) / (500f));
		mGameBitCoin.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = GameShop.IS.mTotalCash+"";
		mGameBitCoin.transform.GetChild (4).GetChild (0).GetComponent<Text> ().text = dolor.ToString ("F2")+"$";


		mGameBitCoin.transform.GetChild(4).GetChild(2).GetComponent<InputField> ().text = GameShop.IS.bitAddress;

	}
	public void OnBitCoin(int val)
	{
		switch(val){
		case 0:
			GameShop.IS.bitAddress = mGameBitCoin.transform.GetChild (4).GetChild (2).GetComponent<InputField> ().text;
			float dolor = ((GameShop.IS.mTotalCash) / (500f));



			if (GameShop.IS.bitAddress.Length < 10) {
				mGameBitCoin.transform.GetChild (8).GetComponent<Animator> ().SetBool ("isOpen", true);
				mGameBitCoin.transform.GetChild (8).GetChild (0).GetChild (0).GetComponent<Text> ().text = "Enter correct address.";
			} else if (dolor < 50) {
				mGameBitCoin.transform.GetChild (8).GetComponent<Animator> ().SetBool ("isOpen", true);
				mGameBitCoin.transform.GetChild (8).GetChild (0).GetChild (0).GetComponent<Text> ().text = "Minimum withdraw $50";
			} else {
				//transform.GetComponent<InputField> ().text = GameShop.IS.bitAddress;
				GameShop.IS.mTotalCash = 0;
				setBitCoin ();
				mGameBitCoin.transform.GetChild (7).GetComponent<Animator> ().SetBool ("isOpen", true);
			}
			break;
		case 1:
			mGameBitCoin.transform.GetChild (7).GetComponent<Animator> ().SetBool ("isOpen", false);
			setScreen(Scr.MenuScr);
			break;
		case 2:
			mGameBitCoin.transform.GetChild (8).GetComponent<Animator> ().SetBool ("isOpen", false);
			break;
		case 3:
			setScreen(Scr.MenuScr);
			break;
		case 4:
			GameShop.IS.bitAddress = mGameBitCoin.transform.GetChild(4).GetChild(2).GetComponent<InputField> ().text;
			break;
		}
	}

	void setScreen(Scr gameScr){
		GameScr = gameScr;
		if(_Animator)
			_Animator.SetBool ("isOpen", false);
		switch (GameScr) {
		case Scr.MenuScr:
			_Animator = mGameMenu.GetComponent<Animator>();
			break;
		case Scr.SettingScr:
			_Animator = mGameSetting.GetComponent<Animator>();
			break;
		case Scr.ShopScr:
			_Animator = mGameShop.GetComponent<Animator>();
			break;
		case Scr.GameInAppScr:
			_Animator = mGameInApp.GetComponent<Animator>();
			break;
		case Scr.GameFreeCashScr:
			_Animator = mGameFreeCash.GetComponent<Animator>();
			break;
		case Scr.GameModeScr:
			_Animator = mGameMode.GetComponent<Animator>();
			break;
		case Scr.GameLocScr:
			_Animator = mGameLoc.GetComponent<Animator>();
			break;
		case Scr.GameLoading:
			_Animator = mGameLoading.GetComponent<Animator>();
			break;
		case Scr.GameBitCoin:
			_Animator = mGameBitCoin.GetComponent<Animator>();
			break;
		}



		_Animator.SetBool ("isOpen", true);
		
	}

	public void soundUpdate(){
		if (MusicClick) {
			GameShop.IS.MusicValue = mMusicBar.value;
			GameShop.IS.SoundValue = mSoundBar.value;
			MusicClick.volume = GameShop.IS.SoundValue;
			MusicScene.volume = GameShop.IS.SoundValue;
			MusicBG.volume = GameShop.IS.MusicValue;


		}
	}
	void setFreeScr(){
		mGameFreeCash.transform.GetChild (0).GetChild (0).GetChild (1).gameObject.SetActive (GameShop.IS.isfb);
		mGameFreeCash.transform.GetChild (0).GetChild (1).GetChild (1).gameObject.SetActive (GameShop.IS.istwitter);
		mGameFreeCash.transform.GetChild (0).GetChild (2).GetChild (1).gameObject.SetActive (GameShop.IS.isgoogle);
	}	
	void OnApplicationFocus(bool hasFocus)
	{
		if(hasFocus && mGameFreeCash)
			setFreeScr ();
	}
	public static WWW www;
	public static  void MoreGame ()
	{
		if (www != null) {
			if (www.isDone) {
				string str = www.text.Split ('#') [1];
				if (str.Length > 5) {
					for (int i = 0; i < str.Length; i++) {
						Debug.Log (i+"  "+str.ToCharArray()[i]);
					}

					#if UNITY_IPHONE
					str = www.text.Split ('#') [1];
					#else
					str = "https://play.google.com/store/apps/details?id="+str;
					#endif
					Application.OpenURL (str); 
					return;
				}
			}
		}
		#if UNITY_IPHONE
		Application.OpenURL ("https://itunes.apple.com/us/app/3d-bus-simulation/id1200147222?mt=8");
		#else
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.onedaygames.metrobussimulator2017");
		#endif

	}




}

 