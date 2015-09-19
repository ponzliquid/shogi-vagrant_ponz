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
			if(dic == null){
				loginUI.OutputMsg("Failed to Login " + loginURL +".");
				loginUI.SetLoginButton (true);
				Debug.LogError ("Err: Failed to Login" + loginURL);
				return;
			}
			UserInfo.Instance.SetUserData (dic);
			UserInfo.Instance.SetLoggingURL (loggingURL);
			FetchRoomState();
			StartCoroutine(loginUI.WaitForOpponent());

			loginUI.OutputMsg("Successful logging in to " + loginURL +".");
			loginUI.SetLogoutButton(true);
			return;
		});

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
			if(dic == null){
				loginUI.OutputMsg("Failed to Login " + loginURL +".");
				loginUI.SetLoginButton (true);
				Debug.LogError ("Err: Failed to Login" + loginURL);
				return;
			}
			UserInfo.Instance.SetUserData (dic);
			UserInfo.Instance.SetLoggingURL (loggingURL);
			FetchRoomState();
			StartCoroutine(loginUI.WaitForOpponent());

			loginUI.OutputMsg("Successful logging in to " + loginURL +".");
			loginUI.SetLogoutButton(true);
			return;
		});
	}

	public void InitRoomState(){
	
	}

	public void FetchRoomState(){
		ShogiHTTP.Instance.State ((Dictionary<string, object> dic) => {
			UserInfo.Instance.SetState (dic);

		});

//		if (download ["state"] == "waiting") {
//			UserInfo.Instance.SetState (download);
//			yield return StartCoroutine(GetRoomState());
//		} else {
//			UserInfo.Instance.SetState (download);
//			yield break;
//		}
	}

	public void LogoutOnLobby(){

		if (UserInfo.Instance.IsUserDataNull ()) {
			return;
		}

		LoginUI loginUI = GameObject.Find("LoginUI").GetComponent<LoginUI>();

		ShogiHTTP.Instance.Logout ((string www) => {
			if(www == "[\"true\"]"){

				UserInfo.Instance.InitUserData ();
				loginUI.OutputMsg("Logged out from server.");
				loginUI.SetLoginButton(true);

				return;
			}
		});
	}
}
