using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {
	public List<MenuItem> menuItems = new List<MenuItem>();
	public Texture2D background;
	public bool on;
	public bool disabled;
	public int yOffset, xOffset = 0;
	public GUISkin guiSkin = null;
	public int sliderLabelXGap, sliderLabelYAdjust = 0;
	public int defaultOutlineR = 110;
	public int defaultOutlineG = 0;
	public int defaultOutlineB = 218;
	public float defaultOutlineA = 1.0f;
	
	// dictionary for keeping original slider labels before updating with numbers
	private Dictionary<string, string> sliderOriginalTexts = new Dictionary<string, string>();
	
	// get initial slider labels so they can update properly
	void Awake() {
		foreach (MenuItem m in menuItems) {
			if (m.type == MenuItem.MenuItemType.HorizontalSlider || m.type == MenuItem.MenuItemType.VerticalSlider){
				sliderOriginalTexts.Add(m.sliderGameValue, m.text);
			}
		}
	}
	
	// draw menu components
	void OnGUI() {
		if (on){
			DrawBackground();
		}
	
		foreach (MenuItem m in menuItems) {
			if (guiSkin){
				GUI.skin = guiSkin;
			}
		
			if (m){
				if (m.guiSkin){
					GUI.skin = m.guiSkin;
				}
			
				if(on && m.visible) {
					DrawMenuItem(m);
					m.setTooltipOn(true);
					
					// draw tooltip box for box style tooltips
					/*if (new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset, m.getWidthI(), m.getHeightI()).Contains(Input.mousePosition) && m.ttType == MenuItem.TooltipType.Box){
						GUI.skin = m.tooltipSkin;
						GUI.Box(new Rect (m.getLeftI()+m.tooltipLeftRel, m.getTopI()+m.tooltipTopRel, m.tooltipWidth, m.tooltipHeight), GUI.tooltip);
					}*/
				}
				else{
					m.setTooltipOn(false);
				}
			}
		}
	}
	
	protected void DrawBackground() {
		if(background) {
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		}
	}
	
	protected void DrawMenuItem(MenuItem m) {
		switch(m.type){
			case MenuItem.MenuItemType.Button:
				DrawButton(m);
				break;
			case MenuItem.MenuItemType.Box:
				DrawBox(m);
				break;
			case MenuItem.MenuItemType.Label:
				DrawLabel(m);
				break;
			case MenuItem.MenuItemType.HorizontalSlider:
				DrawHorizontalSlider(m);
				break;
			case MenuItem.MenuItemType.VerticalSlider:
				DrawVerticalSlider(m);
				break;
			case MenuItem.MenuItemType.VerticalScrollBox:
				DrawVerticalScrollBox(m);
				break;
			case MenuItem.MenuItemType.MenuItemVerticalScrollBox:
				DrawMenuItemVerticalScrollBox(m);
				break;
			default:
				return;
		}
	}
	
	protected void DrawButton(MenuItem m) {
		if (GUI.Button(new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset, m.getWidthI(), m.getHeightI()), m.text)){
			if (m.action && !disabled){
				m.action.Action();
			}
		}
	}
	
	protected void DrawBox(MenuItem m) {
		GUI.Box(new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset, m.getWidthI(), m.getHeightI()), m.text);
	}
	
	protected void DrawLabel(MenuItem m) {
		if (m.outlineLabelText){
			// store original GUI skin color
			Color originalColor = GUI.skin.label.normal.textColor;
			//FontStyle originalFontStyle = GUI.skin.label.fontStyle;

			// temporarily tweak GUI skin color
			Color outlineColor = (m.overrideOutlineColor) ? new Color(m.outlineROverride/255.0f, m.outlineGOverride/255.0f, m.outlineBOverride/255.0f, m.outlineAOverride)
				: new Color(defaultOutlineR/255.0f, defaultOutlineG, defaultOutlineB/255.0f, defaultOutlineA);
			//new Color(0f, 0f, 0f); // hard-coded black outline in interest of time
			GUI.skin.label.normal.textColor = outlineColor;
			GUI.skin.label.hover.textColor = outlineColor;
			GUI.skin.label.active.textColor = outlineColor;
			//GUI.skin.label.fontStyle = FontStyle.Bold;
			
			// draw outline
			GUI.Label(new Rect(m.getLeftI()+xOffset+1, m.getTopI()+yOffset+sliderLabelYAdjust, m.getWidthI()+sliderLabelXGap, m.getHeightI()), m.text);
			GUI.Label(new Rect(m.getLeftI()+xOffset+2, m.getTopI()+yOffset+sliderLabelYAdjust, m.getWidthI()+sliderLabelXGap, m.getHeightI()), m.text);
			GUI.Label(new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset+sliderLabelYAdjust+1, m.getWidthI()+sliderLabelXGap, m.getHeightI()), m.text);
			GUI.Label(new Rect(m.getLeftI()+xOffset-1, m.getTopI()+yOffset+sliderLabelYAdjust, m.getWidthI()+sliderLabelXGap, m.getHeightI()), m.text);
			GUI.Label(new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset+sliderLabelYAdjust-1, m.getWidthI()+sliderLabelXGap, m.getHeightI()), m.text);
			
			// restore original GUI skin color
			GUI.skin.label.normal.textColor = originalColor;
			GUI.skin.label.hover.textColor = originalColor;
			GUI.skin.label.active.textColor = originalColor;
			//GUI.skin.label.fontStyle = originalFontStyle;
		}

		GUI.Label(new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset+sliderLabelYAdjust, m.getWidthI()+sliderLabelXGap, m.getHeightI()), m.text);
	}
	
	protected void DrawHorizontalSlider(MenuItem m) {
		if (GameValues.intValues.ContainsKey(m.sliderGameValue)){
			GameValues.intValues[m.sliderGameValue] = (int)GUI.HorizontalSlider(new Rect(m.getLeftI()+xOffset+sliderLabelXGap, m.getTopI()+yOffset, m.getWidthI(), m.getHeightI()),GameValues.intValues[m.sliderGameValue],m.sliderMin,m.sliderMax);
			m.text = sliderOriginalTexts[m.sliderGameValue] + GameValues.intValues[m.sliderGameValue];
			DrawLabel(m);
			/*if (GameObject.FindWithTag(m.sliderGameValue)){
				GameObject.FindWithTag(m.sliderGameValue).GetComponent<MenuItem>().text = "" + GameValues.intValues[m.sliderGameValue];
			}*/
		}
		else if (GameValues.floatValues.ContainsKey(m.sliderGameValue)){
			GameValues.floatValues[m.sliderGameValue] = (float)GUI.HorizontalSlider(new Rect(m.getLeftI()+xOffset+sliderLabelXGap, m.getTopI()+yOffset, m.getWidthI(), m.getHeightI()),GameValues.floatValues[m.sliderGameValue],m.sliderMin,m.sliderMax);
			m.text = sliderOriginalTexts[m.sliderGameValue] + GameValues.floatValues[m.sliderGameValue];
			DrawLabel(m);
			/*if (GameObject.FindWithTag(m.sliderGameValue)){
				GameObject.FindWithTag(m.sliderGameValue).GetComponent<MenuItem>().text = "" + GameValues.floatValues[m.sliderGameValue];
			}*/
		}
		DrawLabel(m);
	}
		
	protected void DrawVerticalSlider(MenuItem m) {
		if (GameValues.intValues.ContainsKey(m.sliderGameValue)){
			GameValues.intValues[m.sliderGameValue] = (int)GUI.VerticalSlider(new Rect(m.getLeftI()+xOffset+sliderLabelXGap, m.getTopI()+yOffset, m.getWidthI(), m.getHeightI()),GameValues.intValues[m.sliderGameValue],m.sliderMin,m.sliderMax);
			m.text = sliderOriginalTexts[m.sliderGameValue] + GameValues.intValues[m.sliderGameValue];
			DrawLabel(m);
			/*if (GameObject.FindWithTag(m.sliderGameValue)){
				GameObject.FindWithTag(m.sliderGameValue).GetComponent<MenuItem>().text = "" + GameValues.intValues[m.sliderGameValue];
			}*/
		}
		else if (GameValues.floatValues.ContainsKey(m.sliderGameValue)){
			GameValues.floatValues[m.sliderGameValue] = (float)GUI.VerticalSlider(new Rect(m.getLeftI()+xOffset+sliderLabelXGap, m.getTopI()+yOffset, m.getWidthI(), m.getHeightI()),GameValues.floatValues[m.sliderGameValue],m.sliderMin,m.sliderMax);
			m.text = sliderOriginalTexts[m.sliderGameValue] + GameValues.floatValues[m.sliderGameValue];
			DrawLabel(m);
			/*if (GameObject.FindWithTag(m.sliderGameValue)){
				GameObject.FindWithTag(m.sliderGameValue).GetComponent<MenuItem>().text = "" + GameValues.floatValues[m.sliderGameValue];
			}*/
		}
	}
	
	protected void DrawVerticalScrollBox(MenuItem m) {
		m.scrollPosition = GUI.BeginScrollView(new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset, m.getWidthI()+18, m.getHeightI()),
			m.scrollPosition, new Rect(0, 0, m.getWidthI(), m.getScrollBoxHeightI()));
		
		GUI.Box(new Rect(0, 0, m.getWidthI(), m.getScrollBoxHeightI()), m.text);
		
		GUI.EndScrollView();
	}
	
	protected void DrawMenuItemVerticalScrollBox(MenuItem m) {
		m.scrollPosition = GUI.BeginScrollView(new Rect(m.getLeftI()+xOffset, m.getTopI()+yOffset, m.getWidthI()+18, m.getHeightI()),
			m.scrollPosition, new Rect(0, 0, m.getWidthI(), m.getScrollBoxHeightI()));
		
		int y = 0;
		foreach (MenuItem menuItemScrollBoxItem in m.menuItemScrollBoxItems) {
			// TODO: Expand this to work for other MenuItem types besides buttons
			if (GUI.Button(new Rect(0, y, menuItemScrollBoxItem.getWidthI(), menuItemScrollBoxItem.getHeightI()), menuItemScrollBoxItem.text)){
				if (menuItemScrollBoxItem.action && !disabled){
					menuItemScrollBoxItem.action.Action();
				}
			}
			y += menuItemScrollBoxItem.getHeightI();
		}
		
		GUI.EndScrollView();
	}
	
	public void Restart(){
		Awake();
	}
}
