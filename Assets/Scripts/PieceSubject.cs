using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PieceSubject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	private const string PATH_SPRITE_PIECE_64 = "Koma";
	private const float POSX_DEF = 200f, POSY_DEF = 200;
	private const float WIDTH_PIECE_DEF = 50f;
	private bool amIPlayer = false;
	private bool isMyTurn = false;
	private bool isMyPiece = false;

	// execute on instantiating
	public static PieceSubject Instantiate(PieceSubject prefab, Dictionary<string, object> dic){
		PieceSubject obj = Instantiate(prefab) as PieceSubject;

		Transform canvasObj = GameObject.Find("Canvas").GetComponent<Transform>();
		obj.transform.SetParent (canvasObj,false);

		Sprite spr = Resources.Load<Sprite>(PATH_SPRITE_PIECE_64 + "/" + dic["name"].ToString());
		obj.GetComponent<Image> ().overrideSprite = spr;

		obj.name = dic["name"].ToString();

		RectTransform rectTrans = obj.GetComponent<RectTransform> ();
		float buffX = POSX_DEF - WIDTH_PIECE_DEF * (float.Parse(dic ["posx"].ToString()) - 1f);
		float buffY = POSX_DEF - WIDTH_PIECE_DEF * (float.Parse(dic ["posy"].ToString()) - 1f);
		rectTrans.localPosition = new Vector3(buffX, buffY,0);

		if (dic ["owner"].ToString () == BattleInfo.Instance.infoLastPlayer["user_id"].ToString()) {
//			Debug.Log ("P(name,x,y,own) =(" + dic ["name"].ToString () + ", " + rectTrans.localPosition.x.ToString () + ", "
//				+ rectTrans.localPosition.y + ", "
//				+ dic ["owner"].ToString () + ")" + "ひっくり返して");

			rectTrans.rotation = Quaternion.Euler (0, 0, 180);
		}

		return obj;
	}

	
	private void ReloadPieceInfo(){
		ShogiHTTP.Instance.Pieces (UserInfo.Instance.urlLogging,
		                           (Dictionary<string, object> dicPieces) => {
			//
		});
	}

//	void OnMouseDrag(){
//		Debug.Log ("OnMouseDrag");
//		Vector3 objectPointInScreen
//			= Camera.main.WorldToScreenPoint (this.transform.position);
//		Vector3 mousePointInScreen
//			= new Vector3 (Input.mousePosition.x, Input.mousePosition.y, objectPointInScreen.z);
//		Vector3 mousePointInWorld
//			= Camera.main.ScreenToWorldPoint (mousePointInScreen);
//		mousePointInWorld.z = this.transform.position.z;
//		this.transform.position = mousePointInWorld;
//	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData e){
		RectTransform rectTrans = this.GetComponent<RectTransform>();
		rectTrans.SetAsFirstSibling();
	}

	void IDragHandler.OnDrag(PointerEventData ped){
		ped.position = new Vector2 (ped.position.x - POSX_DEF, ped.position.y - POSY_DEF);
		Debug.Log("ped position="+ped.position);
		RectTransform rectTrans = this.GetComponent<RectTransform>();
		rectTrans.localPosition = new Vector3(ped.position.x, ped.position.y,0);
		Debug.Log("rect position="+rectTrans.localPosition);
	}

	void IEndDragHandler.OnEndDrag(PointerEventData e){
		RectTransform rectTrans = this.GetComponent<RectTransform>();
		rectTrans.SetAsLastSibling();
	}

	void Awake(){
		if (UserInfo.Instance.GetUserRole ().ToString () == "player") {
			amIPlayer = true;
			// 関数飛ばす
		}
	}
}
