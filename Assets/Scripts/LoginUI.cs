using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LoginUI : MonoBehaviour {

	private LoginManager loginManager;
	private string playerName = null;
	private string roomNumber = null;
	private string IPaddress = null;
	private GameObject btnLogin, btnLogout;
	private Text textAlert;
	
	public void SetPlayerName(){
		Text name = GameObject.Find ("TxtPlayerName").GetComponent<Text>();
		playerName = name.text;
	}
	
	public void SetRoomNumber(){
		Text roomnum = GameObject.Find ("TxtRoomNumber").GetComponent<Text>();
		roomNumber = roomnum.text;
	}
	
	public void SetIPAddress(){
		Text addr = GameObject.Find ("TxtIPAddr").GetComponent<Text>();
		IPaddress = addr.text;
	}

	public void LoginToServerUI(){
		Debug.Log ("called");
		if(!UserInfo.Instance.IsUserDataNull()){
//			Debug.Log("CAUTION: Already Logged in.");
			textAlert.text = "CAUTION: Already Logged in.";
			return;
		}
		else if(playerName == null || playerName == ""){
//			Debug.Log("CAUTION: Null Name.");
			textAlert.text = "CAUTION: Fill Name.";
			return;
		}
		else if(roomNumber == null || roomNumber == ""){
//			Debug.Log("CAUTION: Null RoomNumber.");
			textAlert.text = "CAUTION: Fill Number.";
			return;
		}
		btnLogin = GameObject.Find ("BtnLogin");
		btnLogin.SetActive(false);
//		Debug.Log ("IPaddress; " + IPaddress);
		if (IPaddress == null || IPaddress == "") {
			StartCoroutine (loginManager.LoginToServer (playerName, roomNumber));
		}else{
			IPaddress = "http://" + IPaddress + "/";
			StartCoroutine (loginManager.LoginToServer (IPaddress, playerName, roomNumber));
		}
		StartCoroutine(LoggedInToServerUI ());
	}

	private IEnumerator LoggedInToServerUI(){
//		Debug.Log ("logged in");
		while (UserInfo.Instance.IsUserDataNull()) {
			yield return new WaitForEndOfFrame();
		}
		btnLogin.SetActive (true);

		Application.LoadLevel("Room");
	}

//	public void LogoutFromServerUI(){
//		if(UserInfo.Instance.IsUserDataNull()){
//			Debug.Log("CAUTION: Already Logged out.");
//			return;
//		}
//		GameObject.Find ("BtnLogout").SetActive (false);
//		StartCoroutine(loginManager.LogoutFromServer (
//			UserInfo.Instance.GetPlayID().ToString(), UserInfo.Instance.GetUserID().ToString()));
//		LoggedOutFromServerUI ();
//	}

	private void LoggedOutFromServerUI(){
		while (!UserInfo.Instance.IsUserDataNull());
		GameObject.Find ("BtnLogout").SetActive (true);
	}


	// Use this for initialization
	void Start () {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();
		textAlert = GameObject.Find ("MsgText").GetComponent<Text> ();
		textAlert.text = "Fill text above.";
	}
	
	// Update is called once per frame
	void Update () {

	}
}
