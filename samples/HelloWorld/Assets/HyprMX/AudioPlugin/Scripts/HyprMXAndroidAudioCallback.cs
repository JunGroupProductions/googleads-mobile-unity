#if UNITY_ANDROID

using System;
using UnityEngine;
using System.Runtime.InteropServices;

class HyprMXAndroidAudioCallback : AndroidJavaProxy
{
    private const string hyprMXClassName = "com.hyprmx.android.sdk.core.HyprMX";

    public HyprMXAndroidAudioCallback() : base("com.hyprmx.android.sdk.core.HyprMXIf$HyprMXAudioAdListener") {
        try
        {
            Debug.Log("[HyprMXAndroidAudioCallback] setAudioAdListener");
            GetHyprMX().Call("setAudioAdListener", this);
        }
        catch (Exception e)
        {
            Debug.Log("[HyprMXAndroidAudioCallback] error in setAudioAdListener:" + e);
        }
     }

    public void onAdAudioStart() {
        HyprMXAudioEventBus.Instance.onAudioStart();
    }
    
    public void onAdAudioEnd()
    {
        HyprMXAudioEventBus.Instance.onAudioEnd();
    }

    internal static AndroidJavaObject GetHyprMX()
    {
        return new AndroidJavaClass(hyprMXClassName).GetStatic<AndroidJavaObject>("INSTANCE");
    }
}
#endif