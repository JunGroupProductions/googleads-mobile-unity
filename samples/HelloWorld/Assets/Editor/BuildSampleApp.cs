using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
using static UnityEditor.AssetDatabase;
using UnityEditor.Build.Reporting;
#endif

// Output the build size or a failure depending on BuildPlayer.

public class BuildSampleApp : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Build/iOS")]
#endif
    static void PerformiOSBuild ()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/iOS",BuildTarget.iOS,BuildOptions.None);
    }
    
#if UNITY_EDITOR
    [MenuItem("Build/Create Android Project")]
#endif
    static void BuildAndroidProject ()
    {
        PerformAndroidBuild("Builds/Android", BuildOptions.AcceptExternalModificationsToPlayer);
    }
    
#if UNITY_EDITOR
    [MenuItem("Build/Build Android APK")]
#endif
    static void BuildAndroidAPK ()
    {
        PerformAndroidBuild("Builds/AdMobUnityAndroid.apk", BuildOptions.None);
    }
    
    static void PerformAndroidBuild (string outputLocation, BuildOptions options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        
        PlayerSettings.Android.forceSDCardPermission = true;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = outputLocation;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.scenes = GetScenePaths();
        buildPlayerOptions.options = options;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static string[] GetScenePaths()
    {
        return new string[] {"Assets/Scenes/MainScene.unity" };
    }
}