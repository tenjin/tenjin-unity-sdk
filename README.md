

Please see our <a href="https://github.com/tenjin/tenjin-unity-sdk/wiki">Release Notes</a> to see detailed version history.

Tenjin Unity plugin
=========
- Allows unity developers to quickly integrate with Tenjin's install API
- Review the [iOS](https://github.com/tenjin/tenjin-ios-sdk) and [Android](https://github.com/tenjin/tenjin-android-sdk) documentation and apply the proper platform settings to your builds. Most importantly:
  1. iOS: make sure you have the right build settings and you include the iOS frameworks you need (below).
  2. Android: make sure you add the necessary `AndroidManifest.xml` requirements (below).
  3. Your "API_KEY" is located on your [Organizations tab](https://www.tenjin.io/dashboard/organizations)

Tenjin install/session integration:
-------
- Include the Assets folder in your Unity project
- In your project's first `Start()` method write the following `BaseTenjin instance = Tenjin.getInstance("API_KEY")` and then `instance.Connect()`

Here's an example of the code:

```csharp
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

  // Use this for initialization
  void Start () {

    BaseTenjin instance = Tenjin.getInstance("API_KEY");
    instance.Connect();
  }

  // Update is called once per frame
  void Update () {

  }

  void OnApplicationPause(bool pauseStatus){
    if (pauseStatus) {
      //do nothing
    }
    else {
      BaseTenjin instance = Tenjin.getInstance("API_KEY");
      instance.Connect();
    }
  }
}
```

Tenjin and GDPR:
-------
As part of GDPR compliance, with Tenjin's SDK you can opt-in, opt-out devices/users, or select which specific device-related params to opt-in or opt-out.  `OptOut()` will not send any API requests to Tenjin and we will not process any events.

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

boolean CheckOptInValue(){
  // check opt-in value
  // return true; // if user opted-in
  return false;
}
```

To opt-in/opt-out specific device-related parameters, you can use the `OptInParams()` or `OptOutParams()`.  `OptInParams()` will only send device-related parameters that are specified.  `OptOutParams()` will send all device-related parameters except ones that are specified.  **Please note that we require at least `ip_address`, `advertising_id`, `developer_device_id`, `limit_ad_tracking`, `referrer` (Android), and `iad` (iOS) to properly track devices in Tenjin's system.**

If you want to only get specific device-related parameters, use `OptInParams()`. In example below, we will only these device-related parameters: `ip_address`, `advertising_id`, `developer_device_id`, `limit_ad_tracking`, `referrer`, and `iad`:

```csharp
BaseTenjin instance = Tenjin.getInstance("API_KEY");

List<string> optInParams = new List<string> {"ip_address", "advertising_id", "developer_device_id", "limit_ad_tracking", "referrer", "iad"};
instance.OptInParams(optInParams);

instance.Connect();
```

If you want to send ALL parameters except specfic device-related parameters, use `OptOutParams()`.  In example below, we will send ALL device-related parameters except: `locale`, `timezone`, and `build_id` parameters.

```csharp
BaseTenjin instance = Tenjin.getInstance("API_KEY");

List<string> optOutParams = new List<string> {"locale", "timezone", "build_id"};
instance.OptOutParams(optOutParams);

instance.Connect();
```

#### Device-Related Parameters

| Param  | Description | Platform | Reference |
| ------------- | ------------- | ------------- | ------------- |
| advertising_id  | Device Advertising ID | All | [Android](https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.html#getAdvertisingIdInfo(android.content.Context)), [iOS](https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614151-advertisingidentifier) |
| developer_device_id | ID for Vendor | iOS | [iOS](https://developer.apple.com/documentation/uikit/uidevice/1620059-identifierforvendor) |
| limit_ad_tracking  | limit ad tracking enabled | All | [Android](https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.Info.html#isLimitAdTrackingEnabled()), [iOS](https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614148-isadvertisingtrackingenabled) |
| platform | platform | All | iOS or Android |
| referrer | Google Play Install Referrer | Android | [Android](https://developer.android.com/google/play/installreferrer/index.html) |
| iad | Apple Search Ad parameters | iOS | [iOS](https://searchads.apple.com/advanced/help/measure-results/#attribution-api) |
| os_version | operating system version | All | [Android](https://developer.android.com/reference/android/os/Build.VERSION.html#SDK_INT), [iOS](https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion) |
| device | device name | All | [Android](https://developer.android.com/reference/android/os/Build.html#DEVICE), [iOS (hw.machine)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html) |
| device_manufacturer | device manufactuer | Android | [Android](https://developer.android.com/reference/android/os/Build.html#MANUFACTURER) |
| device_model | device model | All | [Android](https://developer.android.com/reference/android/os/Build.html#MODEL), [iOS (hw.model)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html) |
| device_brand | device brand | Android | [Android](https://developer.android.com/reference/android/os/Build.html#BRAND) |
| device_product | device product | Android | [Android](https://developer.android.com/reference/android/os/Build.html#PRODUCT) |
| device_model_name | device machine  | iOS | [iOS (hw.model)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html) |
| device_cpu | device cpu name | iOS | [iOS (hw.cputype)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html) |
| carrier | phone carrier | Android | [Android](https://developer.android.com/reference/android/telephony/TelephonyManager.html#getSimOperatorName()) |
| connection_type | cellular or wifi | Android | [Android](https://developer.android.com/reference/android/net/ConnectivityManager.html#getActiveNetworkInfo()) |
| screen_width | device screen width | Android | [Android](https://developer.android.com/reference/android/util/DisplayMetrics.html#widthPixels) |
| screen_height | device screen height | Android | [Android](https://developer.android.com/reference/android/util/DisplayMetrics.html#heightPixels) |
| os_version_release | operating system version | All | [Android](https://developer.android.com/reference/android/os/Build.VERSION.html#RELEASE), [iOS](https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion) |
| build_id | build ID | All | [Android](https://developer.android.com/reference/android/os/Build.html), [iOS (kern.osversion)](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html) |
| locale | device locale | All | [Android](https://developer.android.com/reference/java/util/Locale.html#getDefault()), [iOS](https://developer.apple.com/documentation/foundation/nslocalekey) |
| country | locale country | All | [Android](https://developer.android.com/reference/java/util/Locale.html#getDefault()), [iOS](https://developer.apple.com/documentation/foundation/nslocalecountrycode) |
| timezone | timezone | All | [Android](https://developer.android.com/reference/java/util/TimeZone.html), [iOS](https://developer.apple.com/documentation/foundation/nstimezone/1387209-localtimezone) |

Tenjin install/session integration to handle deeplinks from other services.
-------
If you use other services to produce deferred deep links, you can pass tenjin those deep links to handle the attribution logic with your tenjin enabled deep links.

```csharp
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    BaseTenjin instance = Tenjin.getInstance("API_KEY");
    instance.Connect("your_deeplink://path?test=123");
  }

}
```

Tenjin purchase event integration instructions:
-------
#### iOS IAP Validation
iOS receipt validation requires `transactionId` and `receipt` (`signature` will be set to `null`).  For `receipt`, be sure to send the receipt `Payload`(the base64 encoded ASN.1 receipt) from Unity.   **Important:** If you have subscription IAP, you will need to add your app's public key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retreive your iOS App-Specific Shared Secret from the  <a href="https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/887212194/addons">iTunes Connect Console</a> > Select your app > Features > In-App Purchases > App-Specific Shared Secret.

##### Android IAP Validation
Android receipt validation requires `receipt` and `signature` are required (`transactionId` is set to `null`).  **Important:** You will need to add your app's public key in <a href="https://www.tenjin.io/dashboard/apps" target="_new"> the Tenjin dashboard</a>. You can retreive your Base64-encoded RSA public key from the <a href="https://play.google.com/apps/publish/"> Google Play Developer Console</a> > Select your app > Development Tools > Services & APIs. 

```csharp
  public static void OnProcessPurchase(PurchaseEventArgs purchaseEventArgs) {
    var price = purchaseEventArgs.purchasedProduct.metadata.localizedPriceString;
    var currencyCode = purchaseEventArgs.purchasedProduct.metadata.isoCurrencyCode;

    var wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(purchaseEventArgs.purchasedProduct.receipt);
    if (null == wrapper) {
        return;
    }

    var payload   = (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt
    var productId = purchaseEventArgs.purchasedProduct.definition.id;

    double lPrice = 0;
    double.TryParse(price, out lPrice);

#if UNITY_ANDROID

  var gpDetails = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
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
      BaseTenjin instance = Tenjin.getInstance ("API_KEY");
      instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, null, Receipt, Signature);
  }

  private static void CompletedIosPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string TransactionId, string Receipt)
  {
      BaseTenjin instance = Tenjin.getInstance ("API_KEY");
      instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, TransactionId, Receipt, null);
  }
