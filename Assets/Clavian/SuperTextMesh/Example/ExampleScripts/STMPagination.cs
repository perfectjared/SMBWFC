using UnityEngine;
using System.Collections;

public class STMPagination : MonoBehaviour {
	public SuperTextMesh originalText;
	public SuperTextMesh overflowText;
	
	public void Awake(){
		//clear on runtime
		overflowText.text = "";
	}
	public void OverflowLeftovers(){
		overflowText.text = originalText.leftoverText.TrimStart();
		//if there's no text, Rebuild() doesn't get called anyway
		//use TrimStart() to remove any spaces that might have carried over.
	}
	//is this implementation better? causes errors when the object isn't defined...
	public void OverflowLeftovers(SuperTextMesh stm){
		overflowText.text = stm.leftoverText.TrimStart();
	}
}
