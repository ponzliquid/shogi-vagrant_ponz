using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PieceAllocator : MonoBehaviour {

	public PieceSubject prfbPiece;

	void FetchPieceLocation(){
		ShogiHTTP.Instance.Pieces(UserInfo.Instance.urlLogging, 
		                          (Dictionary<string, object> dicPieces) => {
			foreach(KeyValuePair<string, object> pair in dicPieces){
				Dictionary<string, object> dic = pair.Value as Dictionary<string, object>;
				PieceSubject.Instantiate(prfbPiece, dic);
			}
		});
	}

	void Awake(){
		FetchPieceLocation ();
	}
}
