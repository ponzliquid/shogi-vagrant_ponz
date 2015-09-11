using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using MiniJSON;

public class LoginManager : MonoBehaviour {
	
	private string userID = null;
	private string playID = null;
	private Text textAlert;
	private const string LOCAL_ACCESS_URL = "http://192.168.33.11:3000/";
	private string loggingURL;
	private Dictionary<string,object> download;

	public IEnumerator LoginToServer(string playerName, string roomNumber){

		loggingURL = LOCAL_ACCESS_URL;
		string loginURL = LOCAL_ACCESS_URL + "users/login";

		WWW www = ShogiHTTP.Instance.Login (loginURL, playerName, roomNumber);
		Debug.Log ("Logging in by local address...");
		textAlert.text = "Logging in to " + loginURL + " ...";
		yield return www;
		download = Json.Deserialize (www.text) as Dictionary<string,object>;
		yield return www.text;
		UserInfo.Instance.SetUserData (download);
		UserInfo.Instance.SetLoggingURL (loggingURL);
		textAlert.text = "Successful logging in to " + loginURL +".";
	}

	public IEnumerator LoginToServer(string loginURL, string playerName, string roomNumber){

		loggingURL = loginURL;
		loginURL = loginURL + "users/login";
		
		WWW www = ShogiHTTP.Instance.Login (loginURL, playerName, roomNumber);
		Debug.Log ("Logging in by manual address...");
		textAlert.text = "Logging in to " + loginURL + " ...";
		yield return www;
		download = Json.Deserialize (www.text) as Dictionary<string,object>;
		yield return www.text;
		UserInfo.Instance.SetUserData (download);
		UserInfo.Instance.SetLoggingURL (loggingURL);
		textAlert.text = "Successful logging in to " + loginURL +".";
	}

	public IEnumerator GetRoomState(){
		string url = loggingURL + "plays/" + UserInfo.Instance.GetPlayID().ToString() + "/state";
		Debug.Log ("access state URL : " + url);
		WWW www = ShogiHTTP.Instance.State(url);
		Debug.Log ("Getting Game State...");
		yield return www;
		download = Json.Deserialize (www.text) as Dictionary<string,object>;
		UserInfo.Instance.SetState (download);
//		if (download ["state"] == "waiting") {
//			UserInfo.Instance.SetState (download);
//			yield return StartCoroutine(GetRoomState());
//		} else {
//			UserInfo.Instance.SetState (download);
//			yield break;
//		}
	}

	public IEnumerator LogoutFromServer(string playID, string userID){

		string logoutURL = loggingURL + "users/login";
		
		userID = UserInfo.Instance.GetUserID ().ToString();
		playID = UserInfo.Instance.GetPlayID ().ToString();
		WWW www;
		www = ShogiHTTP.Instance.Logout (logoutURL, playID, userID);
		Debug.Log ("Logging out...");
		yield return www;
		UserInfo.Instance.InitUserData ();
	}
	
	public IEnumerator OnApplicationQuit(){
//		// 自動ログアウト処理
//		while (!UserInfo.Instance.IsUserDataNull ()) {
//			yield return StartCoroutine (
//				LogoutFromServer (UserInfo.Instance.GetPlayID().ToString(), UserInfo.Instance.GetUserID().ToString()));
//		}
//		Debug.Log ("Auto Logout");
		yield return null;
	}

	void Awake(){
	}
	
	void Start () {
		textAlert = GameObject.Find("MsgText").GetComponent<Text>();
	}

	void Update () {
	}
}
