using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Opponent : MonoBehaviour
{

	public Text _TextAnim;
	public Text _TextScore;
	public Transform mGO_Vehicle;
	public Transform mGO_Player;
	public Vector3 mPbox;
	float initx = 3.9f;
	float[] spd = new float[]{ 0.45f, 0.55f, 0.40f, 0.50f };
	bool[] setcar = new bool[]{ true,true,true,true };


	int counter = 0;


	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < 25; i++) {
			//GameObject veh = (GameObject)Instantiate (mGO_Vehicle.GetChild (Random.Range (0, 5)).gameObject);
			GameObject veh = (GameObject)Instantiate (mGO_Vehicle.GetChild(i%mGO_Vehicle.childCount).gameObject);
			veh.transform.parent = transform;
			veh.tag = "Opp";
			veh.AddComponent<OppObj> ();
			int pos = Random.Range (0, 4);
			transform.GetChild (i).GetComponent<OppObj> ().setOpp (spd [pos],new Vector3 (initx * (pos-1.5f), 0, 100 + i * 30));
		}
		Reset ();
	}

	public void Reset ()
	{
		counter = 0;
		for (int i = 0; i < transform.childCount; i++) {
			int pos = Random.Range (0, 4);
			if (GameShop.IS.mGameMode == 1) {
				spd = new float[]{ -.50f, -.65f, 0.45f, 0.60f };
				if(pos < 2)
					transform.GetChild (i).GetComponent<OppObj> ().setOpp (spd [pos],new Vector3 (initx * (pos-1.5f), 0, 50 + i * 60));
				else
					transform.GetChild (i).GetComponent<OppObj> ().setOpp (spd [pos],new Vector3 (initx * (pos-1.5f), 0, 50 + i * 30));

			}else
				transform.GetChild (i).GetComponent<OppObj> ().setOpp (spd [pos],new Vector3 (initx * (pos-1.5f), 0, 100 + i * 30));

			transform.GetChild (i).gameObject.SetActive (i<5);
		}
		_TextAnim.gameObject.SetActive (false);
	}


	void Update ()
	{
		if (GameUi.GameScr == GameUi.Scr.GamePauseScr) {
			return;
		}
		counter++;
		mPbox = mGO_Player.GetChild(0).GetComponent<BoxCollider> ().size*1.3f;
		mPbox.x += 1f;
		_TextScore.text = (int)M.mScore + "";
		_TextScore.transform.parent.gameObject.SetActive (GameShop.IS.mGameMode != 2);
		for (int i = 0; i < transform.childCount; i++) {

			if (transform.GetChild (i).gameObject.activeInHierarchy) {
				if (mGO_Player.position.z > transform.GetChild (i).position.z + 20) {
					transform.GetChild (i).gameObject.SetActive(false);
				}
				if (transform.GetChild (i).position.z < mGO_Player.position.z) {
					Vector3 box = transform.GetChild (i).GetComponent<BoxCollider> ().size;
					if (Rect2RectIntersection (transform.GetChild (i).position.x, transform.GetChild (i).position.z, box.x, box.z, mGO_Player.position.x, mGO_Player.position.z, mPbox.x, mPbox.z)) {
						if (M.SPD > 0.8) {
							if (!transform.GetChild (i).GetComponent<OppObj> ().getscore) {
								_TextAnim.gameObject.SetActive (false);
								_TextAnim.text = "+100";

								transform.GetChild (i).GetComponent<OppObj> ().getscore = true;
								M.mScore += 100;
								_TextScore.text = (int)M.mScore + "";
								_TextAnim.gameObject.SetActive (true);
								M.mCoross++;
							}
						}
					}

				}

				if (mGO_Player.position.z + 40 < transform.GetChild (i).position.z && mGO_Player.position.z + 60 > transform.GetChild (i).position.z && counter > 300 && GameUi.GameScr == GameUi.Scr.GamePlayScr) {
					if (GameShop.IS.mGameMode == 1) {
						counter = 0;
						if (transform.GetChild (i).position.x > 0) {
							check4Side (i);
							if (transform.GetChild (i).position.x > 0 && transform.GetChild (i).position.x < initx*.8f) {
								if (isRightFree) {
									transform.GetChild (i).GetComponent<OppObj> ().GiveSide (initx);
								}
								//						check Right
							} else if (transform.GetChild (i).position.x > initx) {
								if (isLeftFree) {
									transform.GetChild (i).GetComponent<OppObj> ().GiveSide (-initx);
								}
							} 
						}
					} else {
						counter = 0;
						check4Side (i);
						if (transform.GetChild (i).position.x < -initx) {
							if (isRightFree) {
								transform.GetChild (i).GetComponent<OppObj> ().GiveSide (initx);
							}
//						check Right
						} else if (transform.GetChild (i).position.x > initx) {
							if (isLeftFree) {
								transform.GetChild (i).GetComponent<OppObj> ().GiveSide (-initx);
							}
//						check Left
						} else {
							if (isLeftFree && isRightFree) {
								transform.GetChild (i).GetComponent<OppObj> ().GiveSide (transform.GetChild (i).position.x > 0 ? -initx : initx);
							} else if (isLeftFree) {
								transform.GetChild (i).GetComponent<OppObj> ().GiveSide (-initx);
							} else if (isRightFree) {
								transform.GetChild (i).GetComponent<OppObj> ().GiveSide (initx);
							}

//						check Right Left
						}
					}
				}


			}
			if (M.gameOverCounter == 50 && GameShop.IS.mGameMode != 2) {
				transform.GetChild (i).GetComponent<OppObj> ().spd = 0;
			}
		}
		if(M.gameOverCounter == 0)
			GenrateWay ();

		IsInRange ();
	}
	void GenrateWay(){
		float lastPos = findLast ();
		if ( lastPos < mGO_Player.position.z + 150) {
			int[] opp = new int[]{1,1,1,1,1};
			int newv = 0;
			float mindisTance = 110 - M.mDistance*.01f;
			if (mindisTance < 20)
				mindisTance = 20;
			if (GameShop.IS.mGameMode == 1) {
				if (Random.Range (0, 2) == 0) {
					float temp = spd [0]; 
					spd [0] = spd [1];
					spd [1] = temp;
				}
				if (Random.Range (0, 2) == 0) {
					float temp = spd [2]; 
					spd [2] = spd [3];
					spd [3] = temp;
				}
			} else {
				for (int i = 0; i < 4; i++) {
					int pos = Random.Range (0, 4);
					float temp = spd [pos]; 
					spd [pos] = spd [i];
					spd [i] = temp;
				}
			}


			if (M.mDistance > 1000 && M.mDistance < 2000) {
				//opp = new int[]{1,1,1,1,2};
				opp = new int[]{3,2,1,1,1};
			}
			if (M.mDistance > 2000 && M.mDistance < 3000) {
				//opp = new int[]{1,1,1,2,3};
				opp = new int[]{3,2,1,2,2};
			}
			if (M.mDistance > 3000 && M.mDistance < 4000) {
				//opp = new int[]{1,1,2,2,3};
				opp = new int[]{3,2,1,2,1};
			}
			if (M.mDistance > 4000 && M.mDistance < 5000) {
				//opp = new int[]{1,2,2,2,3};
				opp = new int[]{3,1,2,2,2};
			}
			if (M.mDistance > 5000 && M.mDistance < 6000) {
				//opp = new int[]{2,2,2,3,3};
				opp = new int[]{3,1,3,1,3};
			}
			if (M.mDistance > 6000 && M.mDistance < 7000) {
				opp = new int[]{3,1,2,2,1};
			}
			if (M.mDistance > 7000) {
				opp = new int[]{3,3,1,3,3};
			}
			for (int j = 0; j < 5; j++) {
				setPos (opp[j]);
				int k = 0;
				for (int i = 0; i < transform.childCount && k < 4; i++) {
					if (!transform.GetChild (i).gameObject.activeInHierarchy) {
						if (setcar [k]) {
							transform.GetChild (i).gameObject.SetActive (true);
							newv++;
							if(GameShop.IS.mGameMode == 1 && spd [k] < 0)
								transform.GetChild (i).GetComponent<OppObj> ().setOpp (spd [k], new Vector3 (initx * (k - 1.5f), 0, lastPos+(j*60) + 110));
							else
								transform.GetChild (i).GetComponent<OppObj> ().setOpp (spd [k], new Vector3 (initx * (k - 1.5f), 0, lastPos+(j*20) + 110));
						}
						k++;
					}
				}
			}

		}

	}

	float findLast ()
	{
		float z = mGO_Player.transform.position.z +50;
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).gameObject.activeInHierarchy) {
				if (z < transform.GetChild (i).position.z) {
					z = transform.GetChild (i).position.z;
				}
			}
		}
		return z;
	}


	void setPos (int p)
	{
		int pos = Random.Range (0, setcar.GetLength (0));
		switch (p) {
		case 3:
			for (int i = 0; i < setcar.GetLength (0); i++) {
				setcar [i] = i != pos;
			}
			break;
		case 2:
			for (int i = 0; i < setcar.GetLength (0); i++) {
				setcar [i] = i != pos;
			}
			int nRand = Random.Range (0, setcar.GetLength (0));
			if (nRand == pos) {
				nRand++;
				nRand %= setcar.GetLength (0);
			}
			pos = nRand;
			setcar [pos] = true;
			break;
		default:
			for (int i = 0; i < setcar.GetLength (0); i++) {
				setcar [i] = i == pos;
			}
			break;
		}
	}




	void IsInRange(){
		M.IsGO = false;
		for (int i = 0; i < transform.childCount && !M.IsGO; i++) {
			if (mGO_Player.position.z > transform.GetChild(i).position.z-80 && transform.GetChild (i).gameObject.activeInHierarchy) {
				M.IsGO = true;
			}
		}
	}
	public void IsInOver(){
		M.IsGO = false;
		for (int i = 0; i < transform.childCount && !M.IsGO; i++) {
			if (mGO_Player.position.z +60 < transform.GetChild(i).position.z) {
				transform.GetChild (i).gameObject.SetActive (false);
			}
		}
	}

	bool Rect2RectIntersection (double ax, double ay, double adx, double ady, double bx, double by, double bdx,
	                           double bdy)
	{

		ax -= adx / 2;
		ay += ady / 2;
		bx -= bdx / 2;
		by += bdy / 2;
		if (ax + adx > bx && ay - ady < by && bx + bdx > ax && by - bdy < ay) {
			return true;
		}

		return false;
	}
	bool isLeftFree = true;
	bool isRightFree = true;

	void check4Side(int i){
		isLeftFree = true;
		isRightFree = true;
		Vector3 box = transform.GetChild (i).GetComponent<BoxCollider> ().size;
		box.z *= 3;
		if (transform.GetChild (i).position.x > -initx) {//checkLeft
			for (int j = 0; j < transform.childCount && isLeftFree; j++) {
				Vector3 boxJ = transform.GetChild (i).GetComponent<BoxCollider> ().size;
				if (Rect2RectIntersection (transform.GetChild (i).position.x-initx, transform.GetChild (i).position.z, box.x, box.z, 
					transform.GetChild (j).position.x, transform.GetChild (j).position.z, boxJ.x, boxJ.z) && i!=j) {
					isLeftFree = false;
				}
			}
		}
		if (transform.GetChild (i).position.x < initx) {//checkLeft
			for (int j = 0; j < transform.childCount && isRightFree; j++) {
				Vector3 boxJ = transform.GetChild (i).GetComponent<BoxCollider> ().size;
				if (Rect2RectIntersection (transform.GetChild (i).position.x+initx, transform.GetChild (i).position.z, box.x, box.z, 
					transform.GetChild (j).position.x, transform.GetChild (j).position.z, boxJ.x, boxJ.z) && i!=j) {
					isRightFree = false;
				}
			}
		}

		if (transform.GetChild (i).position.x > initx) {
			isRightFree = false;
		}
		if (transform.GetChild (i).position.x < -initx) {
			isLeftFree = false;
		}
	}



	void findNearest(){
	}


}
