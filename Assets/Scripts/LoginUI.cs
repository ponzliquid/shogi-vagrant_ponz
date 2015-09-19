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
	
	private Text textMsg, textState;
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

	public void OutputMsg(string msg){
		textMsg.text = msg;
	}

	public void SetLoginButton(bool sw){
		GameObject btnLogin = GameObject.Find ("BtnLogin");
		btnLogin.SetActive (sw);
	}

	public void LoginToServerUI(){
//		if(!UserInfo.Instance.IsUserDataNull()){
//			textMsg.text = "CAUTION: Already Logged in.";
//			return;
//		}
		if(buffPlayerName == null || buffPlayerName == ""){
			textMsg.text = "CAUTION: Fill Name.";
			return;
		}
		else if(buffRoomNumber == null || buffRoomNumber == ""){
			textMsg.text = "CAUTION: Fill Number.";
			return;
		}

		SetLoginButton (false);
		if (IPaddress == null || IPaddress == "") {
			// URL未指定時、決め打ちアドレスにログイン
			loginManager.LoginToServer (buffPlayerName, buffRoomNumber);
		}else{
			// IPアドレス入力があれば、そのアドレスにログイン
			loginManager.LoginToServer (IPaddress, buffPlayerName, buffRoomNumber);
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
				textState.text = "waiting opponent...";
				Debug.Log("state waiting");
				continue;
			}
			else{
				break;
			}
		}

		Application.LoadLevel("Room");
	}
	
	void Start () {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();

		textMsg = GameObject.Find ("MsgText").GetComponent<Text> ();
		textMsg.text = "Fill text above.";

		textState = GameObject.Find ("StateText").GetComponent<Text> ();
	}
	
	void Update () {
	}
}
