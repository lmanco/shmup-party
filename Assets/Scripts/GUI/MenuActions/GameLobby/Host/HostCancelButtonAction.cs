using UnityEngine;
using System.Collections;

public class HostCancelButtonAction : DefaultMenuAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Action(){
		NetworkManager.GetInstance().StopServer();
		NetworkManager.GetInstance().hostMenu.on = false;
		NetworkManager.GetInstance().networkMenu.on = true;
	}
}
