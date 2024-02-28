# Summary

The Unity SDK for Tenjin. To learn more about Tenjin and our product offering, please visit https://www.tenjin.com.

- Please see our <a href="https://github.com/tenjin/tenjin-unity-sdk/blob/master/RELEASE_NOTES.md" target="_new">Release Notes</a> to see detailed version history of changes.
- Tenjin Unity SDK supports both iOS and Android.
- Review the [iOS][1] and [Android][2] documentation and apply the proper platform settings to your builds.
- For any issues or support, please contact: support@tenjin.com.

> [!NOTE]
> If you are using Unity SDK v1.12.29 or lower, please follow [these](https://docs.google.com/document/d/1AXn_IJXc4z_C-0Dzu7r8stOPFchxd3gCfQiO6tscdFI/edit?usp=sharing) steps before completing the SDK integration.
> To upgrade to v1.12.30 or higher from lower versions, please ensure to remove the Tenjin binaries before installing the latest Unity version.

> [!WARNING]
> If you have `libTenjinSDK.a` and/or `libTenjinSDKUniversal.a` from older Tenjin SDK versions, please delete them and run `pod install` to integrate it on iOS.

# Table of contents

- [SDK Integration][5]
  - [Google Play][6]
  - [Amazon store][7]
  - [OAID][8]
	- [MSA OAID][9]
	- [Huawei OAID][10]
  - [Proguard][11]
  - [App Initilization][12]
  - [App Store][13]
  - [ATTrackingManager (iOS)][14]
	- [Displaying an ATT permission prompt][15]
	  - [Configuring a user tracking description][16]
  - [SKAdNetwork and Conversion Value][17]
  - [SKAdNetwork iOS 15+ Postbacks][18]
  - [GDPR][19]
    - [Opt in/Opt out using CMP consents][72]
  - [Purchase Events][20]
	- [iOS IAP Validation][21]
	- [Android IAP Validation][22]
  - [Custom Events][23]
  - [Server-to-server integration][25]
  - [App Subversion][26]
  - [LiveOps Campaigns][70]
  - [Customer User ID][27]
  - [Analytics Installation ID][71]
  - [Google DMA parameters][73]
  - [Retry/cache events and IAP][69]
  - [Impression Level Ad Revenue Integration][68]
- [Testing][29]

# <a id="sdk-integration"></a> SDK Integration

1. Download the latest Unity SDK from <a href="https://github.com/tenjin/tenjin-unity-sdk/releases" target="_new">here.</a>

2. Import the `TenjinUnityPackage.unitypackage` into your project: `Assets -> Import Package`.

> We have a demo project - [tenjin-unity-sdk-demo][29] that demonstrates the integration of tenjin-unity-sdk. You can this project as example to understand how to integrate the tenjin-unity-sdk.

## <a id="google-play"></a>Google Play
By default, <b>unspecified</b> is the default App Store. Update the app store value to <b>googleplay</b>, if you distribute your app on Google Play Store.

Set your App Store Type value to `googleplay`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");

instance.SetAppStoreType(AppStoreType.googleplay);
```

The Tenjin SDK requires the following permissions:

```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" /> <!-- Required to get network connectivity (i.e. wifi vs. mobile) -->
```

Google Play Services requires all API level 32 (Android 13) apps using the advertising_id(Android Advertising ID (AAID)) to declare the Google Play Services AD_ID permission (shown below) in their manifest file. You are required to update the tenjin-android-sdk to version 1.12.8 in order to use the below permission.

```xml
<uses-permission android:name="com.google.android.gms.permission.AD_ID"/>
```

### <a id="play-services-ads-identifier-google-play"></a>Android Advertising ID (AAID) and Install Referrer
Add <a href="https://developers.google.com/android/guides/setup#list-dependencies" target="_new">Android Advertising ID (AAID)</a> and <a href="https://developer.android.com/google/play/installreferrer/library" target="_new">Install Referrer</a> libraries, add it to your build.gradle file.

```java
dependencies {
  implementation 'com.google.android.gms:play-services-ads-identifier:{version}'
  implementation 'com.android.installreferrer:installreferrer:{version}'
}
```

## <a id="amazon"></a>Amazon store
By default, <b>unspecified</b> is the default App Store. Update the app store value to <b>amazon</b>, if you distribute your app on Amazon store.

Set your App Store Type value to `amazon`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");

instance.SetAppStoreType(AppStoreType.amazon);
```

## <a id="oaid"></a>OAID and other Android App Stores

Tenjin supports promoting your app on other Android App Stores using the Android OAID. We have the following requirements for integrating OAID libraries. **If you plan to release your app outside of Google Play, make sure to implement these OAID libraries.**

### <a id="msa-oaid"></a>MSA OAID
MSA OAID is an advertising ID for devices manufactured in China that the MSA (Mobile Security Alliance) provides. For integration with the <a href="http://www.msa-alliance.cn/col.jsp?id=120" target="_new">MSA library</a>, download the following <a href="https://github.com/tenjin/tenjin-unity-sdk/blob/master/msa-oaid/oaid_sdk_1.0.25.aar" target="_new">oaid\_sdk\_1.0.25.aar</a>.

Place the `oaid_sdk_1.0.25.aar` file in your project's Android libs directory: `/Assets/Plugins/Android`

Set your App Store Type value to `other`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");

instance.SetAppStoreType(AppStoreType.other);
```

### <a id="huawei-oaid"></a>Huawei OAID
For outside of China, you can collect OAID using the library provided by Huawei. For integration with the <a href="https://developer.huawei.com/consumer/en/codelab/HMSAdsOAID/index.html#3" target="_new">Huawei OAID library</a>, download the following Huawei AAR file: <a href="huawei/huawei-ads-identifier.aar" target="_new">huawei-ads-identifier.aar</a>. If your app is in the <a href="https://appgallery.huawei.com/" target="_new">Huawei App Gallery</a>, download and add the Huawei Install Referrer file: <a href="huawei/huawei-ads-installreferrer.aar" target="_new">huawei-ads-installreferrer.aar</a>.

Place the Huawei files in your project's Android libs directory: `/Assets/Plugins/Android`

Set your App Store Type value to `other`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");

instance.SetAppStoreType(AppStoreType.other);
```

## <a id="proguard"></a>Proguard Settings

```java
-keep class com.tenjin.** { *; }
-keep public class com.google.android.gms.ads.identifier.** { *; }
-keep public class com.google.android.gms.common.** { *; }
-keep public class com.android.installreferrer.** { *; }
-keep class * extends java.util.ListResourceBundle {
    protected java.lang.Object[][] getContents();
}
-keepattributes *Annotation*
```

If you are using Huawei libraries, you can to use these settings:

```java
-keep class com.huawei.hms.ads.** { *; }
-keep interface com.huawei.hms.ads.** { *; }
```

## <a id="initialization"></a> App Initialization

1. Get your `SDK_KEY` from your app page. Note: `SDK_KEY` is unique for each of your app. You can create up to 3 keys for the same app.
![image-2]
2. In your project's first `Start()` method, add the following line of code. Also add to `OnApplicationPause()` if you want to send sessions data when a user resumes using the app from the background.

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
        BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
    
        // Sends install/open event to Tenjin
        instance.Connect();
      }
    }
    ```

> [!NOTE]
> Please ensure you implement this code on every `Start()`, not only on the first app open of the app. If we notice that you don't follow our recommendation, we can't give you the proper support or your account might be suspended.

## <a id="app-store"></a> App Store

We support three app store options,
1. googleplay
2. amazon
3. other

By default, <b>unspecified</b> is the default App Store. If you are publishing in a specific App Store, update the app store value to the appropriate app store value. The app store value <b>other</b> is used for Huawei AppGallery and other app stores:

1. `AndroidManifest.xml`:

    ```xml
    <meta-data
        android:name="TENJIN_APP_STORE"
        android:value="{{SET_APP_STORE_TYPE_VALUE}}" />
    ```

2. `SetAppStoreType()`:

    ```csharp
    BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
    
    instance.SetAppStoreType(AppStoreType.{{SET_APP_STORE_TYPE_VALUE}});
    ```

## <a id="attrackingmanager"></a> ATTrackingManager (iOS)

- Starting with iOS 14, you have the option to show the initial <a href="">ATTrackingManager</a> permissions prompt and selection to opt in/opt out users.

- If the device doesn't accept tracking permission, IDFA will become zero. If the device accepts tracking permission, the `Connect()` method will send the IDFA to our servers.

- You can also still call Tenjin `connect()`, without using ATTrackingManager. ATTrackingManager permissions prompt is not obligatory until the early spring of 2021.

```csharp
using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

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
      BaseTenjin instance = Tenjin.getInstance("SDK_KEY");

