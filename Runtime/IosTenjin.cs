using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

public class IosTenjin : BaseTenjin
{

#if UNITY_IPHONE && !UNITY_EDITOR

    [DllImport ("__Internal")]
    private static extern void iosTenjinInit(string apiKey);

    [DllImport ("__Internal")]
    private static extern void iosTenjinInitWithSharedSecret(string apiKey, string sharedSecret);

    [DllImport ("__Internal")]
    private static extern void iosTenjinInitWithAppSubversion(string apiKey, int appSubversion);

    [DllImport ("__Internal")]
    private static extern void iosTenjinInitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion);

    [DllImport ("__Internal")]
    private static extern void iosTenjinConnect();

    [DllImport ("__Internal")]
    private static extern void iosTenjinConnectWithDeferredDeeplink(string deferredDeeplink);

    [DllImport ("__Internal")]
    private static extern void iosTenjinOptIn();

    [DllImport ("__Internal")]
    private static extern void iosTenjinOptOut();

    [DllImport ("__Internal")]
    private static extern void iosTenjinOptInParams(String[] parameters, int size);

    [DllImport ("__Internal")]
    private static extern void iosTenjinOptOutParams(String[] parameters, int size);

    [DllImport ("__Internal")]
    private static extern bool iosTenjinOptInOutUsingCMP();

    [DllImport ("__Internal")]
    private static extern void iosTenjinOptInGoogleDMA();

    [DllImport ("__Internal")]
    private static extern void iosTenjinOptOutGoogleDMA();

    [DllImport ("__Internal")]
    private static extern void iosTenjinRegisterAppForAdNetworkAttribution();
    
    [DllImport ("__Internal")]
    private static extern void iosTenjinUpdateConversionValue(int conversionValue);

    [DllImport ("__Internal")]
    private static extern void iosTenjinUpdatePostbackConversionValue(int conversionValue);

    [DllImport ("__Internal")]
    private static extern void iosTenjinUpdatePostbackConversionValueCoarseValue(int conversionValue, string coarseValue);

    [DllImport ("__Internal")]
    private static extern void iosTenjinUpdatePostbackConversionValueCoarseValueLockWindow(int conversionValue, string coarseValue, bool lockWindow);

    [DllImport ("__Internal")]
    private static extern void iosTenjinRequestTrackingAuthorizationWithCompletionHandler();

    [DllImport ("__Internal")]
    private static extern void iosTenjinAppendAppSubversion(int subversion);

    [DllImport ("__Internal")]
    private static extern void iosTenjinSendEvent(string eventName);

    [DllImport ("__Internal")]
    private static extern void iosTenjinSendEventWithValue(string eventName, string eventValue);

    [DllImport ("__Internal")]
    private static extern void iosTenjinTransaction(string productId, string currencyCode, int quantity, double unitPrice);

    [DllImport ("__Internal")]
    private static extern void iosTenjinTransactionWithReceiptData(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt);

    [DllImport ("__Internal")]
     private static extern void iosTenjinRegisterDeepLinkHandler(DeepLinkHandlerNativeDelegate deepLinkHandlerNativeDelegate);

    [DllImport ("__Internal")]
     private static extern void iosTenjinGetAttributionInfo(AttributionInfoNativeDelegate attributionInfoNativeDelegate);

    [DllImport ("__Internal")]
    private static extern void iosTenjinAppLovinImpressionFromJSON(string jsonString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinIronSourceImpressionFromJSON(string jsonString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinHyperBidImpressionFromJSON(string jsonString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinAdMobImpressionFromJSON(string jsonString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinTopOnImpressionFromJSON(string jsonString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinCASImpressionFromJSON(string jsonString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinTradPlusImpressionFromJSON(string jsonString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinSetDebugLogs();

    [DllImport ("__Internal")]
    private static extern void iosTenjinSetWrapperVersion(string wrapperString);

    [DllImport ("__Internal")]
    private static extern void iosTenjinSetCustomerUserId(string userId);

    [DllImport ("__Internal")]
    private static extern string iosTenjinGetCustomerUserId();

    [DllImport ("__Internal")]
    private static extern void iosTenjinSetCacheEventSetting(bool setting);

    [DllImport ("__Internal")]
    private static extern void iosTenjinSetEncryptRequestsSetting(bool setting);

    [DllImport ("__Internal")]
    private static extern void iosTenjinSetGoogleDMAParameters(bool adPersonalization, bool adUserData);

    [DllImport ("__Internal")]
    private static extern string iosTenjinGetAnalyticsInstallationId();

    [DllImport ("__Internal")]
    private static extern void iosTenjinGetUserProfileDictionary(UserProfileNativeDelegate userProfileNativeDelegate);

    [DllImport ("__Internal")]
    private static extern void iosTenjinResetUserProfile();

    private delegate void DeepLinkHandlerNativeDelegate(IntPtr deepLinkDataPairArray, int deepLinkDataPairCount);
    private delegate void AttributionInfoNativeDelegate(IntPtr attributionInfoDataPairArray, int attributionInfoDataPairCount);
    private delegate void UserProfileNativeDelegate(IntPtr userProfileDataPairArray, int userProfileDataPairCount);
    
    private static readonly Stack<Dictionary<string, string>> deferredDeeplinkEvents = new Stack<Dictionary<string, string>>();
    private static readonly Stack<Dictionary<string, string>> attributionInfoEvents = new Stack<Dictionary<string, string>>();
    private static readonly Stack<Dictionary<string, string>> userProfileEvents = new Stack<Dictionary<string, string>>();

    private static Tenjin.DeferredDeeplinkDelegate registeredDeferredDeeplinkDelegate;
    private static Tenjin.AttributionInfoDelegate registeredAttributionInfoDelegate;

    private static Dictionary<string, string> cachedUserProfile = null;

    public override void Init(string apiKey)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS Initializing - v"+this.SdkVersion);
        }
        SetUnityVersionInNativeSDK();
        ApiKey = apiKey;
        iosTenjinInit (ApiKey);
    }

    public override void InitWithSharedSecret(string apiKey, string sharedSecret)
    {
        if (Debug.isDebugBuild) {
            Debug.Log("iOS Initializing with Shared Secret - v"+this.SdkVersion);
        }
        SetUnityVersionInNativeSDK();
        ApiKey = apiKey;
        SharedSecret = sharedSecret;
        iosTenjinInitWithSharedSecret (ApiKey, SharedSecret);
    }

    public override void InitWithAppSubversion(string apiKey, int appSubversion)
    {
        if (Debug.isDebugBuild) {
            Debug.Log("iOS Initializing with App Subversion: " + appSubversion + " v" +this.SdkVersion);
        }
        SetUnityVersionInNativeSDK();
        ApiKey = apiKey;
        AppSubversion = appSubversion;
        iosTenjinInitWithAppSubversion (ApiKey, AppSubversion);
    }

    public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
    {
        if (Debug.isDebugBuild) {
            Debug.Log("iOS Initializing with Shared Secret + App Subversion: " + appSubversion +" v" +this.SdkVersion);
        }
        SetUnityVersionInNativeSDK();
        ApiKey = apiKey;
        SharedSecret = sharedSecret;
        AppSubversion = appSubversion;
        iosTenjinInitWithSharedSecretAppSubversion (ApiKey, SharedSecret, AppSubversion);
    }

    private void SetUnityVersionInNativeSDK()
    {
        var unitySdkVersion = this.SdkVersion + "u";

        iosTenjinSetWrapperVersion(unitySdkVersion);
    }

    public override void Connect()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS Connecting");
        }
        iosTenjinConnect();
    }
    
    public override void Connect(string deferredDeeplink)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS Connecting with deferredDeeplink " + deferredDeeplink);
        }
        iosTenjinConnectWithDeferredDeeplink (deferredDeeplink);
    }

    public override void OptIn()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS OptIn");
        }
        iosTenjinOptIn ();
    }

    public override void OptOut()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS OptOut");
        }
        iosTenjinOptOut ();
    }
    
    public override void OptInParams(List<string> parameters)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS OptInParams" + parameters.ToString());
        }
        iosTenjinOptInParams (parameters.ToArray(), parameters.Count);
    }

    public override void OptOutParams(List<string> parameters)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS OptOutParams" + parameters.ToString());
        }
        iosTenjinOptOutParams (parameters.ToArray(), parameters.Count);
    }

    public override bool OptInOutUsingCMP()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS OptInOutUsingCMP");
        }
        return iosTenjinOptInOutUsingCMP ();
    }

    public override void OptInGoogleDMA()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS OptInGoogleDMA");
        }
        iosTenjinOptInGoogleDMA ();
    }

    public override void OptOutGoogleDMA()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS OptOutGoogleDMA");
        }
        iosTenjinOptOutGoogleDMA ();
    }

    public override void RegisterAppForAdNetworkAttribution()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS RegisterAppForAdNetworkAttribution");
        }
        iosTenjinRegisterAppForAdNetworkAttribution ();
    }

    public override void UpdateConversionValue(int conversionValue)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS UpdateConversionValue");
        }
        iosTenjinUpdateConversionValue (conversionValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS UpdatePostbackConversionValue");
        }
        iosTenjinUpdatePostbackConversionValue (conversionValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS UpdatePostbackConversionValueCoarseValue");
        }
        iosTenjinUpdatePostbackConversionValueCoarseValue (conversionValue, coarseValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS UpdatePostbackConversionValueCoarseValueLockWindow");
        }
        iosTenjinUpdatePostbackConversionValueCoarseValueLockWindow (conversionValue, coarseValue, lockWindow);
    }

    public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS RequestTrackingAuthorizationWithCompletionHandler");
        }
        Tenjin.authorizationStatusDelegate = trackingAuthorizationCallback;
        iosTenjinRequestTrackingAuthorizationWithCompletionHandler();
    }

    private void SetTrackingAuthorizationStatus(string status)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS SetTrackingAuthorizationStatus " + status);
        }
        Tenjin.authorizationStatusDelegate(Int16.Parse(status));
    }

    public override void AppendAppSubversion(int appSubversion)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS AppendAppSubversion " + appSubversion);
        }
        iosTenjinAppendAppSubversion (appSubversion);
    }

    public override void SendEvent(string eventName)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS Sending Event " + eventName);
        }
        iosTenjinSendEvent(eventName);
    }

    public override void SendEvent(string eventName, string eventValue)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("iOS Sending Event " + eventName + " : " + eventValue);
        }
        iosTenjinSendEventWithValue(eventName, eventValue);
    }

    public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
    {
        signature = null;

        //only if the receipt and transaction_id are not null, then try to validate the transaction. Otherwise manually record the transaction
        if(receipt != null && transactionId != null){
            if (Debug.isDebugBuild) {
                Debug.Log ("iOS Transaction with receipt " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt);
            }
            iosTenjinTransactionWithReceiptData(productId, currencyCode, quantity, unitPrice, transactionId, receipt);
        }
        else{
            if (Debug.isDebugBuild) {
                Debug.Log ("iOS Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice);
            }
            iosTenjinTransaction(productId, currencyCode, quantity, unitPrice);
        }
    }

    public override void TransactionAmazon(string productId, string currencyCode, int quantity, double unitPrice, string receiptId, string userId)
    {
    }

    public override void SetAppStoreType(AppStoreType appStoreType)
    {
    }

    public override void SubscribeAppLovinImpressions()
    {
        TenjinAppLovinIntegration.ListenForImpressions(AppLovinILRDHandler);
    }

    public override void AppLovinImpressionFromJSON(string json)
    {
        AppLovinILRDHandler(json);
    }

    public void AppLovinILRDHandler(string json) {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got AppLovin ILRD impression data {json}");
            iosTenjinAppLovinImpressionFromJSON(json);
        }
    }

    public override void SubscribeIronSourceImpressions()
    {
        TenjinIronSourceIntegration.ListenForImpressions(IronSourceILRDHandler);
    }

    public override void SubscribeLevelPlayRewardedAdImpressions(object rewardedAd)
    {
        Debug.Log("Subscribing to LevelPlay rewarded ILRD");
        TenjinIronSourceIntegration.SubscribeLevelPlayRewardedImpressions(rewardedAd, IronSourceILRDHandler);
    }

    public override void SubscribeLevelPlayInterstitialAdImpressions(object interstitialAd)
    {
        Debug.Log("Subscribing to LevelPlay interstitial ILRD");
        TenjinIronSourceIntegration.SubscribeLevelPlayInterstitialImpressions(interstitialAd, IronSourceILRDHandler);
    }

    public override void SubscribeLevelPlayBannerAdImpressions(object bannerAd)
    {
        Debug.Log("Subscribing to LevelPlay banner ILRD");
        TenjinIronSourceIntegration.SubscribeLevelPlayBannerImpressions(bannerAd, IronSourceILRDHandler);
    }

    public override void IronSourceImpressionFromJSON(string json)
    {
        IronSourceILRDHandler(json);
    }

    public void IronSourceILRDHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got IronSource ILRD impression data {json}");
            iosTenjinIronSourceImpressionFromJSON(json);
        }
    }

    public override void SubscribeHyperBidImpressions()
    {
        TenjinHyperBidIntegration.ListenForImpressions(HyperBidILRDHandler);
    }

    public override void HyperBidImpressionFromJSON(string json)
    {
        HyperBidILRDHandler(json);
    }

    public void HyperBidILRDHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got HyperBid ILRD impression data {json}");
            iosTenjinHyperBidImpressionFromJSON(json);
        }
    }

    public override void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId)
    {
        TenjinAdMobIntegration.ListenForBannerViewImpressions(bannerView, adUnitId, AdMobBannerViewILRDHandler);
    }

    public override void SubscribeAdMobRewardedAdImpressions(object rewardedAd,string adUnitId)
    {
        TenjinAdMobIntegration.ListenForRewardedAdImpressions(rewardedAd, adUnitId, AdMobRewardedAdILRDHandler);
    }

    public override void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId)
    {
        TenjinAdMobIntegration.ListenForInterstitialAdImpressions(interstitialAd, adUnitId, AdMobInterstitialAdILRDHandler);
    }

    public override void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId)
    {
        TenjinAdMobIntegration.ListenForRewardedInterstitialAdImpressions(rewardedInterstitialAd, adUnitId, AdMobRewardedInterstitialAdILRDHandler);
    }

    public override void AdMobImpressionFromJSON(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got AdMob ILRD impression data {json}");
            iosTenjinAdMobImpressionFromJSON(json);
        }
    }

    public void AdMobBannerViewILRDHandler(string json){
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got AdMob BannerView ILRD impression data {json}");
            iosTenjinAdMobImpressionFromJSON(json);
        }
    }

    public void AdMobRewardedAdILRDHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got AdMob RewardedAd ILRD impression data {json}");
            iosTenjinAdMobImpressionFromJSON(json);
        }
    }

    public void AdMobInterstitialAdILRDHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got AdMob InterstitialAd ILRD impression data {json}");
            iosTenjinAdMobImpressionFromJSON(json);
        }
    }

    public void AdMobRewardedInterstitialAdILRDHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got AdMob RewardedInterstitialAd ILRD impression data {json}");
            iosTenjinAdMobImpressionFromJSON(json);
        }
    }

    public override void SubscribeTopOnImpressions()
    {
        TenjinTopOnIntegration.ListenForImpressions(TopOnILRDHandler);
    }

    public override void TopOnImpressionFromJSON(string json)
    {
        TopOnILRDHandler(json);
    }

    public void TopOnILRDHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got TopOn ILRD impression data {json}");
            iosTenjinTopOnImpressionFromJSON(json);
        }
    }

    public override void SubscribeCASImpressions(object casManager)
    {
        TenjinCASIntegration.ListenForImpressions(CASILDRHandler, casManager);
    }

    public override void SubscribeCASBannerImpressions(object casBannerView)
    {
        TenjinCASIntegration.ListenForBannerImpressions(CASILDRHandler, casBannerView);
    }

    public override void CASImpressionFromJSON(string json)
    {
        CASILDRHandler(json);
    }

    public void CASILDRHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got CAS ILRD impression data {json}");
            iosTenjinCASImpressionFromJSON(json);
        }
    }

    public override void SubscribeTradPlusImpressions()
    {
        TenjinTradPlusIntegration.ListenForImpressions(TradPlusILRDHandler);
    }

    public override void TradPlusImpressionFromJSON(string json)
    {
        TradPlusILRDHandler(json);
    }

    public override void TradPlusImpressionFromAdInfo(Dictionary<string, object> adInfo)
    {
        TenjinTradPlusIntegration.ImpressionFromAdInfo(TradPlusILRDHandler, adInfo);
    }

    private void TradPlusILRDHandler(string json)
    {
        if(!string.IsNullOrEmpty(json))
        {
            Debug.Log($"Got TradPlus ILRD impression data {json}");
            iosTenjinTradPlusImpressionFromJSON(json);
        }
    }

    public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::GetDeeplink");
        }
        registeredDeferredDeeplinkDelegate = deferredDeeplinkDelegate;
        iosTenjinRegisterDeepLinkHandler(DeepLinkHandler);
    }

    public override void GetAttributionInfo(Tenjin.AttributionInfoDelegate attributionInfoDelegate)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::GetAttributionInfo");
        }
        registeredAttributionInfoDelegate = attributionInfoDelegate;
        iosTenjinGetAttributionInfo(AttributionInfo);
    }

    public override void SetCustomerUserId(string userId)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::SetCustomerUserId");
        }
        iosTenjinSetCustomerUserId(userId);
    }

    public override string GetCustomerUserId()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::GetCustomerUserId");
        }
        return iosTenjinGetCustomerUserId();
    }

    public override void SetSessionTime(int time)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::SetSessionTime");
        }
    }

    public override void SetCacheEventSetting(bool setting)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::SetCacheEventSetting");
        }
        iosTenjinSetCacheEventSetting(setting);
    }

    public override void SetEncryptRequestsSetting(bool setting)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::SetEncryptRequestsSetting");
        }
        iosTenjinSetEncryptRequestsSetting(setting);
    }

    public override string GetAnalyticsInstallationId()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::GetAnalyticsInstallationId");
        }
        return iosTenjinGetAnalyticsInstallationId();
    }

    public override void SetGoogleDMAParameters(bool adPersonalization, bool adUserData)
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::SetGoogleDMAParameters");
        }
        iosTenjinSetGoogleDMAParameters(adPersonalization, adUserData);
    }

    public override Dictionary<string, string> GetUserProfileDictionary()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::GetUserProfileDictionary");
        }
        // Call native function which will populate the cache via callback
        iosTenjinGetUserProfileDictionary(UserProfileHandler);
        // Wait briefly for callback to populate cache
        System.Threading.Thread.Sleep(10);
        // Process any pending events
        lock (userProfileEvents) {
            while (userProfileEvents.Count > 0) {
                cachedUserProfile = userProfileEvents.Pop();
            }
        }
        return cachedUserProfile ?? new Dictionary<string, string>();
    }

    public override void ResetUserProfile()
    {
        if (Debug.isDebugBuild) {
            Debug.Log ("Sending IosTenjin::ResetUserProfile");
        }
        iosTenjinResetUserProfile();
        cachedUserProfile = null;
    }

    public override void DebugLogs()
    {
        iosTenjinSetDebugLogs();
    }

    private void Update()
    {
        lock (deferredDeeplinkEvents) {
            while (deferredDeeplinkEvents.Count > 0) {
                Dictionary<string, string> deepLinkData = deferredDeeplinkEvents.Pop();
                if (registeredDeferredDeeplinkDelegate != null) {
                    registeredDeferredDeeplinkDelegate(deepLinkData);
                }
            }
        }
        lock (attributionInfoEvents) {
            while (attributionInfoEvents.Count > 0) {
                Dictionary<string, string> attributionInfoData = attributionInfoEvents.Pop();
                if (registeredAttributionInfoDelegate != null) {
                    registeredAttributionInfoDelegate(attributionInfoData);
                }
            }
        }
    }

    [MonoPInvokeCallback(typeof(DeepLinkHandlerNativeDelegate))]
    private static void DeepLinkHandler(IntPtr deepLinkDataPairArray, int deepLinkDataPairCount)
    {
        if (deepLinkDataPairArray == IntPtr.Zero)
            return;

        Dictionary<string, string> deepLinkData = 
            NativeUtility.MarshalStringStringDictionary(deepLinkDataPairArray, deepLinkDataPairCount);

        lock (deferredDeeplinkEvents) {
            deferredDeeplinkEvents.Push(deepLinkData);
        }
    }

    [MonoPInvokeCallback(typeof(AttributionInfoNativeDelegate))]
    private static void AttributionInfo(IntPtr attributionInfoDataPairArray, int attributionInfoDataPairCount)
    {
        if (attributionInfoDataPairArray == IntPtr.Zero)
            return;

        Dictionary<string, string> attributionInfoData =
            NativeUtility.MarshalStringStringDictionary(attributionInfoDataPairArray, attributionInfoDataPairCount);

        lock (attributionInfoEvents) {
            attributionInfoEvents.Push(attributionInfoData);
        }
    }

    [MonoPInvokeCallback(typeof(UserProfileNativeDelegate))]
    private static void UserProfileHandler(IntPtr userProfileDataPairArray, int userProfileDataPairCount)
    {
        if (userProfileDataPairArray == IntPtr.Zero)
            return;

        Dictionary<string, string> userProfileData =
            NativeUtility.MarshalStringStringDictionary(userProfileDataPairArray, userProfileDataPairCount);

        lock (userProfileEvents) {
            userProfileEvents.Push(userProfileData);
        }
    }

    private static class NativeUtility {
        /// <summary>
        /// Marshals a native linear array of structs to the managed array.
        /// </summary>
        public static T[] MarshalNativeStructArray<T>(IntPtr nativeArrayPtr, int nativeArraySize) where T : struct
        {
            if (nativeArrayPtr == IntPtr.Zero)
                throw new ArgumentNullException("nativeArrayPtr");

            if (nativeArraySize < 0)
                throw new ArgumentOutOfRangeException("nativeArraySize");

            T[] managedArray = new T[nativeArraySize];
            IntPtr currentNativeArrayPtr = nativeArrayPtr;
            int structSize = Marshal.SizeOf(typeof(T));
            for (int i = 0; i < nativeArraySize; i++) {
                T marshaledStruct = (T) Marshal.PtrToStructure(currentNativeArrayPtr, typeof(T));
                managedArray[i] = marshaledStruct;
                currentNativeArrayPtr = (IntPtr) (currentNativeArrayPtr.ToInt64() + structSize);
            }

            return managedArray;
        }
        
        /// <summary>
        /// Marshals the native representation to a IDictionary&lt;string, string&gt;.
        /// </summary>
        public static Dictionary<string, string> MarshalStringStringDictionary(IntPtr nativePairArrayPtr, int nativePairArraySize)
        {
            if (nativePairArrayPtr == IntPtr.Zero)
                throw new ArgumentNullException("nativePairArrayPtr");

            if (nativePairArraySize < 0)
                throw new ArgumentOutOfRangeException("nativePairArraySize");

            Dictionary<string, string> dictionary = new Dictionary<string, string>(nativePairArraySize);
            StringStringKeyValuePair[] pairs = MarshalNativeStructArray<StringStringKeyValuePair>(nativePairArrayPtr, nativePairArraySize);
            foreach (StringStringKeyValuePair pair in pairs) {
                dictionary.Add(pair.Key, pair.Value);
            }
            return dictionary;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct StringStringKeyValuePair
        {
            public string Key;
            public string Value;
        }
    }

#else
        public override void Init(string apiKey)
        {
                Debug.Log("iOS Initializing - v" + this.SdkVersion);
                ApiKey = apiKey;
        }

        public override void InitWithSharedSecret(string apiKey, string sharedSecret)
        {
                Debug.Log("iOS Initializing with Shared Secret - v" + this.SdkVersion);
                ApiKey = apiKey;
                SharedSecret = sharedSecret;
        }

        public override void InitWithAppSubversion(string apiKey, int appSubversion)
        {
                Debug.Log("iOS Initializing with App Subversion: " + appSubversion + " v" + this.SdkVersion);
                ApiKey = apiKey;
                AppSubversion = appSubversion;
        }

        public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
        {
                Debug.Log("iOS Initializing with Shared Secret + App Subversion: " + appSubversion + " v" + this.SdkVersion);
                ApiKey = apiKey;
                SharedSecret = sharedSecret;
                AppSubversion = appSubversion;
        }

        public override void Connect()
        {
                Debug.Log("iOS Connecting");
        }

        public override void Connect(string deferredDeeplink)
        {
                Debug.Log("Connecting with deferredDeeplink " + deferredDeeplink);
        }

        public override void SendEvent(string eventName)
        {
                Debug.Log("iOS Sending Event " + eventName);
        }

        public override void SendEvent(string eventName, string eventValue)
        {
                Debug.Log("iOS Sending Event " + eventName + " : " + eventValue);
        }

        public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
        {
                Debug.Log("iOS Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
        }

        public override void TransactionAmazon(string productId, string currencyCode, int quantity, double unitPrice, string receiptId, string userId)
        {
                Debug.Log("iOS TransactionAmazon");
        }

        public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
        {
                Debug.Log("Sending IosTenjin::GetDeeplink");
        }

        public override void GetAttributionInfo(Tenjin.AttributionInfoDelegate attributionInfoDelegate)
        {
                Debug.Log("Sending IosTenjin::GetAttributionInfo");
        }

        public override void OptIn()
        {
                Debug.Log("iOS OptIn");
        }

        public override void OptOut()
        {
                Debug.Log("iOS OptOut");
        }

        public override void OptInParams(List<string> parameters)
        {
                Debug.Log("iOS OptInParams");
        }

        public override void OptOutParams(List<string> parameters)
        {
                Debug.Log("iOS OptOutParams");
        }

        public override bool OptInOutUsingCMP()
        {
                Debug.Log("iOS OptInOutUsingCMP");
                return true;
        }

        public override void OptInGoogleDMA()
        {
                Debug.Log("iOS OptInGoogleMDA");
        }

        public override void OptOutGoogleDMA()
        {
                Debug.Log("iOS OptOutGoogleDMA");
        }

        public override void RegisterAppForAdNetworkAttribution()
        {
                Debug.Log("iOS RegisterAppForAdNetworkAttribution");
        }

        public override void UpdateConversionValue(int conversionValue)
        {
                Debug.Log("iOS UpdateConversionValue: " + conversionValue);
        }

        public override void UpdatePostbackConversionValue(int conversionValue)
        {
                Debug.Log("iOS UpdatePostbackConversionValue: " + conversionValue);
        }

        public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
        {
                Debug.Log("iOS UpdatePostbackConversionValue: " + conversionValue + coarseValue);
        }

        public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
        {
                Debug.Log("iOS UpdatePostbackConversionValue: " + conversionValue + coarseValue + lockWindow);
        }

        public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback)
        {
                Debug.Log("iOS RequestTrackingAuthorizationWithCompletionHandler");
        }

        public override void AppendAppSubversion(int subversion)
        {
                Debug.Log("iOS AppendAppSubversion");
        }

        public override void DebugLogs()
        {
                Debug.Log("Setting debug logs ");
        }

        public override void SubscribeAppLovinImpressions()
        {
                Debug.Log("iOS SubscribeAppLovinImpressions");
        }

        public override void AppLovinImpressionFromJSON(string json)
        {
                Debug.Log("iOS AppLovinImpressionFromJSON");
        }

        public override void SubscribeIronSourceImpressions()
        {
                Debug.Log("iOS SubscribeIronSourceImpressions");
        }

        public override void SubscribeLevelPlayRewardedAdImpressions(object rewardedAd)
        {
                Debug.Log("iOS SubscribeLevelPlayRewardedAdImpressions");
        }

        public override void SubscribeLevelPlayInterstitialAdImpressions(object interstitialAd)
        {
                Debug.Log("iOS SubscribeLevelPlayInterstitialAdImpressions");
        }

        public override void SubscribeLevelPlayBannerAdImpressions(object bannerAd)
        {
                Debug.Log("iOS SubscribeLevelPlayBannerAdImpressions");
        }

        public override void IronSourceImpressionFromJSON(string json)
        {
                Debug.Log("iOS IronSourceImpressionFromJSON");
        }

        public override void SubscribeHyperBidImpressions()
        {
                Debug.Log("iOS SubscribeHyperBidImpressions");
        }

        public override void HyperBidImpressionFromJSON(string json)
        {
                Debug.Log("iOS HyperBidImpressionFromJSON");
        }

        public override void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId)
        {
                Debug.Log("iOS SubscribeAdMobBannerViewImpressions");
        }

        public override void SubscribeAdMobRewardedAdImpressions(object rewardedAd, string adUnitId)
        {
                Debug.Log("iOS SubscribeAdMobRewardedAdImpressions");
        }

        public override void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId)
        {
                Debug.Log("iOS SubscribeAdMobInterstitialAdImpressions");
        }

        public override void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId)
        {
                Debug.Log("iOS SubscribeAdMobRewardedInterstitialAdImpressions");
        }

        public override void AdMobImpressionFromJSON(string json)
        {
                Debug.Log("iOS AdMobImpressionFromJSON");
        }

        public override void SubscribeTopOnImpressions()
        {
                Debug.Log("iOS SubscribeTopOnImpressions");
        }

        public override void TopOnImpressionFromJSON(string json)
        {
                Debug.Log("iOS TopOnImpressionFromJSON");
        }

        public override void SubscribeCASImpressions(object casManager)
        {
                Debug.Log("iOS SubscribeCASImpressions");
        }

        public override void SubscribeCASBannerImpressions(object casBannerView)
        {
                Debug.Log("iOS SubscribeCASBannerImpressions");
        }

        public override void CASImpressionFromJSON(string json)
        {
                Debug.Log("iOS CASImpressionFromJSON");
        }

        public override void SubscribeTradPlusImpressions()
        {
                Debug.Log("iOS SubscribeTradPlusImpressions");
        }

        public override void TradPlusImpressionFromJSON(string json)
        {
                Debug.Log("iOS TradPlusImpressionFromJSON");
        }

        public override void TradPlusImpressionFromAdInfo(Dictionary<string, object> adInfo)
        {
                Debug.Log("iOS TradPlusImpressionFromImpression");
        }

        public override void SetAppStoreType(AppStoreType appStoreType)
        {
                Debug.Log("iOS SetAppStoreType");
        }

        public override void SetCustomerUserId(string userId)
        {
                Debug.Log("iOS SetCustomerUserId");
        }

        public override string GetCustomerUserId()
        {
                Debug.Log("iOS GetCustomerUserId");
                return "";
        }

        public override void SetSessionTime(int time)
        {
                Debug.Log("iOS SetSessionTime");
        }

        public override void SetCacheEventSetting(bool setting)
        {
                Debug.Log("iOS SetCacheEventSetting");
        }

        public override void SetEncryptRequestsSetting(bool setting)
        {
                Debug.Log("iOS SetEncryptRequestsSetting");
        }

        public override void SetGoogleDMAParameters(bool adPersonalization, bool adUserData)
        {
                Debug.Log("iOS SetGoogleDMAParameters");
        }

        public override string GetAnalyticsInstallationId()
        {
                Debug.Log("iOS GetAnalyticsInstallationId");
                return "";
        }

        public override Dictionary<string, string> GetUserProfileDictionary()
        {
                Debug.Log("iOS GetUserProfileDictionary");
                return new Dictionary<string, string>();
        }

        public override void ResetUserProfile()
        {
                Debug.Log("iOS ResetUserProfile");
        }
#endif
}
