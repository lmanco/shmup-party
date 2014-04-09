using UnityEngine;
using System.Collections;

public class EnemyBullet : BoundaryAware {

	private Sprite sprite;
	public float speed = 20f;
	public float direction = 0f;
	private float dirRads;
	public float maxSeconds = 1.0f;
	
	private float seconds;
	
	private const float PI = 3.14f;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>().sprite;
		seconds = 0f;
		dirRads = direction * PI / 180f;
		setBounds();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3((float)Mathf.Cos(dirRads), (float)Mathf.Sin(dirRads), 0) * speed * Time.deltaTime;
		seconds += Time.deltaTime;
		if (seconds > maxSeconds)
			Destroy(gameObject);
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		if (pos.x < -halfWidth || pos.x > Screen.width + halfWidth || pos.y < -halfHeight || pos.y > Screen.height + halfHeight)
			Destroy(gameObject);
	}
	
	public void SetColor(Color color){
		GetComponent<SpriteRenderer>().color = color;
	}
	
	public void setSprite(Sprite sprite){
		this.sprite = sprite;
		GetComponent<SpriteRenderer>().sprite = this.sprite;
	}
}
