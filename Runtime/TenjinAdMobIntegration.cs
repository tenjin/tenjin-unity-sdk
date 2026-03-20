//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class TenjinAdMobIntegration
{
    private static bool _subscribed_banner = false;
    private static bool _subscribed_rewarded = false;
    private static bool _subscribed_interstitial = false;
    private static bool _subscribed_rewarded_interstitial = false;

    public TenjinAdMobIntegration() { }

    public static void ListenForBannerViewImpressions(object bannerView, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        GoogleMobileAds.Api.BannerView newBannerView = (GoogleMobileAds.Api.BannerView)bannerView;
        SubscribeToAd(newBannerView, adUnitId, callback, ref _subscribed_banner, (ad, args) =>
        {
            newBannerView.OnAdPaid += (adValue) => HandleAdPaid(newBannerView.GetResponseInfo(), adValue, adUnitId, callback);
        });
#endif
    }

    public static void ListenForRewardedAdImpressions(object rewardedAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        GoogleMobileAds.Api.RewardedAd newRewardedAd = (GoogleMobileAds.Api.RewardedAd)rewardedAd;
        SubscribeToAd(newRewardedAd, adUnitId, callback, ref _subscribed_rewarded, (ad, args) =>
        {
            newRewardedAd.OnAdPaid += (adValue) => HandleAdPaid(newRewardedAd.GetResponseInfo(), adValue, adUnitId, callback);
        });
#endif
    }

    public static void ListenForInterstitialAdImpressions(object interstitialAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        GoogleMobileAds.Api.InterstitialAd newInterstitialAd = (GoogleMobileAds.Api.InterstitialAd)interstitialAd;
        SubscribeToAd(newInterstitialAd, adUnitId, callback, ref _subscribed_interstitial, (ad, args) =>
        {
            newInterstitialAd.OnAdPaid += (adValue) => HandleAdPaid(newInterstitialAd.GetResponseInfo(), adValue, adUnitId, callback);
        });
#endif
    }

    public static void ListenForRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId, Action<string> callback)
    {
#if tenjin_admob_enabled
        GoogleMobileAds.Api.RewardedInterstitialAd newRewardedInterstitialAd = (GoogleMobileAds.Api.RewardedInterstitialAd)rewardedInterstitialAd;
        SubscribeToAd(newRewardedInterstitialAd, adUnitId, callback, ref _subscribed_rewarded_interstitial, (ad, args) =>
        {
            newRewardedInterstitialAd.OnAdPaid += (adValue) => HandleAdPaid(newRewardedInterstitialAd.GetResponseInfo(), adValue, adUnitId, callback);
        });
#endif
    }

#if tenjin_admob_enabled
    private static void SubscribeToAd<T>(T ad, string adUnitId, Action<string> callback, ref bool subscribedFlag, Action<T, GoogleMobileAds.Api.AdValue> adPaidHandler)
    {
        if (subscribedFlag)
        {
            Debug.Log($"Ignoring duplicate {typeof(T).Name} subscription");
            return;
        }

        adPaidHandler(ad, null);

        subscribedFlag = true;
    }

    private static void HandleAdPaid(GoogleMobileAds.Api.ResponseInfo responseInfo, GoogleMobileAds.Api.AdValue adValue, string adUnitId, Action<string> callback)
    {
        if (responseInfo != null && adValue != null)
        {
            String adResponseId = responseInfo.GetResponseId();
            try
            {
                AdMobImpressionDataToJSON adMobImpressionDataToJSON = new AdMobImpressionDataToJSON()
                {
                    ad_unit_id = adUnitId,
    #if UNITY_ANDROID
                    value_micros = adValue.Value,
    #elif UNITY_IPHONE
                    value_micros = (adValue.Value / 1000000.0),
    #else
                    value_micros = adValue.Value,
    #endif
                    currency_code = adValue.CurrencyCode,
                    response_id = adResponseId,
                    precision_type = adValue.Precision.ToString(),
                    mediation_adapter_class_name = responseInfo.GetMediationAdapterClassName()
                };
                string json = JsonUtility.ToJson(adMobImpressionDataToJSON);
                callback(json);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error parsing ad impression: {ex.ToString()}");
            }
        }
    }
#endif
}

[System.Serializable]
internal class AdMobImpressionDataToJSON
{
    public string currency_code;
    public string ad_unit_id;
    public string response_id;
    public string precision_type;
    public string mediation_adapter_class_name;
#if UNITY_IPHONE
    public double value_micros;
#elif UNITY_ANDROID
    public long value_micros;
#else
    public long value_micros;
#endif
}
