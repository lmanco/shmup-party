using UnityEngine;
using System.Collections;

public class PlayersConnectedBox : MonoBehaviour {

	public MenuItem box;
	private int playersConnected;
	private string initBoxText;
	private static PlayersConnectedBox self;

	// Use this for initialization
	void Start () {
		self = this;
		playersConnected = 0;
		initBoxText = box.text;
		box.text = initBoxText + " " + playersConnected + "/" + GameValues.MAX_PLAYERS;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static PlayersConnectedBox GetInstance() {
		return self;
	}
	
	public void AddPlayersConnected(int players) {
		networkView.RPC("UpdateBox", RPCMode.AllBuffered, players);
	}
	
	[RPC]
	void UpdateBox(int players) {
		playersConnected = playersConnected + players;
		box.text = initBoxText + " " + playersConnected + "/" + GameValues.MAX_PLAYERS;
	}
	
	public void SetPlayersConnected(int playersConnected){
		this.playersConnected = playersConnected;
	}
}
