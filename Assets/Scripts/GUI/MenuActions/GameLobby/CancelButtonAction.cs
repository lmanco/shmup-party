using UnityEngine;
using System.Collections;

public class CancelButtonAction : DefaultMenuAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Action(){
		//Debug.Log("Refreshing");
		if (Network.isServer){
			NetworkManager.GetInstance().StopServer();
			NetworkManager.GetInstance().hostMenu.on = false;
		}
		else{
			Network.Disconnect();
			NetworkManager.GetInstance().clientMenu.on = false;
		}
		NetworkManager.GetInstance().networkMenu.on = true;
		PlayersConnectedBox.GetInstance().SetPlayersConnected(0);
	}
}