#if UNITY_IOS
      if (new Version(Device.systemVersion).CompareTo(new Version("14.0")) >= 0) {
        // Tenjin wrapper for requestTrackingAuthorization
        instance.RequestTrackingAuthorizationWithCompletionHandler((status) => {
          Debug.Log("===> App Tracking Transparency Authorization Status: " + status);

          // Sends install/open event to Tenjin
          instance.Connect();

        });
      }
      else {
          instance.Connect();
      }
#elif UNITY_ANDROID

      // Sends install/open event to Tenjin
      instance.Connect();

#endif
    }
}
```

### <a id="displayattprompt"></a>Displaying an ATT permission prompt

To comply with Apple’s ATT guidelines, you must provide a description for the ATT permission prompt, then implement the permission request in your application.

> [!NOTE]
> You must implement the permission request before serving ads in your game.

#### <a id="configureusertrackdescription"></a> Configuring a user tracking description
Apple requires a description for the ATT permission prompt. You need to set the description with the `NSUserTrackingUsageDescription` key in the `Info.plist` file of your Xcode project. You have to provide a message that informs the user why you are requesting permission to use device tracking data:

- In your Xcode project navigator, open the `Info.plist` file.
- Click the add button (+) beside any key in the property list editor to create a new property key.
- Enter the key name `NSUserTrackingUsageDescription`.
- Select a string value type.
- Enter the app tracking transparency message in the value field. Some examples include:
	- "We will use your data to provide a better and personalized ad experience."
	- "We try to show ads for apps and products that will be most interesting to you based on the apps you use, the device you are on, and the country you are in."
	- "We try to show ads for apps and products that will be most interesting to you based on the apps you use."

> [!NOTE]
> Apple provides specific [app store guidelines][30] that define acceptable use and messaging for all end-user facing privacy-related features. Tenjin does not provide legal advice. Therefore, the information on this page is not a substitute for seeking your own legal counsel to determine the legal requirements of your business and processes, and how to address them.

## <a id="skadnetwork-cv"></a> SKAdNetwork and Conversion Values

As part of <a href="https://developer.apple.com/documentation/storekit/skadnetwork">SKAdNetwork</a>, we created a wrapper method for <a href="https://developer.apple.com/documentation/storekit/skadnetwork/3919928-updatepostbackconversionvalue">`updatePostbackConversionValue(_:)`</a>.
Our methods will register the equivalent SKAdNetwork methods and also send the conversion values to our servers.

`updatePostbackConversionValue(conversionValue: Integer)` 6 bit value should correspond to the in-app event and shouldn't be entered as binary representation but 0-63 integer. Our server will reject any invalid values.

As of iOS 16.1, which supports SKAdNetwork 4.0, you can now send `coarseValue` (String, with possible variants being "low", "medium" or "high") and `lockWindow` (Boolean) as parameters on the update postback method:

`updatePostbackConversionValue(conversionValue: Integer, coarseValue: String)`

`updatePostbackConversionValue(conversionValue: Integer, coarseValue: String, lockWindow: Bool)`

-   For iOS version 16.1+ which supports SKAdNetwork 4.0, you can call this method as many times as you want and can make the conversion value lower or higher than the previous value.
    
-   For iOS versions lower than 16.1 supporting SKAdnetWork versions lower than 4.0, you can call this method and our SDK will automatically detect the iOS version and update `conversionValue` only.

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
      BaseTenjin instance = Tenjin.getInstance("SDK_KEY");

#if UNITY_IOS

      // Sends install/open event to Tenjin
      instance.Connect();

      // Sets SKAdNetwork Conversion Value
      // You will need to use a value between 0-63 for <YOUR 6 bit value>
      instance.updatePostbackConversionValue(<your 6 bit value>);
      
      // For iOS 16.1+ (SKAN 4.0)
      instance.updatePostbackConversionValue(<your 6 bit value>, "medium");
      instance.updatePostbackConversionValue(<your 6 bit value>, "medium", true);

#elif UNITY_ANDROID

      // Sends install/open event to Tenjin
      instance.Connect();

#endif
    }
}
```

