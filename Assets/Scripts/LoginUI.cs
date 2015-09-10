using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LoginUI : MonoBehaviour {

	private LoginManager loginManager;
	private string playerName = null;
	private string roomNumber = null;
	private string IPaddress;
	private GameObject btnLogin, btnLogout;
	
	public void SetPlayerName(){
		Text name = GameObject.Find ("TxtPlayerName").GetComponent<Text>();
		playerName = name.text;
		Debug.Log ("name.text: " + name.text);
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
			Debug.Log("CAUTION: Already Logged in.");
			return;
		}
		else if(playerName == null || playerName == ""){
			Debug.Log("CAUTION: Null Name.");
			return;
		}
		else if(roomNumber == null || roomNumber == ""){
			Debug.Log("CAUTION: Null RoomNumber.");
			return;
		}
		btnLogin = GameObject.Find ("BtnLogin");
		btnLogin.SetActive(false);
		Debug.Log ("false");
		StartCoroutine(loginManager.LoginToServer (playerName, roomNumber));
		StartCoroutine(LoggedInToServerUI ());
	}

	private IEnumerator LoggedInToServerUI(){
		Debug.Log ("logged in");
		while (UserInfo.Instance.IsUserDataNull()) {
			yield return new WaitForEndOfFrame();
		}
		btnLogin.SetActive (true);
	}

	public void LogoutFromServerUI(){
		if(UserInfo.Instance.IsUserDataNull()){
			Debug.Log("CAUTION: Already Logged out.");
			return;
		}
		GameObject.Find ("BtnLogout").SetActive (false);
		LoginManager loginManager = this.GetComponent<LoginManager> ();
		loginManager.LogoutFromServer (UserInfo.Instance.GetPlayID().ToString(), UserInfo.Instance.GetUserID().ToString());
		LoggedOutFromServerUI ();
	}

	private void LoggedOutFromServerUI(){
		while (!UserInfo.Instance.IsUserDataNull());
		GameObject.Find ("BtnLogout").SetActive (true);
	}


	// Use this for initialization
	void Start () {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
