using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour {

	private Text textPlayerName, textOpponentName;
	private Text textAudienceInfoText; // for Debug
	private BattleManager battleManager;

	public void LogoutFromServerUI(){
		GameObject.Find ("BtnLogout").SetActive (false);
		string playID = UserInfo.Instance.GetPlayID ().ToString ();
		string userID = UserInfo.Instance.GetUserID ().ToString ();
		StartCoroutine(battleManager.LogoutFromServer (playID, userID));
		//TODO ここ、処理完了を待ってから次の処理したい、コルーチンでなんとかして
		StartCoroutine(LoggedOutFromServerUI());
	}

	private IEnumerator LoggedOutFromServerUI(){
		while (!UserInfo.Instance.IsUserDataNull()) {
			yield return new WaitForEndOfFrame();
		}
		Application.LoadLevel ("Lobby");
	}

	private void InitPlayerPanel(){
		ShogiHTTP.Instance.Player (UserInfo.Instance.urlLogging,
		                           (Dictionary<string, object> dicPlayerInfo) => {
			battleManager.SetPlayerInfo(dicPlayerInfo);

			// 対戦者の名前と対戦モードかどうかを表示
			if(UserInfo.Instance.GetUserRole().ToString() == "player"){
				textAudienceInfoText.text = "対戦モードです";
				if(BattleInfo.Instance.infoFirstPlayer["user_id"].ToString()
				   == UserInfo.Instance.GetUserID().ToString()){
					textPlayerName.text = BattleInfo.Instance.infoFirstPlayer["name"].ToString();
				}
				else{
					textOpponentName.text = BattleInfo.Instance.infoLastPlayer["name"].ToString();
				}
			}
			else{
				textAudienceInfoText.text = "観戦モードです";
			}
		});
	}

	void Awake() {
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();

		textAudienceInfoText = GameObject.Find ("AudienceInfoText").GetComponent<Text> ();

		textPlayerName = GameObject.Find ("TextPlayerName").GetComponent<Text> ();
		textOpponentName = GameObject.Find ("TextOpponentName").GetComponent<Text> ();

		InitPlayerPanel();

//		Debug.Log ("Your Role :" + UserInfo.Instance.GetUserRole().ToString() );
	}
}
