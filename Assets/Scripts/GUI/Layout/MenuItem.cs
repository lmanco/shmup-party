using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MenuItem : MonoBehaviour {
	public string text = "";
	public string left = "0";
	public string top = "0";
	public string width = "0";
	public string height = "0";
	public DefaultMenuAction action;
	private int leftI, topI, widthI, heightI;
	public enum MenuItemType{
		Button,
		Box,
		Label,
		HorizontalSlider,
		VerticalSlider,
		VerticalScrollBox,
		MenuItemVerticalScrollBox
	};
	public MenuItemType type = MenuItemType.Button;
	public float sliderMin = 0;
	public float sliderMax = 100;
	public string sliderGameValue = "";
	public bool registered = false;
	private Rect myRect;
	public Vector2 scrollPosition = Vector2.zero;
	public string scrollBoxHeight;
	private int scrollBoxHeightI;
	public List<MenuItem> menuItemScrollBoxItems = new List<MenuItem>(); 
	public GUISkin guiSkin = null;
	
	// tooltip stuff
	public string tooltip = "";
	public int tooltipTopRel, tooltipLeftRel, tooltipWidth, tooltipHeight;
	public GUISkin tooltipSkin = null;
	private bool tooltipOn = false;
	public enum TooltipType{
		Box,
		Label
	};
	public TooltipType tooltipType = TooltipType.Label;
	public bool visible = true;
	public bool outlineLabelText = false;
	public bool overrideOutlineColor = false;
	public int outlineROverride = 0;
	public int outlineGOverride = 0;
	public int outlineBOverride = 0;
	public float outlineAOverride = 1.0f;
	
	/*public MenuItem() {
		text = "";
	}
	
	public MenuItem(string text) {
		this.text = text;
	}*/

	// Use this for initialization
	void Start () {
		leftI = Evaluate(left);
		topI = Evaluate(top);
		widthI = Evaluate(width);
		heightI = Evaluate(height);
		scrollBoxHeightI = Evaluate(scrollBoxHeight);
		myRect = new Rect(leftI, topI, widthI, heightI);
	}
	
	// Evaluate mathematical expressions and Screen.width and Screen.height for left, top, width, height
	public static int Evaluate(string expression)
	{
		expression = expression.Replace("Screen.width", Screen.width.ToString());
		expression = expression.Replace("Screen.height", Screen.height.ToString());
		return (int)(double)new System.Xml.XPath.XPathDocument
		(new StringReader("<r/>")).CreateNavigator().Evaluate
		(string.Format("number({0})", new
		System.Text.RegularExpressions.Regex(@"([\+\-\*])")
		.Replace(expression, " ${1} ")
		.Replace("/", " div ")
		.Replace("%", " mod ")));
	}
	// alternate way of doing this - needs System.Data, which doesn't seem to hold up too well when changing scenes
	/*public static int Evaluate(string expression)
    {  
		expression = expression.Replace("Screen.width", Screen.width.ToString());
		expression = expression.Replace("Screen.height", Screen.height.ToString());
		System.Data.DataTable table = new System.Data.DataTable();
		System.Data.DataRow row = table.NewRow();
		try{
			table.Columns.Add("expression", string.Empty.GetType(), expression);
			table.Rows.Add(row);
		}
		catch(EvaluateException e){
			Debug.Log(e);
			return 0;
		}
		return int.Parse((string)row["expression"]);
    } */
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/*
	// Getters for ints that shouldn't be exposed to Unity inspector
	*/
	public int getLeftI(){
		return leftI;
	}
	
	public int getTopI(){
		return topI;
	}
	
	public int getWidthI(){
		return widthI;
	}

	public int getHeightI(){
		return heightI;
	}
	
	public int getScrollBoxHeightI(){
		return scrollBoxHeightI;
	}
	
	public void setTooltipOn(bool on){
		tooltipOn = on;
	}
	
	public void SetVisible(bool on) {
		if(on && !registered) {
			MenuItemManager.RegisterRect(myRect);
			registered = true;
		} else if(!on && registered) {
			MenuItemManager.UnregisterRect(myRect);
			registered = false;
		}
	}
	
	/*
	// Setters for ints that shouldn't be exposed to Unity inspector
	*/
	
	public void setLeftI(int num){
		leftI = num;
	}
	
	public void setTopI(int num){
		topI = num;
	}
	
	public void setWidthI(int num){
		widthI = num;
	}
	
	// FOR TOOLTIPS
	// I know this is an ugly way to do it but it's by far the simplest possible way
	void OnGUI() {
		if (tooltipOn){
			GUI.skin = tooltipSkin ? tooltipSkin : guiSkin;
			GUI.depth = -5;
			if (tooltipType == TooltipType.Box){
				// GOD I'M GOOD
				GUI.Label(new Rect(leftI, topI, widthI, heightI), new GUIContent("", tooltip));
				if (GUI.tooltip == tooltip){
					GUI.Box(new Rect (leftI+tooltipLeftRel, topI+tooltipTopRel, tooltipWidth, tooltipHeight), GUI.tooltip);
				}
			}
			else{
				GUI.Label(new Rect(leftI, topI, widthI, heightI), new GUIContent("", tooltip));
				GUI.Label(new Rect (leftI+tooltipLeftRel, topI+tooltipTopRel, tooltipWidth, tooltipHeight), GUI.tooltip);
			}
		}
	}
}