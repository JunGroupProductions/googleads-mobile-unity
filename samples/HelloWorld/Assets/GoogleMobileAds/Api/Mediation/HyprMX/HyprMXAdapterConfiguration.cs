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
        static public void SetUserId(string customUserId)
        {
#if (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            HyprMXExterns.HYPRUMHyprMXSetCustomUserId(customUserId);
#elif UNITY_ANDROID
            HyprMXAndroidAdapter.SetUserId(customUserId);
#endif
        }
    }

#if (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
    // Externs used by the iOS component.
    internal class HyprMXExterns
    {
        [DllImport("__Internal")]
        internal static extern void HYPRUMHyprMXSetConsent(bool hasUserConsent);

        [DllImport("__Internal")]
        internal static extern void HYPRUMHyprMXSetCustomUserId(string customUserId);
    }
#endif

#if UNITY_ANDROID
    internal class HyprMXAndroidAdapter {
        private const string adapterClassName = "com.hyprmx.android.HyprMXAdapterConfiguration";

        public static void SetUserId(string userId)
        {
            GetAdapter().Call<AndroidJavaObject>("setUserId", userId);
        }

        public static void SetHasUserConsent(bool hasUserConsent)
        {
            GetAdapter().Call<AndroidJavaObject>("setHasUserConsent", hasUserConsent);
        }

        internal static AndroidJavaClass GetAdapter()
        {
            return new AndroidJavaClass(adapterClassName);
        }
    }
#endif
}
