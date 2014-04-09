using UnityEngine;
using System.Collections;

public abstract class Enemy : BoundaryAware {

	//private Sprite sprite;
	public float speed;
	public int numBullets;
	public float shotSpace;
	public float shotInterval; // seconds
	public float shotSpeed;
	public float shotDir;
	private float shotTimer;

	public GameObject bullet;

	// Use this for initialization
	void Start () {
		//sprite = GetComponent<SpriteRenderer>().sprite;
		shotTimer = 0f;
		setBounds();
	}
	
	// Update is called once per frame
	void Update() {
		Shoot();
		Move();
	}

	protected void Shoot() {
		if (shotTimer <= 0){
			shotTimer = shotInterval;
			for (int i=0; i<numBullets; i++){
				GameObject bulletObj = (GameObject)Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
				bulletObj.GetComponent<EnemyBullet>().speed = shotSpeed;
				if (numBullets % 2 != 0)
					bulletObj.GetComponent<EnemyBullet>().direction = (i % 2 == 0) ? shotDir - (i / 2) * shotSpace : shotDir + (i / 2 + 1) * shotSpace;
				else
					bulletObj.GetComponent<EnemyBullet>().direction = (i % 2 == 0) ? shotDir - (i / 2) * shotSpace - (shotSpace / 2) : shotDir + (i / 2) * shotSpace + (shotSpace / 2);
			}
		}
		else
			shotTimer -= Time.deltaTime;
	}
	
	protected void Move() {
		
	}
}
