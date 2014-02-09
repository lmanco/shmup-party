using UnityEngine;
using System.Collections;

public class BoundaryAware : MonoBehaviour {

	protected Vector3 screenBounds = new Vector3(Screen.width, Screen.height, 0);
	protected float halfWidth, halfHeight;

	protected void setBounds () {
		halfWidth = gameObject.renderer.bounds.size.x / 2 * 100;
		halfHeight = gameObject.renderer.bounds.size.y / 2 * 100;	
	}
}
