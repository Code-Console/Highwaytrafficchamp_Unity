using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
	public Text _TextAnim,_TxtSPEED,_TxtDIST;
	public GameObject mGOLight,_TxtHighSeed;
	public GameObject mGOCanvas;
	GameObject mGO_Light;
	public float spd = 0.1f;
	public Transform mCamera;
	public int carPos = 12;

	public GameObject mGOCarPre;
	public float val = 0;
	public float MaxRotation = 8;
	public bool isSeet = false;

	public Vector3 rot = new Vector3();

	float cameraspd=0;
	float blink=0;
	float diffTIme =0;
	public GameObject CarRoot;
	float maxSPD = 1;
	float powerBreak = .008f;


	AudioSource MusicCarRun,MusicRunPause,MusicCarbreak,MusicCresh,MusicHorn;

	void Awake (){
		bool isDestry = true;
		CarRoot = GameObject.Find ("ShopCar");

		if (CarRoot == null) {
			CarRoot = mGOCarPre;
			isDestry = false;
		}
		GameObject Obj = (GameObject)Instantiate (CarRoot.transform.GetChild (GameShop.IS.CarSel).gameObject, new Vector3 (0, 0, 0), Quaternion.identity);
		Obj.transform.GetChild (2).transform.position = new Vector3 (-0.17f, 0.05f, -.25f);
		Obj.transform.GetChild (2).transform.localScale = new Vector3 (1.1f,1,1);
		Obj.SetActive (true);
		Obj.AddComponent<BoxCollider> ();
		Obj.GetComponent<BoxCollider> ().center = new Vector3 (0,1,0);
		Obj.GetComponent<BoxCollider> ().size = new Vector3 (2,2,5);
		Obj.transform.parent = transform;


		mGO_Light = (GameObject)Instantiate (mGOLight, new Vector3 (0, 0.5f, 1f), Quaternion.identity);
		mGO_Light.transform.parent = Obj.transform;
		mGO_Light.SetActive (false);

		if (isDestry) {
			
			Destroy (CarRoot);
		}



		if (GameShop.IS.UPDATE != null ) {
			maxSPD = GameShop.IS.UPDATE [GameShop.IS.CarSel, 0] * .02f + .9f + GameShop.IS.CarSel * .05f;
			powerBreak = (.008f + GameShop.IS.CarSel * .002f + GameShop.IS.UPDATE [GameShop.IS.CarSel, 1] * .001f);
		}


		MusicCresh = gameObject.AddComponent<AudioSource> ();
		MusicCresh.clip = (AudioClip)Resources.Load ("sound/small_impact");
		MusicCresh.volume = GameShop.IS.SoundValue;

		MusicHorn = gameObject.AddComponent<AudioSource> ();
		MusicHorn.clip = (AudioClip)Resources.Load ("sound/horn");
		MusicHorn.volume = GameShop.IS.SoundValue;

		MusicCarbreak = gameObject.AddComponent<AudioSource> ();
		MusicCarbreak.clip = (AudioClip)Resources.Load ("sound/break");
		MusicCarbreak.volume = GameShop.IS.SoundValue;

		MusicCarRun = gameObject.AddComponent<AudioSource> ();
		MusicCarRun.clip = (AudioClip)Resources.Load ("sound/car_normal");
		MusicCarRun.loop = true;
		if (GameShop.IS.isMusic) {
			//MusicCarRun.Play ();
			MusicCarRun.volume = GameShop.IS.SoundValue;
		}


		MusicRunPause = gameObject.AddComponent<AudioSource> ();
		MusicRunPause.clip = (AudioClip)Resources.Load ("sound/car_boost");
		MusicRunPause.loop = true;
		if (GameShop.IS.isMusic) {
			MusicRunPause.Play ();
			MusicRunPause.volume = GameShop.IS.SoundValue;
		}
	}

	void Start ()
	{
		//spd = M.SPD;
		GameUi.GameScr = GameUi.Scr.GamePlayScr;
		reset();
	}

	public bool isLeft,isRight,isForword,isBreak;

	float moveX = 0;
	public float check = .01f;


	public void reset(){
		cameraspd=0;
		M.gameOverCounter = 0;
		Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
		rigidbody.velocity = new Vector3(0f,0f,0f); 
		rigidbody.angularVelocity = new Vector3(0f,0f,0f);
		transform.rotation = Quaternion.Euler (0, 0, 0);
		transform.position = new Vector3 (2,0,0);
		spd = .21f;
		mCamera.position = new Vector3 (0, 10, transform.position.z - carPos);
		isLeft=isRight=isForword=isBreak= false;

		M.mScore = 0;
		M.mDistance = 0;
		M.MAXSPDTIME = 0;
		M.MAXSPDTIMEOPP = 0;
		M.mCoross = 0;
		blink = 50000;
	}


	public void resetTestDrive(){
		cameraspd=0;
		M.gameOverCounter = 0;
		Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
		rigidbody.velocity = new Vector3(0f,0f,0f); 
		rigidbody.angularVelocity = new Vector3(0f,0f,0f);
		transform.rotation = Quaternion.Euler (0, 0, 0);
		transform.position = new Vector3 (2,0,mCamera.position.z + carPos);
		spd = .21f;
		//mCamera.position = new Vector3 (0, 10, transform.position.z - carPos);
		isLeft=isRight=isForword=isBreak= false;

		M.mScore = 0;
		M.mDistance = 0;
		M.MAXSPDTIME = 0;
		M.MAXSPDTIMEOPP = 0;
		blink = 0;
	}



	void Update ()
	{
		
		if (GameUi.GameScr == GameUi.Scr.GamePauseScr) {
			if(MusicCarRun.isPlaying)
				MusicCarRun.Pause ();
			if(MusicRunPause.isPlaying)
				MusicRunPause.Pause ();
			return;
		}
		if (cameraspd == 0) {
			if (Input.GetAxis ("Vertical") < 0 || isBreak) {
				if (spd > 0.25)  {
					spd -= powerBreak;
				}
				if(rot.x < 3)
					rot.x += .5f;

				if(MusicCarRun.isPlaying)
					MusicCarRun.Pause ();
				if(MusicRunPause.isPlaying)
					MusicRunPause.Pause ();
				


			} else  if (Input.GetAxis ("Vertical") > 0 || isForword || GameShop.IS.isAuto) {
				if (spd < maxSPD) {
					spd += .002f + GameShop.IS.CarSel * .0005f;
				}
				if(rot.x > -3)
					rot.x -= .5f;


				if(!MusicCarRun.isPlaying && GameShop.IS.isSound)
					MusicCarRun.Play ();
				if(MusicRunPause.isPlaying)
					MusicRunPause.Pause ();

			}else {
				if (spd > 0.25 ) {
					spd -= .001f;
				} 
				if (rot.x > 0) {
					rot.x -= .5f;
					if(rot.x < 0)
						rot.x = 0;
				}
				if (rot.x < 0) {
					rot.x += .5f;
					if(rot.x > 0)
						rot.x = 0;
				}
				if(MusicCarRun.isPlaying )
					MusicCarRun.Pause ();
				if(!MusicRunPause.isPlaying  && GameShop.IS.isSound)
					MusicRunPause.Play ();
			}
			if(GameShop.IS.isSenser)
				SenserMove ();
			else
				arrowMove ();


			if (spd > .70) {
				M.mScore+=.1f*spd;
			}
		}
		if ((transform.position.x > 6.5 && moveX > 0) || (transform.position.x < -6.5 && moveX < 0)) {
			moveX = 0;
			rot.y = 0;
		}
		float nmov = (moveX*spd)*.15f ;
		transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x + nmov, 0, transform.position.z + spd), Time.deltaTime * M.TimeDelt);
		transform.GetChild(0).GetChild(0).rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (rot.x,rot.y*.1f,rot.y*.6f), Time.deltaTime * 100);

		for (int i = 0; i < 4; i++) {
			transform.GetChild(0).GetChild(1).GetChild(i).rotation *= Quaternion.AngleAxis (10, Vector3.right);
		}

		if (cameraspd == 0)
			mCamera.position = new Vector3 (0, 10, transform.position.z - carPos);
		else {
			mGOCanvas.transform.GetChild (0).GetChild (6).gameObject.SetActive (false);
			mCamera.position += Vector3.forward * cameraspd;
			if (cameraspd > 0.25 && M.gameOverCounter > 50) {
				cameraspd -= .001f;
			} 




//			if (M.gameOverCounter < 50) {
//				mCamera.position += Vector3.forward * cameraspd;
//			} else {
//				spd = 0;
//			}
			if(M.gameOverCounter == 10 && GameShop.IS.mGameMode != 2){
				mGOCanvas.GetComponent<GamePlayUI> ().GameOver ();
				if(MusicCarRun.isPlaying)
					MusicCarRun.Pause ();
				if(MusicRunPause.isPlaying)
					MusicRunPause.Pause ();
			}
			M.gameOverCounter++;
			if(M.gameOverCounter > 150 && GameShop.IS.mGameMode == 2){
				resetTestDrive ();
			}
		}


		if (M.mDistance >20000) {
			mGOCanvas.transform.GetChild (0).GetChild (6).gameObject.SetActive (cameraspd ==0);
			if(cameraspd ==0 && M.mDistance >20155){
				cameraspd = spd;
				spd = 0;
			}

		}

		M.SPD = spd;
		M.mDistance += spd;

		_TxtSPEED.text = (int)(spd*100) + "";

		float val = ((int)(M.mDistance / 10)) / 100f;
		//_TxtDIST.text = val + "" +(((int)(M.mDistance / 10))%100==0?".00":"")+(((int)(M.mDistance / 10))%10==0?"0":"");
		if (((int)(M.mDistance / 10)) % 100 == 0) {
			_TxtDIST.text = val + ".00";
		}
		else if (((int)(M.mDistance / 10)) % 10 == 0) {
			_TxtDIST.text = val + "0";
		} else {
			_TxtDIST.text = val + "";
		}
		if (spd > 1.0) {
			M.MAXSPDTIME+= (Time.time - diffTIme);
			_TxtHighSeed.transform.GetChild (0).GetComponent<Text> ().text = "" +     M.MAXSPDTIME.ToString("F1");


		}
		if(GameShop.IS.mGameMode == 1 && transform.position.x < -.7f && spd > 0.7)
			M.MAXSPDTIMEOPP += (Time.time - diffTIme);
		_TxtHighSeed.SetActive (spd > 1.0 && GameShop.IS.mGameMode != 2);
		_TxtDIST.transform.parent.gameObject.SetActive (GameShop.IS.mGameMode != 2);
		diffTIme = Time.time;

		if (blink < 100) {
			blink++;
			transform.GetChild (0).gameObject.SetActive (blink % 10 < 5 || blink >95);

		}

	}
	public void OnClick(int val){
		switch(val){
		case 0:
			isForword = true;
			break;
		case 1:
			isForword = false;
			break;
		case 2:
			isBreak = true;
			if(GameShop.IS.isSound)
				MusicCarbreak.Play ();
			break;
		case 3:
			isBreak = false;
			break;
		case 4:
			isLeft = true;
			break;
		case 5:
			isLeft = false;
			break;
		case 6:
			isRight = true;
			break;
		case 7:
			isRight = false;
			break;
		}
	}


	void OnCollisionEnter(Collision collision)
	{
		Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
		rigidbody.velocity = new Vector3(0f,0f,0f); 
		rigidbody.angularVelocity = new Vector3(0f,0f,0f);

		transform.rotation = Quaternion.Euler (0, 0, 0);

		Debug.Log (collision.gameObject.tag + "    " + (GameUi.GameScr == GameUi.Scr.GamePlayScr) + "   " + GameUi.GameScr);

		if (collision.gameObject.tag == "Opp") {
			if (!collision.gameObject.GetComponent<OppObj> ().getscore) {
				_TextAnim.gameObject.SetActive (false);
				if (M.mScore > 100) {
					M.mScore -= 100;
					_TextAnim.text = "-100";
					_TextAnim.text = (int)M.mScore + "";
					_TextAnim.gameObject.SetActive (true);
				}
			}

			if (spd > 0.5f || GameShop.IS.mGameMode == 1) {
				if(M.gameOverCounter ==0)
					M.gameOverCounter = 1;
				cameraspd = spd;
				spd = collision.gameObject.GetComponent<OppObj> ().spd;
				GameObject.Find ("Opponent").GetComponent<Opponent> ().IsInOver ();
			} else {
				if (transform.position.z > collision.transform.position.z) {
					collision.gameObject.GetComponent<OppObj> ().spd = 0;
				} else {
					if(M.gameOverCounter ==0)
						M.gameOverCounter = 1;
					collision.gameObject.GetComponent<OppObj> ().setCollide (spd);
					spd = collision.gameObject.GetComponent<OppObj> ().spd;
					spd = .1f;
				}
			}
			if(GameShop.IS.isSound && GameUi.GameScr == GameUi.Scr.GamePlayScr)
				MusicCresh.Play ();
		}

	}
	void OnTriggerEnter(Collider collision) {
		/*
		 if (collision.gameObject.tag == "Opp") {
			if (!collision.gameObject.GetComponent<OppObj> ().getscore) {
				_TextAnim.gameObject.SetActive (false);
				if (M.mScore > 100) {
					M.mScore -= 100;
					_TextAnim.text = "-100";
					_TextAnim.text = (int)M.mScore + "";
					_TextAnim.gameObject.SetActive (true);
				}
			}

			if (spd > 0.5f || GameShop.IS.mGameMode == 1) {
				if(M.gameOverCounter ==0)
					M.gameOverCounter = 1;
				cameraspd = spd;
				spd = collision.gameObject.GetComponent<OppObj> ().spd;
				GameObject.Find ("Opponent").GetComponent<Opponent> ().IsInOver ();
			} else {
				if (transform.position.z > collision.transform.position.z) {
					collision.gameObject.GetComponent<OppObj> ().spd = 0;
				} else {
					if(M.gameOverCounter ==0)
						M.gameOverCounter = 1;
					collision.gameObject.GetComponent<OppObj> ().setCollide (spd);
					spd = collision.gameObject.GetComponent<OppObj> ().spd;
					spd = .1f;
				}
			}
		}
		 */
	}

	void arrowMove(){
		if ((Input.GetAxis ("Horizontal") > 0 )) {
			isLeft = false;
			isRight = Input.GetAxis ("Horizontal")  > .1f;
		}
		if ((Input.GetAxis ("Horizontal") < 0 )) {
			isRight = false;
			isLeft = Input.GetAxis ("Horizontal") < -.1f;
		}

		MaxRotation = (moveX * spd) * 15;

		if (MaxRotation > 8)
			MaxRotation = 8;
		if (MaxRotation < -8)
			MaxRotation = -8;
		if (isRight) {
			
				moveX = 1;
			if (!isSeet)
				val = 0;
			if (rot.y < MaxRotation) {
				val += 10f;
				rot.y += Mathf.Sin (val * Mathf.Deg2Rad)*2;
			}
			isSeet = true;
		}


		if (isLeft) {
				moveX = -1;

			if (!isSeet)
				val = 0;
			if (rot.y > MaxRotation) {
				val += 10f;
				rot.y -= Mathf.Sin (val * Mathf.Deg2Rad)*2;
			}
			isSeet = true;
		}
		if (!isLeft&& !isRight) {
			moveX = 0;

		}

		if (moveX == 0) {
			if (isSeet)
				val = 60;
			if (rot.y > 0 ) {
				if(val > 21)
					val -= 5f;
				rot.y -= Mathf.Sin(val*Mathf.Deg2Rad)*2;

				if (rot.y < 0) {
					rot.y = 0;
				}
			}
			if (rot.y < 0 ) {
				if(val > 21)
					val -= 5f;
				rot.y += Mathf.Sin(val*Mathf.Deg2Rad)*2;

				if (rot.y > 0) {
					rot.y = 0;
				}
			}
			isSeet = false;
		}
	}
	void SenserMove(){
		moveX = Input.acceleration.x * 5f;
		MaxRotation = (moveX * spd)*15;
		if (MaxRotation > 8)
			MaxRotation = 8;
		if (MaxRotation < -8)
			MaxRotation = -8;



		if ((transform.position.x > 6.5 && moveX > 0) || (transform.position.x < -6.5 && moveX < 0)) {
			moveX = 0;
			rot.y = 0;
		}

		isRight = moveX > .1f;
		isLeft = moveX < -.1f;

		if (isRight) {
			if (!isSeet)
				val = 0;
			if (rot.y < MaxRotation) {
				val += 10f;
				rot.y += Mathf.Sin (val * Mathf.Deg2Rad)*2;
			}
			isSeet = true;
		}
		if (isLeft) {
			if (!isSeet)
				val = 0;
			if (rot.y > MaxRotation) {
				val += 10f;
				rot.y -= Mathf.Sin (val * Mathf.Deg2Rad)*2;
			}
			isSeet = true;
		}


		if (!isRight&&!isLeft) {
			if (isSeet)
				val = 60;
			if (rot.y > 0 ) {
				if(val > 21)
					val -= 5f;
				rot.y -= Mathf.Sin(val*Mathf.Deg2Rad)*2;

				if (rot.y < 0) {
					rot.y = 0;
				}
			}
			if (rot.y < 0 ) {
				if(val > 21)
					val -= 5f;
				rot.y += Mathf.Sin(val*Mathf.Deg2Rad)*2;

				if (rot.y > 0) {
					rot.y = 0;
				}
			}
			isSeet = false;
		}
	}



	public void showLight(bool isVeiw){
		mGO_Light.SetActive (isVeiw);
		if (isVeiw && GameShop.IS.isSound) {
			MusicHorn.Play ();
			Horn ();
		}
	}

	public void Horn(){
		Vector3 pos= transform.position+Vector3.up;
		RaycastHit  hit;
		float mSensorLen = 25;
		if (Physics.Raycast (pos, transform.forward, out hit, mSensorLen)) {
			
			if (hit.transform.tag == "Opp") 
			{
				Debug.DrawLine (pos, hit.point, Color.red);
				checkForRight (hit.transform);
			}
		}
	}
	void checkForRight(Transform oppTrance){

		bool isRightFree = true;
		bool isLeftFree = true;

		Vector3 pos = oppTrance.position + Vector3.up;
		RaycastHit hit;
		float mSensorLen = 3.9f;
		float z = oppTrance.GetComponent<BoxCollider> ().size.z / 2f;
		for (int i = 0; i < 5; i++) {
			pos = oppTrance.position + Vector3.up + (Vector3.forward) * z * (i - 2.5f);
			if (Physics.Raycast (pos, transform.right, out hit, mSensorLen)) {
				if (hit.transform.tag == "Opp") {
					Debug.DrawLine (pos, hit.point, Color.red);
					isRightFree = false;
				}
			}
			pos = oppTrance.position + Vector3.up + (Vector3.forward) * z * (i - 2.5f);
			if (Physics.Raycast (pos, -transform.right, out hit, mSensorLen)) {
				if (hit.transform.tag == "Opp") {
					Debug.DrawLine (pos, hit.point, Color.red);
					isLeftFree = false;
				}
			}
		}

		if (GameShop.IS.mGameMode == 1) {
			if (oppTrance.position.x > 0) {
				if (oppTrance.position.x > 0 && oppTrance.position.x < mSensorLen * .8f) {
					if (isRightFree) {
						oppTrance.GetComponent<OppObj> ().GiveSide (mSensorLen);
					}
					//						check Right
				} else if (oppTrance.position.x > mSensorLen) {
					if (isLeftFree) {
						oppTrance.GetComponent<OppObj> ().GiveSide (-mSensorLen);
					}
					//						check Left
				} 
			}
		} else {
			if (oppTrance.position.x < -mSensorLen) {
				if (isRightFree) {
					oppTrance.GetComponent<OppObj> ().GiveSide (mSensorLen);
				}
				//						check Right
			} else if (oppTrance.position.x > mSensorLen) {
				if (isLeftFree) {
					oppTrance.GetComponent<OppObj> ().GiveSide (-mSensorLen);
				}
				//						check Left
			} else {
				if (isLeftFree && isRightFree) {
					oppTrance.GetComponent<OppObj> ().GiveSide (oppTrance.position.x > 0 ? -mSensorLen : mSensorLen);
				} else if (isLeftFree) {
					oppTrance.GetComponent<OppObj> ().GiveSide (-mSensorLen);
				} else if (isRightFree) {
					oppTrance.GetComponent<OppObj> ().GiveSide (mSensorLen);
				}
			}
			Debug.Log ("isRightFree = "+isRightFree+" isLeftFree =  "+isLeftFree);
		}
	}

	public void updateSound(){
		MusicCarRun.volume = GameShop.IS.SoundValue;
		MusicRunPause.volume = GameShop.IS.SoundValue;
		MusicCarbreak.volume = GameShop.IS.SoundValue;
		MusicCresh.volume = GameShop.IS.SoundValue;
		MusicHorn.volume = GameShop.IS.SoundValue;
	}
}
