using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using admob;
//com.hututugames.Highwaytrafficchamp
//com.onedaygames.Highwaytrafficchamp
//${PRODUCT_NAME} Camera Use
/*/ <summary>
 Hello
We need all snacks design should be like circular form 
and pattern should be in synchronise  manner  
C-84 County Walk Colony, Near Shishunkunj School,Opp. Best price bypass(loc. near JHALARIA Village)
/// </summary>
/// 
/// 


Excellent knowledge of Unity(c#), including experience with scripting, textures, animation, GUI styles Level design, game physics and particle systems Experience with game development in Android. iOS, Windows and Mac OS X Strong understanding of object-oriented programming Experience with Augmented reality and virtual reality is plus

Work in core android iPhone web application and games we have strong team to work dedicatedly. 

Please visit out portfolio 
http://hututusoftwares.com/
https://itunes.apple.com/us/developer/yogesh-bangar/id1199008030
https://play.google.com/store/apps/developer?id=Onedaygame24


*/



public class GamePlayUI : MonoBehaviour {
	public GameObject mGORoad;
	GameObject mGameOver;
	GameObject mTrack;
	public GameObject mCar;
	public GameObject mOpponet;
	[SerializeField] Scrollbar mMusicBar=null,mSoundBar=null;
	int TargetFrame=60;
	AudioSource MusicClick;
	public static  AudioSource MusicBG,MusicGameOver;

	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = TargetFrame;
	}
	// Use this for initialization
	void Start () {
		mTrack = (GameObject)Instantiate (mGORoad.transform.GetChild(GameShop.IS.mGameLoc).gameObject, new Vector3 (0, 0, 0), Quaternion.identity);
		mGameOver = transform.GetChild (1).gameObject;
		transform.GetChild (0).GetComponent<Animator> ().SetBool ("isOpen", true);

		MusicClick = gameObject.AddComponent<AudioSource> ();
		MusicClick.clip = (AudioClip)Resources.Load ("sound/click");
		MusicClick.volume = GameShop.IS.SoundValue;

		MusicGameOver = gameObject.AddComponent<AudioSource> ();
		MusicGameOver.clip = (AudioClip)Resources.Load ("sound/gameover");
		MusicGameOver.loop = true;
		MusicGameOver.volume = GameShop.IS.SoundValue;

		MusicBG = gameObject.AddComponent<AudioSource> ();
		MusicBG.clip = (AudioClip)Resources.Load ("sound/gameplay");
		MusicBG.loop = true;
		if (GameShop.IS.isMusic) {
			MusicBG.Play ();
			MusicBG.volume = GameShop.IS.MusicValue;
		}
		mMusicBar.value = GameShop.IS.MusicValue;
		mSoundBar.value = GameShop.IS.SoundValue;

		transform.GetChild (3).GetChild (2).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isSenser ? "TILT" : "TOUCH"; 
		transform.GetChild (3).GetChild (3).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isAuto ? "AUTO" : "MANUAL"; 

		mMusicBar.transform.GetChild(1).GetComponent<Image>().fillAmount =	mMusicBar.value;
		mSoundBar.transform.GetChild(1).GetComponent<Image>().fillAmount =	mSoundBar.value;

		setBreatpos ();
		AdsManager.Instance.Init();
		//if(!GameShop.IS.isRemoveAds)
		//	Admob.Instance ().loadInterstitial ();

		//#if UNITY_IPHONE
		//Admob.Instance ().loadRewardedVideo ("ca-app-pub-3395412980708319/9503846983");
		//#else
		//Admob.Instance ().loadRewardedVideo ("ca-app-pub-7665074309496944/5255736515");
		//#endif
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.targetFrameRate != TargetFrame)
			Application.targetFrameRate = TargetFrame;


		if (M.gameOverCounter > 50) {
			if (Score < M.mScore) {
				
				Score += 23;
				if (Score > M.mScore) {
					Score = (int)M.mScore;
				}
				mGameOver.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = Score + "";
			}
		}
		if(GameUi.GameScr == GameUi.Scr.GameOverScr)
			mGameOver.transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = GameShop.IS.mTotalCash + "";
	}
	int Score =0;
	public void GameOver (){


		Debug.Log ("GameOver  ~~~~ ");
		MusicBG.Pause ();
		if (GameShop.IS.isMusic)
			MusicGameOver.Play ();
		//transform.GetChild (1).GetComponent<Animator> ().SetBool ("isOpen", true);
		Score =0;
		GameUi.GameScr = GameUi.Scr.GameOverScr;
		//if(GameScr == Scr.GameOverScr) 
		{
			//transform.GetChild (0).gameObject.SetActive (false);
			transform.GetChild (0).GetComponent<Animator> ().SetBool ("isOpen", false);
			mGameOver.SetActive (true);


			string str = "";

			float val = ((int)(M.mDistance / 10)) / 100f;
			//_TxtDIST.text = val + "" +(((int)(M.mDistance / 10))%100==0?".00":"")+(((int)(M.mDistance / 10))%10==0?"0":"");
			if (((int)(M.mDistance / 10)) % 100 == 0) {
				str = val + ".00";
			}
			else if (((int)(M.mDistance / 10)) % 10 == 0) {
				str = val + "0";
			} else {
				str = val + "";
			}


			mGameOver.transform.GetChild (7).GetChild (1).GetChild(0).GetComponent<Text> ().text = str; //Distnace
			mGameOver.transform.GetChild (7).GetChild (1).GetChild(1).GetComponent<Text> ().text = M.mCoross+""; //OverTake
			mGameOver.transform.GetChild (7).GetChild (1).GetChild(2).GetComponent<Text> ().text = M.MAXSPDTIME.ToString("F1") + ""; //Over100
			mGameOver.transform.GetChild (7).GetChild (1).GetChild(3).GetComponent<Text> ().text =  GameShop.IS.mGameMode != 1 ? "-":M.MAXSPDTIMEOPP.ToString("F1") + ""; //OpposstiteDir


			mGameOver.transform.GetChild (7).GetChild (2).GetChild(0).GetComponent<Text> ().text = ((int)(M.mDistance*.12f))+""; //Distnace Cash
			mGameOver.transform.GetChild (7).GetChild (2).GetChild(1).GetComponent<Text> ().text = (M.mCoross*10)+""; //OverTake Cash
			mGameOver.transform.GetChild (7).GetChild (2).GetChild(2).GetComponent<Text> ().text = ((int)(M.MAXSPDTIME*5f))+"";  //Over100 Cash
			mGameOver.transform.GetChild (7).GetChild (2).GetChild(3).GetComponent<Text> ().text = GameShop.IS.mGameMode != 1 ?"-":(((int)(M.MAXSPDTIMEOPP*5f))+""); //OpposstiteDir Cash

			int total = (int)((M.mDistance * .12f) + (M.mCoross * 10) + (M.MAXSPDTIME * 5f)+ (M.MAXSPDTIMEOPP * 5f));

			if (GameShop.IS.isDoubleCash) {
				mGameOver.transform.GetChild (7).GetChild (4).GetComponent<Text> ().text = total + " x2"; //Total
				GameShop.IS.mTotalCash += total*2;
			} else {
				mGameOver.transform.GetChild (7).GetChild (4).GetComponent<Text> ().text = total + ""; //Total
				GameShop.IS.mTotalCash += total;
			}

			mGameOver.transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = GameShop.IS.mTotalCash + "";
			mGameOver.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = "0";


			GameShop.IS.ModeScore [GameShop.IS.mGameMode] += M.mDistance / 1000;
		}
		GameShop.IS.Save ();
		if (!GameShop.IS.isRemoveAds) {
			AdsManager.Instance.ShowInterstitial();
			//if (Admob.Instance ().isInterstitialReady ()) {
			//	Admob.Instance ().showInterstitial ();
			//} else {
			//	Admob.Instance ().loadInterstitial ();
			//}
		}
	}
	public void GameOverClick(int val){
		switch (val) {
		case 0://Menu
			GameUi.GameScr = GameUi.Scr.MenuScr;
			SceneManager.LoadScene(0);
			break;
		case 1://GERAGE
			GameUi.GameScr = GameUi.Scr.ShopScr;
			SceneManager.LoadScene(0);
			break;
		case 2://PLAY AGAIN
			Reset();
			break;
		case 3://CLICK PAUSE
			transform.GetChild (2).GetComponent<Animator> ().SetBool ("isOpen", true);
			GameUi.GameScr = GameUi.Scr.GamePauseScr;
			MusicBG.Pause ();
			if (!GameShop.IS.isRemoveAds) {
					//if (Admob.Instance ().isInterstitialReady ()) {
					//	Admob.Instance ().showInterstitial ();
					//} else {
					//	Admob.Instance ().loadInterstitial ();
					//}
					AdsManager.Instance.ShowInterstitial();
				}
			break;
		case 4://GO
				AdsManager.Instance.ShowRewardVideo();
				//if (Admob.Instance ().isRewardedVideoReady()){
				//	Admob.Instance ().showRewardedVideo();
				//} else {
				//	#if UNITY_IPHONE
				//	Admob.Instance ().loadRewardedVideo("ca-app-pub-3395412980708319/9503846983");
				//	#else
				//	Admob.Instance ().loadRewardedVideo("ca-app-pub-7665074309496944/5255736515");
				//	#endif
				//}
				break;
		case 5://FreeCash
			GameUi.GameScr = GameUi.Scr.GameFreeCashScr;
			SceneManager.LoadScene(0);
			break;
		case 6://BuyCAsh
			GameUi.GameScr = GameUi.Scr.GameInAppScr;
			SceneManager.LoadScene(0);
			break;
		}
	}
	public void GamePauseClick(int val){
		switch (val) {
		case 0://Continue
			GameUi.GameScr = GameUi.Scr.GamePlayScr;
			transform.GetChild (2).GetComponent<Animator> ().SetBool ("isOpen", false);
			if(GameShop.IS.isMusic)
				MusicBG.Play ();
			break;
		case 1://Setting
			transform.GetChild (2).GetComponent<Animator> ().SetBool ("isOpen", false);
			transform.GetChild (3).GetComponent<Animator> ().SetBool ("isOpen", true);
			break;
		case 2://PLAY AGAIN
			Reset();
			transform.GetChild (2).GetComponent<Animator> ().SetBool ("isOpen", false);
			GameUi.GameScr = GameUi.Scr.GamePlayScr;
			if(GameShop.IS.isMusic)
				MusicBG.Play ();
			break;
		case 3://Main Menu
			GameUi.GameScr = GameUi.Scr.MenuScr;
			SceneManager.LoadScene(0);
			break;
		case 4://Game Mode
			GameUi.GameScr = GameUi.Scr.GameModeScr;
			SceneManager.LoadScene(0);
			break;
		}
	}
	public void GameSettingClick(int val){
		switch (val) {
		case 0://Music
			GameShop.IS.isMusic = !GameShop.IS.isMusic;
			mMusicBar.transform.parent.GetChild (1).GetChild (0).GetComponent<Image> ().enabled = GameShop.IS.isMusic;
			mMusicBar.transform.GetComponent<Image> ().enabled = GameShop.IS.isMusic;
			mMusicBar.transform.GetChild (1).GetComponent<Image> ().enabled = GameShop.IS.isMusic;
			break;
		case 1://Sound
			GameShop.IS.isSound = !GameShop.IS.isSound;
			mSoundBar.transform.parent.GetChild (1).GetChild (0).GetComponent<Image> ().enabled = GameShop.IS.isSound;
			mSoundBar.transform.GetComponent<Image> ().enabled = GameShop.IS.isSound;
			mSoundBar.transform.GetChild (1).GetComponent<Image> ().enabled = GameShop.IS.isSound;
			break;
		case 2://Steering
			GameShop.IS.isSenser = !GameShop.IS.isSenser;
			transform.GetChild (3).GetChild (2).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isSenser ? "TILT" : "TOUCH"; 
			setBreatpos ();
			break;
		case 3://Accelaretion
			GameShop.IS.isAuto = !GameShop.IS.isAuto;
			transform.GetChild (3).GetChild (3).GetChild (1).GetChild (0).GetComponent<Text> ().text = GameShop.IS.isAuto ? "AUTO" : "MANUAL"; 
			setBreatpos ();
			break;
		case 4://Quality
			break;
		case 5://MusicChange
			GameShop.IS.MusicValue = mMusicBar.value;
			mMusicBar.transform.GetChild(1).GetComponent<Image>().fillAmount =	GameShop.IS.MusicValue;
			updateSound ();
			break;
		case 6://SoundChange
			GameShop.IS.SoundValue = mSoundBar.value;
			mSoundBar.transform.GetChild (1).GetComponent<Image> ().fillAmount =	GameShop.IS.SoundValue;
			updateSound ();
			break;
		case 7://Back
			GameUi.GameScr = GameUi.Scr.GamePlayScr;
			transform.GetChild (3).GetComponent<Animator> ().SetBool ("isOpen", false);
			if(GameShop.IS.isMusic)
				MusicBG.Play ();
			break;
		}
	}
	void Reset(){

		MusicGameOver.Pause ();
		if (GameShop.IS.isMusic)
			MusicBG.Play ();
		mCar.GetComponent<Player>().reset();
		mOpponet.GetComponent<Opponent>().Reset();
		mTrack.GetComponent<Track>().ResetGame();
		mGameOver.SetActive (false);
		transform.GetChild (0).gameObject.SetActive (true);
		transform.GetChild (0).GetComponent<Animator> ().SetBool ("isOpen", true);
		//if(!GameShop.IS.isRemoveAds)
		//	AdsManager.Instance.ShowInterstitial();
		//Admob.Instance ().loadInterstitial ();




	}
	void OnApplicationPause(bool pauseStatus)
	{
		if (GameUi.GameScr == GameUi.Scr.GamePlayScr) {
			transform.GetChild (2).GetComponent<Animator> ().SetBool ("isOpen", true);
			GameUi.GameScr = GameUi.Scr.GamePauseScr;
			if(GameShop.IS.isMusic)
				MusicBG.Pause ();
		}
	}

	void updateSound(){
		if (MusicClick) {
			MusicClick.volume = GameShop.IS.SoundValue;
			MusicGameOver.volume = GameShop.IS.SoundValue;
			MusicBG.volume = GameShop.IS.MusicValue;
			mCar.GetComponent<Player> ().updateSound ();
		}
	}


	void setBreatpos(){
		RectTransform RectBreak = transform.GetChild (0).GetChild (3).GetComponent<RectTransform> ();
		transform.GetChild (0).GetChild (5).gameObject.SetActive (!GameShop.IS.isSenser);
		transform.GetChild (0).GetChild (2).gameObject.SetActive (!GameShop.IS.isAuto);
		if (GameShop.IS.isSenser) {
			RectBreak.anchorMax = new Vector2 (0, 0);
			RectBreak.anchorMin = new Vector2 (0, 0);
			RectBreak.anchoredPosition = new Vector2 (200, 100);
		} else {
			RectBreak.anchorMax = new Vector2 (1, 0);
			RectBreak.anchorMin = new Vector2 (1, 0);
			RectBreak.anchoredPosition = new Vector2 (-351, 113);
		}
	}

}
