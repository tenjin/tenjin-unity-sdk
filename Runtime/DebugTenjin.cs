//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;
using System;
using System.Collections.Generic;

public class DebugTenjin : BaseTenjin
{

    public override void Connect()
    {
        Debug.Log("Connecting " + ApiKey);
    }

    public override void Connect(string deferredDeeplink)
    {
        Debug.Log("Connecting with deferredDeeplink " + deferredDeeplink);
    }

    public override void Init(string apiKey)
    {
        Debug.Log("Initializing - v" + this.SdkVersion);
    }

    public override void InitWithSharedSecret(string apiKey, string sharedSecret)
    {
        Debug.Log("Initializing with Shared Secret - v" + this.SdkVersion);
    }

    public override void InitWithAppSubversion(string apiKey, int appSubversion)
    {
        Debug.Log("Initializing with App Subversion: " + appSubversion + " v" + this.SdkVersion);
    }

    public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
    {
        Debug.Log("Initializing with Shared Secret + App Subversion: " + appSubversion + " v" + this.SdkVersion);
    }

    public override void SendEvent(string eventName)
    {
        Debug.Log("Sending Event " + eventName);
    }

    public override void SendEvent(string eventName, string eventValue)
    {
        Debug.Log("Sending Event " + eventName + " : " + eventValue);
    }

