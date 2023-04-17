using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Runtime.InteropServices;

public interface IHyprMXAudioListener
{
    void onAdAudioStart();
    void onAdAudioEnd();
}

#if (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
internal class HyprMXNativeAudioExtern
    {
    [DllImport("__Internal")]
    internal static extern void HyprMXUConnectAudioListener(bool shouldConnect);
    }
#endif

public class HyprMXAudioEventBus : MonoBehaviour
{
    private static HyprMXAudioEventBus _instance;

    private static readonly object Lock = new object();

    public IHyprMXAudioListener listener;
    public static HyprMXAudioEventBus Instance { 
        get
        {
            if (Quitting)
            {
                return null;
            }
            lock (Lock)
            {
                if (_instance != null) 
                {
                    return _instance;
                }
                var instances = FindObjectsOfType<HyprMXAudioEventBus>();
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                        return Instance = instances[0];
                    Debug.LogWarning($"[HyprMXAudioEventBus] There should never be more than one HyprMXAudioEventBus in the scene, but {count} were found.");
                    for (var i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }
                Debug.Log("[HyprMXAudioEventBus] initializing");
                return Instance = new GameObject("HyprMXAudioEventBus", typeof(HyprMXAudioEventBus)).AddComponent<HyprMXAudioEventBus>();
            }
        }

        private set 
        {
            Debug.Log("[HyprMXAudioEventBus] connect to native");
            connectNativeListener();
        }
     }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static bool Quitting { get; private set; }
    private void OnApplicationQuit()
    {
        Quitting = true;
    }

    private static void connectNativeListener() {
#if (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            Debug.Log("[HyprMXAudioEventBus] invoking iOS layer connector");
            HyprMXNativeAudioExtern.HyprMXUConnectAudioListener(true);
#elif UNITY_ANDROID
            new HyprMXAndroidAudioCallback();
#endif
    }

    public void onAudioStart() 
    {
        Debug.Log("[HyprMXAudioEventBus] onAudioStart()");
        this.listener?.onAdAudioStart();
    }

     public void onAudioEnd() 
    {
        Debug.Log("[HyprMXAudioEventBus] onAudioEnd()");
        this.listener?.onAdAudioEnd();
    }
}