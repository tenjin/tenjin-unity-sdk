# Tenjin – Unity Subscription Tracking

Track subscription purchases with Tenjin for server-side verification and attribution on **iOS** and **Android**.

> **Note:** Android subscription tracking requires Unity SDK 1.18.0+ (bundles Tenjin Android SDK 1.20.0+).

## Method

The same `Subscription` method handles both platforms — pass the iOS parameters on iOS and the Android parameters on Android (leave the others `null`).

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
instance.Subscription(
    productId,                  // string: Product ID
    currencyCode,               // string: e.g., "USD"
    unitPrice,                  // double: e.g., 9.99
    transactionId,              // string (iOS): SK2 transaction ID
    originalTransactionId,      // string (iOS): Original transaction ID (for renewals)
    receipt,                    // string (iOS): JWS signed transaction or base64 receipt
    skTransaction,              // string (iOS): SK2 transaction JSON representation
    purchaseToken,              // string (Android): Google Play purchase token
    purchaseData,               // string (Android): original JSON from the purchase object
    dataSignature               // string (Android): signature for purchase verification
);
```

### Parameters

| Parameter | Type | Platform | Description |
|-----------|------|----------|-------------|
| `productId` | `string` | Both | Product identifier |
| `currencyCode` | `string` | Both | ISO 4217 currency code (e.g., "USD") |
| `unitPrice` | `double` | Both | Price (e.g., 9.99) |
| `transactionId` | `string` | iOS | Transaction ID from StoreKit 2 |
| `originalTransactionId` | `string` | iOS | Original transaction ID (for renewals) |
| `receipt` | `string` | iOS | JWS signed transaction token or base64 receipt |
| `skTransaction` | `string` | iOS | StoreKit 2 transaction JSON representation |
| `purchaseToken` | `string` | Android | Purchase token from Google Play Billing |
| `purchaseData` | `string` | Android | Original JSON from the purchase object |
| `dataSignature` | `string` | Android | Signature for purchase verification |

> **Android note:** Tenjin derives the purchase date from the `purchaseTime` field inside `purchaseData` (the original JSON). On Android, `productId`, `currencyCode`, `purchaseToken`, `purchaseData`, and `dataSignature` are all required — the call is skipped if any are missing.

---

## Helper Methods (iOS only)

### `SubscriptionWithStoreKit`

Fetches the latest StoreKit 2 transaction for a product and sends it to Tenjin in a single native call. No SK2 data needs to be extracted in C#. This is the recommended approach when your IAP library (e.g., RevenueCat, Adapty, Qonversion) doesn't expose SK2 transaction data.

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
instance.SubscriptionWithStoreKit(productId, currencyCode, unitPrice);
```

> **Note:** Requires iOS 16.0+. This method is iOS-only and will no-op on Android.

---

## Using Unity IAP (Direct Integration)

On **iOS**, Unity IAP 5.x exposes SK2 data through `IOrderInfo.Apple`, but the simplest path is `SubscriptionWithStoreKit()`, which fetches the SK2 transaction natively — no manual extraction needed.

On **Android**, the Google Play purchase details live in the unified receipt (`IOrderInfo.Receipt`). Parse the `Payload` to get the original JSON (`json`) and `signature`, then read the `purchaseToken` from the original JSON.

```csharp
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using Json = MiniJSON.Json;

void OnPurchasePending(PendingOrder pendingOrder) {
    var item = pendingOrder.CartOrdered.Items().FirstOrDefault();
    if (item == null) return;

    var product = item.Product;
    var info = pendingOrder.Info;

    // Only track subscriptions here
    if (product.definition.type == ProductType.Subscription) {
        double lPrice = decimal.ToDouble(product.metadata.localizedPrice);
        var currencyCode = product.metadata.isoCurrencyCode;
        var productId = product.definition.id;
        BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");

#if UNITY_IOS
        // Fetches SK2 data natively — no manual extraction needed (iOS 16+)
        instance.SubscriptionWithStoreKit(productId, currencyCode, lPrice);
#elif UNITY_ANDROID
        // Extract Google Play purchase data from the unified receipt
        var wrapper = Json.Deserialize(info.Receipt) as Dictionary<string, object>;
        if (wrapper != null && (string)wrapper["Store"] == "GooglePlay") {
            var payload = Json.Deserialize((string)wrapper["Payload"]) as Dictionary<string, object>;
            var purchaseData = (string)payload["json"];        // original JSON
            var dataSignature = (string)payload["signature"];

            // purchaseToken lives inside the original JSON
            var purchaseJson = Json.Deserialize(purchaseData) as Dictionary<string, object>;
            var purchaseToken = (string)purchaseJson["purchaseToken"];

            instance.Subscription(
                productId, currencyCode, lPrice,
                null, null, null, null,                       // iOS params
                purchaseToken, purchaseData, dataSignature    // Android params
            );
        }
#endif
    }

    UnityIAPServices.DefaultPurchase().ConfirmPurchase(pendingOrder);
}
```

---

## Using RevenueCat (purchases-unity)

RevenueCat does not expose SK2 transaction data at the C# level. Use `SubscriptionWithStoreKit()` to handle everything natively — it fetches the SK2 transaction directly from StoreKit 2 and sends it to Tenjin in a single call.

```csharp
using Purchases;

// Set to avoid duplicate tracking
private HashSet<string> _trackedTransactions = new HashSet<string>();

void SetupRevenueCatListener() {
    Purchases.Purchases.SharedInstance.UpdatedCustomerInfo += (sender, info) => {
        foreach (var entitlement in info.Entitlements.Active) {
            var id = entitlement.Value.ProductIdentifier;

            // Skip if already tracked
            if (_trackedTransactions.Contains(id)) continue;
            _trackedTransactions.Add(id);

            // Get product price from offerings
            Purchases.Purchases.SharedInstance.GetOfferings((offerings, error) => {
                if (error != null) return;

                var packages = offerings.Current?.AvailablePackages;
                if (packages == null) return;

                foreach (var package in packages) {
                    if (package.StoreProduct.Identifier == id) {
                        // Fetches SK2 transaction and sends to Tenjin natively
                        BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
                        instance.SubscriptionWithStoreKit(
                            package.StoreProduct.Identifier,
                            package.StoreProduct.CurrencyCode,
                            (double)package.StoreProduct.Price
                        );
                        break;
                    }
                }
            });
        }
    };
}
```
