using UnityEngine;
using System.Collections;

public class Basic : BoundaryAware {

	public float amplitude = 1.0f;
	public float hSpeed = 7.0f;
	public float vSpeed = 5.0f;
	public bool startRight = true;
	
	private Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		setBounds();
	}
	
	// Update is called once per frame
	void Update () {
		startPos += transform.forward * hSpeed * Time.deltaTime;
		float sine = (startRight) ? Mathf.Sin(Time.time * hSpeed) : -Mathf.Sin(Time.time * hSpeed);
		transform.position = new Vector3(startPos.x + sine * amplitude, transform.position.y - vSpeed * Time.deltaTime, 0);

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPos.y + halfHeight < 0)
			Destroy(gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		if ((collision.gameObject.tag == "Bullet") && (screenPos.y + halfHeight < screenBounds.y))
			Destroy(gameObject);
	}
}
