using UnityEngine;
using System.Collections;

public class TouchOrbit : MonoBehaviour {

	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;

	public float smoothTime = 2f;
	
	float rotationYAxis = 0.0f;
	float rotationXAxis = 0.0f;
	
	float velocityX = 0.0f;
	float velocityY = 0.0f;
	private Touch touch;
	int TargetFrame=60;
	Vector3 Pos1,Pos2; 
	// Use this for initialization
	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = TargetFrame;
	}
	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;
	}
	void Update()
	{
		if(Application.targetFrameRate != TargetFrame)
			Application.targetFrameRate = TargetFrame;

		if (target && GameUi.GameScr == GameUi.Scr.ShopScr) { //&& UiManager.Screen == UiManager.GameScreen.CameraViewScr
			if (Input.GetMouseButtonDown (0)) {
				Pos1 = Input.mousePosition;
				Pos1.z = Camera.main.farClipPlane;
				Pos1 = Camera.main.ScreenToWorldPoint (Pos1);
				Pos1.Normalize ();
				//Debug.Log ("   GetMouseButtonDown  "+Pos1);
//				velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
//				velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
			}
			if (Input.GetMouseButton (0)) {

				Pos2 = Input.mousePosition;
				Pos2.z = Camera.main.farClipPlane;
				Pos2 = Camera.main.ScreenToWorldPoint (Pos2);
				Pos2.Normalize ();
				float dis = Vector2.Distance (Pos2, Pos1) * 3f * Time.deltaTime;
				velocityX += xSpeed * Input.GetAxis ("Mouse X") * dis;
				velocityY += ySpeed * Input.GetAxis ("Mouse Y") * dis;
				//Debug.Log("   GetMouseButton  "+Pos2);
			}
			if (Input.GetMouseButtonUp (0)) {

			}
//			if (Input.touchCount>0 &&(Input.GetTouch(0).phase == TouchPhase.Began|| Input.GetTouch(0).phase == TouchPhase.Moved)) //Input.touchCount == 1 && 
//			{
//				//One finger touch does orbit
//				touch = Input.GetTouch(0);
//				velocityX += xSpeed * touch.deltaPosition.x * 0.02f;
//				velocityY += ySpeed * touch.deltaPosition.y * 0.02f;
//			}
			rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
			rotationXAxis = ClampAngle (rotationXAxis, yMinLimit, yMaxLimit);
			Quaternion toRotation = Quaternion.Euler (rotationXAxis, rotationYAxis, 0);
			Quaternion rotation = toRotation;
			distance = Mathf.Clamp (distance, distanceMin, distanceMax);
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;
			transform.rotation = rotation;
			transform.position = position;
			//transform.position = Vector3.Lerp (transform.position,position, Time.deltaTime * smoothTime);// position;
			//transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.deltaTime*smoothTime);
			//transform.rotation = Quaternion.Euler (0,transform.eulerAngles.y,transform.eulerAngles.z);
			velocityX = Mathf.Lerp (velocityX, 0, Time.deltaTime * smoothTime);
			velocityY = Mathf.Lerp (velocityY, 0, Time.deltaTime * smoothTime);
		} else {
			
		}

	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
	public void OntouchCamera()
	{
		velocityX += xSpeed * 2f;
		velocityY += ySpeed * 2f;
	}
}

