using UnityEngine;
using System.Collections;

public class RefreshHostsButtonAction : DefaultMenuAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Action(){
		//Debug.Log("Refreshing");
		NetworkManager.GetInstance().RefreshHostList();
	}
}
