﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public string gameName = "SHMUP_PARTY_GAME";
	public MenuItem hostList, hostButtonPrefab;
	public HostButtonAction hostButtonActionPrefab;
	public Menu networkMenu, hostMenu, clientMenu;
	public int numConnectedPlayers;
	
	private static NetworkManager self;
	private bool refreshing;
	private HostData[] hostData;
	private string lobbyName = "Lobby";
	
	void Awake(){
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		self = this;
		refreshing = false;
		hostData = new HostData[0];
	}
	
	// Update is called once per frame
	void Update () {
		if (refreshing){
			if (MasterServer.PollHostList().Length > 0){
				refreshing = false;
				//Debug.Log(MasterServer.PollHostList().Length);
				hostData = MasterServer.PollHostList();
				
				
				/*GameObject[] hostButtonActions = GameObject.FindGameObjectsWithTag("HostButtonAction");
				foreach (GameObject hBA in hostButtonsActions){
					Destroy(hBA);
				}*/
				foreach (HostData hostDatum in hostData){
					//HostButtonAction hostButtonAction = (HostButtonAction)Instantiate(hostButtonActionPrefab, Vector3.zero, Quaternion.identity);
					//hostButtonAction.SetHostData(hostDatum);
					MenuItem hostButton = (MenuItem)Instantiate(hostButtonPrefab, Vector3.zero, Quaternion.identity);
					hostButton.text = hostDatum.gameName + " (" + hostDatum.connectedPlayers + "/" + GameValues.MAX_PLAYERS + ")";
					HostButtonAction hostButtonAction = (HostButtonAction)hostButton.action;
					hostButtonAction.SetHostData(hostDatum);
					//hostButton.action = hostButtonAction;
					hostList.menuItemScrollBoxItems.Add(hostButton);
				}
			}
		}
	}
	
	public static NetworkManager GetInstance() {
		return self;
	}
	
	public void StartServer() {
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameName, "SHMUP Party!", "It's a shmup party.");
	}
	
	public void StopServer() {
		Network.Disconnect();
		MasterServer.UnregisterHost();
	}
	
	public void RefreshHostList() {
		// Reset buttons
		hostList.menuItemScrollBoxItems.Clear();
		GameObject[] hostButtons = GameObject.FindGameObjectsWithTag("HostButton");
		foreach (GameObject hB in hostButtons){
			Destroy(hB);
		}
		
		// refresh
		MasterServer.RequestHostList(gameName);
		refreshing = true;
	}
	
	// client
	void OnConnectedToServer(){
		if (Application.loadedLevelName == lobbyName){
			networkMenu.on = false;
			clientMenu.on = true;
		}
	}
	
	// client
	void OnDisconnectedFromServer(NetworkDisconnection info){
		if (Application.loadedLevelName == lobbyName){
			RefreshHostList();
			clientMenu.on = false;
			networkMenu.on = true;
			PlayersConnectedBox.GetInstance().SetPlayersConnected(0);
		}
		else{
			Application.LoadLevel(lobbyName);
		}
	}
	
	// server
	void OnPlayerConnected(NetworkPlayer player) {
		if (Application.loadedLevelName == lobbyName){
			PlayersConnectedBox.GetInstance().AddPlayersConnected(1);
		}
		UpdateNumConnectedPlayers();
	}
	
	// server
	void OnPlayerDisconnected(NetworkPlayer player) {
		if (Application.loadedLevelName == lobbyName){
			PlayersConnectedBox.GetInstance().AddPlayersConnected(-1);
		}
		else{
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")){
				if (player.ToString() == g.GetComponent<Player>().GetPNum()){
					Destroy(g);
					networkView.RPC("DestroyPlayer", RPCMode.Others, player.ToString());
				}
			}
		}
		UpdateNumConnectedPlayers();
	}
	
	public void UpdateNumConnectedPlayers(){
		if (Network.isServer){
			numConnectedPlayers = Network.connections.Length + 1;
			networkView.RPC("SetNumConnectedPlayers", RPCMode.Others, numConnectedPlayers);
		}
	}
	
	[RPC]
	void SetNumConnectedPlayers(int num){
		numConnectedPlayers = num;
	}
	
	[RPC]
	void DestroyPlayer(string pNum){
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")){
			if (pNum == g.GetComponent<Player>().GetPNum()){
				Destroy(g);
			}		
		}
	}
	
	/*void OnServerInitialized(){
		Debug.Log("Server initialized!");
	}
	
	void OnMasterServerEvent(MasterServerEvent mse){
		if (mse == MasterServerEvent.RegistrationSucceeded){
			Debug.Log("Registered server!");
		}
	}*/
}
