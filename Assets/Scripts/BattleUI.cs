﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour {

	private Text textFirstPlayerName, textLastPlayerName;
	private Text textAudienceInfoText; // for Debug
	private BattleManager battleManager;

	public void LogoutFromServerUI(){
		GameObject.Find ("BtnLogout").SetActive (false);
//		string playID = UserInfo.Instance.GetPlayID ().ToString ();
//		string userID = UserInfo.Instance.GetUserID ().ToString ();
		battleManager.LogoutOnBattle ();
	}

	private IEnumerator LoggedOutFromServerUI(){
		while (!UserInfo.Instance.IsUserDataNull()) {
			yield return new WaitForEndOfFrame();
		}
		Application.LoadLevel ("Lobby");
	}

	private void InitPlayerPanel(){

		// 対戦者の名前と対戦モードかどうかを表示
		if(UserInfo.Instance.GetUserRole().ToString() == "player"){
			textAudienceInfoText.text = "対戦モードです";
			if(BattleInfo.Instance.infoFirstPlayer["user_id"].ToString()
			   == UserInfo.Instance.GetUserID().ToString()){
				textFirstPlayerName.text = BattleInfo.Instance.infoFirstPlayer["name"].ToString();
				textLastPlayerName.text = BattleInfo.Instance.infoLastPlayer["name"].ToString();
			}
			else{
				textLastPlayerName.text = BattleInfo.Instance.infoFirstPlayer["name"].ToString();
				textFirstPlayerName.text = BattleInfo.Instance.infoLastPlayer["name"].ToString();
			}
		}
		else{
			textAudienceInfoText.text = "観戦モードです";
		}
	}

	void Start () {
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();

		textAudienceInfoText = GameObject.Find ("AudienceInfoText").GetComponent<Text> ();

		textFirstPlayerName = GameObject.Find ("TextFirstPlayerName").GetComponent<Text> ();
		textLastPlayerName = GameObject.Find ("TextLastPlayerName").GetComponent<Text> ();

		InitPlayerPanel();

//		Debug.Log ("Your Role :" + UserInfo.Instance.GetUserRole().ToString() );
	}
}
