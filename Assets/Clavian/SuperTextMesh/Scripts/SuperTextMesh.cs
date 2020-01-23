//Copyright (c) 2016-2019 Kai Clavier [kaiclavier.com] Do Not Distribute
//Super Text Mesh v1.8.14
using UnityEngine;
using UnityEngine.Events; //for the OnComplete event
#if UNITY_EDITOR
using UnityEditor; //for loading default stuff and menu thing
#endif
using System; // For access to the array class
using System.Linq; //for sorting inspector stuff by creation date, and removing doubles from lists
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; //for in-game UI stuff

#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement; //for OnSceneLoaded
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(SuperTextMesh))]
[CanEditMultipleObjects] //sure why not
public class SuperTextMeshEditor : Editor{
	override public void OnInspectorGUI(){
		serializedObject.Update(); //for onvalidate stuff!
		var stm = target as SuperTextMesh; //get this text mesh as a component
		
	//Actually Drawing it to the inspector:
		Rect r = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, 0f); //get width on inspector, minus scrollbar

		GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout); //create a new foldout style, for the bold foldout headers
		foldoutStyle.fontStyle = FontStyle.Bold; //set it to look like a header
	//TEXT DATA ICON
		//Object textDataObject = stm.data; //get text data object
		GUIStyle textDataStyle = new GUIStyle(EditorStyles.label);
		//textDataStyle.fixedWidth = 14;
		//textDataStyle.fixedHeight = 14;
		//Get Texture2D one of these two ways:
		//Texture2D textDataIcon = AssetDatabase.LoadAssetAtPath("Assets/Clavian/SuperTextMesh/Scripts/SuperTextMeshDataIcon.png", typeof(Texture2D)) as Texture2D;
		Texture2D textDataIcon = EditorGUIUtility.ObjectContent(stm.data, typeof(SuperTextMeshData)).image as Texture2D;
		textDataStyle.normal.background = textDataIcon; //apply
		textDataStyle.active.background = textDataIcon;
		if(GUI.Button(new Rect(r.width - 2, r.y, 16, 16), new GUIContent("", "Edit TextData"), textDataStyle)){ //place at exact spot
			//EditorWindow.GetWindow()
			//EditorUtility.FocusProjectWindow();
			//Selection.activeObject = textDataObject; //go to textdata!
			//EditorGUIUtility.PingObject(textDataObject);
			stm.data.textDataEditMode = !stm.data.textDataEditMode; //show textdata inspector!
			//if(stm.data.textDataEditMode){
			//	stm.data = null;
			//}
		}
	
		if(stm.data.textDataEditMode){//show textdata file instead
			var serializedData = new SerializedObject(stm.data);
			serializedData.Update();

		//Draw it!
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.HelpBox("Editing Text Data. Click the [T] to exit!", MessageType.None, true);

			stm.data.showEffectsFoldout = EditorGUILayout.Foldout(stm.data.showEffectsFoldout, "Effects", foldoutStyle);
			if(stm.data.showEffectsFoldout){
				EditorGUI.indentLevel++;
			//Waves:
				stm.data.showWavesFoldout = EditorGUILayout.Foldout(stm.data.showWavesFoldout, "Waves", foldoutStyle);
				if(stm.data.showWavesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMWaveData> i in stm.data.waves.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each wave
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Wave", "STMWaves/New Wave.asset", "STMWaveData", stm);
				}
			//Jitters:
				stm.data.showJittersFoldout = EditorGUILayout.Foldout(stm.data.showJittersFoldout, "Jitters", foldoutStyle);
				if(stm.data.showJittersFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMJitterData> i in stm.data.jitters.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Jitter", "STMJitters/New Jitter.asset", "STMJitterData", stm);
				}
			//Draw Animations:
				stm.data.showDrawAnimsFoldout = EditorGUILayout.Foldout(stm.data.showDrawAnimsFoldout, "DrawAnims", foldoutStyle);
				if(stm.data.showDrawAnimsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDrawAnimData> i in stm.data.drawAnims.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New DrawAnim", "STMDrawAnims/New DrawAnim.asset", "STMDrawAnimData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showTextColorFoldout = EditorGUILayout.Foldout(stm.data.showTextColorFoldout, "Text Color", foldoutStyle);
			if(stm.data.showTextColorFoldout){
				EditorGUI.indentLevel++;
			//Colors:
				stm.data.showColorsFoldout = EditorGUILayout.Foldout(stm.data.showColorsFoldout, "Colors", foldoutStyle);
				if(stm.data.showColorsFoldout){
				//Gather all data
					foreach(KeyValuePair<string, STMColorData> i in stm.data.colors.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Color", "STMColors/New Color.asset", "STMColorData", stm);
				}
			//Gradients:
				stm.data.showGradientsFoldout = EditorGUILayout.Foldout(stm.data.showGradientsFoldout, "Gradients", foldoutStyle);
				if(stm.data.showGradientsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMGradientData> i in stm.data.gradients.OrderBy(x => -x.Value.GetInstanceID())){ //reorder so the order stays consistent
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Gradient", "STMGradients/New Gradient.asset", "STMGradientData", stm);
				}
			//Textures:
				stm.data.showTexturesFoldout = EditorGUILayout.Foldout(stm.data.showTexturesFoldout, "Textures", foldoutStyle);
				if(stm.data.showTexturesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMTextureData> i in stm.data.textures.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Texture", "STMTextures/New Texture.asset", "STMTextureData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showInlineFoldout = EditorGUILayout.Foldout(stm.data.showInlineFoldout, "Inline", foldoutStyle);
			if(stm.data.showInlineFoldout){
				EditorGUI.indentLevel++;
			//Delays:
				stm.data.showDelaysFoldout = EditorGUILayout.Foldout(stm.data.showDelaysFoldout, "Delays", foldoutStyle);
				if(stm.data.showDelaysFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDelayData> i in stm.data.delays.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Delay", "STMDelays/New Delay.asset", "STMDelayData", stm);
				}
			//Voices:
				stm.data.showVoicesFoldout = EditorGUILayout.Foldout(stm.data.showVoicesFoldout, "Voices", foldoutStyle);
				if(stm.data.showVoicesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMVoiceData> i in stm.data.voices.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Voice", "STMVoices/New Voice.asset", "STMVoiceData", stm);
				}
			//Fonts:
				stm.data.showFontsFoldout = EditorGUILayout.Foldout(stm.data.showFontsFoldout, "Fonts", foldoutStyle);
				if(stm.data.showFontsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMFontData> i in stm.data.fonts.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Font", "STMFonts/New Font.asset", "STMFontData", stm);
				}
			//AudioClips:
				stm.data.showAudioClipsFoldout = EditorGUILayout.Foldout(stm.data.showAudioClipsFoldout, "AudioClips", foldoutStyle);
				if(stm.data.showAudioClipsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMAudioClipData> i in stm.data.audioClips.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Audio Clip", "STMAudioClips/New Audio Clip.asset", "STMAudioClipData", stm);
				}
			//Sound Clips:
			//This one's a bit different! Since it's folders of clips...
				stm.data.showSoundClipsFoldout = EditorGUILayout.Foldout(stm.data.showSoundClipsFoldout, "Sound Clips", foldoutStyle);
				if(stm.data.showSoundClipsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMSoundClipData> i in stm.data.soundClips.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Sound Clip", "STMSoundClips/New Sound Clip.asset", "STMSoundClipData", stm);
				}
			//Quads:
				stm.data.showQuadsFoldout = EditorGUILayout.Foldout(stm.data.showQuadsFoldout, "Quads", foldoutStyle);
				if(stm.data.showQuadsFoldout){
					EditorGUILayout.HelpBox("For information on how this works, please refer to the sample image under 'Quads' in the documentation.", MessageType.None, true);
				//Gather all data:
					foreach(KeyValuePair<string, STMQuadData> i in stm.data.quads.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Quad", "STMQuads/New Quad.asset", "STMQuadData", stm);
				}
			//Materials:
				stm.data.showMaterialsFoldout = EditorGUILayout.Foldout(stm.data.showMaterialsFoldout, "Materials", foldoutStyle);
				if(stm.data.showMaterialsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMMaterialData> i in stm.data.materials.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Material", "STMMaterials/New Material.asset", "STMMaterialData", stm);
				}

				EditorGUI.indentLevel--;
			}

			stm.data.showAutomaticFoldout = EditorGUILayout.Foldout(stm.data.showAutomaticFoldout, "Automatic", foldoutStyle);
			if(stm.data.showAutomaticFoldout){
				EditorGUI.indentLevel++;
			//AutoDelays:
				stm.data.showAutoDelaysFoldout = EditorGUILayout.Foldout(stm.data.showAutoDelaysFoldout, "AutoDelays", foldoutStyle);
				if(stm.data.showAutoDelaysFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDelayData> i in stm.data.autoDelays.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Auto Delay", "STMAutoDelays/New Auto Delay.asset", "STMDelayData", stm);
				}
			//AutoClips:
				stm.data.showAutoClipsFoldout = EditorGUILayout.Foldout(stm.data.showAutoClipsFoldout, "AutoClips", foldoutStyle);
				if(stm.data.showAutoClipsFoldout){
				//Gather all data:
					//STMSoundClipData[] allAutoClips = Resources.LoadAll<STMSoundClipData>("STMAutoClips").OrderBy(x => -x.GetInstanceID()).ToArray(); //do this so order keeps consistent
					foreach(KeyValuePair<string, STMAutoClipData> i in stm.data.autoClips.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Auto Clip", "STMAutoClips/New Auto Clip.asset", "STMAutoClipData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showMasterFoldout = EditorGUILayout.Foldout(stm.data.showMasterFoldout, "Master", foldoutStyle);
			if(stm.data.showMasterFoldout){
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(serializedData.FindProperty("disableAnimatedText"), true);
				EditorGUILayout.PropertyField(serializedData.FindProperty("defaultFont"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("boundsColor"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("textBoundsColor"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("finalTextBoundsColor"));
				EditorGUI.indentLevel--;
			}
			if(GUILayout.Button("Refresh Database")){
				stm.data = null;
			}

			serializedData.ApplyModifiedProperties();
		}else{ //draw actual text mesh inspector:
			
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_text"));

			stm.showAppearanceFoldout = EditorGUILayout.Foldout(stm.showAppearanceFoldout, "Appearance", foldoutStyle);
			if(stm.showAppearanceFoldout){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("font"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("color")); //richtext default stuff...
				if(stm.bestFit == SuperTextMesh.BestFitMode.Always){ //no editing value
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.FloatField("Size", stm.size * stm.bestFitMulti);
					EditorGUI.EndDisabledGroup();
				}else if(stm.bestFit == SuperTextMesh.BestFitMode.OverLimit){
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.FloatField(stm.size * stm.bestFitMulti);
					EditorGUI.EndDisabledGroup();
					EditorGUILayout.EndHorizontal();
				}else{ //no best fit value
					EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
				}
				EditorGUILayout.PropertyField(serializedObject.FindProperty("bestFit"));

				if(stm.font != null){
					EditorGUI.BeginDisabledGroup(!stm.font.dynamic);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("style"));
					EditorGUI.EndDisabledGroup();
				}
				EditorGUILayout.PropertyField(serializedObject.FindProperty("richText"));

				EditorGUILayout.Space(); //////////////////SPACE
				
				if(stm.font != null){
					EditorGUILayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup(!stm.font.dynamic || stm.autoQuality);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("quality")); //text rendering
					EditorGUI.EndDisabledGroup();
					if(stm.uiMode)
					{
						EditorGUILayout.PropertyField(serializedObject.FindProperty("autoQuality"));
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.PropertyField(serializedObject.FindProperty("filterMode"));
				if(!stm.uiMode){
					UnityEngine.Rendering.ShadowCastingMode shadowMode = stm.r.shadowCastingMode;
					stm.r.shadowCastingMode = (UnityEngine.Rendering.ShadowCastingMode)EditorGUILayout.EnumPopup("Shadow Casting Mode", shadowMode);
				}
				//EditorGUILayout.BeginHorizontal();
				//if(GUILayout.Button("Ping")){
					//EditorUtility.FocusProjectWindow();
				//	EditorGUIUtility.PingObject(stm.textMaterial); //select this object
				//}
		//Materials
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("New"))
                {
                    //Debug.Log(ClavianPath);

                    //create new material in the correct folder
                    //give it the correct default shader
                    //assign it to this text mesh
                    string whatShader = stm.uiMode ? "Super Text Mesh/UI/Default" : "Super Text Mesh/Unlit/Default";
                    Material newMaterial = new Material(Shader.Find(whatShader));
                    if(!AssetDatabase.IsValidFolder(STMCustomInspectorTools.ClavianPath + "Materials")){
                        //create folder if it doesn't exist yet
                        AssetDatabase.CreateFolder(STMCustomInspectorTools.ClavianPath.Remove(STMCustomInspectorTools.ClavianPath.Length - 1), "Materials");
                    }
                    AssetDatabase.CreateAsset(newMaterial, AssetDatabase.GenerateUniqueAssetPath(STMCustomInspectorTools.ClavianPath + "Materials/NewMaterial.mat"));
                    stm.textMaterial = newMaterial;
                }
				EditorGUILayout.PropertyField(serializedObject.FindProperty("textMaterial")); //appearance
				EditorGUILayout.EndHorizontal();
				//EditorGUILayout.EndHorizontal();

				if(stm.textMaterial != null){
					stm.showMaterialFoldout = EditorGUILayout.Foldout(stm.showMaterialFoldout, "Material", foldoutStyle);
					if(stm.showMaterialFoldout){ //show shader settings
						STMCustomInspectorTools.DrawMaterialEditor(stm.textMaterial);
					}
				}
			}

			//EditorGUILayout.Space(); //////////////////SPACE
			stm.showPositionFoldout = EditorGUILayout.Foldout(stm.showPositionFoldout, "Position", foldoutStyle);
			if(stm.showPositionFoldout){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("baseOffset")); //physical stuff
				
				if(stm.uiMode){
					string[] anchorNames = new string[]{"Top", "Middle", "Bottom"};
					int[] anchorValues = new int[]{0,3,6};
					EditorGUI.BeginChangeCheck();
					stm.anchor = (TextAnchor)EditorGUILayout.IntPopup("Anchor", (int)Mathf.Floor((float)stm.anchor / 3f) * 3, anchorNames, anchorValues);
					if(EditorGUI.EndChangeCheck())
					{
						stm.Rebuild(); //force rebuild
					}
				}else{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("anchor"));
				}
				//if(!uiMode.boolValue){ //restrict this to non-ui only...?
					EditorGUILayout.PropertyField(serializedObject.FindProperty("alignment"));
				//}
				EditorGUILayout.Space(); //////////////////SPACE
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lineSpacing")); //text formatting
				EditorGUILayout.PropertyField(serializedObject.FindProperty("characterSpacing"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("tabSize"));
				EditorGUILayout.Space(); //////////////////SPACE
				if(!stm.uiMode){ //wrapping text works differently for UI:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("autoWrap")); //automatic...
					if(stm.autoWrap > 0f){
						EditorGUILayout.PropertyField(serializedObject.FindProperty("breakText"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("insertHyphens"));
					}
					EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalLimit"));
					if(stm.verticalLimit > 0f){
						EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalLimitMode"));
					}
				}else{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("uiWrap"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("uiLimit"));
					EditorGUILayout.Space(); //////////////////SPACE
					EditorGUILayout.PropertyField(serializedObject.FindProperty("breakText"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("insertHyphens"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalLimitMode"));
				}
			}
			//EditorGUILayout.Space(); //////////////////SPACE
			stm.showTimingFoldout = EditorGUILayout.Foldout(stm.showTimingFoldout, "Timing", foldoutStyle);
			if(stm.showTimingFoldout){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreTimeScale"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("disableAnimatedText"));
				EditorGUILayout.Space(); //////////////////SPACE
				EditorGUILayout.PropertyField(serializedObject.FindProperty("readDelay")); //technical stuff
				if(stm.readDelay > 0f){
					EditorGUILayout.PropertyField(serializedObject.FindProperty("autoRead"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("rememberReadPosition"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("drawOrder"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("drawAnimName"));
					//stuff that needs progamming to work:
					stm.showFunctionalityFoldout = EditorGUILayout.Foldout(stm.showFunctionalityFoldout, "Functionality", foldoutStyle);
					if(stm.showFunctionalityFoldout){
						EditorGUILayout.PropertyField(serializedObject.FindProperty("speedReadScale"));
						EditorGUILayout.Space(); //////////////////SPACE
						EditorGUILayout.PropertyField(serializedObject.FindProperty("unreadDelay"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("undrawOrder"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("undrawAnimName"));
					}
					//GUIContent drawAnimLabel = new GUIContent("Draw Animation", "What draw animation will be used. Can be customized with TextData.");
					//selectedAnim.intValue = EditorGUILayout.Popup("Draw Animation", selectedAnim.intValue, stm.DrawAnimStrings());
					stm.showAudioFoldout = EditorGUILayout.Foldout(stm.showAudioFoldout, "Audio", foldoutStyle);
					if(stm.showAudioFoldout){
						//EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel); //HEADER
						EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSource"));
						if(stm.audioSource != null){ //flag
							EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClips"), true); //yes, show children
							EditorGUILayout.PropertyField(serializedObject.FindProperty("stopPreviousSound"));
							EditorGUILayout.PropertyField(serializedObject.FindProperty("pitchMode"));
							if(stm.pitchMode == SuperTextMesh.PitchMode.Normal){
								//nothing!
							}
							else if(stm.pitchMode == SuperTextMesh.PitchMode.Single){
								EditorGUILayout.PropertyField(serializedObject.FindProperty("overridePitch"));
							}
							else{ //random between two somethings
								EditorGUILayout.PropertyField(serializedObject.FindProperty("minPitch"));
								EditorGUILayout.PropertyField(serializedObject.FindProperty("maxPitch"));
							}
							if(stm.pitchMode == SuperTextMesh.PitchMode.Perlin){
								EditorGUILayout.PropertyField(serializedObject.FindProperty("perlinPitchMulti"));
							}
							if(stm.speedReadScale < 1000f){
								EditorGUILayout.PropertyField(serializedObject.FindProperty("speedReadPitch"));
							}
						}
					}
				}
			}
			stm.showEventFoldout = EditorGUILayout.Foldout(stm.showEventFoldout, "Events", foldoutStyle);
			if(stm.showEventFoldout){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onRebuildEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onPrintEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onCompleteEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onUndrawnEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onVertexMod"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onPreParse"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onCustomEvent"));
			}
			//EditorGUILayout.Space(); //////////////////SPACE
			//EditorGUILayout.PropertyField(debugMode);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif

[HelpURL("Assets/Clavian/SuperTextMesh/Documentation/SuperTextMesh.html")] //make the help open local documentation
[AddComponentMenu("Mesh/Super Text Mesh", 3)] //allow it to be added as a component
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class SuperTextMesh : MonoBehaviour, ILayoutElement { //MaskableGraphic... rip
	//foldout bools for editor. not on the GUI script, cause they get forgotten
	public bool showAppearanceFoldout = true;
	public bool showMaterialFoldout = true;
	public bool showPositionFoldout = true;
	public bool showTimingFoldout = false;
	public bool showFunctionalityFoldout = false;
	public bool showAudioFoldout = false;
	public bool showEventFoldout = false;
	
	#if UNITY_EDITOR
	//Add to the gameobject menu:
	[MenuItem("GameObject/3D Object/Super 3D Text", false, 4000)] //instantiate a prefab of this
	private static void MakeNewText(MenuCommand menuCommand){
	    //Create a game object
	    //GameObject textFab = Resources.Load("STMPrefabs/New Super Text") as GameObject;
		GameObject textFab = (GameObject)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/STMPrefabs/New Super Text.prefab", typeof(GameObject));
	    GameObject newTextMesh = Instantiate(textFab); //instantiate prefab from assets
	    newTextMesh.transform.name = textFab.name; //so it doesn't have "(Clone)" after
		GameObjectUtility.SetParentAndAlign(newTextMesh, menuCommand.context as GameObject); //Ensure it gets reparented if this was a context click (otherwise does nothing)
		Undo.RegisterCreatedObjectUndo(newTextMesh, "Create " + newTextMesh.name); //Register the creation in the undo system
		Selection.activeObject = newTextMesh;
	}
	[MenuItem("GameObject/UI/Super Text", false, 2001)] //instantiate a prefab of this
	private static void MakeNewUIText(MenuCommand menuCommand){
	    //Create a game object
	   	GameObject textFab = (GameObject)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/STMPrefabs/Super Text.prefab", typeof(GameObject));
	    GameObject newTextMesh = Instantiate(textFab); //instantiate prefab from assets
	    newTextMesh.transform.name = textFab.name; //so it doesn't have "(Clone)" after
		GameObjectUtility.SetParentAndAlign(newTextMesh, menuCommand.context as GameObject); //Ensure it gets reparented if this was a context click (otherwise does nothing)
		Undo.RegisterCreatedObjectUndo(newTextMesh, "Create " + newTextMesh.name); //Register the creation in the undo system
		Selection.activeObject = newTextMesh;
		//force-attach to canvas if it exists, or auto-create new one.
		Canvas myCanvas = (Canvas)FindObjectOfType(typeof(Canvas)); //find a canvas in the scene
		if(myCanvas == null){ //create a new canvas to parent to!
			GameObject newObject = new GameObject();
			newObject.transform.name = "Canvas";
			myCanvas = newObject.AddComponent<Canvas>();
			myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			newObject.AddComponent<CanvasScaler>();
			newObject.AddComponent<GraphicRaycaster>();
			Undo.RegisterCreatedObjectUndo(newObject, "Created New Canvas for UI Super Text");
		}
		newTextMesh.transform.SetParent(myCanvas.transform);
	}
	#endif
	
	private static SuperTextMeshData _data; //made this static so it's only loaded once by default
	public SuperTextMeshData data{
		get{
			if(_data == null){
				//sticking with resource folder since it works in builds
				_data = Resources.Load("SuperTextMeshData") as SuperTextMeshData; //load textdata
				//_data = (SuperTextMeshData)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/SuperTextMeshData.asset", typeof(SuperTextMeshData));
				_data.RebuildDictionaries(); //rebuild dictionaries
			}
			return _data;
		}set{
			_data = value; //for setting it to be null so this gets redone!
		}
	}
	private Transform _t;
	public Transform t{
		get{
			if(_t == null) _t = this.transform;
			return _t;
		}
	}
	private MeshFilter _f;
	public MeshFilter f{
		get{
			if(_f == null) _f = t.GetComponent<MeshFilter>();
			if(_f == null) _f = t.gameObject.AddComponent<MeshFilter>();
			return _f;
		}
	}
	private MeshRenderer _r;
	public MeshRenderer r{
		get{
			if(_r == null) _r = t.GetComponent<MeshRenderer>();
			if(_r == null) _r = t.gameObject.AddComponent<MeshRenderer>();
			return _r;
		}
	}
	private CanvasRenderer _c;
	public CanvasRenderer c{
		get{
			if(_c == null) _c = t.GetComponent<CanvasRenderer>();
			if(_c == null) _c = t.gameObject.AddComponent<CanvasRenderer>();
			return _c;
		}
	}
	//public bool uiMode; //is it in UI mode? please don't change this manually
	public bool uiMode{
		get{
			return t is RectTransform;
		}
	}

	public List<STMTextInfo> info = new List<STMTextInfo>(); //switching this out for an array & using temp lists makes less appear in the deep profiler, but has no effect on GC
	private List<int> lineBreaks = new List<int>(); //what characters are line breaks
	private List<float> lineHeights = new List<float>(); //for each line, the size of the tallest character in each
	[TextArea(3,10)] //[Multiline] also works, but i like this better
	[UnityEngine.Serialization.FormerlySerializedAs("text")]
	public string _text = "<c=rainbow><w>Hello, World!";
	public string text{
		get{
			return this._text;
		}
		set{
			this._text = value ?? ""; //never set it to be a null string or this causes an error.
			Rebuild(); //auto-rebuild
		}
	}
	public string Text{ //legacy fix since v1.6
		get{ //just do the same as text
			return this._text;
		}
		set{
			this._text = value ?? ""; //never set it to be a null string or this causes an error.
			Rebuild(); //auto-rebuild
		}
	}
	[HideInInspector] public string drawText; //text, after removing junk
	[HideInInspector] public string hyphenedText; //text, with junk added to it
	[Tooltip("Font to be used by this text mesh. .rtf, .otf, and Unity fonts are supported.")]
	public Font font;
	[Tooltip("Default color of the text mesh. This can be changed with the <c> tag! See the docs for more info.")]
	public Color32 color = Color.white;
	[Tooltip("Will the text listen to tags like <b> and <i>? See docs for a full list of tags.")]
	public bool richText = true; //care about formatting like <b>?
	[Tooltip("Delay in seconds between letters getting read out. Disabled if set to 0.")]
	public float readDelay = 0f; //disabled if 0.
	
	[Tooltip("Multiple of time for when speeding up text. Set it to a big number like 1000 to show all text immediately.")]
	public float speedReadScale = 2f; //for speeding thru text, this will be the delay. set to 0 to display instantly.
	[Tooltip("Whether reading uses deltaTime or fixedDeltaTime")]
	public bool ignoreTimeScale = true;
	public float GetDeltaTime{
		get{
			return data.disableAnimatedText || disableAnimatedText || !applicationFocused ? 0f : ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
		}
	}
	public float GetTime{
		get{
			return data.disableAnimatedText || disableAnimatedText || !applicationFocused ? 0f : ignoreTimeScale ? Time.unscaledTime : Time.time;
		}
	}
	public float GetDeltaTime2{//for when the text is getting read out
		get{ //don't advance if application is not focused
			return !applicationFocused ? 0f : ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
		}
	}
	public bool disableAnimatedText = false; //disable for just this mesh
	/*
	public float GetTime2{ 
		get{
			return ignoreTimeScale ? Time.unscaledTime : Time.time;
		}
	}*/

	//public int selectedAnim = 0; //what draw animation is selected currently.....
	[Tooltip("Name of what draw animation will be used. Case-sensitive.")]
	public string drawAnimName = "Appear"; //this is a string instead of a custom dropdown so reordering saved animations can't change it

	[Tooltip("Delay between letters, for undrawing.")]
	public float unreadDelay = 0.05f;
	[Tooltip("Undraw order.")]
	public DrawOrder undrawOrder = DrawOrder.AllAtOnce;
	[Tooltip("Undraw animation name.")]
	public string undrawAnimName = "Appear";

	[Tooltip("Audio source for read sound clips. Sound won't be played if null.")]
	public AudioSource audioSource;
	[Tooltip("Default sound to be read by the above audio source. Can be left null to make no sound by default.")]
	public AudioClip[] audioClips;
	[Tooltip("Should a new letter's sound stop a previous one and play, or let the old one keep playing?")]
	public bool stopPreviousSound = true;

	[Tooltip("Pitch options for reading out text.")]
	public PitchMode pitchMode = PitchMode.Normal;
	public enum PitchMode{
		Normal,
		Single,
		Random,
		Perlin
	}
	[Tooltip("New pitch for the sound clip.")]
	[Range(0f,3f)]
	public float overridePitch = 1f;
	[Tooltip("Minimum pitch for random pitches. If same or greater than max pitch, this will be the pitch.")]
	[Range(0f,3f)]
	public float minPitch = 0.9f;
	[Tooltip("Maximum pitch for random pitches.")]
	[Range(0f,3f)]
	public float maxPitch = 1.2f;
	[Range(-2f,2f)]
	[Tooltip("This amount will be ADDED to the pitch when speedreading. Speedreading uses the delay from 'Fast Delay'")]
	public float speedReadPitch = 0f;
	[Tooltip("Multiple for how fast the perlin noise will advance.")]
	public float perlinPitchMulti = 1.0f;
	private bool speedReading = false;
	private bool skippingToEnd = false; //alt version of speedread that just skips to the end

	[HideInInspector] public bool reading = false; //is text currently being read out? this is public so it can be used by other scripts!
	private Coroutine readRoutine; //coroutine that handles reading out text
	[HideInInspector] public bool unreading = false;

	[Tooltip("Size in local space for letters, by default. Can be changed with the <s> tag.")]
	public float size = 1f; //size of letter in local space! not percentage of quality. letters can have diff sizes individually
	
	[HideInInspector] public float bestFitMulti = 1f; //for best fit, size will be multiplied by this...
	
	[Range(1,500)]
	[Tooltip("Point size of text. Try to keep it as small as possible while looking crisp!")]
	public int quality = 64; //actual text size. point size
	[Tooltip("Choose 'Point' for a crisp look. You'll probably want that for pixel fonts!")]
	public FilterMode filterMode = FilterMode.Bilinear;
	//TODO: completely redraw text texture upon quality change. 2016-06-07 note: might have already done this
	[Tooltip("Default letter style. Can be changed with the <i> and <b> tags, using rich text.")]
	public FontStyle style = FontStyle.Normal;
	[Tooltip("Additional offset for the text mesh from the transform, in local space.")]
	public Vector3 baseOffset = Vector3.zero; //for offsetting z, mainly
	[Tooltip("Adjust line spacing between multiple lines of text. 1 is the default for the font.")]
	public float lineSpacing = 1.0f;
	[Tooltip("Adjust additional spacing between characters. 0 is default.")]
	public float characterSpacing = 0.0f;
	[Tooltip("How far tabs indent.")]
	public float tabSize = 4.0f;
	[Tooltip("Distance in local space before a line break is automatically inserted at the previous space. Disabled if set to 0.")]
	public float autoWrap = 12f; //if text on one row exceeds this, insert line break at previously available space
	public float AutoWrap{ //get autowrap limit OR ui bounds
		get{
			if(uiMode && uiWrap)
			{
				//LayoutRebuilder.MarkLayoutForRebuild(tr);
				return (float)tr.rect.width; //get wrap limit, within left and right bounds!
				//return preferredWidth;
			}
			else if(uiMode && !uiWrap) return 0f;
			return autoWrap;
		}
	}
	private RectTransform tr{
		get{
			return t as RectTransform;
		}
	}
	[Tooltip("If true, STM will set its bounds based on RectTransform, without need for Content Size Fitter.")]
	public bool uiWrap = true;
	[Tooltip("If true, STM will set its bounds based on RectTransform, without need for Content Size Fitter.")]
	public bool uiLimit = true;

//	[Tooltip("Should text wrap at the edge of the bounding box, or go over?")]
//	public bool wrapText = true; 
	[Tooltip("With auto wrap, should large words be split to fit in the box?")]
	public bool breakText = false;
	[Tooltip("When large words are split, Should a hyphen be inserted?")]
	public bool insertHyphens = true;
	[Tooltip("The anchor point of the mesh. For UI text, this also controls the alignment.")]
	public TextAnchor anchor = TextAnchor.UpperLeft;
	[Tooltip("Decides where text should align to. Uses the Auto Wrap box as bounds.")]
	public Alignment alignment = Alignment.Left;
	public enum Alignment{
		Left,
		Center,
		Right,
		Justified,
		ForceJustified
	}
	[Tooltip("Maximum vertical space for this text box. Infinite if set to 0.")]
	public float verticalLimit = 0f;
	private float VerticalLimit{
		get{
			if(uiMode && uiLimit) return (float)tr.rect.height;
			//if(uiMode && uiLimit) return preferredHeight;
			else if(uiMode && !uiLimit) return 0f; //text isn't being limited
			return verticalLimit;
		}
	}
//	[Tooltip("For UI text, will cut off text if it goes beyond the vertical limit.")]
//	public bool limitText = true;
	public enum VerticalLimitMode{
		ShowLast,
		CutOff,
		Ignore,
		AutoPause,
		AutoPauseFull
	}
	[Tooltip("How to treat text that goes over the vertical limit.")]
	public VerticalLimitMode verticalLimitMode = VerticalLimitMode.Ignore;

	public string leftoverText; //if verticalLimitMode is set to CutOff, this will be the text that got cutoff + all tags preceeding it.

	[Tooltip("The material to be used by this text mesh. This is a Material so settings can be shared between multiple text meshes easily.")]
	[UnityEngine.Serialization.FormerlySerializedAs("textMat")]
	public Material textMaterial; //material to use on all submeshes or whatever by default. will always be textMaterials[0]
	public Mesh textMesh; //keep track of mesh

	private bool areWeAnimating = false; //do we need to update every frame?

//bounds stuff! I don't use the Bounds class since it's easier for me to grab the corners from here, 
//and it'll be easier for devs to use, too
	[HideInInspector] public Vector3 rawTopLeftBounds; //bounds before transform is applied
	[HideInInspector] public Vector3 rawBottomRightBounds;
	[HideInInspector] public Vector3 rawBottomRightTextBounds; //widest & furthest point on all text, unclamped to bounds

	//[HideInInspector] private float minY; //the y position of the last letter, including cut letters

	[HideInInspector] public Vector3 topLeftBounds;
	[HideInInspector] public Vector3 topRightBounds;
	[HideInInspector] public Vector3 bottomLeftBounds;
	[HideInInspector] public Vector3 bottomRightBounds;
	[HideInInspector] public Vector3 centerBounds;

	[HideInInspector] public Vector3 topLeftTextBounds;
	[HideInInspector] public Vector3 topRightTextBounds;
	[HideInInspector] public Vector3 bottomLeftTextBounds;
	[HideInInspector] public Vector3 bottomRightTextBounds;

	[HideInInspector] public Vector3 centerTextBounds;

	[HideInInspector] public Vector3 finalTopLeftTextBounds;
	[HideInInspector] public Vector3 finalTopRightTextBounds;
	[HideInInspector] public Vector3 finalBottomLeftTextBounds;
	[HideInInspector] public Vector3 finalBottomRightTextBounds;

	[HideInInspector] public Vector3 finalCenterTextBounds;

	private float lowestPosition = 0f; //this is the lowest position of text that will be drawn, clamped within verticalLimit

	private float lowestDrawnPosition; //as the mesh reads out, this is the lowest position drawn, unclamped
	private float lowestDrawnPositionRaw; //without offset
	private float furthestDrawnPosition;

	private float totalWidth; //for figuring out preferred width


	public Vector3 unwrappedBottomRightTextBounds;

	public UnityEvent onCompleteEvent; //when the mesh is done drawing
	public delegate void OnCompleteAction();
	public event OnCompleteAction OnCompleteEvent; //matching unityevent

	public UnityEvent onUndrawnEvent; //for when undrawing finishes
	public delegate void OnUndrawnAction();
	public event OnUndrawnAction OnUndrawnEvent;

	public UnityEvent onRebuildEvent; //when rebuild() is called
	public delegate void OnRebuildAction();
	public event OnRebuildAction OnRebuildEvent;

	public UnityEvent onPrintEvent; //whenever a new letter is printed.
	public delegate void OnPrintAction();
	public event OnPrintAction OnPrintEvent;

	[System.Serializable] public class CustomEvent : UnityEvent<string, STMTextInfo>{} //tag, textinfo, 
	[UnityEngine.Serialization.FormerlySerializedAs("customEvent")]
	public CustomEvent onCustomEvent;
	public delegate void OnCustomAction(string text, STMTextInfo info);
	public event OnCustomAction OnCustomEvent;
	
	[System.Serializable] public class VertexMod : UnityEvent<Vector3[], Vector3[], Vector3[]>{}
	[UnityEngine.Serialization.FormerlySerializedAs("vertexMod")]
	public VertexMod onVertexMod;
	public delegate void OnVertexModAction(Vector3[] verts, Vector3[] middles, Vector3[] positions);
	public event OnVertexModAction OnVertexMod;

	[System.Serializable] public class PreParse : UnityEvent<STMTextContainer>{} //will change string before stm uses it
	[UnityEngine.Serialization.FormerlySerializedAs("preParse")]
	public PreParse onPreParse;
	public delegate void OnPreParseAction(STMTextContainer container);
	public event OnPreParseAction OnPreParse;

	public bool debugMode = false; //pretty much just here to un-hide inspector stuff

	[HideInInspector] public float totalReadTime = 0f;
	[HideInInspector] public float totalUnreadTime = 0f;
	[HideInInspector] public float currentReadTime = 0f; //what position in the mesh it's currently at. Right now, this is just so jitters don't animate more than they should when you speed past em.

	//generate these with ur vert calls or w/e!!!
	private Vector3[] endVerts = new Vector3[0];
	private Color32[] endCol32 = new Color32[0];
	//private int[] endTriangles = new int[0];
	private Vector2[] endUv = new Vector2[0];
	private Vector2[] endUv2 = new Vector2[0]; //overlay images
	private List<Vector4> ratiosAndUvMids = new List<Vector4>(); //ratios of each letter, to be embedded into uv3
	//private Vector2[] uvMids = new Vector2[0]; //centre of the UV on this letter, to be embedded into uv4
	private Vector3[] startVerts = new Vector3[0];
	private Color32[] startCol32 = new Color32[0];
	private Vector3[] midVerts = new Vector3[0];
	private Color32[] midCol32 = new Color32[0];

	private List<SubMeshData> subMeshes = new List<SubMeshData>();

	private float timeDrawn; //Time.time when the mesh was drawn. or Time.unscaledTime, depending

	[Tooltip("Decides if the mesh will read out automatically when rebuilt.")]
	public bool autoRead = true;
	[Tooltip("Decides if the mesh will remember where it was if disabled/enabled while reading.")]
	public bool rememberReadPosition = true;

	[Tooltip("For UI text. If true, quality is automatically set to be the same as size.")]
	public bool autoQuality = false;

	public enum DrawOrder{
		LeftToRight,
		AllAtOnce,
		OneWordAtATime,
		Random,
		RightToLeft,
		ReverseLTR
	}
	[Tooltip("What order the text will draw in. 'All At Once' will ignore read delay. 'Robot' displays one word at a time. If set to 'Random', Read Delay becomes the time it'll take to draw the whole mesh.")]
	public DrawOrder drawOrder = DrawOrder.LeftToRight;

	private bool callReadFunction = false; //will the read function need to be called?

	private int pauseCount = 0; //for <pause>. total amount of <pause>s that were reached while reading multiple times
	private int currentPauseCount = 0; //amount of pauses found this rebuild cycle
	private float autoPauseStopPoint = 0f;

	private List<KeyValuePair<int,string>> allTags = new List<KeyValuePair<int,string>>();
	private List<Font> allFonts = new List<Font>(); //a list of all fonts used on this mesh
	//private List<

	[System.Serializable]
	public enum BestFitMode{
		Off,
		Always,
		OverLimit
	}
	public BestFitMode bestFit = BestFitMode.Off;

	//same as info[info.Count-1].pos.y ut cached

	STMDrawAnimData UndrawAnim{
		get{
			if(data.drawAnims.ContainsKey(undrawAnimName)){
				return data.drawAnims[undrawAnimName];
			}else if(data.drawAnims.ContainsKey("Appear")){
				return data.drawAnims["Appear"];
			}else{
				//Debug.Log("'Appear' draw animation isn't defined!"); //sometimes this'll get called on awake... oH
				data = null;
				return null;
			}
		}
	}
/*
	public string[] DrawAnimStrings(){ //get strings for the dropdown thing
		string[] myStrings = new string[data.drawAnims.Count];
		for(int i=0, iL=myStrings.Length; i<iL; i++){
			myStrings[i] = data.drawAnims[i].name;
		}
		if(selectedAnim >= myStrings.Length){
			selectedAnim = 0; //don't go over if one gets deleted
		}
		return myStrings;
	}
*/
	private bool applicationFocused = true;
	void OnApplicationFocus(bool focused){
		//this is needed cause without it, if mesh is ignoring time scale and application comes back into focus, time will skip forward.
		if(!Application.runInBackground){ //only care if the application cares
			applicationFocused = focused;
		}
	}
	void OnDrawGizmosSelected(){ //draw boundsssss
		//if(autoWrap > 0f){ //bother to draw bounds?
			Gizmos.color = data.boundsColor;
			RecalculateBounds();
			Gizmos.DrawLine(topLeftBounds, topRightBounds); //top
			Gizmos.DrawLine(topLeftBounds, bottomLeftBounds); //left
			Gizmos.DrawLine(topRightBounds, bottomRightBounds); //right
			Gizmos.DrawLine(bottomLeftBounds, bottomRightBounds); //bottom

			Gizmos.color = data.textBoundsColor;
			Gizmos.DrawLine(topLeftTextBounds, topRightTextBounds); //top
			Gizmos.DrawLine(topLeftTextBounds, bottomLeftTextBounds); //left
			Gizmos.DrawLine(topRightTextBounds, bottomRightTextBounds); //right
			Gizmos.DrawLine(bottomLeftTextBounds, bottomRightTextBounds); //bottom

			Gizmos.color = data.finalTextBoundsColor;
			Gizmos.DrawLine(finalTopLeftTextBounds, finalTopRightTextBounds); //top
			Gizmos.DrawLine(finalTopLeftTextBounds, finalBottomLeftTextBounds); //left
			Gizmos.DrawLine(finalTopRightTextBounds, finalBottomRightTextBounds); //right
			Gizmos.DrawLine(finalBottomLeftTextBounds, finalBottomRightTextBounds); //bottom
			//Gizmos.DrawSphere(centerBounds, 0.1f);
			//Gizmos.color = Color.yellow; //draw maxes
			//Gizmos.DrawLine(topRightTextBounds, rawBottomRightTextBounds);
			//Gizmos.DrawLine(bottomLeftTextBounds, rawBottomRightTextBounds);
			//Gizmos.DrawLine()
		//}
	}
	void OnFontTextureRebuilt(Font changedFont){
		//if (changedFont != font) //ignore if font doesn't exist on this mesh
        //    return;
        //Rebuild(currentReadTime, currentReadTime > 0f ? true : autoRead); //the font texture attached to this mesh has changed. a rebuild is neccesary.
		if(textMesh != null && info.Count > 0 && allFonts.Contains(changedFont)){ 
			//2018-05-29: only rebuild if this mesh actually contains the font!
			//RebuildTextInfo();
			
			/*
			//efficient, but rarely this causes text to just... not render
			GetCharacterInfoForArray();
			//update mesh
			SetMesh(currentReadTime);
			*/
			Rebuild(currentReadTime, currentReadTime > 0f ? true : autoRead);
		}
	}
	#if UNITY_5_4_OR_NEWER
	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
		if(this != null && t.gameObject.activeInHierarchy){ //stupid
			StartCoroutine(WaitFrameThenRebuild()); //just do it anyway
		/*
			if(loadSceneMode == LoadSceneMode.Single){
				StartCoroutine(WaitFrameThenRebuild());
			}else{
				Rebuild(autoRead); //otherwise, texture goes missing. miiiight not be neccesary in 5.4+?
			}
		*/
		}
	}
	#else
	void OnLevelWasLoaded(int level){
		StartCoroutine(WaitFrameThenRebuild());
		//Rebuild(autoRead); //otherwise, texture goes missing. 
	}
	#endif
	IEnumerator WaitFrameThenRebuild(){
		yield return null;
		Rebuild(currentReadTime, autoRead);
	}
	
	void OnEnable(){
		Init();
		if(Application.isPlaying){ //cause autoread to work
			if(callReadFunction && rememberReadPosition){ //remember read position?
				if(currentReadTime == 0f){ //same as Start()
					Rebuild(autoRead);
				}else if(currentReadTime >= totalReadTime){ //it finished reading?
					SetMesh(currentReadTime); //skip to end w/o events
				}else{ //it was halfway thru
					Rebuild(currentReadTime, autoRead);
				}
			}else{ //act like Start()
				Rebuild(autoRead);
			}
		}
	}
	void Start(){
		textMesh = null; //THIS IS NEEDED TO PREVENT THE DUPLICATION GLITCH
		//Init(); 
		Rebuild(autoRead);
	}
	void OnDisable(){
		//Debug.Log("Disabled!");
		UnInit();
		if(uiMode){
			DestroyImmediate(textMesh);
			c.Clear();
		}else{
			DestroyImmediate(f.sharedMesh);
		}
	}
	void OnDestroy(){
		//UnInit();
	}
	void Init(){
		//uiMode = t is RectTransform;

		#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded += OnSceneLoaded; //hopefully not an issue if called multiple times?
		#endif
		Font.textureRebuilt += OnFontTextureRebuilt;
	}
	void UnInit(){
		#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded -= OnSceneLoaded;
		#endif
		Font.textureRebuilt -= OnFontTextureRebuilt;
		StopReadRoutine();
	}
	void StopReadRoutine(){
		if(readRoutine != null){
			StopCoroutine(readRoutine); //stop routine, just in case
			reading = false;
		}
	}
	/*
	private void AddRebuildCall(){
		if(Font.textureRebuilt == null || !Font.textureRebuilt.GetInvocationList().Contains(OnFontTextureRebuilt)){
			Font.textureRebuilt += OnFontTextureRebuilt;
		}
	}
	private void RemoveRebuildCall(){
		Font.textureRebuilt -= OnFontTextureRebuilt;
	}
	*/
	#if UNITY_EDITOR //just for updating when the editor changes
	private bool validateAppearance = false;
	#endif
	void OnValidate() {
		if(font != null && !font.dynamic){
			if(font.fontSize > 0){
				quality = font.fontSize;
			}else{
				Debug.Log("You're probably using a custom font! \n Unity's got a bug where custom fonts have their size set to 0 by default and there's no way to change that! So to avoid this error, here's a solution: \n * Drag any font into Unity. Set it to be 'Unicode' or 'ASCII' in the inspector, depending on the characters you want your font to have. \n * Set 'Font Size' to whatever size you want 'quality' to be locked at. \n * Click the gear in the corner of the inspector and 'Create Editable Copy'. \n * Now, under the array of 'Character Rects', change size to 0 to clear everything. \n * Now you have a brand new font to edit that has a font size that's not zero! Yeah!");
			}
			//quality = UseThisFont.fontSize == 0 ? 64 : UseThisFont.fontSize; //for getting around fonts with a default size of 0.
			//Debug.Log("Font size is..." + UseThisFont.fontSize);
			style = FontStyle.Normal;
		}
		//apply automatic quality
		if(autoQuality)
		{
			quality = (int)Mathf.Ceil(size);
		}
		if(size < 0f){size = 0f;}
		if(readDelay < 0f){readDelay = 0f;}
		if(autoWrap < 0f){autoWrap = 0f;}
		if(verticalLimit < 0f){verticalLimit = 0f;}
		if(minPitch > maxPitch){minPitch = maxPitch;}
		if(maxPitch < minPitch){maxPitch = minPitch;}
		if(speedReadScale < 0.01f){speedReadScale = 0.01f;}
		#if UNITY_EDITOR
		validateAppearance = true;
		#endif
	}
	#if UNITY_EDITOR
	public void HideInspectorStuff(){
		HideFlags flag = HideFlags.HideInInspector;
		switch(debugMode){
			case true: flag = HideFlags.None; break;//don't hide!
		}
		if(uiMode){
			for(int i=0, iL=c.materialCount; i<iL; i++){
				if(c.GetMaterial(i) != null){
					c.GetMaterial(i).hideFlags = flag;
				}
			}
			c.hideFlags = flag;
		}else{
			for(int i=0, iL=r.sharedMaterials.Length; i<iL; i++){ //hide shared materials
				if(r.sharedMaterials[i] != null){
					r.sharedMaterials[i].hideFlags = flag;
				}
			}
			r.hideFlags = flag; //hide mesh renderer and filter.
			f.hideFlags = flag;
		}
	}
	#endif

	public void InitializeFont(){
		if(uiMode){ //UI mode
			if(font == null && textMaterial == null){
				size = 32;
				color = new Color32(50,50,50,255);
			}
			if(textMaterial == null){
				//textMaterial = (Material)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/DefaultSTMMaterials/UIDefault.mat", typeof(Material));
				textMaterial = Resources.Load<Material>("DefaultSTMMaterials/UIDefault");
				//sticking with resource folder since it works in builds
			}
			if(font == null){
				if(data.defaultFont != null)
				{
					font = data.defaultFont;
				}
				else
				{
					font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
			}
		}else{
			if(font == null){
				if(data.defaultFont != null)
				{
					font = data.defaultFont;
				}
				else
				{
					font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
			}
			if(textMaterial == null){
				//sticking w/ resource folder since it works in builds
				textMaterial = Resources.Load<Material>("DefaultSTMMaterials/Default");
				//textMaterial = (Material)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/DefaultSTMMaterials/Default.mat", typeof(Material));
			}
		}
	}
	public static void RebuildAll(){ //this uses FindObjectsOfType and is very intensive! Only use when loading.
		SuperTextMesh[] allSTMs = FindObjectsOfType<SuperTextMesh>();
		for(int i=0, iL=allSTMs.Length; i<iL; i++){
			allSTMs[i].Rebuild();
		}
	}
	public void Rebuild(){
		Rebuild(0f, true);
	}
	public void Rebuild(bool readAutomatically){
		Rebuild(0f, readAutomatically);
	}
	public void Rebuild(float startTime){
		Rebuild(startTime, true);
	}
	public void Rebuild(float startTime, bool readAutomatically){
		if(uiMode) Canvas.ForceUpdateCanvases(); //so that everything gets set correctly on awake/start
		if(onRebuildEvent != null && onRebuildEvent.GetPersistentEventCount() > 0) onRebuildEvent.Invoke(); //is it better to just check for null???
		if(OnRebuildEvent != null) OnRebuildEvent();
		if(startTime < totalReadTime){
			pauseCount = 0; //only reset if reeading from the very start, not appending
		} 
		autoPauseStopPoint = -VerticalLimit;
		currentPauseCount = 0;
		timeDrawn = GetTime - startTime < 0f ? 0f : startTime; //remember what time it started! (or would have started)
		currentReadTime = startTime;
		totalReadTime = 0f;
		reading = false;
		unreading = false; //reset this, incase it was fading out
		speedReading = false; //2016-06-09 thank u drak
		skippingToEnd = false;
		//Initialize:
		InitializeFont();
		RebuildTextInfo();
		if(audioSource != null){//initialize an audio source, if there's one. these settings are needed to behave properly
			audioSource.loop = false;
			audioSource.playOnAwake = false;
		}
		if(callReadFunction && Application.isPlaying){
			if(readAutomatically){
				//Debug.Log("Autoreading");
				Read(startTime);
			}else{
				StopReadRoutine();
				//Debug.Log("Not autoreading");
				SetMesh(0f); //show nothing
			}
		}else{
			//Debug.Log("No need to read");
			StopReadRoutine();
			//SetMesh(-1f); //skip to end
			ShowAllText();
		}
		ApplyMaterials();
		if(uiMode) LayoutRebuilder.MarkLayoutForRebuild(tr);
	}
	void Update() {
		#if UNITY_EDITOR
        if(!Application.isPlaying){ //do same thing as onvalidate
            Rebuild(autoRead); //doing it this way avoids the material getting lost when duplicating
        }
		if(validateAppearance && t.gameObject.activeInHierarchy == true && Application.isPlaying){ 
			Rebuild(autoRead);
			validateAppearance = false;
		}
		#endif
 
		if(font != null && textMaterial != null && textMesh != null){ //TODO: make this only get called if something changed, or it's animating
			//RequestAllCharacters(); //keep characters on font texture 
			//v1.4.2: not sure if neccesary, thanks to OnFontTextureRebuilt()?
			//but I'm not sure. it does seem to use a bit of CPU for more meshes, though
			if(!reading && areWeAnimating && currentReadTime >= totalReadTime){
				currentReadTime += GetDeltaTime; //keep updating this, for the jitters
			}
			if(!reading && !unreading && areWeAnimating && (readDelay == 0f || currentReadTime >= totalReadTime)){ //if the mesh needs to keep updating after it's been read out
				//if(currentReadTime >= totalReadTime){ //as long as it's set to auto read, or the current read time is above total read time
					//Debug.Log(currentReadTime + "/" + totalReadTime);
				SetMesh(-1f);
				//}
			}
		}
	}
	void UpdatePreReadMesh(bool undrawingMesh){ //update data needed for pre-existing mesh
		UpdateMesh(0f);

        int arraySize = hyphenedText.Length * 4;

        if (startCol32.Length != arraySize)
            Array.Resize(ref startCol32, arraySize);

        if (startVerts.Length != arraySize)
            Array.Resize(ref startVerts, arraySize);

		STMDrawAnimData myUndrawAnim = UndrawAnim; //just in case...
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			STMDrawAnimData myAnimData = undrawingMesh ? myUndrawAnim : info[i].drawAnimData; //which animation data to use?

			if(info[i].drawAnimData.startColor != Color.clear){ //use a specific start color
				startCol32[4*i + 0] = myAnimData.startColor;
				startCol32[4*i + 1] = myAnimData.startColor;
				startCol32[4*i + 2] = myAnimData.startColor;
				startCol32[4*i + 3] = myAnimData.startColor;
			}else{ //same color but transparent, for better lerping
				startCol32[4*i + 0] = new Color32(endCol32[4*i + 0].r,endCol32[4*i + 0].g,endCol32[4*i + 0].b,0);
				startCol32[4*i + 1] = new Color32(endCol32[4*i + 1].r,endCol32[4*i + 1].g,endCol32[4*i + 1].b,0);
				startCol32[4*i + 2] = new Color32(endCol32[4*i + 2].r,endCol32[4*i + 2].g,endCol32[4*i + 2].b,0);
				startCol32[4*i + 3] = new Color32(endCol32[4*i + 3].r,endCol32[4*i + 3].g,endCol32[4*i + 3].b,0);
			}
			Vector3 middle = new Vector3((endVerts[4*i + 0].x + endVerts[4*i + 1].x + endVerts[4*i + 2].x + endVerts[4*i + 3].x) * 0.25f,
														(endVerts[4*i + 0].y + endVerts[4*i + 1].y + endVerts[4*i + 2].y + endVerts[4*i + 3].y) * 0.25f,
														(endVerts[4*i + 0].z + endVerts[4*i + 1].z + endVerts[4*i + 2].z + endVerts[4*i + 3].z) * 0.25f);
			
			startVerts[4*i + 0] = Vector3.Scale((endVerts[4*i + 0] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
			startVerts[4*i + 1] = Vector3.Scale((endVerts[4*i + 1] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
			startVerts[4*i + 2] = Vector3.Scale((endVerts[4*i + 2] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
			startVerts[4*i + 3] = Vector3.Scale((endVerts[4*i + 3] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * info[i].size);
		}
	}
	public void Read(){
		Read(0f);
	}
	public void Read(float startTime){
		StopReadRoutine();
		readRoutine = StartCoroutine(ReadOutText(startTime));
		//now we have a mesh with nothing on it!
	}
	//cause I keep accidentally typing this? I think this name is better, might swap this in the future
	public void Unread(){UnRead();}
	public void Undraw(){UnRead();}
	public void UnDraw(){UnRead();}
	public void UnRead(){
		//Mesh finalMesh = ShowAllText(); //this is working
		readRoutine = StartCoroutine(UnReadOutText());
	}
	public void SpeedRead(){
		if(reading){
			speedReading = true;
		}
	}
	public void SkipToEnd(){
		if(reading){
			skippingToEnd = true;
		}
	}
	public void RegularRead(){ //return to normal reading speed
		speedReading = false;
	}
	public void ShowAllText(){
		ShowAllText(false); //actually show all text
	}
	private bool wasReadingBefore = false;
	private void ShowAllText(bool unreadingMesh){
		speedReading = false;
		if(unreadingMesh)
		{
			unreading = false; //set to false for the SetMesh() call
			if(currentUnReadTime < totalUnreadTime) currentUnReadTime = totalUnreadTime;
		}
		else
		{
			if(currentReadTime < totalReadTime) currentReadTime = totalReadTime; //this if statement fixes animatefromtimedrawn waves from animating a second time upon OnFontTextureRebuilt
		}
		
		//furthestDrawnPosition = rawBottomRightTextBounds.x;
		//lowestLine = lineBreaks.Count-1;
		wasReadingBefore = reading;
		//make sure reading is true before this is called or else it will read every event
		SetMesh(unreadingMesh ? totalUnreadTime : totalReadTime, unreadingMesh);
		StopReadRoutine();
		//Invoke complete events:
		if(!unreadingMesh){
			
			//if(leftoverText.Length > 0f){ //set leftover text, if any
			//	Debug.Log(leftoverText);
			//}
			if(wasReadingBefore)
			{
				if(onCompleteEvent != null) onCompleteEvent.Invoke();
				if(OnCompleteEvent != null) OnCompleteEvent();
			}
		}else{
			//unreading = false; //nope! Gotta stay in this state til it gets drawn again
			unreading = true;
			if(onUndrawnEvent != null) onUndrawnEvent.Invoke();
			if(OnUndrawnEvent != null) OnUndrawnEvent();
		}
	}
	public void Append(string newText){
		_text += newText;
		Rebuild(totalReadTime, true); //start reading from this point
	}
	public bool Continue(){ //continue reading after being paused. returns true if more text needs to be read
		//goes one extra so that true/false is returned properly
		if(currentPauseCount > pauseCount){ //still need to continue?
			pauseCount++;
			Rebuild(totalReadTime, true);
			return true;
		}
		return false;
	}

	//int lastNum = -1; //the last index to be invoked on the previous cycle, so sounds can't play twice for the same letter!
	//List<int> alreadyInvoked = new List<int>(); //list on indexes that have already been invoked so events cant happen twice
	public int latestNumber = -1; //declare here as a public variable, so the current character drawn can be reached at any time
	void UpdateDrawnMesh(float myTime, bool undrawingMesh){
		UpdateMesh(myTime);
		UpdatePreReadMesh(undrawingMesh);
		//TODO: ^^^ all this stuff, you only have to call again if areweanimating is true.

		STMDrawAnimData myUndrawAnim = UndrawAnim; //get the undraw animation, locally
		//get modified drawnMesh!
		midVerts = new Vector3[hyphenedText.Length * 4];
		midCol32 = new Color32[hyphenedText.Length * 4];

		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each point
			//lerp between start and end
			//Debug.Log((myTime - info[i].readTime) / info[i].animTime);
			STMDrawAnimData myAnimData = undrawingMesh ? myUndrawAnim : info[i].drawAnimData; //which anim to use
			float myReadTime = undrawingMesh ? info[i].unreadTime : info[i].readTime; //what timing to use
			//animate properly! (is there a way to do this by manipulating anim time?? yeah probably tbh)
			float divideAnimAmount = myAnimData.animTime == 0f ? 0.0000001f : myAnimData.animTime; //so it doesn't get NaN'd
			float divideFadeAmount = myAnimData.fadeTime == 0f ? 0.0000001f : myAnimData.fadeTime; //how long fading will last...
			float myAnimPos = (myTime - myReadTime) / divideAnimAmount; // on a range between 0-1 on the curve, the position of the animation
			float myFadePos = (myTime - myReadTime) / divideFadeAmount;

			if(undrawingMesh){ //flip the range! so it lerps backwards
				myAnimPos = 1f - myAnimPos;
				if(myAnimData.fadeTime == 0f)
				{
					myFadePos = 1f;
				}
				else
				{
					myFadePos = 1f - myFadePos;
				}
			}
			/*
			It'd be more efficient to update only vertices that need to be updated.
			However... this isn't done, because it looks bad when a mesh gets laggier as it goes on.
			*/
			midVerts[4*i+0] = LerpWithoutClamp(startVerts[4*i+0],endVerts[4*i+0],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+1] = LerpWithoutClamp(startVerts[4*i+1],endVerts[4*i+1],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+2] = LerpWithoutClamp(startVerts[4*i+2],endVerts[4*i+2],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+3] = LerpWithoutClamp(startVerts[4*i+3],endVerts[4*i+3],myAnimData.animCurve.Evaluate(myAnimPos));

			midCol32[4*i+0] = Color.Lerp(startCol32[4*i+0],endCol32[4*i+0],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+1] = Color.Lerp(startCol32[4*i+1],endCol32[4*i+1],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+2] = Color.Lerp(startCol32[4*i+2],endCol32[4*i+2],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+3] = Color.Lerp(startCol32[4*i+3],endCol32[4*i+3],myAnimData.fadeCurve.Evaluate(myFadePos));
		}
		//ShouldPlaySound(undrawingMesh);
	}
	/*
	void ShouldPlaySound(bool undrawingMesh){
		//int alreadyInvokedCount = alreadyInvoked.Count;
		if(!undrawingMesh){ //dont play sounds if undrawing...
			if(latestNumber != lastNum){ //new number?
				PlaySound(latestNumber); //only play one sound, from the most recent number
				lastNum = latestNumber;
			}
		}else{
			lastNum = -1;
		}
	}
	*/
	Vector3 LerpWithoutClamp(Vector3 A, Vector3 B, float t)
	{
		return A + (B-A)*t;
	}
	bool AreColorsTheSame(Color32 col1, Color32 col2){
		if(col1.r == col2.r && col1.g == col2.g && col1.b == col2.b && col1.a == col2.a){
			return true;
		}
		return false;
	}
	IEnumerator ReadOutText(float startTime){
		//Lerp certain vertices betwwen startmesh and endmesh
		//like, the mesh made by CreatePreReadMesh() and CreateMesh().
		reading = true;
		//float timer = startTime;
		currentReadTime = startTime;
		if(startTime.Equals(0f)){ //for append()
//			alreadyInvoked.Clear();
			//lastNum = -1;
			latestNumber = -1; //reset to -1, not 0. This would stop first character from making a sound/playing an event (2018-05-29)
			lowestDrawnPosition = 0f;
			lowestDrawnPositionRaw = 0f;
			furthestDrawnPosition = 0f;
			//currentMaxX = 0f;
			//currentMinY = 0f;
			//lowestLine = 0;
		}/*
		else if(startTime.Equals(-1f)){
			DoEvent(0);	//so 1st event doesn't get skipped
			2018-05-29 this shouldn't be a problem anymore because of the above
		}
		*/
		//Debug.Log("Total read time is: " + totalReadTime);
		while(currentReadTime < totalReadTime){ //check for null incase the mesh gets deleted midway
			//Debug.Log("Doing this while loop!" + timer);
			if(skippingToEnd){
				//timeDrawn -= totalReadTime; //why not (solves jitters not catching up)
				//timer = totalReadTime;
				currentReadTime = totalReadTime;
			}
			SetMesh(currentReadTime);
			
			float delta = GetDeltaTime2;
			delta *= speedReading ? speedReadScale : 1f;
			//timer += delta;
			//currentReadTime = timer; //I could just use this as the timer, but w/e
			currentReadTime += delta;
			yield return null;
		}
		if(latestNumber != hyphenedText.Length-1){ //catch missed events
			PlaySound(hyphenedText.Length-1); //play final sound! Yep, this seems to fix that 2016-10-13
			DoEvent(hyphenedText.Length-1); //2016-11-02 but only if it hasn't been played yet
		}
		ShowAllText(); //just in case!
	}

	public float currentUnReadTime = 0f;
	IEnumerator UnReadOutText(){
		unreading = true;
		currentUnReadTime = 0f; //always start at 0
		while(currentUnReadTime < totalUnreadTime){ //check for null incase the mesh gets deleted midway
			SetMesh(currentUnReadTime, true);
			currentUnReadTime += GetDeltaTime2;
			yield return null;
		}
		ShowAllText(true);
	}
	/*
	bool IsThisLetterAnimating(int i){ //return true if this letter is animating in some way, not related to drawanim
		if(info[i].waveData != null || info[i].jitterData != null &&
			(info[i].gradientData != null && info[i].gradientData.scrollSpeed != 0) ||
			(info[i].textureData != null && info[i].textureData.scrollSpeed != Vector2.zero)){
			return true;
		}
		return false;
	}
	*/
	void DoEvent(int i){
		if(info[i].ev.Count > 0){ //invoke events...
			for(int j=0, jL=info[i].ev.Count; j<jL; j++){
				if(onCustomEvent != null) onCustomEvent.Invoke(info[i].ev[j], info[i]); //call the event!
				if(OnCustomEvent != null) OnCustomEvent.Invoke(info[i].ev[j], info[i]);
			}
			info[i].ev.Clear(); //since you only want events to be invoked once, I don't see the harm in clearing them this way instead of keeping track
		}
		if(info[i].ev2.Count > 0){ //end repeating events!
			for(int j=0, jL=info[i].ev2.Count; j<jL; j++){
				if(onCustomEvent != null) onCustomEvent.Invoke(info[i].ev2[j], info[i]); //call the event!
				if(OnCustomEvent != null) OnCustomEvent.Invoke(info[i].ev2[j], info[i]);
			}
			info[i].ev2.Clear();
		}
	}
	/*
	char NameToSpecialKey(string name){ //for getting specific autoclips. for things that cant be used in file names
		switch(name.ToLower()){ //also for autodelays
			case "space": return ' ';
			case "tab": return '\t';
			case "line break": case "linebreak": return '\n';
			case "exclamation": case "exclamationpoint": case "exclamation point": return '!';
			case "question": case "questionmark": case "question mark": return '?';
			case "semicolon": return ';';
			case "colon": return ':';
			case "tilde": return '~';
			case "period": return '.';
			case "comma": return ',';
			case "number sign": case "hashtag": return '#';
			case "percent": case "percentsign": case "percent sign": return '%';
			case "ampersand": return '&';
			case "asterix": return '*';
			case "backslash": return '\\';
			case "openbrace": case "open brace": return '{';
			case "closebrace": case "close brace": return '}';
			default: return name[0];
		}
	}
	*/
	string SpecialKeyToName(char ch){ //since you can't put these in filenames
		switch(char.ToLower(ch)){
			case ' ': return "space";
			case '\t': return "tab";
			case '\n': return "line break";
			case '!': return "exclamation point";
			case '?': return "question mark";
			case ';': return "semicolon";
			case ':': return "colon";
			case '~': return "tilde";
			case '.': return "period";
			case ',': return "comma";
			case '#': return "number sign";
			case '%': return "percent";
			case '&': return "ampersand";
			case '*': return "asterix";
			case '\\': return "backslash";
			case '/': return "forwardslash";
			case '{': return "openbrace";
			case '}': return "closebrace";
			default: return new string(ch,1).ToLower(); //always ignore case
		}
	}
	void PlaySound(int i){
		if(audioSource != null){//audio stuff
			if(info[i].stopPreviousSound || !audioSource.isPlaying){
				audioSource.Stop();
				string nameToSearch = info[i].isQuad ? info[i].quadData.name : SpecialKeyToName(hyphenedText[i]);
				AudioClip mySoundClip = null;
				if(info[i].soundClipData != null){
					STMSoundClipData.AutoClip tmpAutoClip = info[i].soundClipData.clips.Find(x => x.name.ToLower() == nameToSearch); //find auto clip
					if(tmpAutoClip != null){
						mySoundClip = tmpAutoClip.clip;
					}
				}
				STMAutoClipData myAutoClip = null;
				if(data.autoClips.ContainsKey(nameToSearch.ToUpper())){ //case
					myAutoClip = data.autoClips[nameToSearch.ToUpper()];
				}else if(data.autoClips.ContainsKey(nameToSearch)){
					myAutoClip = data.autoClips[nameToSearch];
				}

				if(mySoundClip != null){ //use the one attached to this character
					audioSource.clip = mySoundClip;
				}else if(myAutoClip != null){ //if there's a sound clip for this character in Text Data
					audioSource.clip = myAutoClip.clip;
				}else if(info[i].audioClipData != null){ //use sounds attached to character
					audioSource.clip = info[i].audioClipData.clips.Length > 0 ? info[i].audioClipData.clips[UnityEngine.Random.Range(0,info[i].audioClipData.clips.Length)] : null;
				}else if(audioClips.Length > 0){ //use a random audio clip
					audioSource.clip = audioClips.Length > 0 ? audioClips[UnityEngine.Random.Range(0,audioClips.Length)] : null; //get one of the clips
				}else{
					audioSource.clip = null;
				}
				if(audioSource.clip != null){
					switch(info[i].pitchMode){
						case PitchMode.Perlin:
							audioSource.pitch = (Mathf.PerlinNoise(GetTime * perlinPitchMulti, 0f) * (info[i].maxPitch - info[i].minPitch)) + info[i].minPitch; //perlin noise
							break;
						case PitchMode.Random:
							audioSource.pitch = UnityEngine.Random.Range(info[i].minPitch,info[i].maxPitch);
							break;
						case PitchMode.Single:
							audioSource.pitch = info[i].overridePitch;
							break;
						default:
							audioSource.pitch = 1f; //because of speedread pitch
							break;
					}
					if(speedReading){
						audioSource.pitch += info[i].speedReadPitch;
					}
					audioSource.Play();
				}
			}
		}
	}
	//TODO: 2016-06-11 actually, im guessing that these values are a bitmask? you could prob just add & subtract em! but w/e
	FontStyle AddStyle(FontStyle original, FontStyle newStyle){
		if(font.dynamic){
			switch(original){
				case FontStyle.Bold:
					switch(newStyle){
						case FontStyle.Italic:
							return FontStyle.BoldAndItalic;
						default:
							return original;
					}
				case FontStyle.Italic:
					switch(newStyle){
						case FontStyle.Bold:
							return FontStyle.BoldAndItalic;
						default:
							return original;
					}
				case FontStyle.BoldAndItalic:
					return original;
				default: //normal text
					return newStyle;
			}	
		}else{
			return FontStyle.Normal; //non-dynamic fonts can't handle bold/italic
		}
	}
	FontStyle SubtractStyle(FontStyle original, FontStyle subStyle){ //only bold and italic can be added and removed
		if(font.dynamic){
			switch(original){
				case FontStyle.Bold:
					switch(subStyle){
						case FontStyle.Bold:
							return FontStyle.Normal;
						default:
							return original;
					}
				case FontStyle.Italic:
					switch(subStyle){
						case FontStyle.Italic:
							return FontStyle.Normal;
						default:
							return original;
					}
				case FontStyle.BoldAndItalic:
					switch(subStyle){
						case FontStyle.Bold:
							return FontStyle.Italic;
						case FontStyle.Italic:
							return FontStyle.Bold;
						default:
							return original; //just in case?
					}
				default: //normal
					return FontStyle.Normal;
			}
		}else{
			return FontStyle.Normal; //non-dynamic fonts can't handle bold/italic
		}
	}
	bool ValidHexcode (string hex){ //check if a hex code contains the right amount of characters, and allowed characters
		if(hex.Substring(0,1) == "#"){
			hex = hex.Substring(1,hex.Length-1); //trim off #
		}
		if(hex.Length != 3 && hex.Length != 4 && hex.Length != 6 && hex.Length != 8){ //hex code, alpha hex
			return false;
		}
		string allowedLetters = "0123456789ABCDEFabcdef";
		for(int i=0; i<hex.Length; i++){
			if(!allowedLetters.Contains(hex[i].ToString())){
				return false; //invalid string!!!
			}
		}
		return true;
	}
	Color32 HexToColor(string hex){ //convert a hex code string to a color
		if(hex.Substring(0,1) == "#"){
			hex = hex.Substring(1,hex.Length-1); //trim off #
		}
		if(hex.Length == 8){ //RGBA (FF00FF00)
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			byte a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,a);
		}
		if(hex.Length == 4){ //single-byte for RGBA (F0F0)
			byte r = byte.Parse(hex.Substring(0,1) + hex.Substring(0,1), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(1,1) + hex.Substring(1,1), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(2,1) + hex.Substring(2,1), System.Globalization.NumberStyles.HexNumber);
			byte a = byte.Parse(hex.Substring(3,1) + hex.Substring(3,1), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,a);
		}
		if(hex.Length == 3){ //single-byte for RGB (F0F)
			byte r = byte.Parse(hex.Substring(0,1) + hex.Substring(0,1), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(1,1) + hex.Substring(1,1), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(2,1) + hex.Substring(2,1), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,255);
		}
		else{ //RGB (FF00FF)
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,255);
		}
	}
	STMColorData GetColor(string myCol){
		if(data.colors.ContainsKey(myCol)){ //check textdata for a named color
			return data.colors[myCol];
		}
		if(ValidHexcode(myCol)){ //might be a hexcode?
			STMColorData thisCol2 = ScriptableObject.CreateInstance<STMColorData>();
			thisCol2.color = HexToColor(myCol);
			return thisCol2;
		}
		//still no?
		STMColorData thisCol = ScriptableObject.CreateInstance<STMColorData>();
		switch(myCol){ //see if it's a default unity color
			case "red": thisCol.color = Color.red; break;
			case "green": thisCol.color = Color.green; break;
			case "blue": thisCol.color = Color.blue; break;
			case "yellow": thisCol.color = Color.yellow; break;
			case "cyan": thisCol.color = Color.cyan; break;
			case "magenta": thisCol.color = Color.magenta; break;
			case "grey": thisCol.color = Color.grey; break;
			case "gray": thisCol.color = Color.gray; break;
			case "black": thisCol.color = Color.black; break;
			case "clear": thisCol.color = Color.clear; break;
			case "white": thisCol.color = Color.white; break;
			default: thisCol.color = color; break; //default color of mesh
		}
		return thisCol;
	}
	public string preParsedText = "";
	string ParseText(string myText)
    {
        //return a cleaned up string and update textinfo!
		info.Clear();
		preParsedText = myText; //save it anyway
		if((onPreParse != null && onPreParse.GetPersistentEventCount() > 0) || OnPreParse != null)
        {
			STMTextContainer tempContainer = new STMTextContainer(myText);
			if(onPreParse != null) onPreParse.Invoke(tempContainer);
			if(OnPreParse != null) OnPreParse.Invoke(tempContainer);
			myText = tempContainer.text;
			preParsedText = tempContainer.text; //remember this state for later
		}

		//set defaults:
		STMTextInfo myInfo = new STMTextInfo(this); //info for this one character, carried over from last
		allTags.Clear();
		int deletedChars = 0; //for figuring out rawindex
		string insertAfter = "";
		for(int i=0; i<myText.Length; i++)
        { //for each character to parse thru,
			if(info.Count == i && i > 0)
            { //no other delay yet...? /hasnt checkedAgain yet
				//if a quad got put in last time...
				if(info[i-1].isQuad)
				{
					//no delay data set yet and quad name is a registered autodelay?
					if(data.autoDelays.ContainsKey(info[i-1].quadData.name))
					{
						//put delay on next char
						myInfo.delayData = data.autoDelays[info[i-1].quadData.name];
					}
				}
				else if(data.autoDelays.ContainsKey(SpecialKeyToName(myText[i-1])) && (myText[i] == ' ' || myText[i] == '\n' || myText[i] == '\t')) //only if next character is "free". So strings like "Oh......... okay." only see the last delay on periods!
				{ 
					myInfo.delayData = data.autoDelays[SpecialKeyToName(myText[i-1])];
				}
			}
			bool checkAgain = false;
			if(richText && myText[i] == '<'){ //check for count so a pointless debug doesn't appear on rebuild
				int closingIndex = myText.IndexOf(">",i); 
				int equalsIndex = closingIndex > -1 ? myText.IndexOf("=",i, closingIndex-i) : -1; //only look forward for a specific amount of characters
				//Get either closing index or squals index, depending on the kinda tag:
				int endIndex = (equalsIndex > -1 && closingIndex > -1) ? Mathf.Min(equalsIndex,closingIndex) : closingIndex;//for figuring out what the "tag" is
				if(closingIndex != -1){//don't do anything if there's no closing tag at all
					string myTag = myText.Substring(i, endIndex-i+1); //this is the "TAG" like "<c=" or "<br>"
					//only if there's for sure a string TO get
					//Debug.Log("Index is " + i + " equals index is " + equalsIndex + " closing index is " + closingIndex);
					string myString = equalsIndex > -1 ? myText.Substring(equalsIndex+1,closingIndex-equalsIndex-1) : "";//this is the "STRING" like "fire" or "blue"
					//Debug.Log("Found this tag: '" + myTag + "'! And this string: '" + myString + "'.");
					bool clearAfter = true;
					bool exitLoopAfter = false;
					insertAfter = ""; //reset
					switch(myTag){
					//Line Break
						case "<br>":
							insertAfter = '\n'.ToString(); //insert a line break
							break;
					//Color
						case "<c=":
							myInfo.colorData = null; //clear to default
							myInfo.gradientData = null;
							myInfo.textureData = null;

							if(data.textures.ContainsKey(myString)){// is this a texture?
								myInfo.textureData = data.textures[myString];
								//AddMaterial(myInfo.) add material here
							}else{
								//is it a custom color tag?
								if(data.gradients.ContainsKey(myString)){ //is this a gradient?
									myInfo.gradientData = data.gradients[myString];
								}
								else{ //no? try for HEX code & default color
									myInfo.colorData = GetColor(myString);
								}
							}
							break;
						case "</c>":
							myInfo.colorData = null; //clear to default
							myInfo.gradientData = null;
							myInfo.textureData = null;
							break;
					//Size
						case "<s=":
							float thisSize;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSize)){ //parse as a float
								myInfo.size = thisSize * size; //set size relative to the one set in inspector!
							}
							break;
						case "<size=":
							float thisSize2;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSize2)){ //parse as a float
								myInfo.size = thisSize2; //set size directly!
							}
							break;
						case "</s>":
						case "</size>":
							myInfo.size = size;
							break;
					//Delay
						case "<d=":
							if(data.delays.ContainsKey(myString)){ //is there a delay defined in textdata?
								myInfo.delayData = data.delays[myString];//NOTE: delays get overridden, not added
							}else{ //see if it's an integer
								int thisDelay2;
								if(int.TryParse(myString, out thisDelay2)){ //parse as an int
									myInfo.delayData = ScriptableObject.CreateInstance<STMDelayData>(); //create new delay data
									myInfo.delayData.count = thisDelay2;
								}
							}
							break;
						case "<d>":
							if(data.delays.ContainsKey("default")){ //is there a delay defined?
								myInfo.delayData = data.delays["default"];
							}else{
								Debug.Log("Default delay isn't defined!");
							}
							break;
					//Timing
						case "<t=":
							float thisTiming;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisTiming)){ //parse as a float
								if(thisTiming < 0f) thisTiming = 0f; //or else it'll cause a loop!
								myInfo.readTime = thisTiming; //set time to be this float
							}
							break;
					//Event
						case "<e=":
							myInfo.ev.Add(myString); //remember the event!
							break;
					//Repeating Event
						case "<e2=":
							myInfo.ev2.Add(myString); //remember the event!
							break;
						case "</e>":
						case "</e2>":
							myInfo.ev2.Clear(); //forget all. Kinda janky, but whatever. It'll do for now!
							break;
					//Voice
						case "<v=":
							if(data.voices.ContainsKey(myString)){
								insertAfter = data.voices[myString].text; //add the text to the string!
							}
							break;
						case "</v>":
							//myInfo.voiceData = null; //forget it!
							//return everything to default.
							myInfo = new STMTextInfo(this);
							break;
					//Font
						case "<f=": //this switches the font for the whole mesh, but I might as well include it
						case "<font=":
							if(data.fonts.ContainsKey(myString)){
								//useFont = data.fonts[myString].font; //remember the font in this wayyy
								myInfo.fontData = data.fonts[myString];
							}else{
								Debug.Log("Unknown font: '" + myString + "'. Fonts can be defined within the Text Data Inspector and are case-sensitive.");
							}
							break; //switching to a non-dynamic font can return some errors
						case "</f>":
						case "</font>":
							//useFont = null; //forget it!
							myInfo.fontData = null;
							break;
					//Quad
						case "<q=":
						case "<quad=":
							string[] myDividedString = myString.Split(','); //divide it up at commas
							//if quad exists and if this letter doesn't already have quad data:
							if(data.quads.ContainsKey(myDividedString[0]) && myInfo.quadData == null){
								if(myDividedString.Length == 1){
									// normal quad
									myInfo.quadData = data.quads[myString];
									myInfo.isQuad = true; //just assign this manually to save eveyone's time
									insertAfter = "\u2000"; //a character to get used for the quad, in hyphenedtext
								}else if(myDividedString.Length == 2){
									int thisQuadIndex;
									if(int.TryParse(myDividedString[1], out thisQuadIndex)){
										myInfo.quadData = data.quads[myDividedString[0]];
										myInfo.isQuad = true;
										myInfo.quadIndex = thisQuadIndex;
										insertAfter = "\u2000"; //insert unicode quad to take its place
									}
									//override index
								}else if(myDividedString.Length == 3){
									int thisQuadIndexX;
									int thisQuadIndexY;
									if(int.TryParse(myDividedString[1], out thisQuadIndexX) && int.TryParse(myDividedString[2], out thisQuadIndexY)){
										//do some math to figure out what index this x and Y value points to
										myInfo.quadData = data.quads[myDividedString[0]];
										myInfo.isQuad = true;
										myInfo.quadIndex = myInfo.quadData.columns * thisQuadIndexX + thisQuadIndexY;
										insertAfter = "\u2000"; //insert unicode quad to take its place
									}
								}
							}
							break;
					//Material
						case "<m=":
						case "<material=":
							if(data.materials.ContainsKey(myString)){
								myInfo.materialData = data.materials[myString];
							}
							break;
						case "</m>":
						case "</material>":
							myInfo.materialData = null;
							break;
					//Bold & Italic
						case "<b>":
							myInfo.ch.style = AddStyle(myInfo.ch.style, FontStyle.Bold); //mark this character as bold
							break;
						case "</b>":
							myInfo.ch.style = SubtractStyle(myInfo.ch.style, FontStyle.Bold);
							break;
						case "<i>":
							myInfo.ch.style = AddStyle(myInfo.ch.style, FontStyle.Italic); //mark this character as italic
							break;
						case "</i>":
							myInfo.ch.style = SubtractStyle(myInfo.ch.style, FontStyle.Italic);
							break;
					//Waves
						case "<w=":
							if(data.waves.ContainsKey(myString)){ //is it a preset?
								myInfo.waveData = data.waves[myString];
							}
							break;
						case "<w>":
							if(data.waves.ContainsKey("default")){
								myInfo.waveData = data.waves["default"]; //mark this character as bold
							}else{
//								Debug.Log("Default wave isn't defined!");
								//Resources.UnloadAsset(thisWave); //force it to search again??
							}
							break;
						case "</w>":
							myInfo.waveData = null;
							break;
					//Jitters
						case "<j=":
							if(data.jitters.ContainsKey(myString)){ //is it a preset?
								myInfo.jitterData = data.jitters[myString];
							}
							break;
						case "<j>":
							if(data.jitters.ContainsKey("default")){
								myInfo.jitterData = data.jitters["default"];
							}else{
								Debug.Log("Default jitter isn't defined!");
								//Resources.UnloadAsset(thisJitter); //force it to search again?
							}
							break;
						case "</j>":
							myInfo.jitterData = null;
							break;
					//Alignment
						case "<a=":
							switch(myString.ToLower()){ //not case sensitive, for some reason? why not
								case "left": myInfo.alignment = Alignment.Left; break;
								case "right": myInfo.alignment = Alignment.Right; break;
								case "center": case "centre": myInfo.alignment = Alignment.Center; break;
								case "just": case "justify": case "justified": myInfo.alignment = Alignment.Justified; break;
								case "just2": case "justify2": case "justified2": myInfo.alignment = Alignment.ForceJustified; break;
							}
							break;
						case "</a>":
							myInfo.alignment = alignment; //return to default for mesh
							break;
					//Audio Settings
						case "<stopPreviousSound=":
							switch(myString.ToLower()){
								case "true": myInfo.stopPreviousSound = true; break;
								case "false": myInfo.stopPreviousSound = false; break;
							}
							break;
						case "</stopPreviousSound>":
							myInfo.stopPreviousSound = stopPreviousSound; //reset to default
							break;
						case "<pitchMode=":
							switch(myString.ToLower()){
								case "normal": myInfo.pitchMode = PitchMode.Normal; break;
								case "single": myInfo.pitchMode = PitchMode.Single; break;
								case "random": myInfo.pitchMode = PitchMode.Random; break;
								case "perlin": myInfo.pitchMode = PitchMode.Perlin; break;
							}
							break;
						case "</pitchMode>":
							myInfo.pitchMode = pitchMode; //return to default
							break;
						case "<overridePitch=":
							float thisOverridePitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisOverridePitch)){ //parse as a float
								myInfo.overridePitch = thisOverridePitch; //set time to be this float
							}
							break;
						case "</overridePitch>":
							myInfo.overridePitch = overridePitch;
							break;
						case "<minPitch=":
							float thisMinPitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisMinPitch)){ //parse as a float
								myInfo.minPitch = thisMinPitch; //set time to be this float
							}
							break;
						case "</minPitch>":
							myInfo.minPitch = minPitch;
							break;
						case "<maxPitch=":
							float thisMaxPitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisMaxPitch)){ //parse as a float
								myInfo.maxPitch = thisMaxPitch; //set time to be this float
							}
							break;
						case "</maxPitch>":
							myInfo.maxPitch = maxPitch;
							break;
						case "<speedReadPitch=":
							float thisSpeedReadPitch;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSpeedReadPitch)){ //parse as a float
								myInfo.speedReadPitch = thisSpeedReadPitch; //set time to be this float
							}
							break;
						case "</speedReadPitch>":
							myInfo.speedReadPitch = speedReadPitch;
							break;
						case "<readDelay=":
							float thisReadDelay;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisReadDelay)){ //parse as a float
								myInfo.readDelay = thisReadDelay; //set time to be this float
							}
							break;
						case "</readDelay>":
							myInfo.readDelay = readDelay;
							break;
						case "<drawAnim=":
							if(data.drawAnims.ContainsKey(myString)){
								myInfo.drawAnimData = data.drawAnims[myString]; //set draw animation
							}else if(data.drawAnims.ContainsKey("Appear")){
								myInfo.drawAnimData = data.drawAnims["Appear"]; //get first one
							}else{
								Debug.Log("'Appear' draw animation isn't defined!");
							}
							break;
						case "</drawAnim>":
							if(data.drawAnims.ContainsKey(drawAnimName)){
								myInfo.drawAnimData = data.drawAnims[drawAnimName]; //return to default
							}else if(data.drawAnims.ContainsKey("Appear")){
								myInfo.drawAnimData = data.drawAnims["Appear"]; //get first one
							}else{
								Debug.Log("'Appear' draw animation isn't defined!");
							}
							break;
						case "<drawOrder=":
							switch(myString.ToLower()){
								case "lefttoright": case "ltr": myInfo.drawOrder = DrawOrder.LeftToRight; break;
								case "allatonce": case "all": myInfo.drawOrder = DrawOrder.AllAtOnce; break;
								case "onewordatatime": case "robot": myInfo.drawOrder = DrawOrder.OneWordAtATime; break;
								case "random": myInfo.drawOrder = DrawOrder.Random; break;
								case "righttoleft": case "rtl": myInfo.drawOrder = DrawOrder.RightToLeft; break;
								case "reverseltr": myInfo.drawOrder = DrawOrder.ReverseLTR; break;
							}
							break;
						case "</drawOrder>":
							myInfo.drawOrder = drawOrder; //return to default
							break;
						case "<clips=":
							if(data.soundClips.ContainsKey(myString)){
								myInfo.soundClipData = data.soundClips[myString];
							}
							break;
						case "</clips>":
							myInfo.soundClipData = null;
							break;
						case "<audioClips=":
							if(data.audioClips.ContainsKey(myString)){
								myInfo.audioClipData = data.audioClips[myString];
							}
							break;
						case "</audioClips>":
							myInfo.audioClipData = null;
							break;
						case "<indent=": //set the indent to be here
							float thisIndent;
							if(float.TryParse(myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisIndent)){ //parse as a float
								myInfo.indent = thisIndent; //set time to be this float
							}
							break;
						case "</indent>": //return to normal
							myInfo.indent = 0f;
							break;
						case "<pause>":
							//tell mesh to pause here
							currentPauseCount++;
							if(Application.isPlaying && currentPauseCount > pauseCount) exitLoopAfter = true; //only during playmode for display purposes and cause it can break text
							else if(Application.isPlaying) insertAfter = "\u200B"; //2018-09-23 do this to prevent a bug where <e><pause><e> wouldn't play the second event
							break;
						case "<clear>":
							//call all cancel tags

							//color
							myInfo.colorData = null;
							myInfo.gradientData = null;
							myInfo.textureData = null;

							myInfo.size = size;

							myInfo.ev2.Clear();
							break;
					//Default
						default:
							clearAfter = false;//DONT remove characters and do stuff
							break;
					}
					if(clearAfter){
						switch(myTag){
							case "<br>": //ignore single-use tags
							case "<d>":
							case "<d=":
							case "<t=":
							case "<e=":
							case "<q=":
							case "<pause>":
								break;
							default: //remember this tage
								allTags.Add(new KeyValuePair<int, string>(i,myText.Substring(i,closingIndex+1-i))); //remember this tag and where it is
								break;
						}
						
						myText = myText.Remove(i,closingIndex+1-i);
						deletedChars += closingIndex+1-i;
						//Debug.Log("Removing '" + myText.Substring(i,closingIndex+1-i) + "'. The string is now: '" + myText + "'.");
						myText = myText.Insert(i,insertAfter);
						checkAgain = true;
					}
					if(exitLoopAfter){
						//keep track of last pause, and skip over it
						myText = myText.Remove(i,myText.Length-i); //remove everything after
						break;
					}
				}
			}

			if(info.Count - 1 == i){
				info[i] = new STMTextInfo(myInfo); //update older one, it was checking again
				//Debug.Log("Updating older character " + myText[i].ToString() + " to be " + info[i].style);
			}else{
				info.Add(new STMTextInfo(myInfo) ); //add new HAS TO BE NEW OR ELSE IT JUST REFERENCES
			}
			if(checkAgain){
				i--;
			}else{ //stuff that gets reset!! single-use tags.
				myInfo.delayData = null;// reset
				myInfo.quadData = null;
				myInfo.isQuad = false;
				myInfo.ev.Clear();
				myInfo.readTime = -1f; //say that timing hasn't been set for this letter yet
				myInfo.quadIndex = -1;
			}
			myInfo.rawIndex = i + deletedChars;
		}
		if(info.Count > myText.Length){
			//add extra char to myText for tacked-on event
			myText += "\u200B";
		}
		return myText;
	}
	int GetFontSize(Font myFont, STMTextInfo myInfo){ //so dynamic and non-dynamic fonts can be used together
		if(!myFont.dynamic && myFont.fontSize != 0){
			return myFont.fontSize; //always go w/ non-dynamic size first
		}
		if(myInfo.fontData != null){
			if(myInfo.fontData.overrideQuality){
				return myInfo.fontData.quality; //then set quality
			}else{
				return quality;
			}
		}
		if(myInfo.ch.size != 0){
			return myInfo.ch.size; //then natural quality
		}
		return quality; //default
	}
	void RequestAllCharacters(){ //by calling this every frame, should keep specific letters in the texture? not sure if it's a waste
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
			//and special characters:
			myFont.RequestCharactersInTexture("-", GetFontSize(myFont,info[i]), FontStyle.Normal); //still call this, for when you're inserting hyphens anyway
		}
	}
	
	void FigureOutUnwrappedLimits(Vector3 pos){
//		if(uiMode){ //remove startup warnings
		unwrappedBottomRightTextBounds = Vector3.zero;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //from character info...
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
			CharacterInfo ch;
			if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style)){ //does this character exist?
				info[i].ch = ch; //remember character info!
				info[i].UpdateCachedValuesIfChanged();
			}
			else
			{
				myFont = data.defaultFont;
				if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style))
				{
					//change the font on this mesh to the default
					info[i].fontData = new STMFontData(data.defaultFont);
					info[i].ch = ch; //remember character info!
					info[i].UpdateCachedValuesIfChanged();
				}
			}
		}
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each character to draw...
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			float myQuality = (float)GetFontSize(myFont,info[i]);
			if(hyphenedText[i] == '\n'){//drop a line
				pos = new Vector3(info[i].indent, pos.y, 0); //assume left-orintated for now. go back to start of row
				pos -= new Vector3(0, lineSpacing * info[i].size, 0); //drop down
			}
			else if(hyphenedText[i] == '\t'){//tab?
				pos += new Vector3(myQuality * 0.5f * tabSize, 0,0) * (info[i].size / myQuality);
			}
			else{// Advance character position
				pos += info[i].Advance(characterSpacing,myQuality);
			}//remember position data for whatever
			unwrappedBottomRightTextBounds.x = Mathf.Max(unwrappedBottomRightTextBounds.x, pos.x);
			unwrappedBottomRightTextBounds.y = Mathf.Min(unwrappedBottomRightTextBounds.y, pos.y);
		}
//		}
	}
	
	/*
	//just update UVs
	void GetCharacterInfoForArray(){
		//TODO see if setting "quality" to be info[i].ch.size has any GC issues, now: 2016-10-26
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //first, get character info...
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
			CharacterInfo ch;
			if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style)){ //does this character exist?
				info[i].ch = ch; //remember character info!
			}//else, don't draw anything! this charcter won't have info
		}
	}
	*/
	void RebuildTextInfo(){ 
		drawText = ParseText(text); //remove parsing junk (<col>, <b>), and fill textinfo again
		lineBreaks.Clear(); //index of line break characters, for centering
		hyphenedText = string.Copy(drawText);
		Vector3 pos = new Vector3(info.Count > 0 ? info[0].indent : 0f, lineHeights.Count > 0 ? -lineHeights[0] : size, 0f); //keep track of where to place this text
		FigureOutUnwrappedLimits(pos);
		totalWidth = 0f;
		allFonts.Clear();
		if(AutoWrap > 0f){ //use autowrap?
			if(bestFit != BestFitMode.Off){
				bestFitMulti = AutoWrap / unwrappedBottomRightTextBounds.x * 0.99999f; //use this number to keep it just below xMax
				if(bestFit == BestFitMode.OverLimit && bestFitMulti > 1f){
					bestFitMulti = 1f; //don't multiply
				}
			}else{
				bestFitMulti = 1f;
			}
			
			//TODO see if setting "quality" to be info[i].ch.size has any GC issues, now: 2016-10-26
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //first, get character info...
				Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
				info[i].size *= bestFitMulti;

				myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); 
				CharacterInfo ch;
				if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style)){ //does this character exist?
					info[i].ch = ch; //remember character info!
					// If the character changed, update the cached sizing values.
            		info[i].UpdateCachedValuesIfChanged();
				}
				//else, don't draw anything! this charcter won't have info
				//...is how it USED to work! instead, lets draw it in a fallback font:
				else{
					myFont = data.defaultFont;
					if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style))
					{
						//change the font on this mesh to the default
						info[i].fontData = new STMFontData(data.defaultFont);
						info[i].ch = ch; //remember character info!
						info[i].UpdateCachedValuesIfChanged();
					}
				}
				if(!allFonts.Contains(myFont)){ //if this font is not listed yet
					allFonts.Add(myFont);
				}
			}

			float lineWidth = info.Count > 0 ? info[0].indent : 0f;
			int previousBreak = -1;
			for(int i=0; i<info.Count; i++){
				Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
				CharacterInfo breakCh; //moved these into this loop 2016-10-26
				myFont.GetCharacterInfo('\n', out breakCh, GetFontSize(myFont,info[i]), style); //get data for linebreak
				CharacterInfo hyphenCh;
				myFont.RequestCharactersInTexture("-", GetFontSize(myFont,info[i]), style); //still call this, for when you're inserting hyphens anyway
				myFont.GetCharacterInfo('-', out hyphenCh, GetFontSize(myFont,info[i]), style);
				//float hyphenWidth = hyphenCh.advance * (info[i].size / info[i].ch.size); //have hyphen size match last character in row
				
				
				if(hyphenedText[i] == '\n'){ //is this character a line break?
					lineWidth = info[i].indent; //new line, reset
				}else if(hyphenedText[i] == '\t'){ // linebreak with a tab...
					lineWidth += 0.5f * tabSize * info[i].size;
					totalWidth += 0.5f * tabSize * info[i].size;
				}else{
					lineWidth += info[i].Advance(characterSpacing).x;
					totalWidth += info[i].Advance(characterSpacing).x;
				}
				//TODO: watch out for natural hyphens going over bounds limits
				if(lineWidth > AutoWrap && i > previousBreak+1){
					int myBreak = hyphenedText.LastIndexOf(' ',i); //safe spot to do a line break, can be a hyphen
					int myHyphenBreak = hyphenedText.LastIndexOf('-',i);
					int myTabBreak = hyphenedText.LastIndexOf('\t',i); //can break at a tab, too!
					int myActualBreak = Mathf.Max(new int[]{myBreak, myHyphenBreak, myTabBreak}); //get the largest of all 3
					int lastBreak = hyphenedText.LastIndexOf('\n',i); //last place a ine break happened
					if(!breakText && myActualBreak != -1 && myActualBreak > lastBreak){ //is there a space to do a line break? (and no hyphens...) AND we're not breaking text up at all
						//
						if(myActualBreak == myHyphenBreak){ //the break is at a hyphen
							hyphenedText = hyphenedText.Insert(myActualBreak+1, '\n'.ToString());
							info.Insert(myActualBreak+1,new STMTextInfo(info[myActualBreak], breakCh));
							i = myActualBreak+1; //go back
							previousBreak = i;
						}else{
							hyphenedText = hyphenedText.Remove(myActualBreak, 1); //this is wrong, don't remove the space ooops
							hyphenedText = hyphenedText.Insert(myActualBreak, '\n'.ToString());
							i = myActualBreak;
							previousBreak = i;
						}
						
						
					}else if(i > 0){ //split it here! but not if it's the first character
						if(insertHyphens){
							hyphenedText = hyphenedText.Insert(i, "-\n");
							//Debug.Log("This needs a hyphen: " + hyphenedText);
							info.Insert(i,new STMTextInfo(info[i], breakCh));
							info[i].UpdateCachedValuesIfChanged();
							info.Insert(i,new STMTextInfo(info[i], hyphenCh));
							info[i].UpdateCachedValuesIfChanged();
							previousBreak = i+1;
						}else{
							hyphenedText = hyphenedText.Insert(i, "\n");
							info.Insert(i,new STMTextInfo(info[i], breakCh));
							info[i].UpdateCachedValuesIfChanged();
							previousBreak = i;
							//if(AutoWrap < info[i - indexOffset-1].size){ //otherwise, it'll loop foreverrr
							//i += 1;
							//}
							//iL += 1;
							//indexOffset += 1;
						}
					}//no need to check for following space, it'll come up anyway
					lineWidth = info[i].indent; //reset
				}
			}
		}else{ //no autowrap, no need to insert linebreaks
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //from character info...
				Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
				//vvvv very important
				myFont.RequestCharactersInTexture(hyphenedText[i].ToString(), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
				//font.RequestCharactersInTexture(System.Text.Encoding.UTF8.GetString(System.BitConverter.GetBytes(info[i].ch.index)), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
				CharacterInfo ch;
				//get character from font if it exists
				if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style)){ //does this character exist?
					info[i].ch = ch; //remember character info!
					info[i].UpdateCachedValuesIfChanged();
				}
				else{
					//get from default font instead
					myFont = data.defaultFont;
					if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style))
					{
						//change the font on this mesh to the default
						info[i].fontData = new STMFontData(data.defaultFont);
						info[i].ch = ch; //remember character info!
						info[i].UpdateCachedValuesIfChanged();
					}
				}
				if(!allFonts.Contains(myFont)){ //if this font is not listed yet
					allFonts.Add(myFont);
				}
			}
		}
		//before assigning position to letters, figure out what the biggest character in each row is
		lineHeights.Clear();
		float biggest = info.Count > 0 ? info[0].size : size; //just in case this gets called somehow
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			if(hyphenedText[i] == '\n'){ //linebreak?
				lineHeights.Add(biggest);
				//if there's another character beyond this linebreak...
				if(hyphenedText.Length-1 > i){
					biggest = info[i+1].size; //start with next row's first character
				}
			}else{
				biggest = Mathf.Max(biggest, info[i].size);
			}
		}
		lineHeights.Add(biggest); //final line
		//get position
		int passedLineBreaks = 0;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each character to draw...
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			//CharacterInfo ch; //moved this code to the loop above^^^^
			//if(myFont.GetCharacterInfo(hyphenedText[i], out ch, quality, info[i].ch.style)){ //does this character exist?
			//	info[i].ch = ch; //remember character info!
			//}//else, don't draw anything! this charcter won't have info
			float myQuality = (float)GetFontSize(myFont,info[i]);
			info[i].pos = pos; //save this position data!
			//info[i].line = currentLineCount;
			if(hyphenedText[i] == '\n'){//drop a line
				passedLineBreaks++;
				lineBreaks.Add(i == 0 ? 0 : i-1);//first character is a line break? set default
				//start new row at the X position of the indent character
				pos.x = info[i].indent; //assume left-orintated for now. go back to start of row
				pos.y -= lineSpacing * lineHeights[passedLineBreaks]; //drop down
				//currentLineCount++;
			}
			else if(iL - 1 == i){ //last character, and not a line break?
				lineBreaks.Add(i);
			}
			else if(hyphenedText[i] == '\t'){//tab?
				pos += new Vector3(myQuality * 0.5f * tabSize, 0,0) * (info[i].size / myQuality);
			}
			else{// Advance character position
				pos += info[i].Advance(characterSpacing,myQuality);
			}//remember position data for whatever
		}
		lineBreaks = lineBreaks.Distinct().ToList(); //remove doubles, preventing horizontal offset glitch
		
		ApplyOffsetDataToTextInfo(); //just to clean up this very long function...
		TrimCutoffText();
		UpdateRTLDrawOrder();
		ApplyTimingDataToTextInfo();
		ApplyUnreadTimingDataToTextInfo();
		PrepareSubmeshes();
	}
	//
	//public float lowestPosition = 0f;

	void TrimCutoffText(){
		leftoverText = "";
		//remove text that has been pushed BELOW the boundary
		if(VerticalLimit > 0f && verticalLimitMode == VerticalLimitMode.CutOff){
			float cutoffPoint = -VerticalLimit;
			
			//this tells text in cut off mode to adjust accordingly to the anchor point
			switch(anchor){
				case TextAnchor.UpperLeft:
				case TextAnchor.UpperCenter:
				case TextAnchor.UpperRight: break;
				case TextAnchor.MiddleLeft:
				case TextAnchor.MiddleCenter:
				case TextAnchor.MiddleRight:
					//offsetDifference = (-info[info.Count-1].pos.y) * 0.5f;
					//cutoffPoint = (cutoffPoint * 0.5f) + (-info[info.Count-1].pos.y - rawBottomRightBounds.y) * 0.5f;
					cutoffPoint *= 0.5f;
					break;
				case TextAnchor.LowerLeft:
				case TextAnchor.LowerCenter:
				case TextAnchor.LowerRight:
					//wait a min
					//cutoffPoint = -Mathf.Infinity;
					//cutoffPoint = -(-info[info.Count-1].pos.y - rawBottomRightBounds.y);
					cutoffPoint = 0f;
					break;
			}
			cutoffPoint += uiOffset.y;
			
			for(int i=0; i<hyphenedText.Length; i++){
				if(info[i].pos.y < cutoffPoint){ //if this text is below the bounds...
					//cutoffPosition = i-1; //this was the last character before the cutoff
					hyphenedText = hyphenedText.Remove(i, hyphenedText.Length - i); //remove all text after this point
					AssembleLeftoverText();
					return; //found the first character below the limit
				}
				//lowestPosition = info[i].pos.y; //cache it!
			}
		}
		else if(VerticalLimit > 0f && (verticalLimitMode == VerticalLimitMode.AutoPause || 
										verticalLimitMode == VerticalLimitMode.AutoPauseFull)){
			for(int i=0; i<hyphenedText.Length; i++){
				while(info[i].pos.y < autoPauseStopPoint - offset.y){ //if this text is below the bounds... 
					currentPauseCount++;
					//for now, just allow another box-length to be drawn
					autoPauseStopPoint -= VerticalLimit;
					//autoPauseStopPoint += info[i].pos.y;
					if(Application.isPlaying && currentPauseCount > pauseCount){ //same behaviour as pause tag
						hyphenedText = hyphenedText.Remove(i, hyphenedText.Length - i); //remove all text after this point
						AssembleLeftoverText();
						return; //found the first character below the limit
					}
					//}
				}
				//lowestPosition = info[i].pos.y; //cache it!
			}
		}
		if(info.Count > 0){
			//lowestPosition = info[info.Count-1].pos.y; //cache it! this is repeated here for show last mode...
		}
	}
	void AssembleLeftoverText(){
		int cutoffPosition = hyphenedText.Length;
		if(cutoffPosition > 0){
			//first, add all tags up to and including this position
			for(int i=0; i<allTags.Count; i++){ //go thru all tags...
				if(allTags[i].Key <= cutoffPosition){
					leftoverText += allTags[i].Value; //add all tags from before this point
				}else{
					break;
				}
			}
			cutoffPosition = info[cutoffPosition].rawIndex; //translate to raw text index
			//next, add all raw text past this position
			//remove starting spaces from the start here

			leftoverText += preParsedText.Substring(cutoffPosition)/*.TrimStart()*/;
		}
	}

	private Vector3 offset = Vector3.zero;
	private Vector3 uiOffset = Vector3.zero;
	void ApplyOffsetDataToTextInfo(){ //this works!!! ahhhh!!!
		float[] allMaxes = new float[lineBreaks.Count];
		for(int i=0, iL=lineBreaks.Count; i<iL; i++){
			//get max x data from this line
			allMaxes[i] = info[lineBreaks[i]].RelativeAdvance(characterSpacing).x;
			//Debug.DrawRay(t.TransformPoint(info[lineBreaks[i]].RelativeAdvance(characterSpacing)), Vector3.right, Color.red, 0f);
			//Debug.Log("pos: " + info[lineBreaks[i]].pos + " advance: " + info[lineBreaks[i]].RelativeAdvance(characterSpacing));
			//allMaxes[i] = info[lineBreaks[i]].TopRightVert.x;
			if(float.IsNaN(allMaxes[i])){
				allMaxes[i] = 0f; //for rows that are just linebreaks! take THAT
			}
		}
		//calculate this below, not from the values above
		rawBottomRightTextBounds.x = Mathf.Max(allMaxes);
		rawBottomRightTextBounds.y = 0f;
		//lowestY = 0f;
		offset = Vector3.zero; //reset
		//minY = 0f;
		/* */
		if(uiMode){
			//ALIGN TO WHATEVER UI BOX HERE!!!
			//RectTransform tr = t as RectTransform; //(RectTransform(t)) also works!
			uiOffset = Vector3.zero;
			//TODO: during play mode, this doesn't update right...
			switch(anchor){
				case TextAnchor.UpperLeft: uiOffset = new Vector3(tr.rect.xMin, tr.rect.yMax, 0f); break;
				case TextAnchor.UpperCenter: uiOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, tr.rect.yMax, 0f); break;
				case TextAnchor.UpperRight: uiOffset = new Vector3(tr.rect.xMax, tr.rect.yMax, 0f); break;
				case TextAnchor.MiddleLeft: uiOffset = new Vector3(tr.rect.xMin, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); break;
				case TextAnchor.MiddleCenter: uiOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); break;
				case TextAnchor.MiddleRight: uiOffset = new Vector3(tr.rect.xMax, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); break;
				case TextAnchor.LowerLeft: uiOffset = new Vector3(tr.rect.xMin, tr.rect.yMin, 0f); break;
				case TextAnchor.LowerCenter: uiOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, tr.rect.yMin, 0f); break;
				case TextAnchor.LowerRight: uiOffset = new Vector3(tr.rect.xMax, tr.rect.yMin, 0f); break;
			}
			offset -= uiOffset;
		}
		//float lowestVert = 0f;
		//float rightestVert = 0f;
		//float mostLeftVert = Mathf.Infinity; //this is probably a bad idea
		int rowStart = 0; //index of where this row starts
		lowestPosition = 0f;
		for(int i=0, iL=lineBreaks.Count; i<iL; i++){ //for each line of text //2016-06-09 new alignment script
			float myOffsetRight = 0f; //empty space on this row
			myOffsetRight = rawBottomRightTextBounds.x - info[lineBreaks[i]].RelativeAdvance(characterSpacing).x;
			
			if(AutoWrap > 0f){
				myOffsetRight += AutoWrap - rawBottomRightTextBounds.x;
			}
			int spaceCount = 0;
			for(int j=rowStart, jL=lineBreaks[i]+1; j<jL; j++){ //see how many spaces there are
				if(hyphenedText[j] == ' '){
					spaceCount++;
				}
			}
			float justifySpace = spaceCount > 0 ? myOffsetRight / (float)spaceCount : 0f;
			int passedSpaces = 0;
			for(int j=rowStart, jL=lineBreaks[i]+1; j<jL; j++){//if this character is in the range...
				info[j].line = i; //tell info what line the letter is on here
				if(hyphenedText[j] == ' '){
					passedSpaces++;
				}
				//Debug.Log("Aligning character " + j + ", which is: '" + hyphenedText[j] + "'.");
				switch(info[j].alignment){
					case Alignment.Center:
						info[j].pos.x += myOffsetRight / 2f; //use half of empty space
						break;
					case Alignment.Right:
						info[j].pos.x += myOffsetRight;
						break;
					case Alignment.Justified:
						if(jL != hyphenedText.Length && drawText[jL - (hyphenedText.Length - drawText.Length)] != '\n'){ //not the very last row, or a row with a linebreak?
							info[j].pos.x += justifySpace * passedSpaces;
						}
						break;
					case Alignment.ForceJustified:
						info[j].pos.x += justifySpace * passedSpaces; //justify no matter what
						break;
					//do nothing for left-aligned
				}
				
				//if(info[j].pos.y > -VerticalLimit){ //only keep counting if it's not past the line count limit
					//if(VerticalLimit > 0f){
						//minY = Mathf.Min(minY, info[j].pos.y - info[j].size); //yeah this works. shouldn't change with waves/weird letters
					//	rawBottomRightTextBounds.y = Mathf.Min(rawBottomRightTextBounds.y, info[j].pos.y - info[j].size);
					//}else{
					rawBottomRightTextBounds.y = Mathf.Min(rawBottomRightTextBounds.y, info[j].pos.y);
					//}
					if(VerticalLimit == 0f || (VerticalLimit > 0f && verticalLimitMode == VerticalLimitMode.Ignore) || info[j].pos.y >= -VerticalLimit){ //only keep counting if it's not past the line count limit
						lowestPosition = Mathf.Min(lowestPosition, info[j].pos.y);
					}
					//maxX = Mathf.Max(maxX, info[j].BottomRightVert.x);
					//Debug.Log("Character: " + j + " y value: " + info[j].pos.y);
				//}
				//minY = Mathf.Min(minY, info[j].pos.y - info[j].size); //yeah this works. shouldn't change with waves/weird letters
				
				//rawBottomRightTextBounds.y = Mathf.Min(rawBottomRightTextBounds.y, info[j].pos.y - info[j].size);
				//maxX = Mathf.Max(maxX, info[j].BottomRightVert.x);
				//mostLeftVert = Mathf.Min(mostLeftVert, info[j].BottomLeftVert.x);
				//lowestY = Mathf.Min(lowestY, info[j].pos.y); 
			}
			rowStart = lineBreaks[i]+1;
		}
		//2018-01-06 this code is pointless since the limit doesn't matter in this state. But for some reason the limit is wrong so this looks better