```

Manual purchase event integration instructions:
-------

Alternatively, you can pass in-app purchase (IAP) transactions to Tenjin manually with no IAP validation. You can send `string productId`, `string currencyCode`, `int quantity`, and `double unitPrice` setting all other params to `null`.

```csharp
//Here is an example of how to implement the purchase in your post-validated purchase event
void CompletedPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice){

  //pass in the required data for the transaction without receipts

  BaseTenjin instance = Tenjin.getInstance ("API_KEY");
  instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, null, null, null);

  //any other code you want to handle in a completed purchase client side
}
```
- `ProductId` -> the name or ID of the product/purchase that the user is making
- `CurrencyCode` -> the currency code of the price
- `Quantity` -> the number of products/purchases that the user is making
- `UnitPrice` -> the unit price of the product

Total Revenue will be calculated as `Quantity`*`UnitPrice`

Tenjin custom event integration:
-------
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

Tenjin deferred deeplink integration instructions:
-------
Tenjin supports the ability to direct users to a specific part of your app after a new attributed install via Tenjin's campaign tracking URLs. You can utilize the `GetDeeplink` handler to access the deferred deeplink. To test you can follow the instructions found <a href="http://help.tenjin.io/t/how-do-i-use-and-test-deferred-deeplinks-with-my-campaigns/547">here</a>.

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

Android Manifest Requirements
-------
For Unity Android builds make sure you have a manifest file with the following requirements.
- Include `INTERNET` permissions within the manifest tags
- Include Google Play Services within the application tags
- Include Tenjin's INSTALL_REFERRER receiver

```xml
<manifest>
  ...
    <application ...>
      <meta-data android:name="com.google.android.gms.version"
        android:value="@integer/google_play_services_version" />
      ...
      <receiver android:name="com.tenjin.android.TenjinReferrerReceiver" android:exported="true">
        <intent-filter>
          <action android:name="com.android.vending.INSTALL_REFERRER"/>
        </intent-filter>
      </receiver>
      ...
    </application>
    ...
  <uses-permission android:name="android.permission.INTERNET"></uses-permission>
  ...
</manifest>
```

iOS Framework Requirements
-------
- `AdSupport.framework`
- `iAd.framework`
- `StoreKit.framework`
