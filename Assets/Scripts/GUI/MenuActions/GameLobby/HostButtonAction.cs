using UnityEngine;
using System.Collections;

public class HostButtonAction : DefaultMenuAction {

	private HostData hostData;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Action(){
		if (hostData.connectedPlayers < PlayersConnectedBox.MAX_PLAYERS)
			Network.Connect(hostData);
	}
	
	public void SetHostData(HostData hostData){
		this.hostData = hostData;
	}
	
}
