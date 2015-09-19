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

	private GameObject btnLogin,btnLogout;
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
		btnLogin.SetActive (sw);
	}

	public void SetLogoutButton(bool sw){
		btnLogout.SetActive (sw);
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
	}

	public void LogoutFromServerUI(){
		Debug.Log("interrupt: logout");
		SetLogoutButton(false);
		loginManager.LogoutOnLobby ();
	}

	public IEnumerator WaitForOpponent(){
		Debug.Log ("wait for opponent");
		while (UserInfo.Instance.IsStateDataNULL()){
			yield return new WaitForEndOfFrame();
		}

		while(true){
			if(UserInfo.Instance.IsUserDataNull()){
				Debug.Log("canceled matching");
				textState.text = "";
				yield break;
			}
			loginManager.FetchRoomState();
			roomState = UserInfo.Instance.GetState().ToString();

			if(roomState == "exit"){
				Debug.Log("exit");
				yield break;
			}
			else if(roomState == "" || roomState == null){
				Debug.Log("state waiting but userinfo is null");
				yield return new WaitForSeconds(1);
				continue;
			}
			else if(roomState == "waiting"){
				textState.text = "waiting opponent...";
				Debug.Log("state waiting");
				yield return new WaitForSeconds(1);
				continue;
			}
			Debug.Log("move to battle scene");
			break;
		}

		Application.LoadLevel("Room");
	}
	
	void Start () {
		loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();

		btnLogin = GameObject.Find ("BtnLogin");
		btnLogout = GameObject.Find ("BtnLogout");
		SetLogoutButton (false);

		textMsg = GameObject.Find ("MsgText").GetComponent<Text> ();
		textMsg.text = "Fill text above.";

		textState = GameObject.Find ("StateText").GetComponent<Text> ();
	}
}
