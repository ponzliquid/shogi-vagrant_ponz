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
	private GameObject btnLogin, btnLogout, textStateObj;
	private Text textAlert, textState;
	private string roomState;
	
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
//		btnLogin.SetActive (true);
		Debug.Log ("ready to get state");

		while(true){
			roomState = UserInfo.Instance.GetState ();
			if(roomState == "" || roomState == null){
				yield return StartCoroutine (loginManager.GetRoomState());
				Debug.Log("state waiting but null");
				continue;
			}
			else if(roomState == "waiting"){
				yield return StartCoroutine (loginManager.GetRoomState());
				Debug.Log("state waiting");
				continue;
			}
			else{
				break;
			}
		}

//		roomState = UserInfo.Instance.GetState ();
//		StartCoroutine (loginManager.GetRoomState());
//		while(roomState == "" || roomState == null){
//			Debug.Log("state waiting but null");
//			yield return null;
//		}
//		StopCoroutine (loginManager.GetRoomState());
//		while(roomState == "waiting"){
//			Debug.Log("state waiting");
//			textStateObj.SetActive (true);
//			yield return new WaitForSeconds(1);
//			textStateObj.SetActive (false);
//		}
		Application.LoadLevel("Room");
	}

	private void LoggedOutFromServerUI(){
		while (!UserInfo.Instance.IsUserDataNull());
		GameObject.Find ("BtnLogout").SetActive (true);
	}


	// Use this for initialization
	void Start () {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();
		textAlert = GameObject.Find ("MsgText").GetComponent<Text> ();
		textAlert.text = "Fill text above.";
		textStateObj = GameObject.Find ("TextState");
		textState = GameObject.Find ("TextState").GetComponent<Text> ();
		textState.text = "Waiting...";
		textStateObj.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
