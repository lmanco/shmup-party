﻿using UnityEngine;
using System.Collections;

public class HostGameButtonAction : DefaultMenuAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Action(){
		NetworkManager.GetInstance().StartServer();
	}
}
