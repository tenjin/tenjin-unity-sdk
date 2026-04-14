//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AndroidTenjin : BaseTenjin
{

    private const string AndroidJavaTenjinClass = "com.tenjin.android.TenjinSDK";
    private const string AndroidJavaTenjinAppStoreType = "com.tenjin.android.TenjinSDK$AppStoreType";

#if UNITY_ANDROID && !UNITY_EDITOR
	private AndroidJavaObject tenjinJava = null;
	private AndroidJavaObject activity = null;

	public override void Init(string apiKey){
		if (Debug.isDebugBuild) {
            Debug.Log ("Android Initializing - v"+this.SdkVersion);
		}
		ApiKey = apiKey;

        SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstance", activity, apiKey);
	}

	public override void InitWithSharedSecret(string apiKey, string sharedSecret){
		if (Debug.isDebugBuild) {
            Debug.Log("Android Initializing with Shared Secret - v"+this.SdkVersion);
		}
		ApiKey = apiKey;
		SharedSecret = sharedSecret;

        SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstanceWithSharedSecret", activity, apiKey, sharedSecret);
	}

	public override void InitWithAppSubversion(string apiKey, int appSubversion){
		if (Debug.isDebugBuild) {
            Debug.Log("Android Initializing with App Subversion: " + appSubversion + " v" +this.SdkVersion);
		}
		ApiKey = apiKey;
		AppSubversion = appSubversion;

        SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstanceWithAppSubversion", activity, apiKey, appSubversion);
		tenjinJava.Call ("appendAppSubversion", new object[]{appSubversion});
	}

	public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion){
		if (Debug.isDebugBuild) {
            Debug.Log("Android Initializing with Shared Secret + App Subversion: " + appSubversion +" v" +this.SdkVersion);
		}
		ApiKey = apiKey;
		SharedSecret = sharedSecret;
		AppSubversion = appSubversion;

		SetUnityVersionInNativeSDK();

		initActivity();
		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}
		tenjinJava = sdk.CallStatic<AndroidJavaObject> ("getInstanceWithSharedSecretAppSubversion", activity, apiKey, sharedSecret, appSubversion);
		tenjinJava.Call ("appendAppSubversion", new object[]{appSubversion});
	}

    private void SetUnityVersionInNativeSDK() {
		var unitySdkVersion = this.SdkVersion + "u";

		AndroidJavaClass sdk = new AndroidJavaClass (AndroidJavaTenjinClass);
		if (sdk == null){
			throw new MissingReferenceException(
				string.Format("AndroidTenjin failed to load {0} class", AndroidJavaTenjinClass)
			);
		}

		sdk.CallStatic("setWrapperVersion", unitySdkVersion);
    }

	private void initActivity(){
		AndroidJavaClass javaContext = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = javaContext.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public override void Connect() {
		string optInOut = null;
		if (optIn) {
			optInOut = "optin";
		}
		else if (optOut) {
			optInOut = "optout";
		}
		object[] args = new object[]{null, optInOut};
		tenjinJava.Call ("connect", args);
	}

	public override void Connect(string deferredDeeplink){
		string optInOut = null;
		if (optIn) {
			optInOut = "optin";
		}
		else if (optOut) {
			optInOut = "optout";
		}
		object[] args = new object[]{deferredDeeplink, optInOut};
		tenjinJava.Call ("connect", args);
	}

	//SendEvent accepts a single eventName as a String
	public override void SendEvent (string eventName){
		object[] args = new object[]{eventName};
		tenjinJava.Call ("eventWithName", args);
	}

	//SendEvent accepts eventName as a String and eventValue as a String
	public override void SendEvent (string eventName, string eventValue){
		object[] args = new object[]{eventName, eventValue};
		tenjinJava.Call ("eventWithNameAndValue", args);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){

		transactionId = null;
		//if the receipt and signature have values then try to validate. if there are no values then manually log the transaction.
		if(receipt != null && signature != null){
			object[] receiptArgs = new object[]{productId, currencyCode, quantity, unitPrice, receipt, signature};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + receipt + ", " + signature);
			}		
			tenjinJava.Call ("transaction", receiptArgs);
		}
		else{
			object[] args = new object[]{productId, currencyCode, quantity, unitPrice};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice);
			}
			tenjinJava.Call ("transaction", args);
		}
	}

	public override void Subscription(string productId, string currencyCode, double unitPrice, string transactionId, string originalTransactionId, string receipt, string skTransaction, string purchaseToken, string purchaseData, string dataSignature){
		// TODO: uncomment when subscription is available in the Android SDK
		// if(purchaseToken != null && purchaseData != null && dataSignature != null){
		// 	if (Debug.isDebugBuild) {
		// 		Debug.Log ("Android Subscription " + productId + ", " + currencyCode + ", " + unitPrice);
		// 	}
		// 	tenjinJava.Call ("subscription", productId, currencyCode, unitPrice, purchaseToken, purchaseData, dataSignature);
		// }
		// else{
		// 	if (Debug.isDebugBuild) {
		// 		Debug.Log ("Android Subscription missing required Android parameters");
		// 	}
		// }
		if (Debug.isDebugBuild) {
			Debug.Log ("Android Subscription is not yet available");
		}
	}

	public override void SubscriptionWithStoreKit(string productId, string currencyCode, double unitPrice){
		if (Debug.isDebugBuild) {
			Debug.Log ("SubscriptionWithStoreKit is only available on iOS");
		}
	}

		public override void TransactionAmazon(string productId, string currencyCode, int quantity, double unitPrice, string receiptId, string userId){
		if(receiptId != null && userId != null){
			object[] receiptArgs = new object[]{productId, currencyCode, quantity, unitPrice, receiptId, userId, ""};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Amazon Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + receiptId + ", " + userId);
			}		
			tenjinJava.Call ("transactionAmazon", receiptArgs);
		}
		else{
			object[] args = new object[]{productId, currencyCode, quantity, unitPrice};
			if (Debug.isDebugBuild) {
				Debug.Log ("Android Amazon Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice);
			}
			tenjinJava.Call ("transaction", args);
		}
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		DeferredDeeplinkListener onDeferredDeeplinkListener = new DeferredDeeplinkListener(deferredDeeplinkDelegate);
		tenjinJava.Call ("getDeeplink", onDeferredDeeplinkListener);
	}

	private class DeferredDeeplinkListener : AndroidJavaProxy {
		private Tenjin.DeferredDeeplinkDelegate callback;

		public DeferredDeeplinkListener(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkCallback) : base("com.tenjin.android.Callback") {
			this.callback = deferredDeeplinkCallback;
		}

		public void onSuccess(bool clickedTenjinLink, bool isFirstSession, AndroidJavaObject data) {
			Dictionary<string, string> deeplinkData = new Dictionary<string, string>();
			string adNetwork = data.Call<string>("get", "ad_network");
			string advertisingId = data.Call<string>("get", "advertising_id");
			string campaignId = data.Call<string>("get", "campaign_id");
			string campaignName = data.Call<string>("get", "campaign_name");
			string deferredDeeplink = data.Call<string>("get", "deferred_deeplink_url");
			string referrer = data.Call<string>("get", "referrer");
			string siteId = data.Call<string>("get", " site_id");

			if (!string.IsNullOrEmpty(adNetwork)) {
				deeplinkData["ad_network"] = adNetwork;
			}
			if (!string.IsNullOrEmpty(advertisingId)) {
				deeplinkData["advertising_id"] = advertisingId;
			}
			if (!string.IsNullOrEmpty(campaignId)) {
				deeplinkData["campaign_id"] = campaignId;
			}
			if (!string.IsNullOrEmpty(campaignName)) {
				deeplinkData["campaign_name"] = campaignName;
			}
			if (!string.IsNullOrEmpty(deferredDeeplink)) {
				deeplinkData["deferred_deeplink_url"] = deferredDeeplink;
			}
			if (!string.IsNullOrEmpty(referrer)) {
				deeplinkData["referrer"] = referrer;
			}
			if (!string.IsNullOrEmpty(siteId)) {
				deeplinkData["site_id"] = siteId;
			}

			deeplinkData.Add("clicked_tenjin_link", Convert.ToString(clickedTenjinLink));
			deeplinkData.Add("is_first_session", Convert.ToString(isFirstSession));

			callback(deeplinkData);
		}
	}

	public override void GetAttributionInfo(Tenjin.AttributionInfoDelegate attributionInfoDelegate)
	{
		AttributionInfoListener onAttributionInfoListener = new AttributionInfoListener(attributionInfoDelegate);
		Debug.Log ("Sending AndroidTenjin::GetAttributionInfo");
		tenjinJava.Call ("getAttributionInfo", onAttributionInfoListener);
	}

	private class AttributionInfoListener : AndroidJavaProxy
	{
		private Tenjin.AttributionInfoDelegate callback;

		public AttributionInfoListener(Tenjin.AttributionInfoDelegate getAttributionInfoCallback) : base("com.tenjin.android.AttributionInfoCallback")
		{
			this.callback = getAttributionInfoCallback;
		}

		public void onSuccess(AndroidJavaObject data)
		{
			Dictionary<string, string> attributionInfoData = new Dictionary<string, string>();
			string adNetwork = data.Call<string>("get", "ad_network");
			string advertisingId = data.Call<string>("get", "advertising_id");
			string campaignId = data.Call<string>("get", "campaign_id");
			string campaignName = data.Call<string>("get", "campaign_name");
			string siteId = data.Call<string>("get", "site_id");
			string clickId = data.Call<string>("get", "click_id");
			string creativeName = data.Call<string>("get", "creative_name");
			string remoteCampaignId = data.Call<string>("get", "remote_campaign_id");

			if (!string.IsNullOrEmpty(adNetwork)) {
				attributionInfoData["ad_network"] = adNetwork;
			}
			if (!string.IsNullOrEmpty(advertisingId)) {
				attributionInfoData["advertising_id"] = advertisingId;
			}
			if (!string.IsNullOrEmpty(campaignId)) {
				attributionInfoData["campaign_id"] = campaignId;
			}
			if (!string.IsNullOrEmpty(campaignName)) {
				attributionInfoData["campaign_name"] = campaignName;
			}
			if (!string.IsNullOrEmpty(siteId)) {
				attributionInfoData["site_id"] = siteId;
			}
			if (!string.IsNullOrEmpty(clickId)) {
				attributionInfoData["click_id"] = clickId;
			}
			if (!string.IsNullOrEmpty(creativeName)) {
				attributionInfoData["creative_name"] = creativeName;
			}
			if (!string.IsNullOrEmpty(remoteCampaignId)) {
				attributionInfoData["remote_campaign_id"] = remoteCampaignId;
			}

			callback(attributionInfoData);
		}
	}

	public override void OptIn(){
		optIn = true;
		tenjinJava.Call ("optIn");
	}

	public override void OptOut(){
		optOut = true;
		tenjinJava.Call ("optOut");
	}

	public override void OptInParams(List<string> parameters){
		tenjinJava.Call ("optInParams", new object[] {parameters.ToArray()});
	}

	public override void OptOutParams(List<string> parameters){
		tenjinJava.Call ("optOutParams", new object[] {parameters.ToArray()});
	}

	public override bool OptInOutUsingCMP(){
		Debug.Log($"OptInOutUsingCMP");
		return tenjinJava.Call<bool> ("optInOutUsingCMP");
	}

	public override void OptInGoogleDMA(){
		tenjinJava.Call ("optInGoogleDMA");
	}

	public override void OptOutGoogleDMA(){
		tenjinJava.Call ("optOutGoogleDMA");
	}

	public override void RegisterAppForAdNetworkAttribution(){
	}

	public override void UpdateConversionValue(int conversionValue){
		if (Debug.isDebugBuild) {
			Debug.Log ("Android UpdateConversionValue");
		}
	}

	public override void UpdatePostbackConversionValue(int conversionValue){
		if (Debug.isDebugBuild) {
			Debug.Log ("Android UpdatePostbackConversionValue");
		}
	}

	public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue){
		if (Debug.isDebugBuild) {
			Debug.Log ("Android UpdatePostbackConversionValue");
		}
	}

	public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow){
		if (Debug.isDebugBuild) {
			Debug.Log ("Android UpdatePostbackConversionValue");
		}
	}

	public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback) {
	}

	public override void AppendAppSubversion (int appSubversion){
		object[] args = new object[]{appSubversion};
		tenjinJava.Call ("appendAppSubversion", args);
	}

	public override void DebugLogs(){
		Debug.Log ("Debug logs not implemented on android");
	}

	public override void SubscribeAppLovinImpressions()
    {
        Debug.Log("Subscribing to AppLovin ILRD");
        TenjinAppLovinIntegration.ListenForImpressions(AppLovinImpressionHandler);
    }

	public override void AppLovinImpressionFromJSON(string json)
	{
		AppLovinImpressionHandler(json);
	}

	private void AppLovinImpressionHandler(string json)
	{
        Debug.Log($"Got ILRD impression data {json}");
        var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionAppLovin", args);
	}

	public override void SubscribeIronSourceImpressions()
    {
        Debug.Log("Subscribing to IronSource ILRD");
        TenjinIronSourceIntegration.ListenForImpressions(IronSourceImpressionHandler);
    }

    public override void SubscribeLevelPlayRewardedAdImpressions(object rewardedAd)
    {
        Debug.Log("Subscribing to LevelPlay rewarded ILRD");
        TenjinIronSourceIntegration.SubscribeLevelPlayRewardedImpressions(rewardedAd, IronSourceImpressionHandler);
    }

    public override void SubscribeLevelPlayInterstitialAdImpressions(object interstitialAd)
    {
        Debug.Log("Subscribing to LevelPlay interstitial ILRD");
        TenjinIronSourceIntegration.SubscribeLevelPlayInterstitialImpressions(interstitialAd, IronSourceImpressionHandler);
    }

    public override void SubscribeLevelPlayBannerAdImpressions(object bannerAd)
    {
        Debug.Log("Subscribing to LevelPlay banner ILRD");
        TenjinIronSourceIntegration.SubscribeLevelPlayBannerImpressions(bannerAd, IronSourceImpressionHandler);
    }

	public override void IronSourceImpressionFromJSON(string json)
	{
		IronSourceImpressionHandler(json);
	}

	private void IronSourceImpressionHandler(string json)
	{
        Debug.Log($"Got ILRD impression data {json}");
        var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionIronSource", args);
	}

	public override void SubscribeHyperBidImpressions()
    {
        Debug.Log("Subscribing to HyperBid ILRD");
        TenjinHyperBidIntegration.ListenForImpressions(HyperBidImpressionHandler);
    }

	public override void HyperBidImpressionFromJSON(string json)
	{
		HyperBidImpressionHandler(json);
	}

	private void HyperBidImpressionHandler(string json)
	{
        Debug.Log($"Got ILRD impression data {json}");
        var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionHyperBid", args);
	}

	public override void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId)
	{
		Debug.Log("Subscribing to AdMob bannerView ILRD");
		TenjinAdMobIntegration.ListenForBannerViewImpressions(bannerView, adUnitId, AdMobBannerViewImpressionHandler);
	}

	public override void SubscribeAdMobRewardedAdImpressions(object rewardedAd, string adUnitId)
	{
		Debug.Log("Subscribing to AdMob rewardedAd ILRD");
		TenjinAdMobIntegration.ListenForRewardedAdImpressions(rewardedAd, adUnitId, AdMobRewardedAdImpressionHandler);
	}

	public override void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId)
	{
		Debug.Log("Subscribing to AdMob interstitialAd ILRD");
		TenjinAdMobIntegration.ListenForInterstitialAdImpressions(interstitialAd, adUnitId, AdMobInterstitialAdImpressionHandler);
	}

	public override void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId)
	{
		Debug.Log("Subscribing to AdMob rewardedInterstitialAd ILRD");
		TenjinAdMobIntegration.ListenForRewardedInterstitialAdImpressions(rewardedInterstitialAd, adUnitId, AdMobRewardedInterstitialAdImpressionHandler);
	}

	public override void AdMobImpressionFromJSON(string json)
	{
		Debug.Log($"Got admob ILRD data {json}");
		var args = new object[] {json};
		tenjinJava.Call ("eventAdImpressionAdMob", args);
	}

	private void AdMobBannerViewImpressionHandler(string json)
	{
		Debug.Log($"Got admob bannerView ILRD data {json}");
		var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionAdMob", args);
	}

	private void AdMobRewardedAdImpressionHandler(string json)
	{
		Debug.Log($"Got admob rewardedAd ILRD data {json}");
		var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionAdMob", args);
	}

	private void AdMobInterstitialAdImpressionHandler(string json)
	{
		Debug.Log($"Got admob interstitialAd ILRD data {json}");
		var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionAdMob", args);
	}

	private void AdMobRewardedInterstitialAdImpressionHandler(string json)
	{
		Debug.Log($"Got admob rewardedInterstitialAd ILRD data {json}");
		var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionAdMob", args);
	}

	public override void SubscribeTopOnImpressions()
    {
        Debug.Log("Subscribing to TopOn ILRD");
        TenjinTopOnIntegration.ListenForImpressions(TopOnImpressionHandler);
    }

	public override void TopOnImpressionFromJSON(string json)
	{
		TopOnImpressionHandler(json);
	}

	private void TopOnImpressionHandler(string json)
    {
        Debug.Log($"Got ILRD impression data {json}");
        var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionTopOn", args);
    }

	public override void SubscribeCASImpressions(object casManager)
    {
        Debug.Log("Subscribing to CAS ILRD");
        TenjinCASIntegration.ListenForImpressions(CASImpressionHandler, casManager);
    }

	public override void SubscribeCASBannerImpressions(object casBannerView)
	{
		Debug.Log("Subscribing to CAS banner ILRD");
		TenjinCASIntegration.ListenForBannerImpressions(CASImpressionHandler, casBannerView);
	}

	public override void CASImpressionFromJSON(string json)
    {
        CASImpressionHandler(json);
    }

	private void CASImpressionHandler(string json)
    {
        Debug.Log($"Got CAS ILRD impression data {json}");
        var args = new object[] {json};
        tenjinJava.Call ("eventAdImpressionCAS", args);
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

	public void TradPlusILRDHandler(string json)
	{
		if(!string.IsNullOrEmpty(json))
		{
			Debug.Log($"Got TradPlus ILRD impression data {json}");
			var args = new object[] {json};
			tenjinJava.Call ("eventAdImpressionTradPlus", args);
		}
	}


	public override void SetAppStoreType (AppStoreType appStoreType){
		object[] args = new object[]{appStoreType};
		AndroidJavaClass appStoreTypeClass = new AndroidJavaClass(AndroidJavaTenjinAppStoreType); 
		if (appStoreTypeClass != null){
			AndroidJavaObject tenjinAppStoreType = appStoreTypeClass.GetStatic<AndroidJavaObject>(appStoreType.ToString());
			if (tenjinAppStoreType != null) {
				tenjinJava.Call ("setAppStore", tenjinAppStoreType);
			}
		}
	}

	public override void SetCustomerUserId(string userId) {
		Debug.Log($"SetCustomerUserId {userId}");
		var args = new object[] {userId};
		tenjinJava.Call ("setCustomerUserId", args);
	}

	public override string GetCustomerUserId() {
		Debug.Log($"GetCustomerUserId");
		return tenjinJava.Call<string> ("getCustomerUserId");
	}

	public override void SetSessionTime(int time) {
		Debug.Log($"SetSessionTime {time}");
		var args = new object[] {time};
		tenjinJava.Call ("setSessionTime", args);
	}

	public override void SetCacheEventSetting(bool setting) {
		Debug.Log($"SetCacheEventSetting {setting}");
		try {
			using (var javaBoolean = new AndroidJavaObject("java.lang.Boolean", setting)) {
				tenjinJava.Call("setCacheEventSetting", javaBoolean);
			}
		} catch (Exception e) {
			Debug.LogError("Error in SetCacheEventSetting: " + e.Message);
		}
	}

	public override void SetEncryptRequestsSetting(bool setting) {
		Debug.Log($"SetEncryptRequestsSetting {setting}");
		try {
			using (var javaBoolean = new AndroidJavaObject("java.lang.Boolean", setting)) {
				tenjinJava.Call("setEncryptRequestsSetting", javaBoolean);
			}
		} catch (Exception e) {
			Debug.LogError("Error in SetEncryptRequestsSetting: " + e.Message);
		}
	}

	public override string GetAnalyticsInstallationId() {
		Debug.Log($"GetAnalyticsInstallationId");
		return tenjinJava.Call<string> ("getAnalyticsInstallationId");
	}

	public override void SetGoogleDMAParameters(bool adPersonalization, bool adUserData) {
		Debug.Log($"SetGoogleDMAParameters");
		var args = new object[] {adPersonalization, adUserData};
		tenjinJava.Call ("setGoogleDMAParameters", args);
	}

	public override Dictionary<string, string> GetUserProfileDictionary() {
		Debug.Log($"GetUserProfileDictionary");
		AndroidJavaObject profileMap = tenjinJava.Call<AndroidJavaObject> ("getUserProfileDictionary");
		return ConvertJavaMapToDict(profileMap);
	}

	public override void ResetUserProfile() {
		Debug.Log($"ResetUserProfile");
		tenjinJava.Call ("resetUserProfile");
	}

	private Dictionary<string, string> ConvertJavaMapToDict(AndroidJavaObject javaMap) {
		Dictionary<string, string> dict = new Dictionary<string, string>();

		if (javaMap == null) {
			return dict;
		}

		// Get the entry set
		AndroidJavaObject entrySet = javaMap.Call<AndroidJavaObject>("entrySet");
		AndroidJavaObject iterator = entrySet.Call<AndroidJavaObject>("iterator");

		while (iterator.Call<bool>("hasNext")) {
			AndroidJavaObject entry = iterator.Call<AndroidJavaObject>("next");
			string key = entry.Call<string>("getKey");
			AndroidJavaObject valueObj = entry.Call<AndroidJavaObject>("getValue");

			// Convert value to string (handles various types)
			string value = "";
			if (valueObj != null) {
				value = valueObj.Call<string>("toString");
			}

			dict[key] = value;
		}

		return dict;
	}
#else
    public override void Init(string apiKey)
    {
        Debug.Log("Android Initializing - v" + this.SdkVersion);
        ApiKey = apiKey;
    }

    public override void InitWithSharedSecret(string apiKey, string sharedSecret)
    {
        Debug.Log("Android Initializing with Shared Secret - v" + this.SdkVersion);
        ApiKey = apiKey;
        SharedSecret = sharedSecret;
    }

    public override void InitWithAppSubversion(string apiKey, int appSubversion)
    {
        Debug.Log("Android Initializing with App Subversion: " + appSubversion + " v" + this.SdkVersion);
        ApiKey = apiKey;
        AppSubversion = appSubversion;
    }

    public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
    {
        Debug.Log("Android Initializing with Shared Secret + App Subversion: " + appSubversion + " v" + this.SdkVersion);
        ApiKey = apiKey;
        SharedSecret = sharedSecret;
        AppSubversion = appSubversion;
    }

    public override void Connect()
    {
        Debug.Log("Android Connecting");
    }

    public override void Connect(string deferredDeeplink)
    {
        Debug.Log("Android Connecting with deferredDeeplink " + deferredDeeplink);
    }

    public override void SendEvent(string eventName)
    {
        Debug.Log("Android Sending Event " + eventName);
    }

    public override void SendEvent(string eventName, string eventValue)
    {
        Debug.Log("Android Sending Event " + eventName + " : " + eventValue);
    }

    public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
    {
        Debug.Log("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
    }

    public override void TransactionAmazon(string productId, string currencyCode, int quantity, double unitPrice, string receiptId, string userId)
    {
        Debug.Log("Android Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + receiptId + ", " + userId);
    }

    public override void Subscription(string productId, string currencyCode, double unitPrice, string transactionId, string originalTransactionId, string receipt, string skTransaction, string purchaseToken, string purchaseData, string dataSignature)
    {
        Debug.Log("Android Subscription " + productId + ", " + currencyCode + ", " + unitPrice);
    }

    public override void SubscriptionWithStoreKit(string productId, string currencyCode, double unitPrice)
    {
        Debug.Log("SubscriptionWithStoreKit is only available on iOS");
    }

    public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
    {
        Debug.Log("Sending AndroidTenjin::GetDeeplink");
    }

    public override void GetAttributionInfo(Tenjin.AttributionInfoDelegate attributionInfoDelegate)
    {
        Debug.Log("Sending AndroidTenjin::GetAttributionInfo");
    }

    public override void OptIn()
    {
        Debug.Log("Sending AndroidTenjin::OptIn");
    }

    public override void OptOut()
    {
        Debug.Log("Sending AndroidTenjin::OptOut");
    }

    public override void OptInParams(List<string> parameters)
    {
        Debug.Log("Sending AndroidTenjin::OptInParams");
    }

    public override void OptOutParams(List<string> parameters)
    {
        Debug.Log("Sending AndroidTenjin::OptOutParams");
    }

	public override bool OptInOutUsingCMP()
	{
		Debug.Log("Sending AndroidTenjin::OptInOutUsingCMP");
		return true;
	}

	public override void OptInGoogleDMA()
    {
        Debug.Log("Sending AndroidTenjin::OptInGoogleDMA");
    }

	public override void OptOutGoogleDMA()
    {
        Debug.Log("Sending AndroidTenjin::OptOutGoogleDMA");
    }

    public override void AppendAppSubversion(int subversion)
    {
        Debug.Log("Sending AndroidTenjin::AppendAppSubversion :" + subversion);
    }

    public override void SubscribeAppLovinImpressions()
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeAppLovinImpressions ");
    }

    public override void AppLovinImpressionFromJSON(string json)
    {
        Debug.Log("Sending AndroidTenjin:: AppLovinImpressionFromJSON ");
    }

    public override void SubscribeIronSourceImpressions()
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeIronSourceImpressions ");
    }

        public override void SubscribeLevelPlayRewardedAdImpressions(object rewardedAd)
        {
                Debug.Log("Sending AndroidTenjin:: SubscribeLevelPlayRewardedAdImpressions ");
        }

        public override void SubscribeLevelPlayInterstitialAdImpressions(object interstitialAd)
        {
                Debug.Log("Sending AndroidTenjin:: SubscribeLevelPlayInterstitialAdImpressions ");
        }

        public override void SubscribeLevelPlayBannerAdImpressions(object bannerAd)
        {
                Debug.Log("Sending AndroidTenjin:: SubscribeLevelPlayBannerAdImpressions ");
        }

        public override void IronSourceImpressionFromJSON(string json)
        {
                Debug.Log("Sending AndroidTenjin:: IronSourceImpressionFromJSON ");
        }

    public override void SubscribeHyperBidImpressions()
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeHyperBidImpressions ");
    }

    public override void HyperBidImpressionFromJSON(string json)
    {
        Debug.Log("Sending AndroidTenjin:: HyperBidImpressionFromJSON ");
    }

    public override void SubscribeAdMobBannerViewImpressions(object bannerView, string adUnitId)
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeAdMobBannerViewImpressions ");
    }

    public override void SubscribeAdMobRewardedAdImpressions(object rewardedAd, string adUnitId)
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeAdMobRewardedAdImpressions ");
    }

    public override void SubscribeAdMobInterstitialAdImpressions(object interstitialAd, string adUnitId)
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeAdMobInterstitialAdImpressions ");
    }

    public override void SubscribeAdMobRewardedInterstitialAdImpressions(object rewardedInterstitialAd, string adUnitId)
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeAdMobRewardedInterstitialAdImpressions ");
    }

    public override void AdMobImpressionFromJSON(string json)
    {
        Debug.Log("Sending AndroidTenjin:: AdMobImpressionFromJSON ");
    }

    public override void SubscribeTopOnImpressions()
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeTopOnImpressions ");
    }

    public override void TopOnImpressionFromJSON(string json)
    {
        Debug.Log("Sending AndroidTenjin:: TopOnImpressionFromJSON ");
    }

    public override void SubscribeCASImpressions(object casManager)
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeCASImpressions ");
    }

    public override void SubscribeCASBannerImpressions(object casBannerView)
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeCASBannerImpressions ");
    }

    public override void CASImpressionFromJSON(string json)
    {
        Debug.Log("Sending AndroidTenjin:: CASImpressionFromJSON ");
    }

    public override void SubscribeTradPlusImpressions()
    {
        Debug.Log("Sending AndroidTenjin:: SubscribeTradPlusImpressions");
    }

    public override void TradPlusImpressionFromJSON(string json)
    {
        Debug.Log("Sending AndroidTenjin:: TradPlusImpressionFromJSON");
    }

    public override void TradPlusImpressionFromAdInfo(Dictionary<string, object> adInfo)
    {
        Debug.Log("Sending AndroidTenjin:: TradPlusImpressionFromImpression");
    }

    public override void DebugLogs()
    {
        Debug.Log("Setting debug logs ");
    }

    public override void UpdateConversionValue(int conversionValue)
    {
        Debug.Log("Sending UpdateConversionValue: " + conversionValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue)
    {
        Debug.Log("Sending UpdatePostbackConversionValue: " + conversionValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
    {
        Debug.Log("Sending UpdatePostbackConversionValueCoarseValue: " + conversionValue + coarseValue);
    }

    public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
    {
        Debug.Log("Sending UpdatePostbackConversionValueCoarseValueLockWindow: " + conversionValue + coarseValue + lockWindow);
    }

    public override void RegisterAppForAdNetworkAttribution()
    {
        throw new NotImplementedException();
    }

    public override void RequestTrackingAuthorizationWithCompletionHandler(Action<int> trackingAuthorizationCallback)
    {
        throw new NotImplementedException();
    }

    public override void SetAppStoreType(AppStoreType appStoreType)
    {
        Debug.Log("Setting AndroidTenjin::SetAppStoreType: " + appStoreType);
    }

    public override void SetCustomerUserId(string userId)
    {
        Debug.Log("Setting AndroidTenjin::SetCustomerUserId: " + userId);
    }

    public override string GetCustomerUserId()
    {
        Debug.Log("Setting AndroidTenjin::GetCustomerUserId");
        return "";
    }

    public override void SetSessionTime(int time)
    {
        Debug.Log("Setting AndroidTenjin::SetSessionTime: " + time);
    }

    public override void SetCacheEventSetting(bool setting)
    {
        Debug.Log("Setting AndroidTenjin::SetCacheEventSetting: " + setting);
    }

	public override void SetEncryptRequestsSetting(bool setting)
    {
        Debug.Log("Setting AndroidTenjin::SetEncryptRequestsSetting: " + setting);
    }

	public override string GetAnalyticsInstallationId()
    {
        Debug.Log("Setting AndroidTenjin::GetAnalyticsInstallationId");
        return "";
    }

	public override void SetGoogleDMAParameters(bool adPersonalization, bool adUserData)
	{
		Debug.Log("Setting AndroidTenjin::SetGoogleDMAParameters");
	}

	public override Dictionary<string, string> GetUserProfileDictionary()
	{
		Debug.Log("Setting AndroidTenjin::GetUserProfileDictionary");
		return new Dictionary<string, string>();
	}

	public override void ResetUserProfile()
	{
		Debug.Log("Setting AndroidTenjin::ResetUserProfile");
	}
#endif
}