## <a id="skadnetwork-ios15"></a>SKAdNetwork and iOS 15+ Advertiser Postbacks

To specify Tenjin as the destination for your [SK Ad Network postbacks][31], do the following:

1. Select `Info.plist` in the Project navigator in Xcode.
2. Click the Add button (+) beside a key in the property list editor and press Return.
3. Type the key name `NSAdvertisingAttributionReportEndpoint`.
4. Choose String from the pop-up menu in the Type column.
5. Enter `https://tenjin-skan.com`

These steps are adapted from Apple's instructions at [https://developer.apple.com/documentation/storekit/skadnetwork/configuring\_an\_advertised\_app][32].

> [!NOTE]
> If you are using AppLovin MAX for mediation, their Unity SDK will overwrite any value you entered for `NSAdvertisingAttributionReportEndpoint` with their own URL during the build process. You should be able to set the NSAdvertisingAttributionReportEndpoint to `https://tenjin-skan.com` in XCode after it's been overwritten in the following process.

1. Export the iOS app following the steps Unity outlines [here][33].

2. After you build the iOS app, you should have an XCode project that has this structure: https://docs.unity3d.com/Manual/StructureOfXcodeProject.html

3. Navigate to the `Info.plist` file in the XCode project to manually change the NSAdvertisingAttributionReportEndpoint to `https://tenjin-skan.com`.
Otherwise, you can ask your AppLovin account manager to set up forwarding the postbacks to us.