/*
		if(uiMode){ //fix boundary ending up too wide on non-wrapped UI text
			RectTransform tr = t as RectTransform;
			maxX -= tr.rect.xMax; //watch the offset! this corrects the width
			//if(!wrapText) tr.rect.xMax;
			//if(VerticalLimit > 0f) minY = tr.rect.yMax;
			//RectTransform rect = c.transform as RectTransform;
			//tr.sizeDelta = new Vector2(!wrapText ? rawBottomRightBounds.x : tr.sizeDelta.x, !limitText ? topLeftBounds.y : tr.sizeDelta.y);
			//LayoutRebuilder.MarkLayoutForRebuild(tr);
		}
*/
		//clamp
		//minY = VerticalLimit > 0f ? -VerticalLimit + (size * bestFitMulti) : minY;
		//float upperY = size; //push down
		//float lowerY = size * (lineBreaks.Count - 1) * lineSpacing;
		float maxHeight = VerticalLimit > 0f ? -VerticalLimit : rawBottomRightTextBounds.y;
		float maxWidth = AutoWrap > 0f ? AutoWrap : rawBottomRightTextBounds.x; //if autowrapping, base it on box instead of text
		switch(anchor){
			case TextAnchor.UpperLeft: offset += new Vector3(0, 0f, 0); break;
			case TextAnchor.UpperCenter: offset += new Vector3(maxWidth * 0.5f, 0f, 0); break;
			case TextAnchor.UpperRight: offset += new Vector3(maxWidth, 0f, 0); break;
			case TextAnchor.MiddleLeft: offset += new Vector3(0, maxHeight * 0.5f, 0); break;
			case TextAnchor.MiddleCenter: offset += new Vector3(maxWidth * 0.5f, maxHeight * 0.5f, 0); break;
			case TextAnchor.MiddleRight: offset += new Vector3(maxWidth, maxHeight * 0.5f, 0); break;
			case TextAnchor.LowerLeft: offset += new Vector3(0, maxHeight, 0); break;
			case TextAnchor.LowerCenter: offset += new Vector3(maxWidth * 0.5f, maxHeight, 0); break;
			case TextAnchor.LowerRight: offset += new Vector3(maxWidth, maxHeight, 0); break;
		}
		for(int i=0, iL=info.Count; i<iL; i++){ //apply all offsets
			info[i].pos -= offset;
			//if all text goes beyond the vertical limit, move it up
		}
		
		rawTopLeftBounds = offset; //scale to show proper bounds even when parent is scaled weird
		rawBottomRightBounds = new Vector3(AutoWrap > 0f ? offset.x - AutoWrap : offset.x - rawBottomRightTextBounds.x, 
											VerticalLimit > 0f ? VerticalLimit + offset.y : offset.y - maxHeight, 
											offset.z);

		//align text to fit within this new box:
		anchorOffset = Vector3.zero;
		switch(anchor){
			case TextAnchor.UpperLeft:
			case TextAnchor.UpperCenter:
			case TextAnchor.UpperRight: break;
			case TextAnchor.MiddleLeft:
			case TextAnchor.MiddleCenter:
			case TextAnchor.MiddleRight:
				//offsetDifference = (-lowestPosition) * 0.5f;
				anchorOffset.y = VerticalLimit > -rawBottomRightTextBounds.y ? (-rawBottomRightTextBounds.y + rawTopLeftBounds.y - rawBottomRightBounds.y) * 0.5f : 0f;
				break;
			case TextAnchor.LowerLeft:
			case TextAnchor.LowerCenter:
			case TextAnchor.LowerRight:
				//Debug.Log(rawBottomRightTextBounds);
				anchorOffset.y = VerticalLimit > -rawBottomRightTextBounds.y ? -rawBottomRightTextBounds.y + rawTopLeftBounds.y - rawBottomRightBounds.y : 0f;
				break;
		}
		RecalculateBounds();
	}
	Vector3 anchorOffset = Vector3.zero;
	void RecalculateBounds(){
		topLeftBounds = t.TransformPoint(-rawTopLeftBounds);
		topRightBounds = t.TransformPoint(new Vector3(-rawBottomRightBounds.x, -rawTopLeftBounds.y, rawTopLeftBounds.z));
		bottomLeftBounds = t.TransformPoint(new Vector3(-rawTopLeftBounds.x, -rawBottomRightBounds.y, rawBottomRightBounds.z));
		bottomRightBounds = t.TransformPoint(-rawBottomRightBounds);

		centerBounds = Vector3.Lerp(topLeftBounds, bottomRightBounds, 0.5f);

		if(hyphenedText.Length == 0)
		{
			RecalculateTextBounds();
		}
		RecalculateFinalTextBounds();
	}
	private Vector3 TextBounds_leftOffset = Vector3.zero;
	private Vector3 TextBounds_rightOffset = Vector3.zero;
	private float TextBounds_diff = 0f;
	void RecalculateBoundsOffsets(){
		TextBounds_leftOffset = Vector3.zero;
		TextBounds_rightOffset = Vector3.zero;
		TextBounds_diff = rawBottomRightTextBounds.x + rawBottomRightBounds.x - offset.x; //distance between text bounds and autowrap, if any

		switch(alignment){
			case Alignment.Center:
				TextBounds_leftOffset.x += TextBounds_diff / 2f; //use half of empty space
				TextBounds_rightOffset.x += TextBounds_diff / 2f; //use half of empty space
				break;
			case Alignment.Right:
				TextBounds_leftOffset.x += TextBounds_diff;
				TextBounds_rightOffset.x += TextBounds_diff;
				break;
			case Alignment.Justified:
			case Alignment.ForceJustified:
				TextBounds_rightOffset.x += TextBounds_diff;
				break;
			//do nothing for left-aligned
		}
	}
	void RecalculateTextBounds(){
		if(hyphenedText.Length > 0)
		{
			RecalculateBoundsOffsets();

			//TODO: figure out why subtracting offset in this one spot is so different
			float textBoundsBottom = Mathf.Max(lowestDrawnPositionRaw - offset.y, lowestPosition - rawTopLeftBounds.y);
			//line up with text...
			//Debug.Log("lowest drawn: " + lowestDrawnPosition + " lowest: " + lowestPosition);
			topLeftTextBounds = t.TransformPoint(-TextBounds_leftOffset - rawTopLeftBounds + anchorOffset);
			topRightTextBounds = t.TransformPoint(new Vector3(furthestDrawnPosition, 0f, 0f) - rawTopLeftBounds - TextBounds_rightOffset + anchorOffset);
			bottomLeftTextBounds = t.TransformPoint(new Vector3(-rawTopLeftBounds.x, textBoundsBottom, 0f) - TextBounds_leftOffset + anchorOffset);
			bottomRightTextBounds = t.TransformPoint(new Vector3(furthestDrawnPosition - rawTopLeftBounds.x, textBoundsBottom, 0f) - TextBounds_rightOffset + anchorOffset);

			centerTextBounds = Vector3.Lerp(topLeftTextBounds, bottomRightTextBounds, 0.5f);
		}
		else
		{
			topLeftTextBounds = Vector3.zero;
			topRightTextBounds = Vector3.zero;
			bottomLeftTextBounds = Vector3.zero;
			bottomRightTextBounds = Vector3.zero;
			centerTextBounds = Vector3.zero;
		}
	}
	void RecalculateFinalTextBounds(){
		if(hyphenedText.Length > 0)
		{
			RecalculateBoundsOffsets();
			//calculate final bounds:
			finalTopLeftTextBounds = t.TransformPoint(-rawTopLeftBounds - TextBounds_leftOffset + anchorOffset);
			finalTopRightTextBounds = t.TransformPoint(new Vector3(rawBottomRightTextBounds.x, 0f, 0f) - rawTopLeftBounds - TextBounds_rightOffset + anchorOffset);
			finalBottomLeftTextBounds = t.TransformPoint(new Vector3(0f, lowestPosition, 0f) - rawTopLeftBounds - TextBounds_leftOffset + anchorOffset);
			finalBottomRightTextBounds = t.TransformPoint(new Vector3(rawBottomRightTextBounds.x, lowestPosition, 0f) - rawTopLeftBounds - TextBounds_rightOffset + anchorOffset);

			finalCenterTextBounds = Vector3.Lerp(finalTopLeftTextBounds, finalBottomRightTextBounds, 0.5f);
		}
		else
		{
			finalTopLeftTextBounds = Vector3.zero;
			finalTopRightTextBounds = Vector3.zero;
			finalBottomLeftTextBounds = Vector3.zero;
			finalBottomRightTextBounds = Vector3.zero;
			finalCenterTextBounds = Vector3.zero;
		}
	}

	private int[] drawOrderRTL;
	void UpdateRTLDrawOrder (){ //update the RTL draw info, if needed
		//if(drawOrder == DrawOrder.RightToLeft || undrawOrder == DrawOrder.RightToLeft){ //actually calculate? eh, do it anyway
		drawOrderRTL = new int[hyphenedText.Length];
		int currentLine = 0;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			int lastEnd = currentLine > 0 ? lineBreaks[currentLine-1] + 1 : 0;
			if(currentLine < lineBreaks.Count){
				drawOrderRTL[i] = -i + lineBreaks[currentLine] + lastEnd;
				if(lineBreaks[currentLine] == i){ //this was the last character in this row
					//Debug.Log("The end of this line was: " + lineBreaks[currentLine]);
					currentLine++;
				}
			}
		}
		//}
	}
	void ApplyTimingDataToTextInfo(){
		float currentTiming = 0f;
		float furthestPoint = 0f;
		bool needsToRead = false;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			int myIndex = i;
			switch(info[i].drawOrder){
				case DrawOrder.RightToLeft: myIndex = drawOrderRTL[i]; break;
				case DrawOrder.ReverseLTR: myIndex = -i + iL - 1; break;
			}
			if(info[i].readDelay > 0f){ //a delay hasn't been set for this letter, yet
				needsToRead = true;
			}
			float additionalDelay = info[myIndex].delayData != null ? info[myIndex].delayData.count : 0f; //if there's no additional delay data attached... no additional delay
			//get the time it'll be drawn at...
			if(info[myIndex].readTime < 0f){ //if a time hasn't been set for this letter yet
				switch(info[i].drawOrder){
					case DrawOrder.AllAtOnce:
						info[i].readTime = currentTiming;
						break;
					case DrawOrder.Random:
						info[i].readTime = UnityEngine.Random.Range(0f,info[i].readDelay);
						break;
					case DrawOrder.OneWordAtATime:
						if(hyphenedText[i] == ' ' || hyphenedText[i] == '\n' || hyphenedText[i] == '\t' || hyphenedText[i] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
							currentTiming += i == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						}
						info[i].readTime = currentTiming;
						break;	
					case DrawOrder.RightToLeft:
						info[myIndex].readTime = currentTiming; //reverse order!
						currentTiming += myIndex == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						break;
					case DrawOrder.ReverseLTR:
						currentTiming += i == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						info[myIndex].readTime = currentTiming;
						break;
					default: //Left To Right
						//dont add extra for first character
						currentTiming += i == 0 ? additionalDelay * info[i].readDelay : info[i].readDelay + (additionalDelay * info[i].readDelay);
						info[i].readTime = currentTiming;
						break;
				}
			}else{
				currentTiming = info[myIndex].readTime; //pick up from here
			}
			float maxAnimTime = info[i].drawAnimData != null ? Mathf.Max(info[i].drawAnimData.animTime, info[i].drawAnimData.fadeTime) : 0f; //just for initialization, so an error doesn't get returned. drawanim should never be null, really
			furthestPoint = Mathf.Max(info[myIndex].readTime + maxAnimTime, furthestPoint);
		}
		totalReadTime = furthestPoint + 0.00001f; //save it!
		callReadFunction = needsToRead;
	}
	void ApplyUnreadTimingDataToTextInfo(){
		//the other on the switch statement is different than the function above on purpose... might change in the future
		//things have to be done in a slightly different order
		float currentTiming = 0f;
		float furthestPoint = 0f;
		STMDrawAnimData myDrawAnim = UndrawAnim; //store undrawing animation since it'll be called multiple times
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			int myIndex = i;
			switch(undrawOrder){
				case DrawOrder.RightToLeft: myIndex = drawOrderRTL[i]; break;
				case DrawOrder.ReverseLTR: myIndex = -i + iL - 1; break;
			}
			switch(undrawOrder){
				case DrawOrder.AllAtOnce:
					info[i].unreadTime = currentTiming;
					break;
				case DrawOrder.Random:
					info[i].unreadTime = UnityEngine.Random.Range(0f,unreadDelay);
					break;
				case DrawOrder.OneWordAtATime:
					info[i].unreadTime = currentTiming;
					if(hyphenedText[i] == ' ' || hyphenedText[i] == '\n' || hyphenedText[i] == '\t' || hyphenedText[i] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
						currentTiming += unreadDelay;
					}
					break;	
				case DrawOrder.RightToLeft:
					currentTiming += unreadDelay;
					info[myIndex].unreadTime = currentTiming;
					break;
				case DrawOrder.ReverseLTR:
					currentTiming += unreadDelay;
					info[myIndex].unreadTime = currentTiming;
					break;
				default:
					info[i].unreadTime = currentTiming;
					currentTiming += unreadDelay; //<<< this is applied in opposide order as normal read info
					break;
			}
			float maxAnimTime = myDrawAnim != null ? Mathf.Max(myDrawAnim.animTime, myDrawAnim.fadeTime) : 0f;
			furthestPoint = Mathf.Max(info[myIndex].unreadTime + maxAnimTime, furthestPoint);
		}
		totalUnreadTime = furthestPoint; //save it!
	}

    Vector3 WavePosition_Vect = Vector3.zero;
	Vector3 WavePosition(STMTextInfo myInfo, STMWaveControl wave, float myTime)
    {
        //multiply offset by 6 because ??????? seems to work
        //multiply by universal size;
        WavePosition_Vect.x = wave.curveX.Evaluate(((myTime * wave.speed.x) + wave.phase * 6f) + (myInfo.pos.x * wave.density.x / myInfo.size)) * wave.strength.x * myInfo.size;
        WavePosition_Vect.y = wave.curveY.Evaluate(((myTime * wave.speed.y) + wave.phase * 6f) + (myInfo.pos.x * wave.density.y / myInfo.size)) * wave.strength.y * myInfo.size;
        WavePosition_Vect.z = wave.curveZ.Evaluate(((myTime * wave.speed.z) + wave.phase * 6f) + (myInfo.pos.x * wave.density.z / myInfo.size)) * wave.strength.z * myInfo.size;

        return WavePosition_Vect;
	}

    Vector3 WaveRotation_Pivot = Vector3.zero;
    Vector3 WaveRotation_Offset = Vector3.zero;
    Vector3 WaveRotation_ReturnVal = Vector3.zero;
    Vector3 WaveRotation_myRotation = Vector3.zero;
	Quaternion WaveRotation_myQuaternion = new Quaternion();
    Vector3 WaveRotation(STMTextInfo myInfo, STMWaveRotationControl rot, Vector3 vertPos, float myTime)
    {
        //return the offset relative to zero

        //x pivot should be based on letter's width
        //y pivot should be based on local height of mesh
        WaveRotation_Pivot.x = myInfo.pos.x + rot.pivot.x * myInfo.RelativeWidth;
        WaveRotation_Pivot.y = myInfo.pos.y + rot.pivot.y * myInfo.size;
        WaveRotation_Pivot.z = 0f;

        WaveRotation_Offset.x = vertPos.x - WaveRotation_Pivot.x;
        WaveRotation_Offset.y = vertPos.y - WaveRotation_Pivot.y;
        WaveRotation_Offset.z = vertPos.z - WaveRotation_Pivot.z;

        WaveRotation_myRotation.x = 0f;
        WaveRotation_myRotation.y = 0f;
        WaveRotation_myRotation.z = rot.curveZ.Evaluate(((myTime * rot.speed) + rot.phase * 6f) + (myInfo.pos.x * rot.density)) * rot.strength;

		WaveRotation_myQuaternion = Quaternion.Euler(WaveRotation_myRotation);

        WaveRotation_Offset = WaveRotation_myQuaternion * WaveRotation_Offset;

        WaveRotation_ReturnVal.x = WaveRotation_Offset.x + WaveRotation_Pivot.x - vertPos.x;
        WaveRotation_ReturnVal.y = WaveRotation_Offset.y + WaveRotation_Pivot.y - vertPos.y;
        WaveRotation_ReturnVal.z = WaveRotation_Offset.z + WaveRotation_Pivot.z - vertPos.z;

        return WaveRotation_ReturnVal;
    }	

	Vector3 WaveScale(STMTextInfo myInfo, STMWaveScaleControl scale, Vector3 vertPos, float myTime){
		
		Vector3 pivot = myInfo.pos + new Vector3(scale.pivot.x * myInfo.RelativeWidth, scale.pivot.y * myInfo.size, 0f);
		Vector3 offset = vertPos - pivot;
		Vector3 newScale = new Vector3(scale.curveX.Evaluate(((myTime * scale.speed.x) + scale.phase * 6f) + (myInfo.pos.x * scale.density.x)) * scale.strength.x,
										scale.curveY.Evaluate(((myTime * scale.speed.y) + scale.phase * 6f) + (myInfo.pos.x * scale.density.y)) * scale.strength.y,
										1f);
		offset = Vector3.Scale(offset, newScale);
		return offset + pivot - vertPos;
	}
	/*
	Vector3 WaveValue(STMTextInfo myInfo, STMWaveControl position, STMWaveRotationControl rotation, STMWaveScaleControl scale){ //multiply phase by 6 because ??????? seems to work
		//float currentTime = GetTime;
		//float myTime = myInfo.waveData.animateFromTimeDrawn ? GetTime - timeDrawn - myInfo.readTime : GetTime;
		//Vector3 myPos = myInfo.waveData.positionControl ? WavePosition(myInfo, position, myTime) : Vector3.zero;
		//Vector3 myRot = myInfo.waveData.rotationControl ? WaveRotation(myInfo, rotation, myTime) : Vector3.zero;
		//Vector3 mySca = myInfo.waveData.scaleControl ? WaveScale(myInfo, scale, myTime) : Vector3.one;
		//return Vector3.Scale(Quaternion.Euler(myRot) * myPos, mySca); //add it all together
	}
	*/
	
