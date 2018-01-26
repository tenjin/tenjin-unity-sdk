Please see our <a href="https://github.com/Ordinance/tenjin-unity-sdk/wiki">Release Notes</a> to see detailed version history.


Tenjin Unity plugin
=========
- Allows unity developers to quickly integrate with Tenjin's install API
- Review the [iOS](https://github.com/Ordinance/tenjin-ios-sdk) and [Android](https://github.com/Ordinance/tenjin-android-sdk) documentation and apply the proper platform settings to your builds. Most importantly:
  1. iOS: make sure you have the right build settings and you include the iOS frameworks you need (below).
  2. Android: make sure you add the necessary `AndroidManifest.xml` requirements (below).
  3. Your "API_KEY" is located on your [Organizations tab](https://www.tenjin.io/dashboard/organizations)

Tenjin install/session integration:
-------
- Include the Assets folder in your Unity project
- In your project's first `Start()` method write the following `Tenjin.getInstance("<API_KEY>").Connect();`

Here's an example of the code:

```csharp
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    Tenjin.getInstance ("API_KEY").Connect();
  }

  // Update is called once per frame
  void Update () {

  }

  void OnApplicationPause(bool pauseStatus){
    if(pauseStatus){
      //do nothing
    }
    else
    {
      Tenjin.getInstance ("API_KEY").Connect();
    }
  }
}
```

Tenjin install/session integration to handle deeplinks from other services. If you use other services to produce deferred deep links, you can pass tenjin those deep links to handle the attribution logic with your tenjin enabled deep links.
-------

```csharp
using UnityEngine;
using System.Collections;

public class TenjinExampleScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    Tenjin.getInstance ("API_KEY").Connect("your_deeplink://path?test=123");
  }

}
```

Tenjin purchase event integration instructions:
-------
Pass in app purchase (IAP) transactions to Tenjin manually. You can send `string productId`, `string currencyCode`, `int quantity`, and `double unitPrice` setting all other params to `null`.

```csharp
//Here is an example of how to implement the purchase in your post-validated purchase event
void CompletedPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice){

  //pass in the required data for the transaction without receipts
  Tenjin.getInstance("API_KEY").Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, null, null, null);

  //any other code you want to handle in a completed purchase client side
}
```
- `ProductId` -> the name or ID of the product/purchase that the user is making
- `CurrencyCode` -> the currency code of the price
- `Quantity` -> the number of products/purchases that the user is making
- `UnitPrice` -> the unit price of the product

**Our Unity plugin for receipt validation is in beta.** You can try sending additional parameters `string transactionId`, `string receipt`, and `string signature` in that order.

- `transactionId` -> the `transactionId` for an iOS purchase (`null` for Android purchases)
- `receipt` -> the `receipt` for an iOS (base64 encoded) or Android purchase
- `signature` -> the `signature` for an Android purchase (`null` for iOS purchases)

iOS receipt validation requires `transactionId` and `receipt` (`signature` will be set to `null`).

```csharp
//Here is an example of how to implement iOS transaction receipt validation (currently in beta)
void CompletedIosPurchase(string ProductId, string CurrencyCode int Quantity, double UnitPrice, string TransactionId, string Receipt){

  #if UNTIY_IOS
  //pass the necessary data including the transactionId and the receipt
  Tenjin.getInstance("API_KEY").Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, TransactionId, Receipt, null);
}
```

For Android, `receipt` and `signature` are required (`transactionId` is set to `null`).

```csharp
//Here is an example of how to implement iOS transaction receipt validation (currently in beta)
void CompletedAndroidPurchase(string ProductId, string CurrencyCode int Quantity, double UnitPrice, string Receipt, string Signature){

  #if UNTIY_ANDROID
  //pass the necessary data including the transactionId and the receipt
  Tenjin.getInstance("API_KEY").Transaction(ProductId, CurrencyCode, Quantity, UnitPrice, null, Receipt, Signature);
}
```

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
    Tenjin.getInstance("API_KEY").SendEvent("name");

    //event with name and integer value
    Tenjin.getInstance("API_KEY").SendEvent("nameWithValue", "value");
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
    Tenjin.getInstance ("YOUR_TENJIN_API_KEY").Connect();
    Tenjin.getInstance ("YOUR_TENJIN_API_KEY").GetDeeplink (DeferredDeeplinkCallback);
  }

  public void DeferredDeeplinkCallback(Dictionary<string, string> data) {
    bool clicked_tenjin_link = false;
    bool is_first_session = false;
      
    if (data.ContainsKey("clicked_tenjin_link")) {
      clicked_tenjin_link = (data["clicked_tenjin_link"] == "true");
      Debug.Log("===> DeferredDeeplinkCallback ---> clicked_tenjin_link: " + data["clicked_tenjin_link"]);
    }

    if (data.ContainsKey("is_first_session")) {
      is_first_session = (data["is_first_session"] == "true");
      Debug.Log("===> DeferredDeeplinkCallback ---> is_first_session: " + data["is_first_session"]);
    }

    if (data.ContainsKey("ad_network")) {
      Debug.Log("===> DeferredDeeplinkCallback ---> adNetwork: " + data["ad_network"]);
    }

    if (data.ContainsKey("campaign_id")) {
      Debug.Log("===> DeferredDeeplinkCallback ---> campaignId: " + data["campaign_id"]);
    }

    if (data.ContainsKey("advertising_id")) {
      Debug.Log("===> DeferredDeeplinkCallback ---> advertisingId: " + data["advertising_id"]);
    }

    if (data.ContainsKey("deferred_deeplink_url")) {
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
