using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour {

	private Text textPlayerUserID, textPlayerPlayID, textPlayerRole;
	private Text textAudienceInfoText;
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



	void Start () {
		textPlayerUserID = GameObject.Find ("TextPlayerUserID").GetComponent<Text> ();
		textPlayerPlayID = GameObject.Find ("TextPlayerPlayID").GetComponent<Text> ();
		textPlayerRole = GameObject.Find ("TextPlayerRole").GetComponent<Text> ();

		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();

		textPlayerUserID.text = "YourID: " + UserInfo.Instance.GetUserID().ToString();
		textPlayerPlayID.text = "PlayID: " + UserInfo.Instance.GetPlayID().ToString();
		textPlayerRole.text = "YourRole: " + UserInfo.Instance.GetUserRole();

		textAudienceInfoText = GameObject.Find ("AudienceInfoText").GetComponent<Text> ();

		// TODO まだShogiHTTPには完全なURLを送っている
		Dictionary<string, object> dicPlayerInfo = ShogiHTTP.Instance.Player (UserInfo.Instance.urlLogging);
		foreach(object opponentinfo in dicPlayerInfo.Values){
			Debug.Log("opponentinfo.ToString(): " + opponentinfo.ToString());
//			textAudienceInfoText.text = opponentinfo.ToString();
		}
		 
	}

	void Update () {
	}
}