## <a id="gdpr"></a> GDPR

As part of GDPR compliance, with Tenjin's SDK you can opt-in, opt-out devices/users, or select which specific device-related params to opt-in or opt-out. `OptOut()` will not send any API requests to Tenjin, and we will not process any events.

To opt-in/opt-out:

```csharp
void Start () {

  BaseTenjin instance = Tenjin.getInstance("SDK_KEY");

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

- Kindly note that we require the following parameters to properly track devices in Tenjin's system. If one of these mandatory parameters is missing, the event will not be processed or recorded.

  - For Android, 
	- `advertising_id`
  - For iOS
	- `developer_device_id`
- If you are targeting IMEI and/or OAID Ad Networks for Android, add:

  - `imei`
  - `oaid`

- If you intend to use Google Ad Words, you will also need to add:
  - `platform`
  - `os_version`
  - `app_version`
  - `locale`
  - `device_model`
  - `build_id`

If you want to only get specific device-related parameters, use `OptInParams()`. In example below, we will only these device-related parameters: `ip_address`, `advertising_id`, `developer_device_id`, `limit_ad_tracking`, `referrer`, and `iad`:

```csharp
BaseTenjin instance = Tenjin.getInstance("SDK_KEY");

List<string> optInParams = new List<string> {"ip_address", "advertising_id", "developer_device_id", "limit_ad_tracking", "referrer", "iad"};
instance.OptInParams(optInParams);

instance.Connect();
```

If you want to send ALL parameters except specific device-related parameters, use `OptOutParams()`. In the example below, we will send ALL device-related parameters except: `locale`, `timezone`, and `build_id` parameters.

```csharp
BaseTenjin instance = Tenjin.getInstance("SDK_KEY");

List<string> optOutParams = new List<string> {"locale", "timezone", "build_id"};
instance.OptOutParams(optOutParams);

instance.Connect();
```

### <a id="optin-cmp"></a>Opt in/out using CMP
You can automatically opt in or opt out using your CMP consents (purpose 1) which are already saved in the user's device. The method returns a boolean to let you know if it's opted in or out.

`OptInOutUsingCMP()`

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
optInOut = instance.OptInOutUsingCMP(); 
```

#### Device-Related Parameters

