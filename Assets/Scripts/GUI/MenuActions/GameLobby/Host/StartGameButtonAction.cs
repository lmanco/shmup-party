using UnityEngine;
using System.Collections;

public class StartGameButtonAction : DefaultMenuAction {

	public string firstScene = "Level";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Action(){
		networkView.RPC("StartGame", RPCMode.AllBuffered);
		//Application.LoadLevel(firstScene);
	}
	
	[RPC]
	void StartGame() {
		Application.LoadLevel(firstScene);
	}
}
