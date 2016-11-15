using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CircularMenu : MonoBehaviour {

	public List<Button> buttons = new List<Button>();
	private Vector2 Mouseposition;
	private Vector2 fromVector2M = new Vector2(0.5f,1.0f);
	private Vector2 centercircle = new Vector2(0.5f,0.5f);
	private Vector2 toVector2M;

	public int menuItems;
	public int CurMenuItem;
	private int OldMenuItem;

	void Start () {
		menuItems = buttons.Count;
		foreach (Button button in buttons) {
			Image img =   button.transform.FindChild ("buttonImage").gameObject.GetComponent<Image>();
			Color normalColor = button.colors.normalColor;
			normalColor.a = 1;
			img.color = normalColor;
			button.interactable = false;
		}
		CurMenuItem = 0;
		OldMenuItem = 0;
	}

	void Update () {

	}

	public void GetCurrentMenuItem(){
		Mouseposition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		toVector2M = new Vector2 (Mouseposition.x / Screen.width, Mouseposition.y / Screen.height);
		float angle = (Mathf.Atan2 (fromVector2M.y - centercircle.y, fromVector2M.x - centercircle.x)
		              - Mathf.Atan2 (toVector2M.y - centercircle.y, toVector2M.x - centercircle.x)) * Mathf.Rad2Deg;
		if (angle < 0)
			angle += 360;
		CurMenuItem = (int)(angle / (360 / menuItems));

		if (CurMenuItem != OldMenuItem) {
			toVector2M.y = centercircle.y;
			fromVector2M.x = centercircle.x;
		}
	}

	public void ButtonAction(){
		if (CurMenuItem == 0)
			Mouseposition.x = Mouseposition.y;
	}
}

[System.Serializable]
public class MenuButton{
	public string name;
	public Image scenceImage;
	public Color NormalColor = Color.white;
	public Color HighlightedColor = Color.grey;
	public Color PressedColor = Color.gray;


}