| Param                 | Description                  | Platform | Reference                                 |
| --------------------- | ---------------------------- | -------- | ----------------------------------------- |
| ip\_address           | IP Address                   | All      |                                           |
| advertising\_id       | Device Advertising ID        | All      | [Android][34], [iOS][35]                  |
| developer\_device\_id | ID for Vendor                | iOS      | [iOS][36]                                 |
| oaid                  | Open Advertising ID          | Android  | [Android][37]                             |
| imei                  | Device IMEI                  | Android  | [Android][38]                             |
| limit\_ad\_tracking   | limit ad tracking enabled    | All      | [Android][39], [iOS][40]                  |
| platform              | platform                     | All      | iOS or Android                            |
| referrer              | Google Play Install Referrer | Android  | [Android][41]                             |
| iad                   | Apple Search Ad parameters   | iOS      | [iOS][42]                                 |
| os\_version           | operating system version     | All      | [Android][43], [iOS][44]                  |
| device                | device name                  | All      | [Android][45], [iOS (hw.machine)][46]     |
| device\_manufacturer  | device manufactuer           | Android  | [Android][47]                             |
| device\_model         | device model                 | All      | [Android][48], [iOS (hw.model)][49]       |
| device\_brand         | device brand                 | Android  | [Android][50]                             |
| device\_product       | device product               | Android  | [Android][51]                             |
| device\_model\_name   | device machine               | iOS      | [iOS (hw.model)][52]                      |
| device\_cpu           | device cpu name              | iOS      | [iOS (hw.cputype)][53]                    |
| carrier               | phone carrier                | Android  | [Android][54]                             |
| connection\_type      | cellular or wifi             | Android  | [Android][55]                             |
| screen\_width         | device screen width          | Android  | [Android][56]                             |
| screen\_height        | device screen height         | Android  | [Android][57]                             |
| os\_version\_release  | operating system version     | All      | [Android][58], [iOS][59]                  |
| build\_id             | build ID                     | All      | [Android][60], [iOS (kern.osversion)][61] |
| locale                | device locale                | All      | [Android][62], [iOS][63]                  |
| country               | locale country               | All      | [Android][64], [iOS][65]                  |
| timezone              | timezone                     | All      | [Android][66], [iOS][67]                  |

## <a id="purchase-events"></a>Purchase Events

## <a id="ios-iap-validation"></a>iOS IAP Validation

iOS receipt validation requires `transactionId` and `receipt`. For `receipt`, be sure to send the receipt `Payload` (the base64 encoded ASN.1 receipt) from Unity.

**IMPORTANT:** If you have subscription IAP, you will need to add your app's shared secret in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retrieve your iOS App-Specific Shared Secret from the <a href="https://appstoreconnect.apple.com/" target="_new">App Store Connect</a> \> Select your app \> General \> App Information \> App-Specific Shared Secret.

## <a id="android-iap-validation"></a>Android IAP Validation

### <a id="android-iap-validation-google"></a>Google Play App Store

Google Play receipt validation requires `receipt` and `signature` parameters.

**IMPORTANT:** You will need to add your app's public key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retrieve your Base64-encoded RSA public key from the <a href="https://play.google.com/apps/publish/" target="_new"> Google Play Developer Console</a> \> Select your app \> Monetization setup.

> [!WARNING]
> For Google play, Please ensure to 'acknowledge' the purchase event before sending it to Tenjin. For more details, read <a href="https://developer.android.com/google/play/billing/integrate#non-consumable-products" target="_blank">here</a>.


### <a id="android-iap-validation-amazon"></a>Amazon AppStore

Amazon AppStore receipt validation requires `receiptId` and `userId` parameters.

> [!IMPORTANT]
> You will need to add your Amazon app's Shared Key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. The shared secret can be found on the Shared Key in your developer account with the <a href="https://developer.amazon.com/settings/console/sdk/shared-key/" target="_new">Amazon Appstore account</a> 


