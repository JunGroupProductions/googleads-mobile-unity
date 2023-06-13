using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

namespace GoogleMobileAds.Samples
{
    /// <summary>
    /// Demonstrates how to use the Google Mobile Ads MobileAds Instance.
    /// </summary>
    [AddComponentMenu("GoogleMobileAds/Samples/GoogleMobileAdsController")]
    public class GoogleMobileAdsController : MonoBehaviour
    {
        private static bool _isInitialized;
        public static bool tagAsAgeRestrictedUser = false;

        /// <summary>
        /// Initializes the MobileAds SDK
        /// </summary>
        private void Start()
        {
            // On Android, Unity is paused when displaying interstitial or rewarded video.
            // This setting makes iOS behave consistently with Android.
            MobileAds.SetiOSAppPauseOnBackground(true);

            // When true all events raised by GoogleMobileAds will be raised
            // on the Unity main thread. The default value is false.
            // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
        }

        public void Initialize()
        {
            Debug.Log("[GMAC] Google Mobile Ads attempting to initialize.");
            // Demonstrates how to configure Google Mobile Ads.
            // Google Mobile Ads needs to be run only once and before loading any ads.
            if (_isInitialized)
            {
                Debug.Log("[GMAC] Google Mobile Ads has already been initialized.");
                return;
            }

            // Set your test devices.
            // https://developers.google.com/admob/unity/test-ads
            List<string> deviceIds = new List<string>()
            {
                AdRequest.TestDeviceSimulator,
                // Add your test device IDs (replace with your own device IDs).
                #if UNITY_IPHONE
                "96e23e80653bb28980d3f40beb58915c"
                #elif UNITY_ANDROID
                "75EF8D155528C04DACBBA6F36F433035"
                #endif
            };

            #if UNITY_IPHONE
                deviceIds.Add("d0f6835328b6e25db17827f558da4a57");
                deviceIds.Add("c7a8402daaab2bfbbc11c00789bfbb9a");
                deviceIds.Add("ce3f3d3df7dec8363692d9c350ea3273");
                deviceIds.Add("9b8e7cd0c8e533bea624be402e1b63f4");
                deviceIds.Add("cae20e92e4de34c05a3baf4469821bd5");
                deviceIds.Add("25dd40f317a693a87415825c74a92f7c");
                deviceIds.Add("c15544b80963f07e83aa64404f3f0472");
                deviceIds.Add("5297e92a7b704e109894f9f93a5b95c1");
                deviceIds.Add("d3ee944ef42a161ca72ad64f8917d326");
                deviceIds.Add("77690d2b93df46c31704fedd60fe15c6");
                deviceIds.Add("40fbde9b6b858720ecd6b2dd08e708a7");
                deviceIds.Add("44da8626d67dd4526be5917a53f5270f");
                deviceIds.Add("01cd2aaa7dc1d607fa63504f0e0d436d");
                deviceIds.Add("12586e42584a2a4522183a0adbacdaa1");

            #elif UNITY_ANDROID
                deviceIds.Add("C3E75CF03C0BF982C374667B1BD825AF");
                deviceIds.Add("5D29DB113CD64CBC402BE865D8B3D6C1");
                deviceIds.Add("D9566E99EE1A058EF2FAC80EC4933EAB");
                deviceIds.Add("3BD3FA6FEA24486E7667706E000FD41C");
                deviceIds.Add("1B8FE35DA1F36CFBEE0D24CA63AB6001");
                deviceIds.Add("7DB92CC400AA8089309F717A9C777446");
                deviceIds.Add("55D3A1B733C833634A41F448F48F88A6");
                deviceIds.Add("702A2A19985F51314F4FC8B6A76A1862");
                deviceIds.Add("6661C5F3A526FBBC132ED66199BA7520");
                deviceIds.Add("ED9B35B446489DECD2F4822E784CEE0E");
                deviceIds.Add("54473E0666467B55E52B9591BD1FAED7");
            #endif

            // Configure your RequestConfiguration with Child Directed Treatment
            // and the Test Device Ids.
            RequestConfiguration requestConfiguration = new RequestConfiguration
            {
                TestDeviceIds = deviceIds,
                TagForChildDirectedTreatment = TagForChildDirectedTreatment.True
            };

            MobileAds.SetRequestConfiguration(requestConfiguration);

            // Initialize the Google Mobile Ads SDK.
            Debug.Log("[GMAC] Google Mobile Ads Initializing.");
            MobileAds.Initialize((InitializationStatus initstatus) =>
            {
                if (initstatus == null)
                {
                    Debug.LogError("[GMAC] Google Mobile Ads initialization failed.");
                    return;
                }

                // If you use mediation, you can check the status of each adapter.
                var adapterStatusMap = initstatus.getAdapterStatusMap();
                if (adapterStatusMap != null)
                {
                    foreach (var item in adapterStatusMap)
                    {
                        Debug.Log(string.Format("[GMAC] Adapter {0} is {1}",
                            item.Key,
                            item.Value.InitializationState));
                    }
                }

                Debug.Log("[GMAC] Google Mobile Ads initialization complete.");
                _isInitialized = true;
            });
        }

        /// <summary>
        /// Opens the AdInspector.
        /// </summary>
        public void OpenAdInspector()
        {
            Debug.Log("[GMAC] Opening ad Inspector.");
            MobileAds.OpenAdInspector((AdInspectorError error) =>
            {
                // If the operation failed, an error is returned.
                if (error != null)
                {
                    Debug.Log("[GMAC] Ad Inspector failed to open with error: " + error);
                    return;
                }

                Debug.Log("[GMAC] Ad Inspector opened successfully.");
            });
        }
    }
}
