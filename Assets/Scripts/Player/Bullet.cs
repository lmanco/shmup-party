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
		transform.position += transform.up * speed * Time.deltaTime;
		seconds += Time.deltaTime;
		if (seconds > maxSeconds)
			Destroy(gameObject);
	}
}