### iOS and Android IAP Example:

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

    var store     = (string)wrapper["Store"]; // GooglePlay, AmazonAppStore, AppleAppStore, etc.
    var payload   = (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt. For Android, it is the raw JSON receipt.
    var productId = purchaseEventArgs.purchasedProduct.definition.id;

#if UNITY_ANDROID

  if (store.Equals("GooglePlay")) {
    var googleDetails = Json.Deserialize(payload) as Dictionary<string, object>;
    var googleJson    = (string)googleDetails["json"];
    var googleSig     = (string)googleDetails["signature"];

    CompletedAndroidPurchase(productId, currencyCode, 1, lPrice, googleJson, googleSig);
  }

  if (store.Equals("AmazonApps")) {
    var amazonDetails   = Json.Deserialize(payload) as Dictionary<string, object>;
    var amazonReceiptId = (string)amazonDetails["receiptId"];
    var amazonUserId    = (string)amazonDetails["userId"];

    CompletedAmazonPurchase(productId, currencyCode, 1, lPrice, amazonReceiptId, amazonUserId);
  }

#elif UNITY_IOS

  var transactionId = purchaseEventArgs.purchasedProduct.transactionID;

  CompletedIosPurchase(productId, currencyCode, 1, lPrice , transactionId, payload);

#endif

  }

  private static void CompletedAndroidPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string Receipt, string Signature)
  {
    BaseTenjin instance = Tenjin.getInstance("SDK_KEY");
    instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, null, Receipt, Signature);
  }

  private static void CompletedIosPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string TransactionId, string Receipt)
  {
    BaseTenjin instance = Tenjin.getInstance("SDK_KEY");
    instance.Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, TransactionId, Receipt, null);
  }

  private static void CompletedAmazonPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice, string ReceiptId, string UserId)
  {
    BaseTenjin instance = Tenjin.getInstance("SDK_KEY");
    instance.TransactionAmazon(ProductId, CurrencyCode, Quantity, UnitPrice, ReceiptId, UserId);
  }
```

**Disclaimer:** If you are implementing purchase events on Tenjin for the first time, make sure to verify the data with other tools you’re using before you start scaling up your user acquisition campaigns using purchase data.

:warning: **(Flexible App Store Commission setup)**


Choose between 15% and 30% App Store’s revenue commission via our new setup. The steps are -
* Go to CONFIGURE --> Apps
* Click on the app you want to change it for
* Under the ‘App Store Commission’ section click ‘Edit’
* Choose 30% or 15% as your desired app store commission.
* Select the start date and end date (Or you can keep the end date blank if you dont want an end date)
* Click Save (note: the 15% commission can be applied only to dates moving forward and not historical dates. So please set the start date from the date you make the change and forward)

### <a id="subscription-iap"></a> Subscription IAP

- You are responsible to send a subscription transaction one time during each subscription interval (i.e., For example, for a monthly subscription, you will need to send us 1 transaction per month). In the example timeline below, a transaction event should only be sent at the "First Charge" and "Renewal" events. During the trial period, do not send Tenjin the transaction event.

  <img src="https://docs-assets.developer.apple.com/published/6631e50f32/110c0e3f-e0e3-4dbd-bc28-d8db4b28bd1c.png" />

- Tenjin does not de-dupe duplicate transactions.

- If you have iOS subscription IAP, you will need to add your app's public key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new"> Tenjin dashboard</a>. You can retrieve your iOS App-Specific Shared Secret from the <a href="https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/887212194/addons">iTunes Connect Console</a> \> Select your app \> Features \> In-App Purchases \> App-Specific Shared Secret.

- For more information on iOS subscriptions, please see: <a href="https://developer.apple.com/documentation/storekit/in-app_purchase/subscriptions_and_offers">Apple documentation on Working with Subscriptions</a>

- For more information on Android subscriptions, please see: <a href="https://developer.android.com/distribute/best-practices/earn/subscriptionss">Google Play Billing subscriptions documentation</a>

## <a id="custom-events"></a> Custom Events

**IMPORTANT: Limit custom event names to less than 80 characters. Do not exceed 500 unique custom event names.**

- Include the Assets folder in your Unity project
- In your projects' method for the custom event, write the following for a named event: `Tenjin.getInstance("<SDK_KEY>").SendEvent("name")` and the following for a named event with an integer value: `Tenjin.getInstance("<SDK_KEY>").SendEvent("nameWithValue","value")`
- Make sure `value` passed is an integer. If `value` is not an integer, your event will not be passed.

Here's an example of the code:

```csharp
void MethodWithCustomEvent(){
    //event with name
    BaseTenjin instance = Tenjin.getInstance ("SDK_KEY");
    instance.SendEvent("name");

    //event with name and integer value
    instance.SendEvent("nameWithValue", "value");
}
```

`.SendEvent("name")` is for events that are static markers or milestones. This would include things like `tutorial_complete`, `registration`, or `level_1`.

`.SendEvent("name", "value")` is for events that you want to do math on a property of that event. For example, `("coins_purchased", "100")` will let you analyze a sum or average of the coins that are purchased for that event.


## <a id="server-to-server"></a>Server-to-server integration

Tenjin offers server-to-server integration, which is a paid feature. If you want to access to the documentation, please send email to support@tenjin.com and discuss the pricing.

## <a id="subversion"></a>App Subversion parameter for A/B Testing (requires DataVault)

If you are running A/B tests and want to report the differences, we can append a numeric value to your app version using the `AppendAppSubversion()` method. For example, if your app version `1.0.1`, and set `AppendAppSubversion(8888)`, it will report app version as `1.0.1.8888`.

This data will appear within DataVault, where you will be able to run reports using the app subversion values.

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
instance.AppendAppSubversion(8888);
instance.Connect();
```

