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
//			while( BattleInfo.Instance.IsPlayerInfoNull() ){
//				yield return new WaitForEndOfFrame();
//			}
			Debug.Log("referring battle info");
			textPlayerName.text = BattleInfo.Instance.infoFirstPlayer["name"].ToString();
			textOpponentName.text = BattleInfo.Instance.infoLastPlayer["name"].ToString();
		});
	}

	void Start () {
		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();

		textAudienceInfoText = GameObject.Find ("AudienceInfoText").GetComponent<Text> ();

		textPlayerName = GameObject.Find ("TextPlayerName").GetComponent<Text> ();
		textOpponentName = GameObject.Find ("TextOpponentName").GetComponent<Text> ();

		InitPlayerPanel();

		Debug.Log ("Your Role :" + UserInfo.Instance.GetUserRole().ToString() );

		if (UserInfo.Instance.GetUserRole().ToString() == "player") {
			textAudienceInfoText.text = "対戦モードです";
		}
		else if(UserInfo.Instance.GetUserRole().ToString() == "watcher"){
			textAudienceInfoText.text = "観戦モードです";
		}
	}
}
