using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 20f;
	public float maxSeconds = 1.0f;
	
	private float seconds;

	// Use this for initialization
	void Start () {
		seconds = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine){
			transform.position += transform.up * speed * Time.deltaTime;
			seconds += Time.deltaTime;
			if (seconds > maxSeconds){
				networkView.RPC("Destroy", RPCMode.Others);
				Destroy(gameObject);
			}
		}
	}
	
	[RPC]
	void Destroy() {
		Destroy(gameObject);
	}
	
	public void SetColor(Color color){
		GetComponent<SpriteRenderer>().color = color;
		Color theColor = GetComponent<SpriteRenderer>().color;
		networkView.RPC("UpdateColor", RPCMode.Others, theColor.r, theColor.g, theColor.b, theColor.a);
	}
	
	[RPC]
	void UpdateColor(float r, float g, float b, float a){
		GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
	}
}
