//Copyright (c) 2019 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class STMMatchRect : MonoBehaviour 
{
	private RectTransform tr; //this object's own transform
	public SuperTextMesh stm; //stm used for reference
	public Vector2 size;
	public Vector2 offset;
	
	//set up events!
	public void OnEnable()
	{
		stm.OnPrintEvent += Match;
		tr = GetComponent<RectTransform>();
	}
	public void OnDisable()
	{
		stm.OnPrintEvent -= Match;
	}
	//make this object's rect match STM's current text rect.
	public void Match()
	{
		//box size, plus offset
		size.x = stm.bottomRightTextBounds.x - stm.topLeftTextBounds.x;
		size.y = -stm.bottomRightTextBounds.y + stm.topLeftTextBounds.y;
		offset.x = stm.t.position.x + stm.rawTopLeftBounds.x + stm.rawBottomRightBounds.x * 2f;
		offset.y = stm.t.position.y - size.y - stm.rawTopLeftBounds.y;
		tr.sizeDelta = size;
		tr.position = offset;
		tr.pivot = Vector2.zero;
	}
}
