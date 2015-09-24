using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PieceSubject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
							IPointerClickHandler, IRecieveMessage{

	private const string PATH_SPRITE_PIECE_64 = "Koma";
	private const string PATH_SPRITE_PIECE_32 = "30x32";
	private const float POSX_DEF = 200f, POSY_DEF = 200;
	private const float WIDTH_PIECE_DEF = 50f;

	protected Dictionary<string, object> dicPieceInfo;
	private Sprite sprSmall, sprBig;
	private RectTransform rectTrans;
	private int myPieceID;
	private bool amIPlayer = false;
	private bool isMyTurn = false;
	private bool isMyPiece = false;
	private bool isWaitingToDestinationSelect = false;

	protected virtual Vector3 CalcDestination (){
		// TODO すべての移動先の計算
		return Vector3.zero;
	}

	protected virtual void ReqRememberSelectedPiece(){
		// TODO 待ち状態なので自分の座標をBoardClockに送る
	}

	private bool IsAccessible(){
		if(amIPlayer && isMyTurn && isMyPiece){
			return true;
		}
		return false;
	}

	private void ShrinkSprite(){

		this.GetComponent<Image> ().overrideSprite = sprSmall;
	}

	private void ExpandSprite(){

		this.GetComponent<Image> ().overrideSprite = sprBig;
	}

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

	public void Init(){
		if (UserInfo.Instance.GetUserRole ().ToString () == "player") {
			amIPlayer = true;
		}

		Transform canvasObj = GameObject.Find("Canvas").GetComponent<Transform>();
		this.transform.SetParent (canvasObj,false);

		sprSmall = Resources.Load<Sprite>(PATH_SPRITE_PIECE_32 + "/" + dicPieceInfo["name"].ToString());
		sprBig = Resources.Load<Sprite>(PATH_SPRITE_PIECE_64 + "/" + dicPieceInfo["name"].ToString());
		ExpandSprite ();

		this.name = dicPieceInfo["name"].ToString();
		rectTrans = this.GetComponent<RectTransform> ();
		rectTrans.localPosition = ConvertOriginalPosToLocalPos (dicPieceInfo);

		if (dicPieceInfo ["owner"].ToString () == BattleInfo.Instance.infoLastPlayer["user_id"].ToString()) {
			rectTrans.rotation = Quaternion.Euler (0, 0, 180);
		}
	}

	// following: EventTriger -----------------------------------------------------------
	protected virtual void ReqShowDestination(){
		// TODO 計算した移動先すべてを全BoardBlockにインヴォケーション
	}

	protected virtual void ReqUnshowDestination(){
		// TODO ReqShowDestination()の取り消し
	}

	// following: EventListener -----------------------------------------------------------

	public void RecvUpdatePiecePos(int sentID, Dictionary<string, object> dic){
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

	public void RecvShowDestination(Vector3 vec3){}
	public void RecvUnshowDestination(){}

	public void RecvMoveToDestination(){
		// TODO Blockから送られてきた移動先に移動
	}
	
	public void OnPointerEnter(PointerEventData ev){
		if (!IsAccessible ()){
			return;
		}
		CalcDestination ();
		ReqShowDestination ();
		ShrinkSprite ();
	}
	
	public void OnPointerExit(PointerEventData ev){
		if (!IsAccessible ()){
			return;
		}
		ReqUnshowDestination ();
		ExpandSprite ();
	}
	
	public void OnPointerClick(PointerEventData ev){
		if (!IsAccessible ()){
			return;
		}
		ExpandSprite ();
		ReqRememberSelectedPiece ();
	}

	void Update(){
		if (dicPieceInfo ["owner"].ToString () == UserInfo.Instance.GetUserID ().ToString ()) {
			isMyPiece = true;
		} else {
			isMyPiece = false;
		}

		if (BattleInfo.Instance.infoPlay ["turn_player"].ToString ()
			== UserInfo.Instance.GetUserID ().ToString ()) {
			isMyTurn = true;
		} else {
			isMyTurn = false;
		}
	}
}
