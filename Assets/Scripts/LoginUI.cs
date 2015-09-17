using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LoginUI : MonoBehaviour {

	private LoginManager loginManager;
	private string buffPlayerName = null;
	private string buffRoomNumber = null;
	private string IPaddress = null;
	private GameObject btnLogin, btnLogout, textStateObj;
	private Text textAlert, textState;
	private string roomState;
	
	public void SetPlayerName(){
		Text name = GameObject.Find ("TxtPlayerName").GetComponent<Text>();
		buffPlayerName = name.text;
	}
	
	public void SetRoomNumber(){
		Text roomnum = GameObject.Find ("TxtRoomNumber").GetComponent<Text>();
		buffRoomNumber = roomnum.text;
	}
	
	public void SetIPAddress(){
		Text addr = GameObject.Find ("TxtIPAddr").GetComponent<Text>();
		IPaddress = addr.text;
	}

	public void LoginToServerUI(){
		if(!UserInfo.Instance.IsUserDataNull()){
			textAlert.text = "CAUTION: Already Logged in.";
			return;
		}
		else if(buffPlayerName == null || buffPlayerName == ""){
			textAlert.text = "CAUTION: Fill Name.";
			return;
		}
		else if(buffRoomNumber == null || buffRoomNumber == ""){
			textAlert.text = "CAUTION: Fill Number.";
			return;
		}
		btnLogin = GameObject.Find ("BtnLogin");
		btnLogin.SetActive(false);
		if (IPaddress == null || IPaddress == "") {
			/* URL未指定時、決め打ちアドレスにログイン */
			StartCoroutine (loginManager.LoginToServer (buffPlayerName, buffRoomNumber));
		}else{
			/* 指定URLにログイン */
			IPaddress = "http://" + IPaddress + "/";
			StartCoroutine (loginManager.LoginToServer (IPaddress, buffPlayerName, buffRoomNumber));
		}
		StartCoroutine(LoggedInToServerUI ());
	}

	private IEnumerator LoggedInToServerUI(){
		while (UserInfo.Instance.IsUserDataNull()) {
			yield return new WaitForEndOfFrame();
		}

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

		Application.LoadLevel("Room");
	}

	private void LoggedOutFromServerUI(){
		while (!UserInfo.Instance.IsUserDataNull());
		GameObject.Find ("BtnLogout").SetActive (true);
	}
	
	void Start () {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();
		textAlert = GameObject.Find ("MsgText").GetComponent<Text> ();
		textAlert.text = "Fill text above.";
		textStateObj = GameObject.Find ("TextState");
		textState = GameObject.Find ("TextState").GetComponent<Text> ();
		textState.text = "Waiting...";
		textStateObj.SetActive (false);
	}
	
	void Update () {
	}
}
