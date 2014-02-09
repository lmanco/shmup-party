using UnityEngine;
using System.Collections;

public class Player : BoundaryAware {

	public string horizontal = "Horizontal";
	public string vertical = "Vertical";
	public string fire = "Fire1";
	public float moveSpeed = 8.0f;
	public GameObject bullet;
	public float fireIntervalSeconds = 0.1f;
	public float blinkIntervalSeconds = 0.033f;
	public float blinkTimeSeconds = 2.0f;
	public float lives = 5;

	private float fireTimer;
	private bool spawning, blinking;
	private Sprite playerSprite;
	private float blinkTimer = 0f;

	// Use this for initialization
	void Start () {
		fireTimer = 0f;
		playerSprite = GetComponent<SpriteRenderer>().sprite;
		setBounds();
		
		Spawn();
	}

	// Update is called once per frame
	void Update () {
		if (blinking) {
			blinkTimer += Time.deltaTime;
			if (blinkTimer >= blinkTimeSeconds){
				blinking = false;
				blinkTimer = 0;
			}
		}
	
		if (spawning){
			if (transform.position.y >= -4){
				transform.position = new Vector3(transform.position.x, -4, 0);
				spawning = false;
			}
			else
				transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, 0);
		}
		else{
			Move();
			Shoot();
			UpdateFireTimer();
		}
	}

	void Move () {
		Vector3 hMovement = transform.right * Input.GetAxisRaw(horizontal) * moveSpeed * Time.deltaTime;
		Vector3 vMovement = transform.up * Input.GetAxisRaw(vertical) * moveSpeed * Time.deltaTime;
		
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + hMovement + vMovement);
		
		if (screenPos.x + halfWidth > screenBounds.x){
			transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(screenBounds.x-halfWidth, 0, 0)).x, transform.position.y, transform.position.z);
		}
		else if (screenPos.x - halfWidth < 0){
			transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(halfWidth, 0, 0)).x, transform.position.y, transform.position.z);
		}
		else{
			transform.position += hMovement;
		}

		if (screenPos.y + halfHeight > screenBounds.y){
			transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(new Vector3(0, screenBounds.y-halfHeight, 0)).y, transform.position.z);
		}
		else if (screenPos.y - halfHeight < 0){
			transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(new Vector3(0, halfHeight, 0)).y, transform.position.z);
		}
		else{
			transform.position += vMovement;
		}
	}
	
	void Shoot () {
		if (Input.GetButton(fire) && fireTimer <= 0){
			fireTimer = fireIntervalSeconds;
			Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + halfHeight / 100.0f + 0.1f, 0), Quaternion.identity);
		}
	}
	
	void UpdateFireTimer () {
		if (fireTimer > 0)
			fireTimer -= Time.deltaTime;
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Death" && !blinking){
			lives--;
			Spawn();
		}
	}
	
	IEnumerator Blink() {
		blinking = true;
		while (blinking){
			yield return new WaitForSeconds(blinkIntervalSeconds);
			GetComponent<SpriteRenderer>().sprite = (GetComponent<SpriteRenderer>().sprite == null) ?
				playerSprite : null;
		}
		if (GetComponent<SpriteRenderer>().sprite == null)
			GetComponent<SpriteRenderer>().sprite = playerSprite;
	}
	
	void Spawn() {
		transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0)).x, -10, 0);
		spawning = true;
		StartCoroutine("Blink");
	}
}
