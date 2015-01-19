Tenjin Unity plugin
=========
- Allows unity developers to quickly integrate with Tenjin's install API
- Please review the [iOS](https://github.com/Ordinance/tenjin-ios-sdk) and [Android](https://github.com/Ordinance/tenjin-android-sdk) SDK documentation to make sure your settings are applied to each of Unity's builds. Specifically, on iOS make sure the build settings are set properly in XCode, and for Android make sure you add the necessary `AndroidManifest.xml` requirements.

Tenjin install/session integration:
-------
- Include the Assets folder in your Unity project
- In your project's first `Start()` method write the following `Tenjin.getInstance("<API_KEY>").Connect();`

Here's an example of the code:
```
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

```
//Here is an example of how to implement the purchase in your post-validated purchase event
void CompletedPurchase(string ProductId, string CurrencyCode, int Quantity, double UnitPrice){
  
  //pass in the required data for the transaction
  Tenjin.getInstance("API_KEY").Transaction(ProductId, CurrencyCode, Quantity, UnitPrice);

  //any other code you want to handle in a completed purchase client side
}
```

Tenjin custom event integration:
-------
- Include the Assets folder in your Unity project
- In your projects method for the custom event write the following for a named event: `Tenjin.getInstance("<API_KEY>").SendEvent("name")` and the following for a named event with a value: `Tenjin.getInstance("<API_KEY>").SendEvent("nameWithValue","value")`

Here's an example of the code:
```
void MethodWithCustomEvent(){
    //event with name
    Tenjin.getInstance("API_KEY").SendEvent("name");

    //event with name and value
    Tenjin.getInstance("API_KEY").SendEvent("nameWithValue", "value");
}
```
