namespace GoogleMobileAds.Api.Mediation.HyprMX
{
    using UnityEngine;
    using System.Runtime.InteropServices;
    public class HyprMXAdapterConfiguration
    {
        static public void SetHasUserConsent(bool hasUserConsent)
        {
#if (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            HyprMXExterns.HYPRUMHyprMXSetConsent(hasUserConsent);
#elif UNITY_ANDROID
            HyprMXAndroidAdapter.SetHasUserConsent(hasUserConsent);
#endif
        }
    }

#if (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
    // Externs used by the iOS component.
    internal class HyprMXExterns
    {
        [DllImport("__Internal")]
        internal static extern void HYPRUMHyprMXSetConsent(bool hasUserConsent);
    }
#endif

#if UNITY_ANDROID
    internal class HyprMXAndroidAdapter {
        public static void SetHasUserConsent(bool hasUserConsent)
        {
            try
            {
                using var singletonClass = new AndroidJavaClass("com.hyprmx.android.HyprMXAdapterConfiguration");
                using var instance = singletonClass.GetStatic<AndroidJavaObject>("INSTANCE");
                // The parameter type is java.lang.Boolean, not primitive boolean.
                // The following code creates a java.lang.Boolean object so the method can be resolved correctly.
                AndroidJavaObject javaBoolean = new AndroidJavaObject("java.lang.Boolean", hasUserConsent); 
                instance.Call("setHasUserConsent", javaBoolean); 
            }
            catch (AndroidJavaException e)
            {
                Debug.LogError("HyprMX: Error setting user consent: " + e.Message);
            }
        }
    }
#endif
}