## <a id="liveops-campaigns"></a>LiveOps Campaigns

Tenjin supports retrieving of user attribution information, like sourcing ad network and campaign, from the SDK. This will allow developers to collect and analyze user-level attribution data in real-time. Here are the possible use cases using Tenjin LiveOps Campaigns:

- If you have your own data anlytics tool, custom callback will allow you to tie the attribution data to your in-game data per device level.
- Show different app content depending on where the user comes from. For example, if user A is attributed to organic and user B is attributed to Facebook and user B is likely to be more engaged with your app, then you want to show a special in-game offer after the user installs the app. If you want to discuss more specific use cases, please write to support@tenjin.com.

> [!WARNING]
> LiveOps Campaigns is a paid feature, so please contact your Tenjin account manager if you would like to get access.

## <a id="customer-user-id"></a>Customer User ID

You can set and get customer user id to send as a parameter on events.

`.SetCustomerUserId("user_id")`

`.GetCustomerUserId()`

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
instance.SetCustomerUserId("user_id");
string userId = instance.GetCustomerUserId(); 
```

## <a id="analytics-id"></a>Analytics Installation ID
You can get the analytics id which is generated randomly and saved in the local storage of the device.

`GetAnalyticsInstallationId()`

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
analyticsId = instance.GetAnalyticsInstallationId; 
```

## <a id="google-dma"></a>Google DMA parameters
If you already have a CMP integrated, Google DMA parameters will be automatically collected by the Tenjin SDK. There’s nothing to implement in the Tenjin SDK if you have a CMP integrated.
If you want to override your CMP, or simply want to build your own consent mechanisms, you can use the following:

`SetGoogleDMAParameters(bool, bool)`

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
instance.SetGoogleDMAParameters(adPersonalization, adUserData); 
```

## <a id="retry-cache"></a>Retry/cache events and IAP
You can enable/disable retrying and caching events and IAP when requests fail or users don't have internet connection. These events will be sent after a new event has been added to the queue and user has recovered connection.

`.SetCacheEventSetting(true)`

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
instance.SetCacheEventSetting(true);
```

## <a id="ilrd"></a>Impression Level Ad Revenue Integration

Tenjin supports the ability to integrate with the Impression Level Ad Revenue (ILRD) feature from,
- AppLovin
- Unity LevelPlay
- HyperBid
- AdMob
- Topon
- CAS
- TradPlus

This feature allows you to receive events which correspond to your ad revenue which is affected by each advertisement shown to a user. To enable this feature, follow the below instructions.

> [!WARNING]
> ILRD is a paid feature, so please contact your Tenjin account manager to discuss the price at first before sending ILRD events.

# <a id="testing"></a>Testing

You can verify if the integration is working through our <a href="https://www.tenjin.io/dashboard/sdk_diagnostics">Live Test Device Data Tool</a>. Add your `advertising_id` or `IDFA/GAID` to the list of test devices. You can find this under Support -\> <a href="https://www.tenjin.io/dashboard/debug_app_users">Test Devices</a>. Go to the <a href="https://www.tenjin.io/dashboard/sdk_diagnostics">SDK Live page</a> and send the test events from your app. You should see live events come in:

<br />

![][image-1]

