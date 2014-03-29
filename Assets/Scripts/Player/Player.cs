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
	private int spawnX;
	private Sprite playerSprite;
	private float blinkTimer = 0f;
	private string pNum;
	
	//private Vector3 targetPosition;
	//private float adjustSpeed = 10.0f;

	// Use this for initialization
	void Start () {
		playerSprite = GetComponent<SpriteRenderer>().sprite;
		if (networkView.isMine){
			fireTimer = 0f;
			setBounds();
			spawning = true;
			pNum = "none";
		
			//Spawn();
		}
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
	
		if (networkView.isMine){		
			if (spawning){
				networkView.RPC("UpdatePosition", RPCMode.Others, transform.position, transform.rotation);
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
		//else
			//transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * adjustSpeed);
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
		
		if ((hMovement.x != 0.0) || (vMovement.y != 0.0)){
			networkView.RPC("UpdatePosition", RPCMode.Others, transform.position, transform.rotation);
		}
	}
	
	void Shoot () {
		if (Input.GetButton(fire) && fireTimer <= 0){
			fireTimer = fireIntervalSeconds;
			GameObject bulletObj = (GameObject)Network.Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + halfHeight / 100.0f + 0.1f, 0), Quaternion.identity, 0);
			bulletObj.GetComponent<Bullet>().SetColor(GetComponent<SpriteRenderer>().color);
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
	
	public void Spawn() {
		transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(spawnX, 0, 0)).x, -10, 0);
		spawning = true;
		StartCoroutine("Blink");
		if (networkView.isMine){
			networkView.RPC("UpdatePosition", RPCMode.Others, transform.position, transform.rotation);
			networkView.RPC("StartBlink", RPCMode.Others);
		}
	}
	
	public void SetColor(Color color){
		GetComponent<SpriteRenderer>().color = color;
		Color theColor = GetComponent<SpriteRenderer>().color;
		networkView.RPC("UpdateColor", RPCMode.OthersBuffered, theColor.r, theColor.g, theColor.b, theColor.a);
	}
	
	[RPC]
	void UpdateColor(float r, float g, float b, float a){
		GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
	}
	
	public void SetSpawnX(int spawnX){
		this.spawnX = spawnX;
	}
	
	public void SetPNum(string pNum){
		this.pNum = pNum;
		networkView.RPC("UpdatePNum", RPCMode.OthersBuffered, pNum);
	}
	
	public string GetPNum(){
		return pNum;
	}
	
	[RPC]
	void UpdatePNum(string pNum){
		this.pNum = pNum;
	}
	
	/*[RPC]
	void UpdatePosition(Vector3 newPosition, Quaternion rotation) {
		targetPosition = newPosition;
		transform.rotation = rotation;
	}*/
	
	[RPC]
	void UpdatePosition(Vector3 newPosition, Quaternion rotation) {
		transform.position = newPosition;
		transform.rotation = rotation;
	}
	
	[RPC]
	void StartBlink(){
		StartCoroutine("Blink");
	}
}
