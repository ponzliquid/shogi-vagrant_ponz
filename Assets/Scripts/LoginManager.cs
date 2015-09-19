using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using MiniJSON;

public class LoginManager : MonoBehaviour {

	private const string LOCAL_ACCESS_URL = "http://192.168.33.11:3000/";
	private Dictionary<string,object> download;

	// 決め打ちアドレスでログイン
	public void LoginToServer(string playerName, string roomNumber){

		string loggingURL = LOCAL_ACCESS_URL;
		string loginURL = LOCAL_ACCESS_URL + "users/login";

		LoginUI loginUI = GameObject.Find("LoginUI").GetComponent<LoginUI>();
		Debug.Log ("Logging in by local address...");

		ShogiHTTP.Instance.Login (loginURL, playerName, roomNumber,
		                          (Dictionary<string, object> dic) => {
			UserInfo.Instance.SetUserData (dic);
			UserInfo.Instance.SetLoggingURL (loggingURL);

			loginUI.OutputMsg("Successful logging in to " + loginURL +".");
			return;
		});
		loginUI.OutputMsg("Failed to Login " + loginURL +".");
		Debug.LogError ("Err: Failed to Login" + loginURL);
		return;
	}

	// 入力アドレスでログイン
	public void LoginToServer(string loginURL, string playerName, string roomNumber){

		loginURL = "http://" + loginURL + "/";
		string loggingURL = loginURL;
		loginURL = loginURL + "users/login";
		
		LoginUI loginUI = GameObject.Find("LoginUI").GetComponent<LoginUI>();
		Debug.Log ("Logging in by manual address...");

		ShogiHTTP.Instance.Login (loginURL, playerName, roomNumber,
		                          (Dictionary<string, object> dic) => {
			UserInfo.Instance.SetUserData (dic);
			UserInfo.Instance.SetLoggingURL (loggingURL);

			loginUI.OutputMsg("Successful logging in to " + loginURL +".");
			return;
		});
		loginUI.OutputMsg("Failed to Login " + loginURL +".");
		loginUI.SetLoginButton (true);
		Debug.LogError ("Err: Failed to Login" + loginURL);
		return;
	}

	public string GetRoomState(){
		string url = UserInfo.Instance.urlLogging + "plays/"
					+ UserInfo.Instance.GetPlayID().ToString() + "/state";
		ShogiHTTP.Instance.State(url)

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

//	public void LogoutFromServer(string playID, string userID){
//
//		string logoutURL = loggingURL + "users/login";
//		
////		userID = UserInfo.Instance.GetUserID ().ToString();
////		playID = UserInfo.Instance.GetPlayID ().ToString();
//
//		ShogiHTTP.Instance.Logout (logoutURL, playID, userID,
//		                           (string str) => {
//			if(str == "true"){
//				Debug.Log ("Success Logout");
//				UserInfo.Instance.InitUserData ();
//				return;
//			}
//			Debug.LogError("Err: Failed Logout");
//		});
//	}

	
}
