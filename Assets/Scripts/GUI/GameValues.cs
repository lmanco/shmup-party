using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameValues : MonoBehaviour {

	//player editable values
	// e.g. public int someInt = 10;
	// e.g. public float someFloat = 10.0f;
	/*public float acceleration = 10.0f; //multiplied by the trigger axis. how fast are we accelerating?
	public float adjustSpeed = 2; // how fast to adjust to terrain
	public float friction = 0.95f; //multiplied into the x/z velocities every frame.
	public float maxRotation = 30.0f;
	public float sensitivity = 15.0f;
	public float topSpeed = 100.0f; //how fast are we allowed to go?
	*/
	
	// dictionaries of editable values for menu inspector integration
	public static Dictionary<string, int> intValues = new Dictionary<string, int>();
	public static Dictionary<string, float> floatValues = new Dictionary<string, float>();

	void Awake () {
		// add int values to dictionaries here
		// e.g. intValues.Add("someInt", someInt);
		
		// add float values to dictionaries here
		// e.g. floatValues.Add("someFloat", someFloat);
		/*floatValues.Add("acceleration", acceleration);
		floatValues.Add("adjustSpeed", adjustSpeed);
		floatValues.Add("friction", friction);
		floatValues.Add("maxRotation", maxRotation);
		floatValues.Add("sensitivity", sensitivity);
		floatValues.Add("topSpeed", topSpeed);
	
		DontDestroyOnLoad(gameObject);
		*/
	}
}