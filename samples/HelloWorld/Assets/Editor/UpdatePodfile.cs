using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using System.IO;
#if UNITY_EDITOR_OSX
public class PostProcessIOS : MonoBehaviour {
	[PostProcessBuild(45)]//must be between 40 and 50 to ensure that it's not overriden by Podfile generation (40) and that it's added before "pod install" (50)
	private static void PostProcessBuild_ModifyPodfile(BuildTarget target, string buildPath)
	{
	    if (target == BuildTarget.iOS)
	    {

	    	string[] linesArray = File.ReadAllLines(buildPath + "/Podfile");
	        linesArray[0] = "source 'https://cdn.cocoapods.org/'";
	        linesArray[1] = "source 'git@github.com:JunGroupProductions/Private-Cocoapod-Specs.git'";
	        File.WriteAllLines(buildPath + "/Podfile", linesArray);
	    }
	}
}
#endif