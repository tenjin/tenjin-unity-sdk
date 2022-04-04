# Summary

The Unity SDK for Tenjin. To learn more about Tenjin and our product offering, please visit https://www.tenjin.com.

- Please see our <a href="https://github.com/tenjin/tenjin-unity-sdk/blob/master/RELEASE_NOTES.md" target="_new">Release Notes</a> to see detailed version history of changes.
- Tenjin Unity SDK supports both iOS and Android.
- Review the [iOS][1] and [Android][2] documentation and apply the proper platform settings to your builds.
- For any issues or support, please contact: support@tenjin.com
- **iOS Notes**:

  - Xcode 12 requirement, if you’re using Unity iOS SDK v1.12.0 and higher.
  - When building iOS, confirm that these frameworks were automatically added to the Xcode build. If any are missing, you will need to add them manually.
	- AdServices.framework
	- AdSupport.framework
	- AppTrackingTransparency.framework
	- iAd.framework
	- StoreKit.framework
  - For AppTrackingTransparency, be sure to update your project `.plist` file and add `Privacy - Tracking Usage Description` <a href="https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription" target="_new">(NSUserTrackingUsageDescription)</a> along with the text message you want to display to users. This library is only available in iOS 14.0+.
  - For <a href="https://developer.apple.com/documentation/iad/setting_up_apple_search_ads_attribution" target="_new">Apple Search Ads Attribution</a> support, please be sure to upgrade to v1.12.6+ and add the `AdServices.framework` library. This library is only available in iOS 14.3+.

- **Android Notes**:

  1. If you have another SDK installed which already has Google Play Services installed or uses [PlayServicesResolver][3], you may need to delete duplicate libraries:

```
 /Assets/Plugins/Android/play-services-ads-identifier--*.aar
 /Assets/Plugins/Android/play-services-basement---*.aar
```

  2. If you are using Tenjin Unity SDK alongside another SDK in Unity version 2019.4.21f1 and higher, and are using Gradle to build the Android App, you might face build errors such as `DuplicateMethodException` etc., or find that referrer install is not working. If that is the case, please do the following:
	 * Remove all the `*.aar` files from the `Assets/Plugins/Android` folder except `tenjin.aar`.
	 * Add the following to your `mainTemplate.gradle` file:
		```groovy
		    // Android Resolver Repos Start
		    ([rootProject] + (rootProject.subprojects as List)).each { project ->
		        project.repositories {
		            def unityProjectPath = $/file:///**DIR_UNITYPROJECT**/$.replace("\\", "/")
		            maven {
		                url "https://maven.google.com"
		            }
		            maven {
		                url "https://s3.amazonaws.com/moat-sdk-builds"
		            }
		            maven {
		                url 'https://developer.huawei.com/repo/'
		            }
		            mavenLocal()
		            mavenCentral()
		            google()
		        }
		    }

		// Android Resolver Repos End
		    apply plugin: 'com.android.library'
		    **APPLY_PLUGINS**
		    dependencies {
		        implementation fileTree(dir: 'libs', include: ['*.jar'])
		    // Android Resolver Dependencies Start
		        implementation 'com.android.support:multidex:1.0.3'
		        implementation 'com.google.android.gms:play-services-analytics:{version}'
		        implementation 'com.android.installreferrer:installreferrer:{version}'
		        implementation 'com.huawei.hms:ads-identifier:{version}'
		        implementation 'com.huawei.hms:ads-installreferrer:{version}'
		        androidTestImplementation('com.android.support.test.espresso:espresso-core:3.0.2', {
		            exclude group: 'com.android.support', module: 'support-annotations'
		        })
		    // Android Resolver Dependencies End
		    **DEPS**}
		```
	  * Add the following entry to the `gradleTemplate.properties` file:
		```
		android.useAndroidX=true
		```
  3. If you see the following errors on the app initialization, move tenjin.aar file from `/Assets/Plugins/Android/Tenjin/libs` to `/Assets/Plugins/Android/`. Also check the Proguard Settings [here][4].

	```
	AndroidJavaException: java.lang.NoSuchMethodError: no static method with name='setWrapperVersion'
	AndroidJavaException: java.lang.ClassNotFoundException: com.tenjin.android.TenjinSDK
	```
  4. If you update SDK to v1.12.9 or higher, we are going to make version 2019.4.21f1 the minimum Unity 3D version. If you prefer to use the older version, please contact us, so we can help you with a customized solution. Furthermore, upgrade the External Dependency Manager for Unity to 1.12.167.

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
  - [Purchase Events][20]
	- [iOS IAP Validation][21]
	- [Android IAP Validation][22]
  - [Custom Events][23]
  - [Deferred Deeplinks][24]
  - [Server-to-server integration][25]
  - [App Subversion][26]
  - [Impression Level Ad Revenue Integration][27]
	- [AppLovin Impression Level Ad Revenue Integration][28]
	- [IronSource Impression Level Ad Revenue Integration][29]
