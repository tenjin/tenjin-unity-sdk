Tenjin Unity plugin
=========
- Allows unity developers to quickly integrate with Tenjin's install API
- Review the [iOS](https://github.com/Ordinance/tenjin-ios-sdk) and [Android](https://github.com/Ordinance/tenjin-android-sdk) documentation and apply the proper platform settings to your builds. Most importantly:
  1. iOS: make sure you have the right build settings and you include the iOS frameworks you need (below).
  2. Android: make sure you add the necessary `AndroidManifest.xml` requirements (below).

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
Tenjin purchase event integration instructions:
-------
Pass any in app purchase (IAP) transaction manually. To use this method you will need a `string productId`, `string currencyCode`, `int quantity`, and `double unitPrice`.

```csharp
//Here is an example of how to implement the purchase in your post-validated purchase event
void CompletedPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice){
  
  //pass in the required data for the transaction
  Tenjin.getInstance("API_KEY").Transaction(ProductId, CurrencyCode, Quantity, UnitPrice);

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
- In your projects method for the custom event write the following for a named event: `Tenjin.getInstance("<API_KEY>").SendEvent("name")` and the following for a named event with a value: `Tenjin.getInstance("<API_KEY>").SendEvent("nameWithValue","value")`

Here's an example of the code:
```csharp
void MethodWithCustomEvent(){
    //event with name
    Tenjin.getInstance("API_KEY").SendEvent("name");

    //event with name and value
    Tenjin.getInstance("API_KEY").SendEvent("nameWithValue", "value");
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

