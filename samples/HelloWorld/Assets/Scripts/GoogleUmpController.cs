using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api.Mediation.HyprMX;
using GoogleMobileAds.Samples;

namespace GoogleMobileAds.Sample
{
    /// <summary>
    /// Demonstrates how to use Google Mobile Ads User Messaging Platform
    /// to manage user consent and privacy settings.
    /// </summary>
    public class GoogleUmpController : MonoBehaviour
    {
        public void ConsentGiven()
        {
            Debug.Log("[Settings] SetHasUserConsent to true");
            HyprMXAdapterConfiguration.SetHasUserConsent(true);
        }
        
        public void ConsentDeclined()
        {
            Debug.Log("[Settings] SetHasUserConsent to false");
            HyprMXAdapterConfiguration.SetHasUserConsent(false);
        }

        public void TagAsAgeRestrictedUser()
        {
            Debug.Log("[Settings] tagAsAgeRestrictedUser to true");
            GoogleMobileAdsController.tagAsAgeRestrictedUser = true;
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
            Debug.Log("[Settings] Setting custom user id " + result);
        }
    }
}
