//
//  Copyright (c) 2025 Tenjin. All rights reserved.
//

using System;
using UnityEngine;

public class TenjinCloudXIntegration
{
    private static bool _subscribed_cloudx = false;

    public TenjinCloudXIntegration()
    {
    }

    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_cloudx_enabled
        if (_subscribed_cloudx)
        {
            Debug.Log("Ignoring duplicate CloudX subscription");
            return;
        }

        Action<CloudX.CloudXAd> handler = (impression) =>
        {
            CloudXAdImpressionData adImpressionData = new CloudXAdImpressionData()
            {
                revenue = impression.Revenue,
                ad_revenue_currency = "USD",
                network_name = impression.NetworkName,
                ad_unit_id = impression.AdUnitId,
                format = impression.AdFormat.ToString(),
                placement = impression.Placement,
                network_placement = impression.NetworkPlacement
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };

        CloudX.CloudXAdsCallbacks.Banner.OnAdRevenuePaid += handler;
        CloudX.CloudXAdsCallbacks.Mrec.OnAdRevenuePaid += handler;
        CloudX.CloudXAdsCallbacks.Interstitial.OnAdRevenuePaid += handler;
        CloudX.CloudXAdsCallbacks.Rewarded.OnAdRevenuePaid += handler;

        _subscribed_cloudx = true;
#endif
    }
}

[System.Serializable]
internal class CloudXAdImpressionData
{
    public double revenue;
    public string ad_revenue_currency;
    public string network_name;
    public string ad_unit_id;
    public string format;
    public string placement;
    public string network_placement;
}
