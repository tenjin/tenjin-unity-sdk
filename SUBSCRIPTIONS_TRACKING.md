# Tenjin – Unity Subscription Tracking

Track subscription purchases with Tenjin for server-side verification and attribution.

> **Note:** Subscription tracking is currently only available on iOS. Android support is coming soon.

## Method

```csharp
BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
instance.Subscription(
    productId,                  // string: Product ID
    currencyCode,               // string: e.g., "USD"
    unitPrice,                  // double: e.g., 9.99
    transactionId,              // string: SK2 transaction ID
    originalTransactionId,      // string: Original transaction ID (for renewals)
    receipt,                    // string: JWS signed transaction or base64 receipt
    skTransaction,              // string: SK2 transaction JSON representation
    null,                       // string: Android purchase token (not yet available)
    null,                       // string: Android purchase data (not yet available)
    null                        // string: Android data signature (not yet available)
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
| `purchaseToken` | `string` | Android | Purchase token from Google Play Billing (coming soon) |
| `purchaseData` | `string` | Android | Original JSON from the purchase object (coming soon) |
| `dataSignature` | `string` | Android | Signature for purchase verification (coming soon) |

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

## Using Unity IAP (Direct Integration, iOS only)

Unity IAP 5.x provides SK2 data through the `IOrderInfo.Apple` properties. Extract the JWS receipt and decode the payload to get the SK2 transaction JSON.

```csharp
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System.Text;
using Json = MiniJSON.Json;

void OnPurchasePending(PendingOrder pendingOrder) {
    var item = pendingOrder.CartOrdered.Items().FirstOrDefault();
    if (item == null) return;

    var product = item.Product;
    var info = pendingOrder.Info;

    // Check if this is a subscription
    if (product.definition.type == ProductType.Subscription) {
        var price = product.metadata.localizedPrice;
        double lPrice = decimal.ToDouble(price);
        var currencyCode = product.metadata.isoCurrencyCode;
        var productId = product.definition.id;

        // Fetches SK2 data natively — no manual extraction needed
        BaseTenjin instance = Tenjin.getInstance("<SDK_KEY>");
        instance.SubscriptionWithStoreKit(productId, currencyCode, lPrice);
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