<br />

[1]:	https://github.com/tenjin/tenjin-ios-sdk
[2]:	https://github.com/tenjin/tenjin-android-sdk
[3]:	https://github.com/googlesamples/unity-jar-resolver
[4]:	#proguard
[5]:	#sdk-integration
[6]:	#google-play
[7]:	#amazon
[8]:	#oaid
[9]:	#msa-oaid
[10]:	#huawei-oaid
[11]:	#proguard
[12]:	#initialization
[13]:	#app-store
[14]:	#attrackingmanager
[15]:	#displayattprompt
[16]:	#configureusertrackdescription
[17]:	#skadnetwork-cv
[18]:	#skadnetwork-ios15
[19]:	#gdpr
[20]:	#purchase-events
[21]:	#ios-iap-validation
[22]:	#android-iap-validation
[23]:	#custom-events
[24]:	#deferred-deeplinks
[25]:	#server-to-server
[26]:	#subversion
[27]:	#customer-user-id
[28]:	#testing
[29]:	https://github.com/tenjin/tenjin-unity-sdk-demo
[30]:	https://developer.apple.com/app-store/user-privacy-and-data-use/
[31]:	https://developer.apple.com/documentation/storekit/skadnetwork/receiving_ad_attributions_and_postbacks
[32]:	https://developer.apple.com/documentation/storekit/skadnetwork/configuring_an_advertised_app
[33]:	https://docs.unity3d.com/Manual/iphone-GettingStarted.html
[34]:	https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.html#getAdvertisingIdInfo(android.content.Context)
[35]:	https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614151-advertisingidentifier
[36]:	https://developer.apple.com/documentation/uikit/uidevice/1620059-identifierforvendor
[37]:	http://www.msa-alliance.cn/col.jsp?id=120
[38]:	https://developer.android.com/reference/android/telephony/TelephonyManager#getImei()
[39]:	https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.Info.html#isLimitAdTrackingEnabled()
[40]:	https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614148-isadvertisingtrackingenabled
[41]:	https://developer.android.com/google/play/installreferrer/index.html
[42]:	https://searchads.apple.com/advanced/help/measure-results/#attribution-api
[43]:	https://developer.android.com/reference/android/os/Build.VERSION.html#SDK_INT
[44]:	https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion
[45]:	https://developer.android.com/reference/android/os/Build.html#DEVICE
[46]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[47]:	https://developer.android.com/reference/android/os/Build.html#MANUFACTURER
[48]:	https://developer.android.com/reference/android/os/Build.html#MODEL
[49]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[50]:	https://developer.android.com/reference/android/os/Build.html#BRAND
[51]:	https://developer.android.com/reference/android/os/Build.html#PRODUCT
[52]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[53]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[54]:	https://developer.android.com/reference/android/telephony/TelephonyManager.html#getSimOperatorName()
[55]:	https://developer.android.com/reference/android/net/ConnectivityManager.html#getActiveNetworkInfo()
[56]:	https://developer.android.com/reference/android/util/DisplayMetrics.html#widthPixels
[57]:	https://developer.android.com/reference/android/util/DisplayMetrics.html#heightPixels
[58]:	https://developer.android.com/reference/android/os/Build.VERSION.html#RELEASE
[59]:	https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion
[60]:	https://developer.android.com/reference/android/os/Build.html
[61]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[62]:	https://developer.android.com/reference/java/util/Locale.html#getDefault()
[63]:	https://developer.apple.com/documentation/foundation/nslocalekey
[64]:	https://developer.android.com/reference/java/util/Locale.html#getDefault()
[65]:	https://developer.apple.com/documentation/foundation/nslocalecountrycode
[66]:	https://developer.android.com/reference/java/util/TimeZone.html
[67]:	https://developer.apple.com/documentation/foundation/nstimezone/1387209-localtimezone
[68]:	#ilrd
[69]:   #retry-cache
[70]:   #liveops-campaigns
[71]: #analytics-id
[72]: #optin-cmp
[73]: #google-dma

[image-1]:	https://s3.amazonaws.com/tenjin-instructions/sdk_live_purchase_events_2.png
[image-2]:	https://s3.amazonaws.com/tenjin-instructions/app_api_key.png