- [Testing][30]

# <a id="sdk-integration"></a> SDK Integration

1. Download the latest Unity SDK from <a href="https://github.com/tenjin/tenjin-unity-sdk/releases" target="_new">here.</a>

2. Import the `TenjinUnityPackage.unitypackage` into your project: `Assets -> Import Package`.

3. By default, we have included <a href="https://developers.google.com/android/guides/setup" target="_new">Google Play Services</a> AAR files as part of our SDK. If you do not plan on using Google Play Services, you can delete these AAR files:

```
 /Assets/Plugins/Android/play-services-*.aar
 /Assets/Plugins/Android/installreferrer-*.aar
```

> We have a demo project - [tenjin-unity-sdk-demo][31] that demonstrates the integration of tenjin-unity-sdk. You can this project as example to understand how to integrate the tenjin-unity-sdk.

## <a id="google-play"></a>Google Play
By default, <b>unspecified</b> is the default App Store. Update the app store value to <b>googleplay</b>, if you distribute your app on Google Play Store.

Set your App Store Type value to `googleplay`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

instance.SetAppStoreType(AppStoreType.googleplay);
```

## <a id="amazon"></a>Amazon store
By default, <b>unspecified</b> is the default App Store. Update the app store value to <b>amazon</b>, if you distribute your app on Amazon store.

Set your App Store Type value to `amazon`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

instance.SetAppStoreType(AppStoreType.amazon);
```

## <a id="oaid"></a>OAID and other Android App Stores

Tenjin supports promoting your app on other Android App Stores using the Android OAID. We have the following requirements for integrating OAID libraries. **If you plan to release your app outside of Google Play, make sure to implement these OAID libraries.**

### <a id="msa-oaid"></a>MSA OAID
MSA OAID is an advertising ID for devices manufactured in China that the MSA (Mobile Security Alliance) provides. For integration with the <a href="http://www.msa-alliance.cn/col.jsp?id=120" target="_new">MSA library</a>, download the following <a href="https://github.com/tenjin/tenjin-unity-sdk/blob/master/msa-oaid/oaid_sdk_1.0.25.aar" target="_new">oaid\_sdk\_1.0.25.aar</a>.

Place the `oaid_sdk_1.0.25.aar` file in your project's Android libs directory: `/Assets/Plugins/Android`

Set your App Store Type value to `other`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

instance.SetAppStoreType(AppStoreType.other);
```

### <a id="huawei-oaid"></a>Huawei OAID
For outside of China, you can collect OAID using the library provided by Huawei. For integration with the <a href="https://developer.huawei.com/consumer/en/codelab/HMSAdsOAID/index.html#3" target="_new">Huawei OAID library</a>, download the following Huawei AAR file: <a href="huawei/huawei-ads-identifier.aar" target="_new">huawei-ads-identifier.aar</a>. If your app is in the <a href="https://appgallery.huawei.com/" target="_new">Huawei App Gallery</a>, download and add the Huawei Install Referrer file: <a href="huawei/huawei-ads-installreferrer.aar" target="_new">huawei-ads-installreferrer.aar</a>.

Place the Huawei files in your project's Android libs directory: `/Assets/Plugins/Android`

Set your App Store Type value to `other`:

```csharp
BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

