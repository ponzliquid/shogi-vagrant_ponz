using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PieceAllocator : MonoBehaviour {

	public PieceSubject prfbPiece;
	private float perSec = 1.0f;
	private float timer;

	private void SetPieceLocation(){
		ShogiHTTP.Instance.Pieces(UserInfo.Instance.urlLogging, 
		                          (Dictionary<string, object> dicPieces) => {
			foreach(KeyValuePair<string, object> pair in dicPieces){
				Dictionary<string, object> dicChild = pair.Value as Dictionary<string, object>;
				// 取得情報に基づき駒を生成
				PieceSubject.Instantiate(prfbPiece, int.Parse(pair.Key), dicChild);
			}
		});
	}

	private void FetchPieceLocation(){
		ShogiHTTP.Instance.Pieces(UserInfo.Instance.urlLogging, 
		                          (Dictionary<string, object> dicPieces) => {
			foreach(KeyValuePair<string, object> pair in dicPieces){
				Dictionary<string, object> dicChild = pair.Value as Dictionary<string, object>;
				int pieceID = int.Parse(pair.Key.ToString());
				ExecuteEvents.Execute<IRecieveMessage>(
					target: gameObject,
					eventData: null,
					functor: (recieveTarget,y)=>recieveTarget.UpdatePiecePosition(pieceID, dicChild));
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
			FetchPieceLocation ();
		}
	}

	void Start(){
		prfbPiece = Resources.Load<PieceSubject>("Prefabs/Piece");
		SetPieceLocation ();
		resetTimer ();
	}

	void Update(){

		if(false/*相手のターンなら*/){
			DoOnEverySecond ();
		}
	}
}
