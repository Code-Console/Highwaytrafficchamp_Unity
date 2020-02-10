
// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DownloadTex : MonoBehaviour {
	IEnumerator Start()
	{
		#if UNITY_IPHONE
		string url = "http://hututusoftwares.com/Link/iphone.jpg";
		#else
		string url = "http://hututusoftwares.com/Link/ads.jpg";
		#endif


		WWW www = new WWW(url);
		yield return www;
		GetComponent<Image>().sprite = Sprite.Create( www.texture, new Rect(0.0f, 0.0f,  www.texture.width,  www.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
	}
}