instance.SetAppStoreType(AppStoreType.other);
```

## <a id="proguard"></a>Proguard Settings

```java
-keep class com.tenjin.** { *; }
-keep public class com.google.android.gms.ads.identifier.** { *; }
-keep public class com.google.android.gms.common.** { *; }
-keep public class com.android.installreferrer.** { *; }
-keep class * extends java.util.ListResourceBundle {
    protected Object[][] getContents();
}
-keepattributes *Annotation*
```

If you are using Huawei libraries, you can to use these settings:

```java
-keep class com.huawei.hms.ads.** { *; }
-keep interface com.huawei.hms.ads.** { *; }
```

## <a id="initialization"></a> App Initialization

1. Get your `<API_KEY>` from your <a href="https://www.tenjin.io/dashboard/docs" target="_new">Tenjin dashboard</a>.
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
    BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

    // Sends install/open event to Tenjin
    instance.Connect();
  }
}
```

**NOTE:** Please ensure you implement this code on every `Start()`, not only on the first app open of the app. If we notice that you don't follow our recommendation, we can't give you the proper support or your account might be suspended.

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
BaseTenjin instance = Tenjin.getInstance("<API_KEY>");

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
      BaseTenjin instance = Tenjin.getInstance("API_KEY");

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

> Note: You must implement the permission request before serving ads in your game.

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

> Note: Apple provides specific [app store guidelines][32] that define acceptable use and messaging for all end-user facing privacy-related features. Tenjin does not provide legal advice. Therefore, the information on this page is not a substitute for seeking your own legal counsel to determine the legal requirements of your business and processes, and how to address them.

## <a id="skadnetwork-cv"></a> SKAdNetwork and Conversion Values

As part of <a href="https://developer.apple.com/documentation/storekit/skadnetwork">SKAdNetwork</a>, we created wrapper methods for `registerAppForAdNetworkAttribution()` and <a href="https://developer.apple.com/documentation/storekit/skadnetwork/3566697-updateconversionvalue">`updateConversionValue(_:)`</a>.
Our methods will register the equivalent SKAdNetwork methods and also send the conversion values to our servers.

`updateConversionValue(_:)` 6 bit value should correspond to the in-app event and shouldn't be entered as binary representation but 0-63 integer. Our server will reject any invalid values.

- <a href="https://docs.google.com/spreadsheets/d/1jrRrTP6YX62of2WaJamtPBSWZJ-97IpTWn0IwTroH6Y/edit#gid=1596716780">Examples for IAP based games </a>
- <a href="https://docs.google.com/spreadsheets/d/15JaN44yQyW7dqqRGi5Wwnq2P6ng-4n6EztMmMj5A7c4/edit#gid=0">Examples for Ad revenue based games </a>

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

## <a id="skadnetwork-ios15"></a>SKAdNetwork and iOS 15+ Advertiser Postbacks

To specify Tenjin as the destination for your [SK Ad Network postbacks][33], do the following:

1. Select `Info.plist` in the Project navigator in Xcode.
2. Click the Add button (+) beside a key in the property list editor and press Return.
3. Type the key name `NSAdvertisingAttributionReportEndpoint`.
4. Choose String from the pop-up menu in the Type column.
5. Enter `https://tenjin-skan.com`

These steps are adapted from Apple's instructions at [https://developer.apple.com/documentation/storekit/skadnetwork/configuring\_an\_advertised\_app][34].

**NOTE**: If you are using AppLovin MAX for mediation, their Unity SDK will overwrite any value you entered for `NSAdvertisingAttributionReportEndpoint` with their own URL during the build process. You should be able to set the NSAdvertisingAttributionReportEndpoint to `https://tenjin-skan.com` in XCode after it's been overwritten in the following process.

1. Export the iOS app following the steps Unity outlines [here][35].

2. After you build the iOS app, you should have an XCode project that has this structure: https://docs.unity3d.com/Manual/StructureOfXcodeProject.html

3. Navigate to the `Info.plist` file in the XCode project to manually change the NSAdvertisingAttributionReportEndpoint to `https://tenjin-skan.com`.
Otherwise, you can ask your AppLovin account manager to set up forwarding the postbacks to us.

## <a id="gdpr"></a> GDPR