    public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
    {
        Debug.Log("Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
    }

    public override void TransactionAmazon(string productId, string currencyCode, int quantity, double unitPrice, string receiptId, string userId)
    {
        Debug.Log("Transaction Amazon" + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + receiptId + ", " + userId);
    }

    public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
    {
        Debug.Log("Sending DebugTenjin::GetDeeplink");
    }

    public override void GetAttributionInfo(Tenjin.AttributionInfoDelegate attributionInfoDelegate)
    {
        Debug.Log("Sending DebugTenjin::GetAttributionInfo");
    }

    public override void OptIn()
    {
        Debug.Log("OptIn ");
    }

    public override void OptOut()
    {
        Debug.Log("OptOut ");
    }

    public override void OptInParams(List<string> parameters)
    {
        Debug.Log("OptInParams");
    }

    public override void OptOutParams(List<string> parameters)
    {
        Debug.Log("OptOutParams");
    }

    public override bool OptInOutUsingCMP()
    {
        Debug.Log("OptInOutUsingCMP");
        return true;
    }

    public override void OptInGoogleDMA()
    {
        Debug.Log("OptInGoogleDMA");
    }

    public override void OptOutGoogleDMA()
    {
        Debug.Log("OptOutGoogleDMA");
    }

    public override void DebugLogs()
    {
        Debug.Log("Setting debug logs ");
    }

    public override void AppendAppSubversion(int subversion)
    {
        Debug.Log("AppendAppSubversion: " + subversion);
    }

    private void ImpressionHandler(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

    public override void SubscribeAppLovinImpressions()
    {
        Debug.Log("Subscribing to applovin impressions");
        TenjinAppLovinIntegration.ListenForImpressions(AppLovinImpressionHandler);
    }

    public override void AppLovinImpressionFromJSON(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

    private void AppLovinImpressionHandler(string json)
    {
        Debug.Log($"Got applovin impression data {json}");
    }

    public override void SubscribeIronSourceImpressions()
    {
        Debug.Log("Subscribing to ironsource impressions");
        TenjinIronSourceIntegration.ListenForImpressions(IronSourceImpressionHandler);
    }

    public override void IronSourceImpressionFromJSON(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

    public override void SubscribeLevelPlayRewardedAdImpressions(object rewardedAd)
    {
        Debug.Log("Subscribing to LevelPlay rewarded impressions");
        TenjinIronSourceIntegration.SubscribeLevelPlayRewardedImpressions(rewardedAd, IronSourceImpressionHandler);
    }

    public override void SubscribeLevelPlayInterstitialAdImpressions(object interstitialAd)
    {
        Debug.Log("Subscribing to LevelPlay interstitial impressions");
        TenjinIronSourceIntegration.SubscribeLevelPlayInterstitialImpressions(interstitialAd, IronSourceImpressionHandler);
    }

    public override void SubscribeLevelPlayBannerAdImpressions(object bannerAd)
    {
        Debug.Log("Subscribing to LevelPlay banner impressions");
        TenjinIronSourceIntegration.SubscribeLevelPlayBannerImpressions(bannerAd, IronSourceImpressionHandler);
    }

    private void IronSourceImpressionHandler(string json)
    {
        Debug.Log($"Got ironsource impression data {json}");
    }

    public override void SubscribeHyperBidImpressions()
    {
        Debug.Log("Subscribing to hyperbid impressions");
        TenjinHyperBidIntegration.ListenForImpressions(HyperBidImpressionHandler);
    }

    public override void HyperBidImpressionFromJSON(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

    private void HyperBidImpressionHandler(string json)
    {
        Debug.Log($"Got hyperbid impression data {json}");
    }

    public override void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId)
    {
        Debug.Log("Subscribing to admob bannerView impressions");
        TenjinAdMobIntegration.ListenForBannerViewImpressions(bannerView, adUnitId, AdMobBannerViewImpressionHandler);
    }

    public override void SubscribeAdMobRewardedAdImpressions(object rewardedAd, string adUnitId)
    {
        Debug.Log("Subscribing to admob rewardedAd impressions");
        TenjinAdMobIntegration.ListenForRewardedAdImpressions(rewardedAd, adUnitId, AdMobRewardedAdImpressionHandler);
    }

    public override void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId)
    {
        Debug.Log("Subscribing to admob interstitialAd impressions");
        TenjinAdMobIntegration.ListenForInterstitialAdImpressions(interstitialAd, adUnitId, AdMobInterstitialAdImpressionHandler);
    }

    public override void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId)
    {
        Debug.Log("Subscribing to admob rewardedInterstitialAd impressions");
        TenjinAdMobIntegration.ListenForRewardedInterstitialAdImpressions(rewardedInterstitialAd, adUnitId, AdMobRewardedInterstitialAdImpressionHandler);
    }

    public override void AdMobImpressionFromJSON(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

    private void AdMobBannerViewImpressionHandler(string json)
    {
        Debug.Log($"Got admob bannerView impression data {json}");
    }

    private void AdMobRewardedAdImpressionHandler(string json)
    {
        Debug.Log($"Got admob rewardedAd impression data {json}");
    }

    private void AdMobInterstitialAdImpressionHandler(string json)
    {
        Debug.Log($"Got admob interstitialAd impression data {json}");
    }

    private void AdMobRewardedInterstitialAdImpressionHandler(string json)
    {
        Debug.Log($"Got admob rewardedInterstitialAd impression data {json}");
    }

    public override void RegisterAppForAdNetworkAttribution()
    {
        Debug.Log("RegisterAppForAdNetworkAttribution");
    }

    public override void UpdateConversionValue(int conversionValue)
    {
        Debug.Log("UpdateConversionValue: " + conversionValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue)
    {
        Debug.Log("UpdatePostbackConversionValue: " + conversionValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
    {
        Debug.Log("UpdatePostbackConversionValueCoarseValue: " + conversionValue + coarseValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
    {
        Debug.Log("UpdatePostbackConversionValueCoarseValueLockWindow: " + conversionValue + coarseValue + lockWindow);
    }

    public override void SubscribeTopOnImpressions()
    {
        Debug.Log("Subscribing to topon impressions");
        TenjinTopOnIntegration.ListenForImpressions(TopOnImpressionHandler);
    }

    public override void TopOnImpressionFromJSON(string json)
    {
        Debug.Log($"Got impression data {json}");
    }

    private void TopOnImpressionHandler(string json)
    {
        Debug.Log($"Got topon impression data {json}");
    }

    public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback)
    {
        Debug.Log("RequestTrackingAuthorizationWithCompletionHandler");
    }

    public override void SetAppStoreType(AppStoreType appStoreType)
    {
        Debug.Log("SetAppStoreType");
    }

    public override void SetCustomerUserId(string userId)
    {
        Debug.Log("SetCustomerUserId");
    }

    public override string GetCustomerUserId()
    {
        Debug.Log("GetCustomerUserId");
        return "";
    }

    public override void SetSessionTime(int time)
    {
        Debug.Log("SetSessionTime");
    }

    public override void SetCacheEventSetting(bool setting)
    {
        Debug.Log("SetCacheEventSetting");
    }

    public override void SetEncryptRequestsSetting(bool setting)
    {
        Debug.Log("SetEncryptRequestsSetting");
    }

    public override void SetGoogleDMAParameters(bool adPersonalization, bool adUserData)
    {
        Debug.Log("SetGoogleDMAParameters");
    }

    public override string GetAnalyticsInstallationId()
    {
        Debug.Log("GetAnalyticsInstallationId");
        return "";
    }

    public override void CASImpressionFromJSON(string json)
    {
        Debug.Log("CASImpressionFromJSON");
    }

    public override void SubscribeCASImpressions(object casManager)
    {
        Debug.Log("SubscribeCASImpressions");
    }

    public override void SubscribeCASBannerImpressions(object bannerView)
    {
        Debug.Log("SubscribeCASBannerImpressions");
    }

    public override void SubscribeTradPlusImpressions()
    {
        Debug.Log("SubscribeTradPlusImpressions");
    }

    public override void TradPlusImpressionFromJSON(string json)
    {
        Debug.Log("TradPlusImpressionFromJSON");
    }

    public override void TradPlusImpressionFromAdInfo(Dictionary<string, object> adInfo)
    {
        Debug.Log("TradPlusImpressionFromAdInfo");
    }

    public override Dictionary<string, string> GetUserProfileDictionary()
    {
        Debug.Log("GetUserProfileDictionary");
        return new Dictionary<string, string>();
    }

    public override void ResetUserProfile()
    {
        Debug.Log("ResetUserProfile");
    }
}