Vector3 JitterValue_MyJit = Vector3.zero;
	Vector3 JitterValue(STMTextInfo myInfo, STMJitterData jit)
    {
		float myTime = currentReadTime - myInfo.readTime; //time that's different for each letter
		switch(jit.perlin)
        {
			case true:
                //weird perlin jitter... could use some work, but it's a jitter effect that scales with time
                JitterValue_MyJit.x = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * (jit.distance.Evaluate(Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x, 0f)) * jit.amount * (Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x, 0f) - 0.5f)) * myInfo.size;
                JitterValue_MyJit.y = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * (jit.distance.Evaluate(Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x + 30f, 0f)) * jit.amount * (Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x + 30f, 0f) - 0.5f)) * myInfo.size;
                JitterValue_MyJit.z = 0f;
				break;
			default:
                //ditance over time... so jitters can also only happen AS a letter is drawn.
                JitterValue_MyJit.x = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * jit.distance.Evaluate(UnityEngine.Random.value) * jit.amount * (UnityEngine.Random.value - 0.5f) * myInfo.size; //make jit follow curve
                JitterValue_MyJit.y = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * jit.distance.Evaluate(UnityEngine.Random.value) * jit.amount * (UnityEngine.Random.value - 0.5f) * myInfo.size;
                JitterValue_MyJit.z = 0f;
				break;
		}

		return JitterValue_MyJit;
	}

	void PrepareSubmeshes(){
		//since this only needs to be calculated during Rebuild(), putting this in its own function.

		subMeshes = new List<SubMeshData>(); //include default submesh
		subMeshes.Add(new SubMeshData(this)); //add default submesh
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //go thru all info
			//This also only needs to be changed on rebuild(), move it sometime 2016-10-26 TODO
			//get and assign submesh/triangles for this letter
			SubMeshData thisSubMesh = DoesSubmeshExist(this,info[i]); //is there a submesh for this texture yet?
			//Debug.Log("This info's font is " + info[i].fontData);
			if(thisSubMesh == null){ //doesn't exist yet??
				thisSubMesh = new SubMeshData(this, info[i]); //create new
				subMeshes.Add(thisSubMesh); //and add to submesh list
			}
			//vvvv doing is this way creates garbage
			//thisSubMesh.tris.AddRange(new int[]{4*i+0,4*i+1,4*i+2,4*i+0,4*i+2,4*i+3}); //add tris for this letter
			//vvvv this way seems fine tho
			thisSubMesh.tris.Add(4*i + 0);
			thisSubMesh.tris.Add(4*i + 1);
			thisSubMesh.tris.Add(4*i + 2);
			thisSubMesh.tris.Add(4*i + 0);
			thisSubMesh.tris.Add(4*i + 2);
			thisSubMesh.tris.Add(4*i + 3);
		}


		//subMeshes = new SubMeshData[subMeshCount]; //create an array to hold all these sebmeshes

	}

   //--------------------------------------------------
    // Performance updates performed by RedVonix
    // http://www.RedVonix.com/
    //
    // All comments from Red are marked with "RV:"
    //--------------------------------------------------

    // Cache objects here that we'll use in UpdateMesh
    private Vector3 UpdateMesh_waveValue = Vector3.zero; //universal
    private Vector3 UpdateMesh_waveValueTopLeft = Vector3.zero;
    private Vector3 UpdateMesh_waveValueTopRight = Vector3.zero;
    private Vector3 UpdateMesh_waveValueBottomRight = Vector3.zero;
    private Vector3 UpdateMesh_waveValueBottomLeft = Vector3.zero;
    private Vector3 UpdateMesh_lowestLineOffset = Vector3.zero;
    private Vector3 UpdateMesh_wavePosition;
    private Vector2 UpdateMesh_uvOffset = new Vector2();
    private STMTextInfo CurrentTextInfo;
    Vector3[] UpdateMesh_Middles = new Vector3[0];
    Vector3[] UpdateMesh_Positions = new Vector3[0];

    // These are used in UpdateMesh for math conversions to avoid doing casting
    private Vector3 cacheVectThree;
    Vector3 jitterValue;
    private Vector2 vectA;
    private Vector2 vectAA;
    private Vector2 vectB;
    private Vector2 vectBB;
    private Vector2 vectC;
    private Vector2 vectCC;
    private Vector2 vectD;
    private Vector2 vectDD;
    private Vector2 infoVect = new Vector2();

	private Vector2 ratioHold;
	private Vector2 uvMidHold;
	private Vector4 ratioAndUvHold;

	private bool doPrintEventAfter = false;
	private bool doEventAfter = false;

	void UpdateMesh(float myTime) 
	{ //set the data for the endmesh

        // Store the GetTime value so we don't have to calculate it multiple times on every call of UpdateMesh.
        float GetTimeTime = GetTime;
		
		// Same with the VerticalLimit
		float VerticalLimitStored = VerticalLimit;

        int targArraySize = hyphenedText.Length * 4;

		//Mesh mesh = new Mesh();
		areWeAnimating = false;

        //if(hyphenedText.Length > 0){ //bother to draw it?

        // RV: Only update the array sizes here if we need to.
        // Generate a mesh for the characters we want to print.
        if (endVerts.Length != targArraySize)
            Array.Resize(ref endVerts, targArraySize);

        //endTriangles = new int[hyphenedText.Length * 6];
        if (endUv.Length != targArraySize)
            Array.Resize(ref endUv, targArraySize);

        if (endUv2.Length != targArraySize)
            Array.Resize(ref endUv2, targArraySize);//overlay images

        if (endCol32.Length != targArraySize)
            Array.Resize(ref endCol32, targArraySize);

		if (ratiosAndUvMids.Count != targArraySize)
			ratiosAndUvMids = new List<Vector4>(new Vector4[targArraySize]);

		//if (uvMids.Length != targArraySize)
		//	Array.Resize(ref uvMids, targArraySize);

		//int tallestVisibleIndex = 0;
		//float highestVisiblePoint = rawBottomRightBounds.y;
		//bool playedSoundThisFrame = false;
		//float offsetDifference = 0f;
		//info[info.Count-1].pos.y

		for(int i=0, iL=hyphenedText.Length; i<iL; i++)
        {
            // RV: Grab the current STMTextInfo object only once from the list it's in, as acquiring it over tand over just adds unneeded overhead.
            CurrentTextInfo = info[i];
			//Debug.Log(CurrentTextInfo.character);
			ratioHold = CurrentTextInfo.ratio;
			//ratios[4 * i + 0] = CurrentTextInfo.ratio;
			//ratios[4 * i + 1] = CurrentTextInfo.ratio;
			//ratios[4 * i + 2] = CurrentTextInfo.ratio;
			//ratios[4 * i + 3] = CurrentTextInfo.ratio;
			if(!reading)
			{
				//do every event
				DoEvent(i);
			}

            //just to get timing info for events
			//these are used to that positional data is updated before events are called
			doPrintEventAfter = false;
			doEventAfter = false;
            if (reading)
            {
                float divideAnimAmount = CurrentTextInfo.drawAnimData.animTime == 0f ? 0.0000001f : CurrentTextInfo.drawAnimData.animTime; //so it doesn't get NaN'd
				float myAnimPos = (myTime - CurrentTextInfo.readTime) / divideAnimAmount; // on a range between 0-1 on the curve, the position of the animation
				if(myAnimPos > 0f && i > latestNumber){ 
					doEventAfter = true;
					//if(!playedSoundThisFrame){
					//	playedSoundThisFrame = true;

					//ignore 0-width space, as it's used for tacked-on events
					if(hyphenedText[i] != '\u200B')
					{
						//}
						doPrintEventAfter = true;
						if(hyphenedText[i] != ' ' && hyphenedText[i] != '\n')
						{
							lowestDrawnPosition = Mathf.Min(lowestDrawnPosition, CurrentTextInfo.pos.y);
							lowestDrawnPositionRaw = Mathf.Min(lowestDrawnPosition, CurrentTextInfo.pos.y + offset.y);
							//Debug.Log(CurrentTextInfo.pos.y);
							furthestDrawnPosition = Mathf.Max(furthestDrawnPosition, CurrentTextInfo.RelativeAdvance(characterSpacing).x + offset.x + TextBounds_rightOffset.x);
						}
					}
					latestNumber = Mathf.Max(latestNumber, i); //find latest number to start from next frame
					/*
					if(info[i].pos.y + info[i].size + lowestLineOffset.y < 0f && //this number is below the limit
						info[i].pos.y + info[i].size + lowestLineOffset.y > highestVisiblePoint){ //is this number taller than the current tallest number?
							highestVisiblePoint = info[i].pos.y + info[i].size + lowestLineOffset.y;
							tallestVisibleIndex = i;
					}
					*/
					
				}
			}else if(!Application.isPlaying || VerticalLimitStored == 0f || !(verticalLimitMode == VerticalLimitMode.AutoPause ||
																		verticalLimitMode == VerticalLimitMode.AutoPauseFull)){ //don't do this for autopauses
				latestNumber = hyphenedText.Length-1;
				lowestDrawnPosition = info[latestNumber].pos.y; //assume the final letter
				lowestDrawnPositionRaw = info[latestNumber].pos.y + offset.y;
				furthestDrawnPosition = rawBottomRightTextBounds.x; //this causes some sizes to be incorrect in-editor, but should be fine as text reads out
			}
			RecalculateTextBounds();
			if(doEventAfter)
			{
				DoEvent(i); //do every event up to this integer
			}
			if(doPrintEventAfter)
			{
				PlaySound(i); //only play one sound this frame, from the first letter drawn this frame
				if(onPrintEvent != null) onPrintEvent.Invoke();
				if(OnPrintEvent != null) OnPrintEvent();
			}
			//if(offsetDifference > 0f) offsetDifference = 0f;
			//this is done here so text grows from middle/lower zones
			UpdateMesh_lowestLineOffset = anchorOffset;
			//push text up if it goes below vertical limit. uses a multiple of size to keep consistent line drop sizes
			//info[i].pos.y < -rawBottomRightBounds.y
			if(VerticalLimitStored > 0f && (verticalLimitMode == VerticalLimitMode.ShowLast || 
										verticalLimitMode == VerticalLimitMode.AutoPause ||
										verticalLimitMode == VerticalLimitMode.AutoPauseFull) &&
				lowestDrawnPosition < -rawBottomRightBounds.y){ //if the bounds extend beyond the vertical limit
				
				
				//push text up!
				//and round to nearest multiple of size, so text lines up with top of box
				//UpdateMesh_lowestLineOffset.y = Mathf.Ceil((-lowestDrawnPosition - rawBottomRightBounds.y) / (size * lineSpacing)) * (size * lineSpacing);
				//line up with top of next row...
				//UpdateMesh_lowestLineOffset.y = Mathf.Ceil((-lowestDrawnPosition - rawBottomRightBounds.y) / (size * lineSpacing)) * (size * lineSpacing);
				//UpdateMesh_lowestLineOffset.y = -lowestDrawnPosition;
				//UpdateMesh_lowestLineOffset.y = lineHeights[0];
				//UpdateMesh_lowestLineOffset.y = -lowestDrawnPosition - rawBottomRightBounds.y;
				for(int j=0; j<lineHeights.Count; j++){
					//for every line...
					if(UpdateMesh_lowestLineOffset.y >= -lowestDrawnPosition - rawBottomRightBounds.y){
						break;
					}
					//start at end of array?
					UpdateMesh_lowestLineOffset.y += lineHeights[j];

				}
                if (verticalLimitMode == VerticalLimitMode.AutoPauseFull){
                    //round to nearest multiple of verticallimit
                   	UpdateMesh_lowestLineOffset.y = Mathf.Ceil(UpdateMesh_lowestLineOffset.y / rawBottomRightBounds.y) * rawBottomRightBounds.y;
					//UpdateMesh_lowestLineOffset.y = Mathf.Ceil(UpdateMesh_lowestLineOffset.y / (size * lineSpacing)) * (size * lineSpacing);
				}
			}

		//Vertex data:
		//animated stuffffff
			jitterValue = Vector3.zero;
			if(CurrentTextInfo.jitterData != null && !data.disableAnimatedText && !disableAnimatedText)
            { //just dont jitter if animating text is overridden
				areWeAnimating = true;
				jitterValue = JitterValue(CurrentTextInfo, CurrentTextInfo.jitterData); //get jitter data
			}

            UpdateMesh_waveValue = Vector3.zero; //universal
			UpdateMesh_waveValueTopLeft = Vector3.zero;
			UpdateMesh_waveValueTopRight = Vector3.zero;
			UpdateMesh_waveValueBottomRight = Vector3.zero;
			UpdateMesh_waveValueBottomLeft = Vector3.zero;
			//Vector3 UpdateMesh_waveValueRotation = Vector3.zero;
			//Vector3 UpdateMesh_waveValueRotPivot = Vector3.zero;
			
			if(CurrentTextInfo.waveData != null && CurrentTextInfo.size != 0 && !data.disableAnimatedText && !disableAnimatedText){
				areWeAnimating = true;
				float waveTime = CurrentTextInfo.waveData.animateFromTimeDrawn ? currentReadTime - CurrentTextInfo.readTime : GetTimeTime;
				if(CurrentTextInfo.waveData.positionControl){
					UpdateMesh_waveValue = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.position, waveTime);
				}

                // RV: Following was converted to add individual dimensions of arrays rather than whole arrays, as adding
                //      full arrays causes the creation of a new array which creates additional GC and processing time.
				if(CurrentTextInfo.waveData.individualVertexControl)
                {
                    UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.topLeft, waveTime);
                    UpdateMesh_waveValueTopLeft.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueTopLeft.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueTopLeft.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.topRight, waveTime);
                    UpdateMesh_waveValueTopRight.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueTopRight.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueTopRight.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.bottomRight, waveTime);
                    UpdateMesh_waveValueBottomRight.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueBottomRight.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueBottomRight.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.bottomLeft, waveTime);
                    UpdateMesh_waveValueBottomLeft.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueBottomLeft.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueBottomLeft.z += UpdateMesh_wavePosition.z;
                }
				
				if(CurrentTextInfo.waveData.rotationControl)
                {
                    UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.TopLeftVert, waveTime);
                    UpdateMesh_waveValueTopLeft.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueTopLeft.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueTopLeft.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.TopRightVert, waveTime);
                    UpdateMesh_waveValueTopRight.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueTopRight.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueTopRight.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.BottomRightVert, waveTime);
                    UpdateMesh_waveValueBottomRight.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueBottomRight.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueBottomRight.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.BottomLeftVert, waveTime);
                    UpdateMesh_waveValueBottomLeft.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueBottomLeft.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueBottomLeft.z += UpdateMesh_wavePosition.z;
                }
				if(CurrentTextInfo.waveData.scaleControl)
                {
                    UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.TopLeftVert, waveTime);
                    UpdateMesh_waveValueTopLeft.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueTopLeft.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueTopLeft.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.TopRightVert, waveTime);
                    UpdateMesh_waveValueTopRight.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueTopRight.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueTopRight.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.BottomRightVert, waveTime);
                    UpdateMesh_waveValueBottomRight.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueBottomRight.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueBottomRight.z += UpdateMesh_wavePosition.z;

                    UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.BottomLeftVert, waveTime);
                    UpdateMesh_waveValueBottomLeft.x += UpdateMesh_wavePosition.x;
                    UpdateMesh_waveValueBottomLeft.y += UpdateMesh_wavePosition.y;
                    UpdateMesh_waveValueBottomLeft.z += UpdateMesh_wavePosition.z;
                }
				
			}
			
			//if text isn't different, you don't need to update UVs, or triangles
			//only need to update vertices of animated text
			//only need to update color of text w/ animated colors

			//for cutting off old text
            if ((VerticalLimitStored > 0f && verticalLimitMode != VerticalLimitMode.Ignore) &&  //if vertical limit is on and not set to ignore...
                (CurrentTextInfo.pos.y + CurrentTextInfo.size + UpdateMesh_lowestLineOffset.y - anchorOffset.y > -rawTopLeftBounds.y + 0.00001f/* || info[i].pos.y < -rawBottomRightBounds.y*/))
            { //hide all text that extends beyond the vertical limit
              //if using a limited vertical space and this text's line is before the text that should be shown
                endVerts[4 * i + 0] = Vector3.zero; //hide it!
                endVerts[4 * i + 1] = Vector3.zero;
                endVerts[4 * i + 2] = Vector3.zero;
                endVerts[4 * i + 3] = Vector3.zero;
            }
            else
            {
				//assign vertices

				endVerts[4 * i + 0].x = ((CurrentTextInfo.TopLeftVert.x + jitterValue.x + UpdateMesh_waveValueTopLeft.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + baseOffset.x);
				endVerts[4 * i + 0].y = ((CurrentTextInfo.TopLeftVert.y + jitterValue.y + UpdateMesh_waveValueTopLeft.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + baseOffset.y);
				endVerts[4 * i + 0].z = ((CurrentTextInfo.TopLeftVert.z + jitterValue.z + UpdateMesh_waveValueTopLeft.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + baseOffset.z);

				endVerts[4 * i + 1].x = ((CurrentTextInfo.TopRightVert.x + jitterValue.x + UpdateMesh_waveValueTopRight.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + baseOffset.x);
				endVerts[4 * i + 1].y = ((CurrentTextInfo.TopRightVert.y + jitterValue.y + UpdateMesh_waveValueTopRight.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + baseOffset.y);
				endVerts[4 * i + 1].z = ((CurrentTextInfo.TopRightVert.z + jitterValue.z + UpdateMesh_waveValueTopRight.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + baseOffset.z);

				endVerts[4 * i + 2].x = ((CurrentTextInfo.BottomRightVert.x + jitterValue.x + UpdateMesh_waveValueBottomRight.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + baseOffset.x);
				endVerts[4 * i + 2].y = ((CurrentTextInfo.BottomRightVert.y + jitterValue.y + UpdateMesh_waveValueBottomRight.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + baseOffset.y);
				endVerts[4 * i + 2].z = ((CurrentTextInfo.BottomRightVert.z + jitterValue.z + UpdateMesh_waveValueBottomRight.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + baseOffset.z);
				
				endVerts[4 * i + 3].x = ((CurrentTextInfo.BottomLeftVert.x + jitterValue.x + UpdateMesh_waveValueBottomLeft.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + baseOffset.x);
				endVerts[4 * i + 3].y = ((CurrentTextInfo.BottomLeftVert.y + jitterValue.y + UpdateMesh_waveValueBottomLeft.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + baseOffset.y);
				endVerts[4 * i + 3].z = ((CurrentTextInfo.BottomLeftVert.z + jitterValue.z + UpdateMesh_waveValueBottomLeft.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + baseOffset.z);

                if (!CurrentTextInfo.isQuad)
                {
                    //Assign text UVs
                    //OPTO: this only needs to be changed on Rebuild()
                    endUv[4 * i + 0] = CurrentTextInfo.ch.uvTopLeft;
                    endUv[4 * i + 1] = CurrentTextInfo.ch.uvTopRight;
                    endUv[4 * i + 2] = CurrentTextInfo.ch.uvBottomRight;
                    endUv[4 * i + 3] = CurrentTextInfo.ch.uvBottomLeft;

					uvMidHold.x = (CurrentTextInfo.ch.uvTopLeft.x + CurrentTextInfo.ch.uvTopRight.x) * 0.5f;
					uvMidHold.y = (CurrentTextInfo.ch.uvTopLeft.y + CurrentTextInfo.ch.uvBottomLeft.y) * 0.5f;
                }
                else
                {
                    //choose whether to use built-in index or an override
                    endUv[4 * i + 0] = CurrentTextInfo.quadData.UvTopLeft(GetTimeTime, CurrentTextInfo.quadIndex);
                    endUv[4 * i + 1] = CurrentTextInfo.quadData.UvTopRight(GetTimeTime, CurrentTextInfo.quadIndex);
                    endUv[4 * i + 2] = CurrentTextInfo.quadData.UvBottomRight(GetTimeTime, CurrentTextInfo.quadIndex);
                    endUv[4 * i + 3] = CurrentTextInfo.quadData.UvBottomLeft(GetTimeTime, CurrentTextInfo.quadIndex);

					uvMidHold = CurrentTextInfo.quadData.UvMiddle(GetTimeTime, CurrentTextInfo.quadIndex);

                    if (CurrentTextInfo.quadData.columns > 1 && CurrentTextInfo.quadData.animDelay > 0f && CurrentTextInfo.quadIndex < 0)
                    {
                        areWeAnimating = true;
                    }
                }
            }
			//combine into one array
			ratioAndUvHold.x = ratioHold.x;
			ratioAndUvHold.y = ratioHold.y;
			ratioAndUvHold.z = uvMidHold.x;
			ratioAndUvHold.w = uvMidHold.y;

			//Debug.Log(ratiosAndUvMids.Count);
			ratiosAndUvMids[4 * i + 0] = ratioAndUvHold;
			ratiosAndUvMids[4 * i + 1] = ratioAndUvHold;
			ratiosAndUvMids[4 * i + 2] = ratioAndUvHold;
			ratiosAndUvMids[4 * i + 3] = ratioAndUvHold;
			

			
		//Scrolling Textures:
           //make sure last character isn't a tab, space, or line break.
            if (CurrentTextInfo.textureData != null && (i != iL-1 || (i == iL-1 && CurrentTextInfo.TopRightVert != Vector3.zero))){ //not last character nothing!
				if(CurrentTextInfo.textureData.scrollSpeed != Vector2.zero){
					areWeAnimating = true; //update this every frame
				}

                UpdateMesh_uvOffset.x = GetTimeTime * CurrentTextInfo.textureData.scrollSpeed.x;
                UpdateMesh_uvOffset.y = GetTimeTime * CurrentTextInfo.textureData.scrollSpeed.y;

                //Vector2 uvMulti = Vector2
				
				float uv2Scale = 1f;
				if(CurrentTextInfo.textureData.scaleWithText){
					uv2Scale = 1f / CurrentTextInfo.size;
				}

                // Fetch values and store them in existing objects to avoid doing casting
                // or creating new objects.
                cacheVectThree = endVerts[4 * i + 0];
                vectA.x = cacheVectThree.x;
                vectA.y = cacheVectThree.y;

                cacheVectThree = endVerts[4 * i + 1];
                vectB.x = cacheVectThree.x;
                vectB.y = cacheVectThree.y;

                cacheVectThree = endVerts[4 * i + 2];
                vectC.x = cacheVectThree.x;
                vectC.y = cacheVectThree.y;

                cacheVectThree = endVerts[4 * i + 3];
                vectD.x = cacheVectThree.x;
                vectD.y = cacheVectThree.y;

                if (CurrentTextInfo.textureData.relativeToLetter){//keep uvs relative to each letter?
                                                          //just draw texture as a square
                    infoVect.x = CurrentTextInfo.pos.x;
                    infoVect.y = CurrentTextInfo.pos.y;

                    endUv2[4 * i + 0].x = uv2Scale * (vectA.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 0].y = uv2Scale * (vectA.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

                    endUv2[4 * i + 1].x = uv2Scale * (vectB.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 1].y = uv2Scale * (vectB.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

                    endUv2[4 * i + 2].x = uv2Scale * (vectC.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 2].y = uv2Scale * (vectC.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

                    endUv2[4 * i + 3].x = uv2Scale * (vectD.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 3].y = uv2Scale * (vectD.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;
                }
                else
                {
                    endUv2[4 * i + 0].x = uv2Scale * vectA.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 0].y = uv2Scale * vectA.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

                    endUv2[4 * i + 1].x = uv2Scale * vectB.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 1].y = uv2Scale * vectB.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

                    endUv2[4 * i + 2].x = uv2Scale * vectC.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 2].y = uv2Scale * vectC.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

                    endUv2[4 * i + 3].x = uv2Scale * vectD.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
                    endUv2[4 * i + 3].y = uv2Scale * vectD.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;
                }
			}

			//match UV2 to UV1?
			if(CurrentTextInfo.isQuad){ //quad silhouette?
				if(!CurrentTextInfo.quadData.silhouette){
					endUv2[4*i + 0] = endUv[4*i+0]; //same
					endUv2[4*i + 1] = endUv[4*i+1];
					endUv2[4*i + 2] = endUv[4*i+2];
					endUv2[4*i + 3] = endUv[4*i+3];
				}
			}

            //Color data:
            if (CurrentTextInfo.isQuad && !CurrentTextInfo.quadData.silhouette)
            { //if it's a quad but not a silhouette
                endCol32[4 * i + 0] = Color.white; //set color to be white, so it doesn't interfere with texture
                endCol32[4 * i + 1] = Color.white;
                endCol32[4 * i + 2] = Color.white;
                endCol32[4 * i + 3] = Color.white;
            }
            else if (CurrentTextInfo.gradientData != null)
            { //gradient speed + gradient spread
                if (CurrentTextInfo.gradientData.scrollSpeed != 0)
                {
                    areWeAnimating = true;
                }
                switch (CurrentTextInfo.gradientData.direction)
                {
                    case STMGradientData.GradientDirection.Vertical:
                        switch (CurrentTextInfo.gradientData.smoothGradient)
                        {
                            case false: //hard gradient
                                endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                break;
                            default:
                                endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + ((CurrentTextInfo.pos.y + CurrentTextInfo.size) * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + ((CurrentTextInfo.pos.y + CurrentTextInfo.size) * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                break;
                        }
                        break;
                    default: //horizontal
                        switch (CurrentTextInfo.gradientData.smoothGradient)
                        {
                            case false:
                                endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f)); //this works!
                                endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                break;
                            default://smooth gradient
                                endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f)); //this works!
                                endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 1].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 2].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size), 1f));
                                endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 3].x * CurrentTextInfo.gradientData.gradientSpread / info[i].size), 1f));
                                break;
                        }
                        break;
                }
            }
            else if (CurrentTextInfo.textureData != null)
            { //it has a texture
                endCol32[4 * i + 0] = Color.white; //set color to be white, so it doesn't interfere with texture
                endCol32[4 * i + 1] = Color.white;
                endCol32[4 * i + 2] = Color.white;
                endCol32[4 * i + 3] = Color.white;
            }
            else if (CurrentTextInfo.colorData != null)
            { //use colordata
                endCol32[4 * i + 0] = CurrentTextInfo.colorData.color;
                endCol32[4 * i + 1] = CurrentTextInfo.colorData.color;
                endCol32[4 * i + 2] = CurrentTextInfo.colorData.color;
                endCol32[4 * i + 3] = CurrentTextInfo.colorData.color;
            }
            else
            { //use default color
                endCol32[4 * i + 0] = color;
                endCol32[4 * i + 1] = color;
                endCol32[4 * i + 2] = color;
                endCol32[4 * i + 3] = color;
            }
		}

        //If you want to modify vertices (curve, offset, etc) you can do it directly, here
        //ApplyCurveToVertices(endVerts);
        if((onVertexMod != null && onVertexMod.GetPersistentEventCount() > 0) || OnVertexMod != null){
			//Debug.Log("Updating vertex mod");
            if (UpdateMesh_Middles.Length != hyphenedText.Length)
                Array.Resize(ref UpdateMesh_Middles, hyphenedText.Length); //Update the array with the middle of each letter

            if (UpdateMesh_Positions.Length != hyphenedText.Length)
                Array.Resize(ref UpdateMesh_Positions, hyphenedText.Length);

			for(int i=0, iL=hyphenedText.Length; i<iL; i++){
                UpdateMesh_Middles[i] = info[i].Middle;
                UpdateMesh_Positions[i] = info[i].pos;
			}
			if(onVertexMod != null) onVertexMod.Invoke(endVerts, UpdateMesh_Middles, UpdateMesh_Positions); //modify end verts externally
			if(OnVertexMod != null) OnVertexMod.Invoke(endVerts, UpdateMesh_Middles, UpdateMesh_Positions);
			areWeAnimating = true; //just in case, so things like the sketch effect work.
		}
		//TODO: assign normals by hand instead of using this. but really, whatever. You dont need normals.
		//mesh.RecalculateNormals(); //2016-07-05 i dont need to do this
		//}
		if(data.disableAnimatedText || disableAnimatedText){
			areWeAnimating = false; //override constant updates
		}
		//else{
			//mesh.Optimize(); //not sure if this would actually help, since verts will rarely be shared
		//}
		//return mesh;
	}
	void SetMesh(float timeValue){
		SetMesh(timeValue, false);
	}
	//actually update the mesh attached to the meshfilter
	void SetMesh(float timeValue, bool undrawingMesh){ //0 == start mesh, < 0 == end mesh, > 0 == midway mesh
		if(textMesh == null){
			textMesh = new Mesh(); //create the mesh initially
			textMesh.MarkDynamic(); //just do it
		}
		textMesh.Clear();
		if(text.Length > 0){
			if(reading || unreading){ //which set to use...?
				UpdateDrawnMesh(timeValue, undrawingMesh);
				textMesh.vertices = midVerts;
				textMesh.colors32 = midCol32;
			}else if(timeValue == 0f || undrawingMesh){//show nothing
				UpdatePreReadMesh(undrawingMesh); //pas this so it know which animation to use. always renders a pre-read mesh
				textMesh.vertices = startVerts;
				textMesh.colors32 = startCol32;
				//Debug.Log("showing empty");
			}else{
				UpdateMesh(totalReadTime+1f);
				textMesh.vertices = endVerts;
				textMesh.colors32 = endCol32;
				//Debug.Log("showing filled");
			}

			textMesh.uv = endUv; //this technically only needs to be set on Rebuild()
			textMesh.uv2 = endUv2; //use 2nd texture...
			//textMesh.uv3 = ratios; //for new shader!
			textMesh.SetUVs(2, ratiosAndUvMids);

			//apply tris and submeshes
			if(subMeshes.Count > 1){ //this also only needs to be set on Rebuild()
                //use submeshes instead of setting triangles for entire mesh:
                textMesh.subMeshCount = subMeshes.Count;
                for(int i=0, iL=subMeshes.Count; i<iL; i++){
                    textMesh.SetTriangles(subMeshes[i].tris, i); //apply to mesh
                }
            }else if(subMeshes.Count > 0){
				//do it this way because of errors with quads
                textMesh.subMeshCount = 1;
                //set triangles for entire mesh:
                //textMesh.triangles = subMeshes[0].tris.ToArray();
				//Debug.Log(subMeshes.Count);
                textMesh.SetTriangles(subMeshes[0].tris, 0); //causes less garbage?
			}
			//else, do nothing!!
			//textMesh.UploadMeshData(false); //send to graphics API manually...?
		}
		ApplyMesh();
	}
	void ApplyMesh(){
		if(uiMode){ //UI mode
			c.SetMesh(textMesh);
		}else{
			f.sharedMesh = textMesh; //I dont think this has to be set multiple times but w/e
		}
	}

	void ClearMaterials(){
		//clear r.sharedMaterials, here
		if(uiMode){
			for(int i=0, iL=c.materialCount; i<iL; i++){
				DestroyImmediate(c.GetMaterial(i));
			}
			c.materialCount = 0;
		}else{
			for(int i=0, iL=r.sharedMaterials.Length; i<iL; i++){
				DestroyImmediate(r.sharedMaterials[i]);
			}
		}
	}
	private Material[] newMats;
	void ApplyMaterials(){ //turn submesh data into material data
		//do a check first to see if materials need to change
		ClearMaterials();

		newMats = new Material[subMeshes.Count];
		for(int i=0, iL=newMats.Length; i<iL; i++){
			newMats[i] = subMeshes[i].AsMaterial;
			//different details will have to be set here,
		}
		if(uiMode){//for now, simple way to disallow multiple materials on canvas, since it seems to cause a crash
			//2017-02-12 fixed it??
			//2017-04-14 FIXED IT
			if(this != null && t.gameObject.activeInHierarchy){ //prevents text from rendering weird
				#if UNITY_2017_1_OR_NEWER
				Canvas parentCanvas = t.GetComponentInParent<Canvas>();
				if(parentCanvas != null) parentCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
				c.materialCount = newMats.Length+1;
				for(int j=0; j<c.materialCount-1; j++){
					c.SetMaterial(newMats[j],j);
				}
				#else
				//only show 1st material, multi materials were not supported before 2017.1
				c.materialCount = 1;
				c.SetMaterial(newMats[0],0);
				#endif
			}
		}else{
			r.sharedMaterials = newMats; //update!
		}
		#if UNITY_EDITOR
		HideInspectorStuff(); //this is the only time you're really gonna need this, so OnValidate() makes sense...?
		#endif
	}
	SubMeshData DoesSubmeshExist(SuperTextMesh stm, STMTextInfo info){ //find a submesh that this character can exist on
		for(int i=0, iL=subMeshes.Count; i<iL; i++){
			bool safe = true;

			if(info.materialData != null){ //it has material data?
				if(subMeshes[i].refMat != info.materialData.material){ //if the two materials dont match
					safe = false;
				}
			}else{ //there's no material data on this letter, so compare to STM default
				if(subMeshes[i].refMat != stm.textMaterial){ //
					safe = false;
				}
			}

			if(info.textureData != null){ //there's texture data?
				if(subMeshes[i].refMask != info.textureData.texture){ //if the two textures dont match...
					//return subMeshes[i];
					//Debug.Log("Existing textures dont match.");
					safe = false; //not the same!
				}
			}else{
				//vvv check for this, since quads can use the refmask, too
				if(info.quadData == null && subMeshes[i].refMask != null){ //if this submesh has texture data, is not null too
					safe = false;
					//Debug.Log("non-Existing textures dont match.");
				}
			}

			if(info.fontData != null){
				if(subMeshes[i].refFont != info.fontData.font){
					//return subMeshes[i];
					safe = false;
					//Debug.Log("Existing fonts dont match.");
				}
			}
			//submesh data ALWAYS has font, fontdata might not
			else{ //no fontdata on the mesh?
				if(subMeshes[i].refFont != stm.font){
					safe = false;
					//Debug.Log("non-Existing fonts dont match.");
				}
			}
			//TODO: check for silhouette differences, too?
			if(info.isQuad){ //if it has quad data 
				if(subMeshes[i].refTex != info.quadData.texture){ //if the two textures aren't the same...
					safe = false;
				}
				if((subMeshes[i].refTex == subMeshes[i].refMask) == info.quadData.silhouette){ //if they're not both a silhouette
					safe = false;
				}
			}else{ //no quad data
				if(subMeshes[i].refTex != null){ //but the submesh does have it
					safe = false;
				}
			}
			

			if(safe){
				return subMeshes[i]; //the two submeshes are the same!
			}
		}
		//return new SubMeshData(stm, info);
		return null;
	}

	//ILayoutElement Stuff. Content Size Fitter.
	//i Don't think these two are needed since accesors are used
	public virtual void CalculateLayoutInputHorizontal() {}
    public virtual void CalculateLayoutInputVertical() {}
	public virtual float minWidth{
		get { return 0; }
	}
	public virtual float preferredWidth{
        get{
            return unwrappedBottomRightTextBounds.x;
			//return (float)tr.rect.width;
        }
    }
	public virtual float flexibleWidth { get { return -1; } }
	public virtual float minHeight{
        get { return 0; }
    }
	public virtual float preferredHeight{
		get{
			//Rebuild();
			return -rawBottomRightTextBounds.y;
			//return (float)tr.rect.height;
		}
	}
	public virtual float flexibleHeight { 
		get { 
			return -rawBottomRightTextBounds.y; 
		} 
	}
	public virtual int layoutPriority { get { return 0; } }
	
}
[System.Serializable]
public class SubMeshData { //used internally by STM for keeping track of submeshes
	//public string name;
	public List<int> tris = new List<int>(); 
	public Material refMat; //material these tris will reference
	public Font refFont; //maybe make these FontData, TextureData, ShaderData?
	public Texture refTex; //for quads/inline images
	public Texture refMask; //masks/textures/non-silhouette quads
	public Vector2 maskTiling;
	public FilterMode refFilter;
	