As part of GDPR compliance, with Tenjin's SDK you can opt-in, opt-out devices/users, or select which specific device-related params to opt-in or opt-out. `OptOut()` will not send any API requests to Tenjin, and we will not process any events.

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

If you want to send ALL parameters except specific device-related parameters, use `OptOutParams()`. In the example below, we will send ALL device-related parameters except: `locale`, `timezone`, and `build_id` parameters.

```csharp
BaseTenjin instance = Tenjin.getInstance("API_KEY");

List<string> optOutParams = new List<string> {"locale", "timezone", "build_id"};
instance.OptOutParams(optOutParams);

instance.Connect();
```

#### Device-Related Parameters

| Param                 | Description                  | Platform | Reference                                 |
| --------------------- | ---------------------------- | -------- | ----------------------------------------- |
| ip\_address           | IP Address                   | All      |                                           |
| advertising\_id       | Device Advertising ID        | All      | [Android][36], [iOS][37]                  |
| developer\_device\_id | ID for Vendor                | iOS      | [iOS][38]                                 |
| oaid                  | Open Advertising ID          | Android  | [Android][39]                             |
| imei                  | Device IMEI                  | Android  | [Android][40]                             |
| limit\_ad\_tracking   | limit ad tracking enabled    | All      | [Android][41], [iOS][42]                  |
| platform              | platform                     | All      | iOS or Android                            |
| referrer              | Google Play Install Referrer | Android  | [Android][43]                             |
| iad                   | Apple Search Ad parameters   | iOS      | [iOS][44]                                 |
| os\_version           | operating system version     | All      | [Android][45], [iOS][46]                  |
| device                | device name                  | All      | [Android][47], [iOS (hw.machine)][48]     |
| device\_manufacturer  | device manufactuer           | Android  | [Android][49]                             |
| device\_model         | device model                 | All      | [Android][50], [iOS (hw.model)][51]       |
| device\_brand         | device brand                 | Android  | [Android][52]                             |
| device\_product       | device product               | Android  | [Android][53]                             |
| device\_model\_name   | device machine               | iOS      | [iOS (hw.model)][54]                      |
| device\_cpu           | device cpu name              | iOS      | [iOS (hw.cputype)][55]                    |
| carrier               | phone carrier                | Android  | [Android][56]                             |
| connection\_type      | cellular or wifi             | Android  | [Android][57]                             |
| screen\_width         | device screen width          | Android  | [Android][58]                             |
| screen\_height        | device screen height         | Android  | [Android][59]                             |
| os\_version\_release  | operating system version     | All      | [Android][60], [iOS][61]                  |
| build\_id             | build ID                     | All      | [Android][62], [iOS (kern.osversion)][63] |
| locale                | device locale                | All      | [Android][64], [iOS][65]                  |
| country               | locale country               | All      | [Android][66], [iOS][67]                  |
| timezone              | timezone                     | All      | [Android][68], [iOS][69]                  |

<br/>

## <a id="purchase-events"></a>Purchase Events

## <a id="ios-iap-validation"></a>iOS IAP Validation

iOS receipt validation requires `transactionId` and `receipt` (`signature` will be set to `null`). For `receipt`, be sure to send the receipt `Payload`(the base64 encoded ASN.1 receipt) from Unity.

**IMPORTANT:** If you have subscription IAP, you will need to add your app's shared secret in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retrieve your iOS App-Specific Shared Secret from the <a href="https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/887212194/addons" target="_new">iTunes Connect Console</a> \> Select your app \> Features \> In-App Purchases \> App-Specific Shared Secret.

## <a id="android-iap-validation"></a>Android IAP Validation

Android receipt validation requires `receipt` and `signature` are required (`transactionId` is set to `null`).

**IMPORTANT:** You will need to add your app's public key in the <a href="https://www.tenjin.io/dashboard/apps" target="_new">Tenjin dashboard</a>. You can retrieve your Base64-encoded RSA public key from the <a href="https://play.google.com/apps/publish/" target="_new"> Google Play Developer Console</a> \> Select your app \> Development Tools \> Services & APIs. Please note that for Android, we currently only support IAP transactions from Google Play.

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

