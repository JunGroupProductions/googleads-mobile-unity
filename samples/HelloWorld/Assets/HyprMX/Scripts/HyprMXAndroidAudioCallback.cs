#if UNITY_ANDROID

using UnityEngine;
using System.Runtime.InteropServices;

class HyprMXAndroidAudioCallback : AndroidJavaProxy
{
    private const string hyprMXClassName = "com.hyprmx.android.sdk.core.HyprMX";

    public HyprMXAndroidAudioCallback() : base("com.hyprmx.android.sdk.core.HyprMXIf.HyprMXAudioAdListener") {
        GetHyprMX().Call("setAudioAdListener", this);
        HyprMXAudioEventBus.Instance.onAudioStart();
     }

    public void onAdAudioStart() {
        Debug.Log("ENTER callback onAdAudioStart");
        HyprMXAudioEventBus.Instance.onAudioStart();
    }
    
    public void onAdAudioEnd()
    {
        Debug.Log("ENTER callback onAdAudioEnd");
        HyprMXAudioEventBus.Instance.onAudioEnd();
    }

    internal static AndroidJavaObject GetHyprMX()
    {
        return new AndroidJavaClass(hyprMXClassName).GetStatic<AndroidJavaObject>("INSTANCE");
    }
}
#endif