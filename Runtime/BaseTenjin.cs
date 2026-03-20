//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseTenjin : MonoBehaviour
{

    protected string apiKey;
    protected string sharedSecret;
    protected bool optIn;
    protected bool optOut;
    protected int appSubversion;

    public string SdkVersion { get; } = "1.16.3";

    public string ApiKey
    {
        get
        {
            return this.apiKey;
        }
        set
        {
            this.apiKey = value;
        }
    }

    public string SharedSecret
    {
        get
        {
            return this.sharedSecret;
        }
        set
        {
            this.sharedSecret = value;
        }
    }

    public int AppSubversion
    {
        get
        {
            return this.appSubversion;
        }
        set
        {
            this.appSubversion = value;
        }
    }

    public abstract void Init(string apiKey);
    public abstract void InitWithSharedSecret(string apiKey, string sharedSecret);
    public abstract void InitWithAppSubversion(string apiKey, int appSubversion);
    public abstract void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion);
    public abstract void Connect();
    public abstract void Connect(string deferredDeeplink);
    public abstract void OptIn();
    public abstract void OptOut();
    public abstract void OptInParams(List<string> parameters);
    public abstract void OptOutParams(List<string> parameters);
    public abstract bool OptInOutUsingCMP();
    public abstract void OptInGoogleDMA();
    public abstract void OptOutGoogleDMA();
    public abstract void AppendAppSubversion(int subversion);
    public abstract void SendEvent(string eventName);
    public abstract void SendEvent(string eventName, string eventValue);
    public abstract void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature);
    public abstract void TransactionAmazon(string productId, string currencyCode, int quantity, double unitPrice, string receiptId, string userId);
    public abstract void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate);
    public abstract void GetAttributionInfo(Tenjin.AttributionInfoDelegate attributionInfoDelegate);
    public abstract void RegisterAppForAdNetworkAttribution();
    public abstract void UpdateConversionValue(int conversionValue);
    public abstract void UpdatePostbackConversionValue(int conversionValue);
    public abstract void UpdatePostbackConversionValue(int conversionValue, string coarseValue);
    public abstract void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow);
    public abstract void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback);
    public abstract void DebugLogs();
    public abstract void SetAppStoreType(AppStoreType appStoreType);
    public abstract void SubscribeAppLovinImpressions();
    public abstract void AppLovinImpressionFromJSON(string json);
    public abstract void SubscribeIronSourceImpressions();
    public abstract void IronSourceImpressionFromJSON(string json);
    public abstract void SubscribeLevelPlayRewardedAdImpressions(object rewardedAd);
    public abstract void SubscribeLevelPlayInterstitialAdImpressions(object interstitialAd);
    public abstract void SubscribeLevelPlayBannerAdImpressions(object bannerAd);
    public abstract void SubscribeHyperBidImpressions();
    public abstract void HyperBidImpressionFromJSON(string json);
    public abstract void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId);
    public abstract void SubscribeAdMobRewardedAdImpressions(object rewardedAd, string adUnitId);
    public abstract void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId);
    public abstract void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId);
    public abstract void AdMobImpressionFromJSON(string json);
    public abstract void SubscribeTopOnImpressions();
    public abstract void TopOnImpressionFromJSON(string json);
    public abstract void SubscribeCASImpressions(object casManager);
    public abstract void SubscribeCASBannerImpressions(object bannerView);
    public abstract void CASImpressionFromJSON(string json);
    public abstract void SubscribeTradPlusImpressions();
    public abstract void TradPlusImpressionFromJSON(string json);
    public abstract void TradPlusImpressionFromAdInfo(Dictionary<string, object> adInfo);
    public abstract void SetCustomerUserId(string userId);
    public abstract string GetCustomerUserId();
    public abstract void SetSessionTime(int time);
    public abstract void SetCacheEventSetting(bool setting);
    public abstract void SetEncryptRequestsSetting(bool setting);
    public abstract string GetAnalyticsInstallationId();
    public abstract void SetGoogleDMAParameters(bool adPersonalization, bool adUserData);
    public abstract Dictionary<string, string> GetUserProfileDictionary();
    public abstract void ResetUserProfile();
}
