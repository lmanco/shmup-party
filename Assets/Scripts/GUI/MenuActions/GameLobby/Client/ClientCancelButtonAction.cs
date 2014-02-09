using UnityEngine;
using System.Collections;

public class ClientCancelButtonAction : DefaultMenuAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Action(){
		Network.Disconnect();
		NetworkManager.GetInstance().clientMenu.on = false;
		NetworkManager.GetInstance().networkMenu.on = true;
	}
}
