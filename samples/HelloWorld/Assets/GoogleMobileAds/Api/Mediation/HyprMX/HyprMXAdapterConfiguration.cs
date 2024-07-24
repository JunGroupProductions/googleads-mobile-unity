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
        private const string adapterClassName = "com.hyprmx.android.HyprMXAdapterConfiguration";

        public static void SetHasUserConsent(bool hasUserConsent)
        {
            GetAdapter().Call("setHasUserConsent", hasUserConsent);
        }

        internal static AndroidJavaObject GetAdapter()
        {
            return new AndroidJavaClass(adapterClassName).GetStatic<AndroidJavaObject>("INSTANCE");
        }
    }
#endif
}