	public SubMeshData(SuperTextMesh stm){ //create default
		this.refMat = stm.textMaterial; //default text material
		this.refFont = stm.font;
		this.refFilter = stm.filterMode;
	}
	public SubMeshData(SuperTextMesh stm, STMTextInfo info){ //from different data types
		//this.refMask = texData.texture;
		this.refMat = info.materialData != null ? info.materialData.material : stm.textMaterial;
		this.refFont = info.fontData != null ? info.fontData.font : stm.font;
		//this.refFilter = info.quadData != null ? info.quadData.filterMode : info.fontData != null ? info.fontData.overrideFilterMode ? info.fontData.filterMode : stm.filterMode;
		//this one's so long... just write it out this way
		if(info.isQuad){
			if(info.quadData.overrideFilterMode){
				this.refFilter = info.quadData.filterMode;
			}else{
				this.refFilter = stm.filterMode;
			}
		}else if(info.fontData != null){
			if(info.fontData.overrideFilterMode){
				this.refFilter = info.fontData.filterMode;
			}else{
				this.refFilter = stm.filterMode;
			}
		}else{
			this.refFilter = stm.filterMode;
		}
		this.refMask = info.textureData != null ? info.textureData.texture : null;
		this.maskTiling = info.textureData != null ? info.textureData.tiling : Vector2.one;
		if(info.isQuad && !info.quadData.silhouette){ //nah, use quad instead...
			this.refMask = info.quadData.texture;
		}
		this.refTex = info.isQuad ? info.quadData.texture : null;
	}
	public Material AsMaterial{
		get{
			//create new material
			Material newMat = new Material(refMat.shader);
			newMat.CopyPropertiesFromMaterial(refMat);
			newMat.SetTexture("_MainTex", refTex ?? refFont.material.mainTexture); //go w/ reftex unless its null, then use font
			newMat.SetTexture("_MaskTex", refMask);
			newMat.SetTextureScale("_MaskTex", maskTiling);
			newMat.mainTexture.filterMode = refFilter;
			return newMat;
		}
	}
	//LayoutElement Garbage

	//public virtual void CalculateLayoutInputHorizontal() {}
    //public virtual void CalculateLayoutInputVertical() {}
	//public float
}
