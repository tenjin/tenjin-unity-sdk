# Summary

The Unity SDK for Tenjin. To learn more about Tenjin and our product offering, please visit https://www.tenjin.com.

- Please see our <a href="https://github.com/tenjin/tenjin-unity-sdk/blob/master/RELEASE_NOTES.md" target="_new">Release Notes</a> to see detailed version history of changes.
- Tenjin Unity SDK supports both iOS and Android.
- Review the [iOS](https://github.com/tenjin/tenjin-ios-sdk) and [Android](https://github.com/tenjin/tenjin-android-sdk) documentation and apply the proper platform settings to your builds.
- For any issues or support, please contact: support@tenjin.com
- iOS Notes:

  - Xcode 12 is required if using Unity iOS SDK v1.12.0 and higher.
  - When building iOS, confirm that these frameworks were automatically added to the Xcode build. If any are missing, you will need to add them manually.
    - AdSupport.framework
    - AppTrackingTransparency.framework
    - iAd.framework
    - StoreKit.framework
  - For AppTrackingTransparency, be sure update your project `.plist` file and add `Privacy - Tracking Usage Description` <a href="https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription" target="_new">(NSUserTrackingUsageDescription)</a> along with the text message you want to display to users.

- Android Notes:

  1. If you have another SDK installed which already has Google Play Services installed or uses [PlayServicesResolver](https://github.com/googlesamples/unity-jar-resolver), you may need to delete duplicate libraries:

  ```
   /Assets/Plugins/Android/play-services-ads-identifier--*.aar
   /Assets/Plugins/Android/play-services-basement---*.aar
  ```

  2. If you use the Unity SDK version 1.12.4 or below and see the following errors on the app initialization, move tenjin.aar file from `/Assets/Plugins/Android/Tenjin/libs` to `/Assets/Plugins/Android/`.

  ```
  AndroidJavaException: java.lang.NoSuchMethodError: no static method with name='setWrapperVersion'
  ```

  or

  ```
  AndroidJavaException: java.lang.ClassNotFoundException: com.tenjin.android.TenjinSDK
  ```

# Table of contents

- [SDK Integration](#sdk-integration)

  - [OAID](#oaid)
    - [MSA OAID](#msa-oaid)
    - [Huawei OAID](#huawei-oaid)
  - [App Initilization](#initialization)
  - [App Store](#app-store)
  - [ATTrackingManager (iOS)](#attrackingmanager)
  - [SKAdNetwork and Conversion Value](#skadnetwork-cv)
  - [GDPR](#gdpr)
  - [Purchase Events](#purchase-events)
    - [iOS IAP Validation](#ios-iap-validation)
    - [Android IAP Validation](#android-iap-validation)
  - [Custom Events](#custom-events)
  - [Deferred Deeplinks](#deferred-deeplinks)
  - [App Subversion](#subversion)

- [Testing](#testing)

# <a id="sdk-integration"></a> SDK Integration

1. Download the latest Unity SDK from <a href="https://github.com/tenjin/tenjin-unity-sdk/releases" target="_new">here.</a>

2. Import the `TenjinUnityPackage.unitypackage` into your project: `Assets -> Import Package`.

3. By default, we have included <a href="https://developers.google.com/android/guides/setup" target="_new">Google Play Services</a> AAR files as part of our SDK. If you do not plan on using Google Play Services, you can delete these AAR files:

```
 /Assets/Plugins/Android/play-services-*.aar
 /Assets/Plugins/Android/installreferrer-*.aar
```

## <a id="oaid"></a>OAID and other Android App Stores

Tenjin supports promoting on other Android App Stores using the Android OAID. We have the following options for integrating OAID libraries.

### <a id="msa-oaid"></a>MSA OAID (In China)

For integration with the <a href="http://www.msa-alliance.cn/col.jsp?id=120" target="_new">MSA libary</a>, download the following <a href="msa-oaid/oaid_sdk_1.0.25.aar" target="_new">oaid_sdk_1.0.25.aar</a>.

Place the `oaid_sdk_1.0.25.aar` file in your project's Android libs directory: `/Assets/Plugins/Android`

Set your App Store Type value to `other`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

instance.SetAppStoreType(AppStoreType.other);
```

### <a id="huawei-oaid"></a>Huawei OAID (Outside China)

For integration with the <a href="https://developer.huawei.com/consumer/en/codelab/HMSAdsOAID/index.html#3" target="_new">Huawei OAID libary</a>, download the following Huawei AAR file: <a href="huawei/huawei-ads-identifier.aar" target="_new">huawei-ads-identifier.aar</a>. If your app is in the <a href="https://appgallery.huawei.com/" target="_new">Huawei App Gallery</a>, download and add the Huawei Install Referrer file: <a href="huawei/huawei-ads-installreferrer.aar" target="_new">huawei-ads-installreferrer.aar</a>. Place the Huawei files in your project's Android libs directory: `/Assets/Plugins/Android`

## <a id="initialization"></a> App Initialization

1. Get your `<API_KEY>` from your <a href="https://www.tenjin.io/dashboard/docs" target="_new">Tenjin dashboard</a>.
2. In your project's first `Start()` method add the following line of code. Also add to `OnApplicationPause()` if you want to send sessions data when a user resumes using the app from the background.

```csharp
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

  void Start() {
    TenjinConnect();
  }

  void OnApplicationPause(bool pauseStatus) {
    if (!pauseStatus) {
      TenjinConnect();
    }
  }

  public void TenjinConnect() {
    BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

    // Sends install/open event to Tenjin
    instance.Connect();
  }
}
```

## Tenjin initialization with ATTrackingManager:

Starting with iOS 14, you have the option to show the initial <a href="">ATTrackingManager</a> permissions prompt and selection to opt in/opt out users.
If the device doesn't accept tracking permission, IDFA will become zero. If the device accepts tracking permission, the `connect()` method will send the IDFA to our servers.
You can also still call Tenjin `connect()`, without using ATTrackingManager. ATTrackingManager permissions prompt is not obligatory until the early spring of 2021.

## <a id="app-store"></a> App Store

By default, <a href="https://play.google.com/" target="_new">Google Play</a> is the default App Store. If you are publishing in a different App Store, update to the appropriate `TenjinSDK.AppStoreType.*` value:

```csharp
BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

instance.SetAppStoreType(AppStoreType.{{SET_APP_STORE_TYPE_VALUE}});
```

Current `AppStoreType` options:

- `googleplay`
- `amazon`
- `other`

## <a id="attrackingmanager"></a> ATTrackingManager (iOS)

- Starting with iOS 14, you have the option to show the initial <a href="">ATTrackingManager</a> permissions prompt and selection to opt in/opt out users.

- If the device doesn't accept tracking permission, IDFA will become zero. If the device accepts tracking permission, the `Connect()` method will send the IDFA to our servers.

- You can also still call Tenjin `Connect()`, without using ATTrackingManager. ATTrackingManager permissions prompt is not obligatory until the start of 2021.
  > > > > > > > Add SDK v1.12.5

```csharp
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

    void Start() {
      TenjinConnect();
    }

    void OnApplicationPause(bool pauseStatus) {
      if (!pauseStatus) {
        TenjinConnect();
      }
    }

    public void TenjinConnect() {
      BaseTenjin instance = Tenjin.getInstance("API_KEY");

#if UNITY_IOS

      // Tenjin wrapper for requestTrackingAuthorization
      instance.RequestTrackingAuthorizationWithCompletionHandler((status) => {
        Debug.Log("===> App Tracking Transparency Authorization Status: " + status);

        // Sends install/open event to Tenjin
        instance.Connect();

      });

#elif UNITY_ANDROID

      // Sends install/open event to Tenjin
      instance.Connect();

#endif
    }
}
```

## <a id="skadnetwork-cv"></a> SKAdNetwork and Conversion Values

As part of <a href="https://developer.apple.com/documentation/storekit/skadnetwork">SKAdNetwork</a>, we created wrapper methods for `registerAppForAdNetworkAttribution()` and <a href="https://developer.apple.com/documentation/storekit/skadnetwork/3566697-updateconversionvalue">`updateConversionValue(_:)`</a>.
Our methods will register the equivalent SKAdNetwork methods and also send the conversion values on our servers.

<<<<<<< HEAD
`updateConversionValue(_:)` 6 bit value should correspond to the in-app event and shouldn't be entered as binary representation but 0-63 integer. Our server will reject any invalid values. Please refer to [this](https://docs.tenjin.com/en/tracking/sk_adnetwork.html#cv) page for how to implement conversion values.
=======
`updateConversionValue(_:)` 6 bit value should correspond to the in-app event and shouldn't be entered as binary representation but 0-63 integer. Our server will reject any invalid values.

- <a href="https://docs.google.com/spreadsheets/d/1jrRrTP6YX62of2WaJamtPBSWZJ-97IpTWn0IwTroH6Y/edit#gid=1596716780">Examples for IAP based games </a>
- <a href="https://docs.google.com/spreadsheets/d/15JaN44yQyW7dqqRGi5Wwnq2P6ng-4n6EztMmMj5A7c4/edit#gid=0">Examples for Ad revenue based games </a>
  > > > > > > > Add SDK v1.12.5

```csharp
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

    void Start() {
      TenjinConnect();
    }

    void OnApplicationPause(bool pauseStatus) {
      if (!pauseStatus) {
        TenjinConnect();
      }
    }

    public void TenjinConnect() {
      BaseTenjin instance = Tenjin.getInstance("API_KEY");

#if UNITY_IOS

      // Registers SKAdNetwork app for attribution
      instance.RegisterAppForAdNetworkAttribution();

      // Sends install/open event to Tenjin
      instance.Connect();

      // Sets SKAdNetwork Conversion Value
      // You will need to use a value between 0-63 for <YOUR 6 bit value>
      instance.UpdateConversionValue(<your 6 bit value>);

#elif UNITY_ANDROID

      // Sends install/open event to Tenjin
      instance.Connect();

#endif
    }
}
```

## <a id="gdpr"></a> GDPR

As part of GDPR compliance, with Tenjin's SDK you can opt-in, opt-out devices/users, or select which specific device-related params to opt-in or opt-out. `OptOut()` will not send any API requests to Tenjin and we will not process any events.

To opt-in/opt-out:

```csharp
void Start () {

  BaseTenjin instance = Tenjin.getInstance("API_KEY");

  boolean userOptIn = CheckOptInValue();

  if (userOptIn) {
    instance.OptIn();
  }
  else {
    instance.OptOut();
  }

  instance.Connect();
}

boolean CheckOptInValue()
{
  // check opt-in value
  // return true; // if user opted-in
  return false;
}
```

- To opt-in/opt-out specific device-related parameters, you can use the `OptInParams()` or `OptOutParams()`.

- `OptInParams()` will only send device-related parameters that are specified. `OptOutParams()` will send all device-related parameters except ones that are specified.

- Please note that we require the following parameters to properly track devices in Tenjin's system:

  - `ip_address`
  - `advertising_id`
  - `limit_ad_tracking`
  - `referrer` (Android)
  - `iad` (iOS)

- If you are targeting IMEI and/or OAID Ad Networks, add:

  - `imei`
  - `oaid`

- If you plan on using Google Ad Words, you will also need to add:
  - `platform`
  - `os_version`
  - `locale`
  - `device_model`
  - `build_id`

If you want to only get specific device-related parameters, use `OptInParams()`. In example below, we will only these device-related parameters: `ip_address`, `advertising_id`, `developer_device_id`, `limit_ad_tracking`, `referrer`, and `iad`:

```csharp
BaseTenjin instance = Tenjin.getInstance("API_KEY");

List<string> optInParams = new List<string> {"ip_address", "advertising_id", "developer_device_id", "limit_ad_tracking", "referrer", "iad"};
instance.OptInParams(optInParams);

instance.Connect();
```

If you want to send ALL parameters except specfic device-related parameters, use `OptOutParams()`. In example below, we will send ALL device-related parameters except: `locale`, `timezone`, and `build_id` parameters.

```csharp
BaseTenjin instance = Tenjin.getInstance("API_KEY");

List<string> optOutParams = new List<string> {"locale", "timezone", "build_id"};
instance.OptOutParams(optOutParams);

instance.Connect();
```

#### Device-Related Parameters

| Param               | Description                  | Platform | Reference                                                                                                                                                                                                                                                                               |
| ------------------- | ---------------------------- | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ip_address          | IP Address                   | All      |                                                                                                                                                                                                                                                                                         |
| advertising_id      | Device Advertising ID        | All      | [Android](<https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.html#getAdvertisingIdInfo(android.content.Context)>), [iOS](https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614151-advertisingidentifier) |
| developer_device_id | ID for Vendor                | iOS      | [iOS](https://developer.apple.com/documentation/uikit/uidevice/1620059-identifierforvendor)                                                                                                                                                                                             |
| limit_ad_tracking   | limit ad tracking enabled    | All      | [Android](<https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.Info.html#isLimitAdTrackingEnabled()>), [iOS](https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614148-isadvertisingtrackingenabled)        |
| platform            | platform                     | All      | iOS or Android                                                                                                                                                                                                                                                                          |
| referrer            | Google Play Install Referrer | Android  | [Android](https://developer.android.com/google/play/installreferrer/index.html)                                                                                                                                                                                                         |
| iad                 | Apple Search Ad parameters   | iOS      | [iOS](https://searchads.apple.com/advanced/help/measure-results/#attribution-api)                                                                                                                                                                                                       |
| os_version          | operating system version     | All      | [Android](https://developer.android.com/reference/android/os/Build.VERSION.html#SDK_INT), [iOS](https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion)                                                                                                         |
| device              | device name                  | All      | [Android](https://developer.android.com/reference/android/os/Build.html#DEVICE), [iOS (hw.machine)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html)                                                                              |
| device_manufacturer | device manufactuer           | Android  | [Android](https://developer.android.com/reference/android/os/Build.html#MANUFACTURER)                                                                                                                                                                                                   |
| device_model        | device model                 | All      | [Android](https://developer.android.com/reference/android/os/Build.html#MODEL), [iOS (hw.model)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html)                                                                                 |
| device_brand        | device brand                 | Android  | [Android](https://developer.android.com/reference/android/os/Build.html#BRAND)                                                                                                                                                                                                          |
| device_product      | device product               | Android  | [Android](https://developer.android.com/reference/android/os/Build.html#PRODUCT)                                                                                                                                                                                                        |
| device_model_name   | device machine               | iOS      | [iOS (hw.model)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html)                                                                                                                                                                 |
| device_cpu          | device cpu name              | iOS      | [iOS (hw.cputype)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html)                                                                                                                                                               |
| carrier             | phone carrier                | Android  | [Android](<https://developer.android.com/reference/android/telephony/TelephonyManager.html#getSimOperatorName()>)                                                                                                                                                                       |
| connection_type     | cellular or wifi             | Android  | [Android](<https://developer.android.com/reference/android/net/ConnectivityManager.html#getActiveNetworkInfo()>)                                                                                                                                                                        |
| screen_width        | device screen width          | Android  | [Android](https://developer.android.com/reference/android/util/DisplayMetrics.html#widthPixels)                                                                                                                                                                                         |
| screen_height       | device screen height         | Android  | [Android](https://developer.android.com/reference/android/util/DisplayMetrics.html#heightPixels)                                                                                                                                                                                        |
| os_version_release  | operating system version     | All      | [Android](https://developer.android.com/reference/android/os/Build.VERSION.html#RELEASE), [iOS](https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion)                                                                                                         |
| build_id            | build ID                     | All      | [Android](https://developer.android.com/reference/android/os/Build.html), [iOS (kern.osversion)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html)                                                                                 |
| locale              | device locale                | All      | [Android](<https://developer.android.com/reference/java/util/Locale.html#getDefault()>), [iOS](https://developer.apple.com/documentation/foundation/nslocalekey)                                                                                                                        |
| country             | locale country               | All      | [Android](<https://developer.android.com/reference/java/util/Locale.html#getDefault()>), [iOS](https://developer.apple.com/documentation/foundation/nslocalecountrycode)                                                                                                                |
| timezone            | timezone                     | All      | [Android](https://developer.android.com/reference/java/util/TimeZone.html), [iOS](https://developer.apple.com/documentation/foundation/nstimezone/1387209-localtimezone)                                                                                                                |

<br />

## <a id="purchase-events"></a>Purchase Events

## <a id="ios-iap-validation"></a>iOS IAP Validation

iOS receipt validation requires `transactionId` and `receipt` (`signature` will be set to `null`). For `receipt`, be sure to send the receipt `Payload`(the base64 encoded ASN.1 receipt) from Unity.

<<<<<<< HEAD
Tenjin purchase event integration instructions:

---

#### iOS IAP Validation

# iOS receipt validation requires `transactionId` and `receipt` (`signature` will be set to `null`). For `receipt`, be sure to send the receipt `Payload`(the base64 encoded ASN.1 receipt) from Unity.

**IMPORTANT:** If you have subscription IAP, you will need to add your app's shared secret in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retreive your iOS App-Specific Shared Secret from the <a href="https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/887212194/addons" target="_new">iTunes Connect Console</a> > Select your app > Features > In-App Purchases > App-Specific Shared Secret.

> > > > > > > Add SDK v1.12.5

## <a id="android-iap-validation"></a>Android IAP Validation

<<<<<<< HEAD

#### Android IAP Validation

Android receipt validation requires `receipt` and `signature` are required (`transactionId` is set to `null`).

# **IMPORTANT:** You will need to add your app's public key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retreive your Base64-encoded RSA public key from the <a href="https://play.google.com/apps/publish/" target="_new"> Google Play Developer Console</a> > Select your app > Development Tools > Services & APIs.

Android receipt validation requires `receipt` and `signature` are required (`transactionId` is set to `null`).

**IMPORTANT:** You will need to add your app's public key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retreive your Base64-encoded RSA public key from the <a href="https://play.google.com/apps/publish/" target="_new"> Google Play Developer Console</a> > Select your app > Development Tools > Services & APIs.

### iOS and Android IAP Example:

> > > > > > > Add SDK v1.12.5

In the example below, we are using the widely used <a href="https://gist.github.com/darktable/1411710" target="_new">MiniJSON</a> library for JSON deserializing.

```csharp
  public static void OnProcessPurchase(PurchaseEventArgs purchaseEventArgs) {
    var price = purchaseEventArgs.purchasedProduct.metadata.localizedPrice;
    double lPrice = decimal.ToDouble(price);
    var currencyCode = purchaseEventArgs.purchasedProduct.metadata.isoCurrencyCode;

    var wrapper = Json.Deserialize(purchaseEventArgs.purchasedProduct.receipt) as Dictionary<string, object>;  // https://gist.github.com/darktable/1411710
    if (null == wrapper) {
        return;
    }

    var payload   = (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt
    var productId = purchaseEventArgs.purchasedProduct.definition.id;

#if UNITY_ANDROID

  var gpDetails = Json.Deserialize(payload) as Dictionary<string, object>;
  var gpJson    = (string)gpDetails["json"];
  var gpSig     = (string)gpDetails["signature"];

  CompletedAndroidPurchase(productId, currencyCode, 1, lPrice, gpJson, gpSig);

#elif UNITY_IOS

  var transactionId = purchaseEventArgs.purchasedProduct.transactionID;

  CompletedIosPurchase(productId, currencyCode, 1, lPrice , transactionId, payload);

#endif

  }

  private static void CompletedAndroidPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string Receipt, string Signature)
  {
      BaseTenjin instance = Tenjin.getInstance("API_KEY");
      instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, null, Receipt, Signature);
  }

  private static void CompletedIosPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string TransactionId, string Receipt)
  {
      BaseTenjin instance = Tenjin.getInstance("API_KEY");
      instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, TransactionId, Receipt, null);
  }
```

### <a id="subscription-iap"></a> Subscription IAP

- You are responsible to send subscription transaction one time during each subscription interval (i.e. For example, for a monthly subscription, you will need to send us 1 transaction per month). A transaction event should only be sent at the "First Charge" and "Renewal" events. During the trial period, do not send Tenjin the transaction event.

<<<<<<< HEAD
**Please note that you are responsible to send subscription transaction one time during each subscription interval (i.e. For example, for a monthly subscription, you will need to send us 1 transaction per month).**
=======

- Tenjin does not de-dupe duplicate transactions.

  > > > > > > > Add SDK v1.12.5

- If you have iOS subscription IAP, you will need to add your app's public key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new"> Tenjin dashboard</a>. You can retreive your iOS App-Specific Shared Secret from the <a href="https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/887212194/addons">iTunes Connect Console</a> > Select your app > Features > In-App Purchases > App-Specific Shared Secret.

<<<<<<< HEAD
<img src="https://docs-assets.developer.apple.com/published/6631e50f32/110c0e3f-e0e3-4dbd-bc28-d8db4b28bd1c.png" />
=======

- For more information on iOS subscriptions, please see: <a href="https://developer.apple.com/documentation/storekit/in-app_purchase/subscriptions_and_offers">Apple documentation on Working with Subscriptions</a>

  > > > > > > > Add SDK v1.12.5

- For more information on Android subscriptions, please see: <a href="https://developer.android.com/distribute/best-practices/earn/subscriptionss">Google Play Billing subscriptions documentation</a>

## <a id="custom-events"></a> Custom Events

**IMPORTANT: Limit custom event names to less than 80 characters. Do not exceed 500 unique custom event names.**

<<<<<<< HEAD
Tenjin custom event integration:

---

**IMPORTANT: Limit custom event names to less than 80 characters. Do not exceed 500 unique custom event names.**

=======

> > > > > > > Add SDK v1.12.5

- Include the Assets folder in your Unity project
- In your projects method for the custom event write the following for a named event: `Tenjin.getInstance("<API_KEY>").SendEvent("name")` and the following for a named event with an integer value: `Tenjin.getInstance("<API_KEY>").SendEvent("nameWithValue","value")`
- Make sure `value` passed is an integer. If `value` is not an integer, your event will not be passed.

Here's an example of the code:

```csharp
void MethodWithCustomEvent(){
    //event with name
    BaseTenjin instance = Tenjin.getInstance ("API_KEY");
    instance.SendEvent("name");

    //event with name and integer value
    instance.SendEvent("nameWithValue", "value");
}
```

`.SendEvent("name")` is for events that are static markers or milestones. This would include things like `tutorial_complete`, `registration`, or `level_1`.

`.SendEvent("name", "value")` is for events that you want to do math on a property of that event. For example, `("coins_purchased", "100")` will let you analyze a sum or average of the coins that have been purchased for that event.

## <a id="deferred-deeplinks"></a> Deferred Deeplinks

Tenjin supports the ability to direct users to a specific part of your app after a new attributed install via Tenjin's campaign tracking URLs. You can utilize the `GetDeeplink` method and callback to access the deferred deeplink through the data object. To test you can follow the instructions found <a href="http://help.tenjin.io/t/how-do-i-use-and-test-deferred-deeplinks-with-my-campaigns/547">here</a>.

```csharp

public class TenjinExampleScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    BaseTenjin instance = Tenjin.getInstance("API_KEY");
    instance.Connect();
    instance.GetDeeplink(DeferredDeeplinkCallback);
  }

  public void DeferredDeeplinkCallback(Dictionary<string, string> data) {
    bool clicked_tenjin_link = false;
    bool is_first_session = false;

    if (data.ContainsKey("clicked_tenjin_link")) {
      //clicked_tenjin_link is a BOOL to handle if a user clicked on a tenjin link
      clicked_tenjin_link = (data["clicked_tenjin_link"] == "true");
      Debug.Log("===> DeferredDeeplinkCallback ---> clicked_tenjin_link: " + data["clicked_tenjin_link"]);
    }

    if (data.ContainsKey("is_first_session")) {
      //is_first_session is a BOOL to handle if this session for this user is the first session
      is_first_session = (data["is_first_session"] == "true");
      Debug.Log("===> DeferredDeeplinkCallback ---> is_first_session: " + data["is_first_session"]);
    }

    if (data.ContainsKey("ad_network")) {
      //ad_network is a STRING that returns the name of the ad network
      Debug.Log("===> DeferredDeeplinkCallback ---> adNetwork: " + data["ad_network"]);
    }

    if (data.ContainsKey("campaign_id")) {
      //campaign_id is a STRING that returns the tenjin campaign id
      Debug.Log("===> DeferredDeeplinkCallback ---> campaignId: " + data["campaign_id"]);
    }

    if (data.ContainsKey("advertising_id")) {
      //advertising_id is a STRING that returns the advertising_id of the user
      Debug.Log("===> DeferredDeeplinkCallback ---> advertisingId: " + data["advertising_id"]);
    }

    if (data.ContainsKey("deferred_deeplink_url")) {
      //deferred_deeplink_url is a STRING that returns the deferred_deeplink of the campaign
      Debug.Log("===> DeferredDeeplinkCallback ---> deferredDeeplink: " + data["deferred_deeplink_url"]);
    }

    if (clicked_tenjin_link && is_first_session) {
      //use the deferred_deeplink_url to direct the user to a specific part of your app
      if (String.IsNullOrEmpty(data["deferred_deeplink_url"]) == false) {
      }
    }
  }

}

```

<<<<<<< HEAD

## Server-to-server integration

Tenjin offers server-to-server integration. If you want to access to the documentation, please send email to support@tenjin.com.

## App Subversion parameter for A/B Testing (requires DataVault)

=======

> > > > > > > Add SDK v1.12.5

Below are the parameters, if available, that are returned in the deferred deeplink callback:

<<<<<<< HEAD
This data will appear within DataVault where you will be able to run reports using the app subversion values.
=======
| Parameter | Description |
| --------------------- | ---------------------------------------------------------------- |
| advertising_id | Advertising ID of the device |
| ad_network | Ad network of the campaign |
| campaign_id | Tenjin campaign ID |
| campaign_name | Tenjin campaign name |
| site_id | Site ID of source app |
| referrer | The referrer params from the app store |
| deferred_deeplink_url | The deferred deep-link of the campaign |
| clicked_tenjin_link | Boolean representing if the device was tracked by Tenjin |
| is_first_session | Boolean representing if this is the first session for the device |

> > > > > > > Add SDK v1.12.5

## <a id="subversion"></a>App Subversion parameter for A/B Testing (requires DataVault)

If you are running A/B tests and want to report the differences, we can append a numeric value to your app version using the `AppendAppSubversion()` method. For example, if your app version `1.0.1`, and set `AppendAppSubversion(8888)`, it will report app version as `1.0.1.8888`.

This data will appear within DataVault where you will be able to run reports using the app subversion values.

```csharp
BaseTenjin instance = Tenjin.getInstance("<API KEY>");
instance.AppendAppSubversion(8888);
instance.Connect();
```

# <a id="testing"></a>Testing

You can verify if the integration is working through our <a href="https://www.tenjin.io/dashboard/sdk_diagnostics">Live Test Device Data Tool</a>. Add your `advertising_id` or `IDFA/GAID` to the list of test devices. You can find this under Support -> <a href="https://www.tenjin.io/dashboard/debug_app_users">Test Devices</a>. Go to the <a href="https://www.tenjin.io/dashboard/sdk_diagnostics">SDK Live page</a> and send a test events from your app. You should see a live events come in:

<br />

![](https://s3.amazonaws.com/tenjin-instructions/sdk_live_purchase_events.png)

<br /><br />
