//
//  Copyright (c) 2023 Tenjin. All rights reserved.
//

using System;
using UnityEngine;
using System.Collections.Generic;

public class TenjinTradPlusIntegration
{
    private static bool _subscribed_tradplus = false;

    public TenjinTradPlusIntegration()
    {
    }
    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_tradplus_enabled
        if (_subscribed_tradplus)
        {
            Debug.Log("Ignoring duplicate TradPlus subscription");
            return;
        }
        TradplusAds.Instance().AddGlobalAdImpression(delegate(Dictionary<string, object> adInfo) 
        {
            ImpressionFromAdInfo(callback, adInfo);
        }
        _subscribed_tradplus = true;
#endif
    }

    public static void ImpressionFromAdInfo(Action<string> callback, Dictionary<string, object> adInfo)
    {
        TradPlusAdImpressionData adImpressionData;
        if (Application.platform == RuntimePlatform.Android)
        {
            adImpressionData = MapAndroidAdData(adInfo);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            adImpressionData = MapIosAdData(adInfo);
        }
        else
        {
            return;
        }

        string json = JsonUtility.ToJson(adImpressionData);
        callback(json);
    }

    // Mapping for Android
    private static TradPlusAdImpressionData MapAndroidAdData(Dictionary<string, object> adInfo)
    {
        object tempValue;

        TradPlusAdImpressionData adImpressionData = new TradPlusAdImpressionData()
        {
            revenue = adInfo.TryGetValue("ecpm", out tempValue) ? tempValue?.ToString() : null,
            network_name = adInfo.TryGetValue("adSourceName", out tempValue) ? tempValue?.ToString() : null,
            ad_unit_id = adInfo.TryGetValue("tpAdUnitId", out tempValue) ? tempValue?.ToString() : null,
            revenue_precision = adInfo.TryGetValue("ecpmPrecision", out tempValue) ? tempValue?.ToString() : null,
            segment = adInfo.TryGetValue("segmentId", out tempValue) ? tempValue?.ToString() : null,
            placement = adInfo.TryGetValue("adSourcePlacementId", out tempValue) ? tempValue?.ToString() : null,
            ab_test = adInfo.TryGetValue("bucketId", out tempValue) ? tempValue?.ToString() : null,
            country = adInfo.TryGetValue("isoCode", out tempValue) ? tempValue?.ToString() : null,
            format = adInfo.TryGetValue("format", out tempValue) ? tempValue?.ToString() : null,
            ad_revenue_currency = "USD"
        };

        return adImpressionData;
    }

    // Mapping for iOS
    private static TradPlusAdImpressionData MapIosAdData(Dictionary<string, object> adInfo)
    {
        object tempValue;

        TradPlusAdImpressionData adImpressionData = new TradPlusAdImpressionData()
        {
            revenue = adInfo.TryGetValue("ecpm", out tempValue) ? tempValue?.ToString() : null,
            network_name = adInfo.TryGetValue("adNetworkName", out tempValue) ? tempValue?.ToString() : null,
            ad_unit_id = adInfo.TryGetValue("adunit_id", out tempValue) ? tempValue?.ToString() : null,
            creative_id = adInfo.TryGetValue("creativeIdentifier", out tempValue) ? tempValue?.ToString() : null,
            revenue_precision = adInfo.TryGetValue("ecpm_precision", out tempValue) ? tempValue?.ToString() : null,
            segment = adInfo.TryGetValue("segment_id", out tempValue) ? tempValue?.ToString() : null,
            placement = adInfo.TryGetValue("adsource_placement_id", out tempValue) ? tempValue?.ToString() : null,
            ab_test = adInfo.TryGetValue("bucket_id", out tempValue) ? tempValue?.ToString() : null,
            country = adInfo.TryGetValue("country_code", out tempValue) ? tempValue?.ToString() : null,
            format = adInfo.TryGetValue("placement_ad_type", out tempValue) && tempValue is int intValue ? FormatFromNumber(intValue) : null,
            ad_revenue_currency = "USD"
        };

        return adImpressionData;
    }

    // Helper method to convert format number to string
    private static string FormatFromNumber(int number)
    {
        string[] options = { "native", "interstitial", "splash", "banner", "rewarded", "offerwall" };
        return number < options.Length ? options[number] : "none";
    }
}

[System.Serializable]
internal class TradPlusAdImpressionData
{
    public string revenue;
    public string ad_revenue_currency;
    public string network_name;
    public string ad_unit_id;
    public string format;
    public string creative_id;
    public string revenue_precision;
    public string segment;
    public string placement;
    public string ab_test;
    public string country;
}