**Disclaimer:** If you are implementing purchase events on Tenjin for the first time, make sure to verify the data with other tools you’re using before you start scaling up your user acquisition campaigns using purchase data.

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
- In your projects method for the custom event, write the following for a named event: `Tenjin.getInstance("<API_KEY>").SendEvent("name")` and the following for a named event with an integer value: `Tenjin.getInstance("<API_KEY>").SendEvent("nameWithValue","value")`
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

Tenjin supports the ability to direct users to a specific part of your app after a new attributed installation via Tenjin's campaign tracking URLs. You can utilize the `GetDeeplink` method and callback to access the deferred deeplink through the data object. To test, you can follow the instructions found <a href="http://help.tenjin.io/t/how-do-i-use-and-test-deferred-deeplinks-with-my-campaigns/547">here</a>.

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
      clicked_tenjin_link = (data["clicked_tenjin_link"].ToLower() == "true");
      Debug.Log("===> DeferredDeeplinkCallback ---> clicked_tenjin_link: " + clicked_tenjin_link);
    }

    if (data.ContainsKey("is_first_session")) {
      //is_first_session is a BOOL to handle if this session for this user is the first session
      is_first_session = (data["is_first_session"].ToLower() == "true");
      Debug.Log("===> DeferredDeeplinkCallback ---> is_first_session: " + is_first_session);
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

Below are the parameters, if available, that are returned to the deferred deeplink callback:

| Parameter             | Description                                                      |
| --------------------- | ---------------------------------------------------------------- |
| advertising\_id        | Advertising ID of the device                                     |
| ad\_network            | Ad network of the campaign                                       |
| campaign\_id           | Tenjin campaign ID                                               |
| campaign\_name         | Tenjin campaign name                                             |
| site\_id               | Site ID of source app                                            |
| referrer              | The referrer params from the app store                           |
| deferred\_deeplink\_url | The deferred deep-link of the campaign                           |
| clicked\_tenjin\_link   | Boolean representing if the device was tracked by Tenjin         |
| is\_first\_session      | Boolean representing if this is the first session for the device |

## <a id="server-to-server"></a>Server-to-server integration

Tenjin offers server-to-server integration. If you want to access to the documentation, please send email to support@tenjin.com.

## <a id="subversion"></a>App Subversion parameter for A/B Testing (requires DataVault)

If you are running A/B tests and want to report the differences, we can append a numeric value to your app version using the `AppendAppSubversion()` method. For example, if your app version `1.0.1`, and set `AppendAppSubversion(8888)`, it will report app version as `1.0.1.8888`.

This data will appear within DataVault, where you will be able to run reports using the app subversion values.

```csharp
BaseTenjin instance = Tenjin.getInstance("<API KEY>");
instance.AppendAppSubversion(8888);
instance.Connect();
```

# <a id="ilrd"></a>Impression Level Ad Revenue Integration

Tenjin supports the ability to integrate with the Impression Level Ad Revenue (ILRD) feature from,
- AppLovin
- IronSource

This feature allows you to receive events which correspond to your ad revenue is affected by each advertisement show to a user. To enable this feature, follow the below instructions.

> *NOTE*: ILRD is a paid product, please contact your Tenjin account manager to discuss the price.

## <a id="applovin"></a>AppLovin Impression Level Ad Revenue Integration
> *NOTE*, Please ensure you have the latest AppLovin Unity SDK installed (\>AppLovin-MAX-Unity-Plugin-5.1.2-Android-11.1.2-iOS-11.1.1)

The Tenjin SDK can listen to AppLovin ILRD ad impressions and send revenue events to Tenjin.  This integration will send revenue related for each ad impression served from AppLovin.  Here are the steps to integrate:

1. Install the AppLovin Unity SDK: https://dash.applovin.com/documentation/mediation/unity/getting-started/integration#download-the-latest-unity-plugin
2. When initializing the Tenjin SDK, subscribe to AppLovin Impressions:

```csharp
var tenjin = Tenjin.getInstance("<YOUR-TENJIN-API_KEY>");
tenjin.Connect();
tenjin.SubscribeAppLovinImpressions();
```

Below is an example of AppLovin Banner integration and subscribing to impression events.

```csharp
public class AppLovinBehavior : MonoBehaviour
{

#if UNITY_ANDROID && !UNITY_EDITOR
    string _banner = "<BANNER_AD_ID>";
#elif UNITY_IPHONE && !UNITY_EDITOR
    string _banner = "<BANNER_AD_ID>";
#else
    string _banner = "<BANNER_AD_ID>";
#endif


    void Start()
    {
        InitializeAppLovin();
    }

    private void InitializeAppLovin()
    {
        var tenjin = Tenjin.getInstance("<YOUR-TENJIN-API_KEY>");
        tenjin.Connect();
        tenjin.SubscribeAppLovinImpressions();

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
        };
        MaxSdk.SetSdkKey(applovinKey);
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        ShowBanner();
    }

    public void InitializeBannerAds()
    {
        MaxSdk.CreateBanner(_banner, MaxSdkBase.BannerPosition.TopCenter);
        MaxSdk.SetBannerBackgroundColor(_banner, Color.yellow);
    }

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log($"Received impression data - {arg2.Revenue} - {arg2.AdUnitIdentifier} - {arg2.NetworkPlacement}");
    }

    private void ShowBanner()
    {
        InitializeBannerAds();
        MaxSdk.ShowBanner(_banner);
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }

    public void GoBack()
    {
        MaxSdk.HideBanner(_banner);
    }
```

Here is an example impression level revenue data entry from AppLovin:

| Parameter  | Example |
| ------------- | ------------- |
| max[creative\_id] | 2813803997 |
| max[placement] | placementBanner |
| max[format] | BANNER |
| max[country] | DE |
| max[ad\_revenue\_currency] | USD |
| max[network\_placement] | banner\_regular |
| max[publisher\_revenue\_decimal] | 0.000047455200000000006 |
| max[revenue\_precision] | exact |
| max[ad\_unit\_id] | a7d1aa174c93c716 |
| max[publisher\_revenue\_micro] | 47.455200000000005 |
| max[revenue] | 4.7455200000000006E-5 |
| max[network\_name] | APPLOVIN\_EXCHANGE |

## <a id="ironsource"></a>IronSource Impression Level Ad Revenue Integration
> *NOTE*, Please ensure you have the latest IronSource Unity SDK installed (\>IronSource\_IntegrationManager\_v7.2.1)

The Tenjin SDK can listen to IronSource ILRD ad impressions and send revenue events to Tenjin.  This integration will send revenue related for each ad impression served from IronSource.  Here are the steps to integrate:

1. Install the IronSource Unity SDK: https://developers.is.com/ironsource-mobile/unity/unity-plugin/#step-2
2. When initializing the Tenjin SDK, subscribe to IronSource Impressions:

```csharp
var tenjin = Tenjin.getInstance("<API_KEY>");
tenjin.Connect();
tenjin.SubscribeIronSourceImpressions();
```

Below is an example of IronSource Banner integration and subscribing to impression events.

```csharp
public class IronSourceBehavior : MonoBehaviour
{
    void Start()
    {
        InitializeIronSource();
    }
    private void InitializeIronSource()
    {
        var tenjin = Tenjin.getInstance("<YOUR-TENJIN-API_KEY>");
        t.Connect();
        t.SubscribeIronSourceImpressions();

        IronSource.Agent.init ("<YOUR-IRONSOURCE-API-KEY>");
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        // Banner ad
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP);
        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionSuccessEvent += ImpressionSuccessEvent;
    }

    void SdkInitializationCompletedEvent()
    {
        // IronSource SDK is initialized, start loading ads
    }

    void OnApplicationPause(bool isPaused)
    {                 
        IronSource.Agent.onApplicationPause(isPaused);
    }

    void ImpressionSuccessEvent(IronSourceImpressionData arg1)
    {
        Debug.Log($"Received impression data - {arg1.revenue} - {arg1.adUnit} - {arg1.placement}");
    }

    public void GoBack()
    {
      // Hide banner ad
        IronSource.Agent.hideBanner();
    }
```

Here is an example impression level revenue data entry from IronSource:

| Parameter  | Example |
| ------------- | ------------- |
| ironsource[auction\_id] | 4a9fba00-a6c6-11ec-b5a2-817ec8dcf90b\_1977367705|
| ironsource[segment\_name] | String |
| ironsource[precision] | BID |
| ironsource[revenue] | 0.099 |
| ironsource[instance\_id] | 4334854 |
| ironsource[lifetime\_revenue] | 0.099 |
| ironsource[publisher\_revenue\_decimal] | 0.099 |
| ironsource[placement] | DefaultBanner |
| ironsource[ab] | A |
| ironsource[encrypted\_cpm] | String |
| ironsource[country] | DE |
| ironsource[ad\_unit] | banner |
| ironsource[ad\_network] | ironsource |
| ironsource[instance\_name] | Bidding |
| ironsource[publisher\_revenue\_micro] | 99000 |

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
[27]:	#ilrd
[28]:	#applovin
[29]:	#ironsource
[30]:	#testing
[31]:	https://github.com/tenjin/tenjin-unity-sdk-demo
[32]:	https://developer.apple.com/app-store/user-privacy-and-data-use/
[33]:	https://developer.apple.com/documentation/storekit/skadnetwork/receiving_ad_attributions_and_postbacks
[34]:	https://developer.apple.com/documentation/storekit/skadnetwork/configuring_an_advertised_app
[35]:	https://docs.unity3d.com/Manual/iphone-GettingStarted.html
[36]:	https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.html#getAdvertisingIdInfo(android.content.Context)
[37]:	https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614151-advertisingidentifier
[38]:	https://developer.apple.com/documentation/uikit/uidevice/1620059-identifierforvendor
[39]:	http://www.msa-alliance.cn/col.jsp?id=120
[40]:	https://developer.android.com/reference/android/telephony/TelephonyManager#getImei()
[41]:	https://developers.google.com/android/reference/com/google/android/gms/ads/identifier/AdvertisingIdClient.Info.html#isLimitAdTrackingEnabled()
[42]:	https://developer.apple.com/documentation/adsupport/asidentifiermanager/1614148-isadvertisingtrackingenabled
[43]:	https://developer.android.com/google/play/installreferrer/index.html
[44]:	https://searchads.apple.com/advanced/help/measure-results/#attribution-api
[45]:	https://developer.android.com/reference/android/os/Build.VERSION.html#SDK_INT
[46]:	https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion
[47]:	https://developer.android.com/reference/android/os/Build.html#DEVICE
[48]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[49]:	https://developer.android.com/reference/android/os/Build.html#MANUFACTURER
[50]:	https://developer.android.com/reference/android/os/Build.html#MODEL
[51]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[52]:	https://developer.android.com/reference/android/os/Build.html#BRAND
[53]:	https://developer.android.com/reference/android/os/Build.html#PRODUCT
[54]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[55]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[56]:	https://developer.android.com/reference/android/telephony/TelephonyManager.html#getSimOperatorName()
[57]:	https://developer.android.com/reference/android/net/ConnectivityManager.html#getActiveNetworkInfo()
[58]:	https://developer.android.com/reference/android/util/DisplayMetrics.html#widthPixels
[59]:	https://developer.android.com/reference/android/util/DisplayMetrics.html#heightPixels
[60]:	https://developer.android.com/reference/android/os/Build.VERSION.html#RELEASE
[61]:	https://developer.apple.com/documentation/uikit/uidevice/1620043-systemversion
[62]:	https://developer.android.com/reference/android/os/Build.html
[63]:	https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man3/sysctl.3.html
[64]:	https://developer.android.com/reference/java/util/Locale.html#getDefault()
[65]:	https://developer.apple.com/documentation/foundation/nslocalekey
[66]:	https://developer.android.com/reference/java/util/Locale.html#getDefault()
[67]:	https://developer.apple.com/documentation/foundation/nslocalecountrycode
[68]:	https://developer.android.com/reference/java/util/TimeZone.html
[69]:	https://developer.apple.com/documentation/foundation/nstimezone/1387209-localtimezone

[image-1]:	https://s3.amazonaws.com/tenjin-instructions/sdk_live_purchase_events_2.png
