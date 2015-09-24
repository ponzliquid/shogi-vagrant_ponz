using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PieceSubject : MonoBehaviour, IRecieveMessage{

	private const string PATH_SPRITE_PIECE_64 = "Koma";
	private const float POSX_DEF = 200f, POSY_DEF = 200;
	private const float WIDTH_PIECE_DEF = 50f;
	private Dictionary<string, object> dicPieceInfo;
	private RectTransform rectTrans;
	private int myPieceID;
	private int posXOriginal, posYOriginal;
	private bool amIPlayer = false;
	private bool isMyTurn = false;
	private bool isMyPiece = false;
	private bool isAllowedToSelect = false;
	private bool isWaitingToDestinationSelect = false;

	// execute on instantiating
//	public static PieceSubject Instantiate(PieceSubject prefab, int pieceID, Dictionary<string, object> dic){
//		PieceSubject obj = Instantiate(prefab) as PieceSubject;
//
//		this.myPieceID = pieceID;
//		this.posXOriginal = int.Parse(dic["posx"].ToString());
//		this.posYOriginal = int.Parse(dic["posy"].ToString());
//
//		Transform canvasObj = GameObject.Find("Canvas").GetComponent<Transform>();
//		obj.transform.SetParent (canvasObj,false);
//
//		Sprite spr = Resources.Load<Sprite>(PATH_SPRITE_PIECE_64 + "/" + dic["name"].ToString());
//		obj.GetComponent<Image> ().overrideSprite = spr;
//
//		obj.name = dic["name"].ToString();
//
//		RectTransform rectTransOnInstance = obj.GetComponent<RectTransform> ();
//		float buffX = POSX_DEF - WIDTH_PIECE_DEF * (float.Parse(dic ["posx"].ToString()) - 1f);
//		float buffY = POSX_DEF - WIDTH_PIECE_DEF * (float.Parse(dic ["posy"].ToString()) - 1f);
//		rectTransOnInstance.localPosition = new Vector3(buffX, buffY,0);
//
//		if (dic ["owner"].ToString () == BattleInfo.Instance.infoLastPlayer["user_id"].ToString()) {
//			rectTransOnInstance.rotation = Quaternion.Euler (0, 0, 180);
//		}
//		return obj;
//	}

	public void SetPieceID(int id){
		myPieceID = id;
	}
	
	public void SetPieceInfo(Dictionary<string, object> dic){
		dicPieceInfo = dic;	
	}

	private Vector3 ConvertOriginalPosToLocalPos(Dictionary<string, object> dic){
		float buffX = POSX_DEF - WIDTH_PIECE_DEF * (float.Parse(dic ["posx"].ToString()) - 1f);
		float buffY = POSX_DEF - WIDTH_PIECE_DEF * (float.Parse(dic ["posy"].ToString()) - 1f);
		Vector3 vec3 = new Vector3(buffX, buffY,0);
		return vec3;
	}

	public void UpdatePiecePosition(int sentID, Dictionary<string, object> dic){
		// IDが自分のものか
		// dicから座標をとってくる
		// それをConvertOriginalPosToLocalPos()へ
		if(sentID != myPieceID){
			Debug.Log("not me");
			return;
		}
		rectTrans.localPosition = ConvertOriginalPosToLocalPos (dic);
		Debug.Log ("update position");
	}

	public void Init(){
		Transform canvasObj = GameObject.Find("Canvas").GetComponent<Transform>();
		this.transform.SetParent (canvasObj,false);

		Sprite spr = Resources.Load<Sprite>(PATH_SPRITE_PIECE_64 + "/" + dicPieceInfo["name"].ToString());
		this.GetComponent<Image> ().overrideSprite = spr;

		this.name = dicPieceInfo["name"].ToString();
		rectTrans = this.GetComponent<RectTransform> ();
		rectTrans.localPosition = ConvertOriginalPosToLocalPos (dicPieceInfo);

		if (dicPieceInfo ["owner"].ToString () == BattleInfo.Instance.infoLastPlayer["user_id"].ToString()) {
			rectTrans.rotation = Quaternion.Euler (0, 0, 180);
		}
	}

	void Awake(){
		if (UserInfo.Instance.GetUserRole ().ToString () == "player") {
			amIPlayer = true;
		}
		//ほか、駒のメタ情報を登録
	}

	void Start(){
//		rectTrans = this.GetComponent<RectTransform> ();
		Debug.Log ("recttrans");
//		Init ();
	}
}
