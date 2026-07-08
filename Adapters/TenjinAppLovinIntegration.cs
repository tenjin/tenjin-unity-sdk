//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using UnityEngine;

public class TenjinAppLovinIntegration
{
    private static bool _subscribed_applovin = false;
    public TenjinAppLovinIntegration()
    {
    }
    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_applovin_enabled
        if (_subscribed_applovin)
        {
            Debug.Log("Ignoring duplicate applovin subscription");
            return;
        }
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (arg1, arg2) =>
        {
            AdImpressionData adImpressionData = new AdImpressionData()
            {
                revenue = arg2.Revenue,
                ad_revenue_currency = "USD",
                country = MaxSdk.GetSdkConfiguration().CountryCode,
                network_name = arg2.NetworkName,
                ad_unit_id = arg2.AdUnitIdentifier,
                format = arg2.AdFormat,
                placement = arg2.Placement,
                network_placement = arg2.NetworkPlacement,
                creative_id = arg2.CreativeIdentifier,
                revenue_precision = arg2.RevenuePrecision
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };

        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (arg1, arg2) =>
        {
            AdImpressionData adImpressionData = new AdImpressionData()
            {
                revenue = arg2.Revenue,
                ad_revenue_currency = "USD",
                country = MaxSdk.GetSdkConfiguration().CountryCode,
                network_name = arg2.NetworkName,
                ad_unit_id = arg2.AdUnitIdentifier,
                format = arg2.AdFormat,
                placement = arg2.Placement,
                network_placement = arg2.NetworkPlacement,
                creative_id = arg2.CreativeIdentifier,
                revenue_precision = arg2.RevenuePrecision
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };

        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += (arg1, arg2) =>
        {
            AdImpressionData adImpressionData = new AdImpressionData()
            {
                revenue = arg2.Revenue,
                ad_revenue_currency = "USD",
                country = MaxSdk.GetSdkConfiguration().CountryCode,
                network_name = arg2.NetworkName,
                ad_unit_id = arg2.AdUnitIdentifier,
                format = arg2.AdFormat,
                placement = arg2.Placement,
                network_placement = arg2.NetworkPlacement,
                creative_id = arg2.CreativeIdentifier,
                revenue_precision = arg2.RevenuePrecision
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };

        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (arg1, arg2) =>
        {
            AdImpressionData adImpressionData = new AdImpressionData()
            {
                revenue = arg2.Revenue,
                ad_revenue_currency = "USD",
                country = MaxSdk.GetSdkConfiguration().CountryCode,
                network_name = arg2.NetworkName,
                ad_unit_id = arg2.AdUnitIdentifier,
                format = arg2.AdFormat,
                placement = arg2.Placement,
                network_placement = arg2.NetworkPlacement,
                creative_id = arg2.CreativeIdentifier,
                revenue_precision = arg2.RevenuePrecision
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };
        _subscribed_applovin = true;
#endif
    }
}

[System.Serializable]
internal class AdImpressionData
{
    public double revenue;
    public string ad_revenue_currency;
    public string country;
    public string network_name;
    public string ad_unit_id;
    public string format;
    public string placement;
    public string network_placement;
    public string creative_id;
    public string revenue_precision;
}
