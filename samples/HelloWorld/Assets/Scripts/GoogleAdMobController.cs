using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using GoogleMobileAds.Api.Mediation.HyprMX;

public class GoogleAdMobController : MonoBehaviour
{
    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private float deltaTime;
    private bool isShowingAppOpenAd;
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;
    public bool showFpsMeter = true;
    public Text fpsMeter;
    public Text statusText;

    private string customUserId = "custom_user_id";
    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        // Add some test device IDs (replace with your own device IDs).
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

#elif UNITY_ANDROID
        deviceIds.Add("C3E75CF03C0BF982C374667B1BD825AF");
        deviceIds.Add("5D29DB113CD64CBC402BE865D8B3D6C1");
        deviceIds.Add("D9566E99EE1A058EF2FAC80EC4933EAB");
        deviceIds.Add("3BD3FA6FEA24486E7667706E000FD41C");
        deviceIds.Add("1B8FE35DA1F36CFBEE0D24CA63AB6001");
        deviceIds.Add("7DB92CC400AA8089309F717A9C777446");
#endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);

        GUI.Label(new Rect(10, Screen.height - 70, Screen.width - 1, Screen.height - 50), "Custom User ID:");

        customUserId = GUI.TextField(new Rect(10, Screen.height - 45, Screen.width - 1, Screen.height - 25), customUserId, 25);

        if (GUI.Button(new Rect((Screen.width/2) - 40, (Screen.height - 20), 60, 20), "Consent Given"))
        {
            HyprMXAdapterConfiguration.SetHasUserConsent(true);
            HyprMXAdapterConfiguration.SetUserId(customUserId);
        }

        if (GUI.Button(new Rect((Screen.width / 2) + 40, (Screen.height - 20), 60, 20), "Consent Declined"))
        {
            HyprMXAdapterConfiguration.SetHasUserConsent(false);
            HyprMXAdapterConfiguration.SetUserId(customUserId);
        }
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            statusText.text = "Initialization complete";
            SetCustomId();
        });
    }

    private void Update()
    {
        if (showFpsMeter)
        {
            fpsMeter.gameObject.SetActive(true);
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsMeter.text = string.Format("{0:0.} fps", fps);
        }
        else
        {
            fpsMeter.gameObject.SetActive(false);
        }
    }


    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .Build();
    }

    public void OnApplicationPause(bool paused)
    {
        // Display the app open ad when the app is foregrounded.
        if (!paused)
        {
            ShowAppOpenAd();
        }
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {
        statusText.text = "Requesting Banner Ad.";

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-6816316814885940/3848484471";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-6816316814885940/5339483948";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        statusText.text = "Requesting Interstitial Ad.";

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-6816316814885940/1709490738";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-6816316814885940/7896249896";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            statusText.text = "Interstitial ad is not ready yet";
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        statusText.text = "Requesting Rewarded Ad.";
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-6816316814885940/3865412331";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-6816316814885940/6132545386";
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
        rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
        else
        {
            statusText.text = "Rewarded ad is not ready yet.";
        }
    }

    public void RequestAndLoadRewardedInterstitialAd()
    {
        statusText.text = "Requesting Rewarded Interstitial Ad.";

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
        {
            if (error != null)
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "RewardedInterstitialAd load failed, error: " + error;
                });
                return;
            }
            this.rewardedInterstitialAd = rewardedInterstitialAd;
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                statusText.text = "RewardedInterstitialAd loaded";
            });
            // Register for ad events.
            this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "Rewarded Interstitial presented.";
                });
            };
            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "Rewarded Interstitial dismissed.";
                });
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "Rewarded Interstitial failed to present.";
                });
                this.rewardedInterstitialAd = null;
            };
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((reward) => {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "User Rewarded: " + reward.Amount;
                });
            });
        }
        else
        {
            statusText.text = "Rewarded ad is not ready yet.";
        }
    }

    #endregion

    #region APPOPEN ADS

    public void RequestAndLoadAppOpenAd()
    {
        statusText.text = "Requesting App Open Ad.";
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/3419835294";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/5662855259";
#else
        string adUnitId = "unexpected_platform";
#endif
        // create new app open ad instance
        AppOpenAd.LoadAd(adUnitId, ScreenOrientation.Portrait, CreateAdRequest(), (appOpenAd, error) =>
        {
            if (error != null)
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "AppOpenAd load failed, error: " + error;
                });
                return;
            }
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                statusText.text = "AppOpenAd loaded. Please background the app and return.";
            });
            this.appOpenAd = appOpenAd;
        });
    }

    public void ShowAppOpenAd()
    {
        if (isShowingAppOpenAd)
        {
            return;
        }
        if (appOpenAd == null)
        {
            return;
        }
        // Register for ad events.
        this.appOpenAd.OnAdDidDismissFullScreenContent += (sender, args) =>
        {
            isShowingAppOpenAd = false;
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                Debug.Log("AppOpenAd dismissed.");
                if (this.appOpenAd != null)
                {
                    this.appOpenAd.Destroy();
                    this.appOpenAd = null;
                }
            });
        };
        this.appOpenAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
        {
            isShowingAppOpenAd = false;
            var msg = args.AdError.GetMessage();
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                statusText.text = "AppOpenAd present failed, error: " + msg;
                if (this.appOpenAd != null)
                {
                    this.appOpenAd.Destroy();
                    this.appOpenAd = null;
                }
            });
        };
        this.appOpenAd.OnAdDidPresentFullScreenContent += (sender, args) =>
        {
            isShowingAppOpenAd = true;
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                Debug.Log("AppOpenAd presented.");
            });
        };
        this.appOpenAd.OnAdDidRecordImpression += (sender, args) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                Debug.Log("AppOpenAd recorded an impression.");
            });
        };
        this.appOpenAd.OnPaidEvent += (sender, args) =>
        {
            string currencyCode = args.AdValue.CurrencyCode;
            long adValue = args.AdValue.Value;
            string suffix = "AppOpenAd received a paid event.";
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                string msg = string.Format("{0} (currency: {1}, value: {2}", suffix, currencyCode, adValue);
                statusText.text = msg;
            });
        };
        appOpenAd.Show();
    }

    #endregion

    public void ConsentGiven()
    {
        HyprMXAdapterConfiguration.SetHasUserConsent(true);
        statusText.text = "Consent Set to Granted";
    }

    public void ConsentDeclined()
    {
        HyprMXAdapterConfiguration.SetHasUserConsent(false);
        statusText.text = "Consent Set to Declined";
    }

    public void SetCustomId()
    {
        string result = "";
        int length = 15;
        for (int i = 0; i < length; i++)
        {
            char c = (char)('A' + UnityEngine.Random.Range(0, 26));
            result += c;
        }
        HyprMXAdapterConfiguration.SetUserId(result);
        statusText.text = "Setting custom user id " + result;
    }

    #region AD INSPECTOR

    public void OpenAdInspector()
    {
        statusText.text = "Open Ad Inspector.";

        MobileAds.OpenAdInspector((error) =>
        {
            if (error != null)
            {
                string errorMessage = error.GetMessage();
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "Ad Inspector failed to open, error: " + errorMessage;
                });
            }
            else
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                    statusText.text = "Ad Inspector closed.";
                });
            }
        });
    }

    #endregion
}
