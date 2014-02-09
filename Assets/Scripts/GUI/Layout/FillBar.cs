using UnityEngine;
using System.Collections;

public class FillBar : MonoBehaviour {

	public Texture2D texture;
	public int x = 0;
	public int y = 0;
	public float clipAmount = 0.0f;
	private const int DEPTH = 100;
	public enum DrainDirection{
		LeftToRight,
		RightToLeft,
		TopToBottom,
		BottomToTop
	};
	public DrainDirection direction = DrainDirection.LeftToRight;
	public bool on = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI () {
		if (texture && on){
			GUI.depth = DEPTH;
			Rect groupRect, textureRect;
			switch(direction){
				case DrainDirection.LeftToRight:
					groupRect = new Rect(x+clipAmount, y, texture.width, texture.height);
					textureRect = new Rect(-clipAmount, 0, texture.width, texture.height);
					break;
				case DrainDirection.RightToLeft:
					groupRect = new Rect(x-clipAmount, y, texture.width, texture.height);
					textureRect = new Rect(clipAmount, 0, texture.width, texture.height);
					break;
				case DrainDirection.TopToBottom:
					groupRect = new Rect(x, y+clipAmount, texture.width, texture.height);
					textureRect = new Rect(0, -clipAmount, texture.width, texture.height);
					break;
				case DrainDirection.BottomToTop:
					groupRect = new Rect(x, y-clipAmount, texture.width, texture.height);
					textureRect = new Rect(0, clipAmount, texture.width, texture.height);
					break;
				default:
					return;
			}
			GUI.BeginGroup(groupRect);
			GUI.DrawTexture(textureRect, texture);
			GUI.EndGroup();
		}
	}
}
