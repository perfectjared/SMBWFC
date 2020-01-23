using UnityEngine;
using System.Collections;

public class STMSetTextExample : MonoBehaviour {
	public SuperTextMesh stm;
	public string myString;
	
	void Awake () {
		SetText();
	}
	
	[ContextMenu("SetText")]
	public void SetText()
	{
		stm.Text = myString;
	}
}
