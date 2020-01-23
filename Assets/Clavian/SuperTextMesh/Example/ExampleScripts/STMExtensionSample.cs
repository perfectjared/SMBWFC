//Copyright (c) 2018 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

public static class STMExtensionSample {

	public static void SetTextWithStringAtStart(this SuperTextMesh stm, string extra){
		stm.text = extra + stm.text;
	}
	public static void SetUppercaseText(this SuperTextMesh superTextMesh, string newText){
		superTextMesh.text = newText.ToUpper();
	}
}
