using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour {

	public GameObject player;
	public Transform spawnPoint;

	// Use this for initialization
	void Start () {
		int numPlayers = NetworkManager.GetInstance().numConnectedPlayers;
		string pNumStr = Network.player.ToString();
		int pNum = Convert.ToInt32(pNumStr);
		
		GameObject playerObj = (GameObject)Network.Instantiate(player, spawnPoint.position, Quaternion.identity, 0);
		Player myPlayer = playerObj.GetComponent<Player>();
		
		myPlayer.SetPNum(pNumStr);
		
		// temporary
		switch (pNum){
			case 0:
				myPlayer.SetColor(new Color(0, 0, 1, 1));
				break;
			case 1:
				myPlayer.SetColor(new Color(1, 0, 0, 1));
				break;
			case 2:
				myPlayer.SetColor(new Color(1, 1, 0, 1));
				break;
			case 3:
				myPlayer.SetColor(new Color(0, 1, 0, 1));
				break;
			case 4:
				myPlayer.SetColor(new Color(1, 0, 1, 1));
				break;
			case 5:
				myPlayer.SetColor(new Color(1, 0.5f, 0, 1));
				break;
			default:
				myPlayer.SetColor(new Color(1, 1, 1, 1));
				break;
		}
		
		myPlayer.SetSpawnX((Screen.width / numPlayers) * pNum + (Screen.width / (numPlayers * 2)));
		myPlayer.Spawn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
