using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PieceSubject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
							IPointerClickHandler, IRecieveMessage{

	private const string PATH_SPRITE_PIECE_64 = "Koma";
	private const string PATH_SPRITE_PIECE_32 = "miniKoma";
	private const float POSX_DEF = 200f, POSY_DEF = 200;
	private const float WIDTH_PIECE_DEF = 50f;

	protected Dictionary<string, object> dicPieceInfo;
	private Sprite sprSmall, sprBig;
	private RectTransform rectTrans;
	private int myPieceID;
	public bool amIPlayer = false;
	public bool isMyTurn = false;
	public bool isMyPiece = false;
	private bool isWaitingToDestinationSelect = false;

	protected virtual Vector3 CalcDestination (){
		// TODO すべての移動先の計算
		return Vector3.zero;
	}

	protected virtual void ReqRememberSelectedPiece(){
		// TODO 待ち状態なので自分の座標をBoardClockに送る
		ExecuteEvents.Execute<IRecieveMessage>(
			target: gameObject,
			eventData: null,
			functor: (recieveTarget,y)=>recieveTarget.RecvRememberSelectedPiece(rectTrans.localPosition));
		SendMessage("RecvRememberSelectedPiece", rectTrans.localPosition);
	}

	private bool IsAccessible(){
		if(!amIPlayer){
			Debug.Log("youre not player");
			return false;
		}
		else if(!isMyTurn){
			Debug.Log("its not your turn");
			return false;
		}
		else if(!isMyPiece){
			Debug.Log("that piece is not yours");
			return false;
		}
		return true;
	}

	private void ShrinkSprite(){

//		this.GetComponent<Image> ().overrideSprite = sprSmall;
		this.GetComponent<Image> ().color = Color.magenta;
	}

	private void ExpandSprite(){

		this.GetComponent<Image> ().overrideSprite = sprBig;
		this.GetComponent<Image> ().color = Color.white;
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
	public void RecvRememberSelectedPiece(Vector3 v){}

	public void RecvMoveToDestination(Vector3 v){
		// TODO Blockから送られてきた移動先に移動
		Debug.Log ("move position to : " + v);
		rectTrans.localPosition = v;
	}
	
	public void OnPointerEnter(PointerEventData ev){
		if (!IsAccessible ()){
//			Debug.Log("UnAccessible Enter: " + dicPieceInfo["name"].ToString());
			return;
		}
		Debug.Log ("OnPointerEnter: " + dicPieceInfo["name"].ToString());
		CalcDestination ();
		ReqShowDestination ();
		ShrinkSprite ();
	}
	
	public void OnPointerExit(PointerEventData ev){
		if (!IsAccessible ()){
//			Debug.Log("UnAccessible Exit: " + dicPieceInfo["name"].ToString());
			return;
		}
		ReqUnshowDestination ();
		ExpandSprite ();
	}
	
	public void OnPointerClick(PointerEventData ev){
		if (!IsAccessible ()){
//			Debug.Log("UnAccessible Click: " + dicPieceInfo["name"].ToString());
			return;
		}
		Debug.Log ("OnPointerClick: " + dicPieceInfo["name"].ToString());
		ExpandSprite ();
		ReqRememberSelectedPiece ();
	}

	void Start(){
		if (dicPieceInfo ["owner"].ToString () == UserInfo.Instance.GetUserID ().ToString ()) {
			isMyPiece = true;
		} else {
			isMyPiece = false;
		}
	}

	void Update(){

		if (BattleInfo.Instance.infoPlay ["turn_player"].ToString ()
			== UserInfo.Instance.GetUserID ().ToString ()) {
			isMyTurn = true;
		} else {
			isMyTurn = false;
		}
	}
}
