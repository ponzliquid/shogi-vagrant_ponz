using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PieceAllocator : SingletonMonoBehaviour<PieceAllocator> {

	public PieceSubject prfbPiece;

	private float perSec = 1.0f;
	private float timer;

	private void InstantiatePieces(){
		ShogiHTTP.Instance.Pieces(UserInfo.Instance.urlLogging, 
		                          (Dictionary<string, object> dicPieces) => {
			foreach(KeyValuePair<string, object> pair in dicPieces){
				Dictionary<string, object> dicChild = pair.Value as Dictionary<string, object>;
				// 取得情報に基づき駒を生成
//				PieceSubject.Instantiate(prfbPiece, int.Parse(pair.Key), dicChild);
				int pieceID = int.Parse(pair.Key.ToString());
//				InstantiatePieceObject(pieceID, dicChild);.
				var obj = Instantiate (prfbPiece);
				PieceSubject subj = obj.GetComponent<PieceSubject>();
				subj.SetPieceID(pieceID);
				subj.SetPieceInfo(dicChild);
				subj.Init();
			}
		});
	}

	private void ReqUpdatePieces(){
		ShogiHTTP.Instance.Pieces(UserInfo.Instance.urlLogging, 
		                          (Dictionary<string, object> dicPieces) => {
			foreach(KeyValuePair<string, object> pair in dicPieces){
				Dictionary<string, object> dicChild = pair.Value as Dictionary<string, object>;
				int pieceID = int.Parse(pair.Key.ToString());
				Debug.Log("update pieces");
				GameObject obj = GameObject.Find(dicChild["name"].ToString());
				ExecuteEvents.Execute<IRecieveMessage>(
					target: obj,
					eventData: null,
					functor: (x,y)=>x.RecvUpdatePiecePos(pieceID, dicChild));
			}
		});
		// piece.jsonを取得
		// 全コマ情報を分解
		// 各駒に、EventSendMsgでIDと移動先を送信
	}


	private void resetTimer(){
		timer = perSec;
	}

	private void DoOnEverySecond(){
		timer -= Time.deltaTime;
		if (timer <= 0.0f) {
			ReqUpdatePieces ();
			// ほか、毎秒行いたい処理をここに
			resetTimer();
		}
	}

	void Start(){
		prfbPiece = Resources.Load<PieceSubject>("Prefabs/Piece");
		InstantiatePieces ();
		resetTimer ();
	}

	void Update(){
		if(BattleInfo.Instance.IsPlayInfoNull()){
//			new WaitForEndOfFrame();
			return;
		}
		if(BattleInfo.Instance.infoPlay["turn_player"].ToString()
		   != UserInfo.Instance.GetUserID().ToString()){
			Debug.Log("opponent turn");
			DoOnEverySecond ();
		}
		Debug.Log ("my turn");
	}
}
