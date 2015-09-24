using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	private GameObject objPieceAllocator;
	private GameObject objBattleUI;

	private float perSec = 1.0f;
	private float timer;

	public void LogoutOnBattle(){
		if (UserInfo.Instance.IsUserDataNull ()) {
			return;
		}
		
		ShogiHTTP.Instance.Logout ((string www) => {
			if(www == "[\"true\"]"){
				Debug.Log ("Success Logout");
				UserInfo.Instance.InitUserData ();
				Application.LoadLevel("Lobby");
			}
		});
	}

	public void SetPlayerInfo(Dictionary<string, object> dicPlayerInfo){
		foreach(KeyValuePair<string, object> pair in dicPlayerInfo){
			Dictionary<string, object> dic = pair.Value as Dictionary<string, object>;
			if(pair.Key.ToString() == "first_player"){
				BattleInfo.Instance.SetFirstPlayerInfo(dic);
				Debug.Log( BattleInfo.Instance.infoFirstPlayer ["user_id"].ToString () );
			}
			else{
				BattleInfo.Instance.SetLastPlayerInfo(dic);
				Debug.Log ("seting battle info: last");
			}
		}
	}

	// TODO 関数作れ：foreachで回してPieceAllocatorに40個全部投げる関数
//	public void SetPieceInfo(Dictionary<string, object> dicPiece){
//		foreach(KeyValuePair<string, object> pair in dicPiece){
//			Dictionary<string, object> dic = pair.Value as Dictionary<string, object>;
//			// 一気に
//		}
//	}

	private void AdjustAnglesOfUI(){
		if (BattleInfo.Instance.infoFirstPlayer ["user_id"].ToString ()
		    == UserInfo.Instance.GetUserID ().ToString ()) {
			Debug.Log("先手");
		} else {
			RectTransform rectTrans = GameObject.Find("Board").GetComponent<RectTransform>();
			rectTrans.rotation = Quaternion.Euler (0, 0, 180);
			Debug.Log("後手");
		}
	}

	private void FetchBattlePlayerInfo(){
		ShogiHTTP.Instance.Player (UserInfo.Instance.urlLogging,
		                           (Dictionary<string, object> dicPlayerInfo) => {
			SetPlayerInfo(dicPlayerInfo);
			AdjustAnglesOfUI();

			CreateScriptComponent.Create("PieceAllocator");
			CreateScriptComponent.Create("BattleUI");
//			CreateScriptComponent.Create("BoardUI");

			// TODO 常に勝者を確認する
		});
	}

	private void UpdatePlayInfo(){
		ShogiHTTP.Instance.Play (UserInfo.Instance.urlLogging,
		                           (Dictionary<string, object> dicPlayerInfo) => {
			BattleInfo.Instance.SetPlayInfo(dicPlayerInfo);
			Debug.Log ("update play info");
		});
	}

	private void resetTimer(){
		timer = perSec;
	}

	private void DoOnEverySecond(){
		timer -= Time.deltaTime;
		if (timer <= 0.0f) {
			UpdatePlayInfo ();
			// ほか、毎秒行いたい処理をここにどうぞ
			resetTimer();
		}
	}

	void Awake(){
		FetchBattlePlayerInfo ();
		UpdatePlayInfo ();
	}

	void Start(){
		resetTimer ();
	}

	void Update(){
		if(BattleInfo.Instance.IsPlayInfoNull()){
			return;
		}
		if(BattleInfo.Instance.infoPlay["turn_player"].ToString()
		   != UserInfo.Instance.GetUserID().ToString()){
			DoOnEverySecond ();
		}
	}
}
