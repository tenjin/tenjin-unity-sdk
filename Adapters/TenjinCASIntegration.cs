//
//  Copyright (c) 2023 Tenjin. All rights reserved.
//

using System;
using UnityEngine;

public class TenjinCASIntegration
{
    private static bool _subscribed_cas = false;

    public TenjinCASIntegration()
    {
    }
    public static void ListenForImpressions(Action<string> callback, object casManager)
    {
#if tenjin_cas_enabled
        if (_subscribed_cas)
        {
            Debug.Log("Ignoring duplicate CAS subscription");
            return;
        }
        CAS.IMediationManager manager = (CAS.IMediationManager) casManager;
        // Interstitial
        manager.OnInterstitialAdImpression += (impression) =>
        {
            string revenuePrecision;
            switch (impression.priceAccuracy)
            {
                case CAS.PriceAccuracy.Floor:
                    revenuePrecision = "floor";
                    break;
                case CAS.PriceAccuracy.Bid:
                    revenuePrecision = "bid";
                    break;
                case CAS.PriceAccuracy.Undisclosed:
                    revenuePrecision = "undisclosed";
                    break;
                default:
                    revenuePrecision = "none";
                    break;
            }
            CASAdImpressionData adImpressionData = new CASAdImpressionData()
            {
                revenue = impression.cpm,
                ad_revenue_currency = "USD",
                network_name = impression.network.ToString(),
                ad_unit_id = impression.identifier,
                format = "interstitial",
                creative_id = impression.creativeIdentifier,
                revenue_precision = revenuePrecision
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };

        // Rewarded
        manager.OnRewardedAdImpression += (impression) =>
        {
            string revenuePrecision;
            switch (impression.priceAccuracy)
            {
                case CAS.PriceAccuracy.Floor:
                    revenuePrecision = "floor";
                    break;
                case CAS.PriceAccuracy.Bid:
                    revenuePrecision = "bid";
                    break;
                case CAS.PriceAccuracy.Undisclosed:
                    revenuePrecision = "undisclosed";
                    break;
                default:
                    revenuePrecision = "none";
                    break;
            }
            CASAdImpressionData adImpressionData = new CASAdImpressionData()
            {
                revenue = impression.cpm,
                ad_revenue_currency = "USD",
                network_name = impression.network.ToString(),
                ad_unit_id = impression.identifier,
                format = "rewarded",
                creative_id = impression.creativeIdentifier,
                revenue_precision = revenuePrecision
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };
        _subscribed_cas = true;
#endif
    }
    public static void ListenForBannerImpressions(Action<string> callback, object casBannerView)
    {
#if tenjin_cas_enabled
        // Banner
        CAS.IAdView bannerView = (CAS.IAdView) casBannerView;
        bannerView.OnImpression += (view, impression) =>
        {
            string revenuePrecision;
            switch (impression.priceAccuracy)
            {
                case CAS.PriceAccuracy.Floor:
                    revenuePrecision = "floor";
                    break;
                case CAS.PriceAccuracy.Bid:
                    revenuePrecision = "bid";
                    break;
                case CAS.PriceAccuracy.Undisclosed:
                    revenuePrecision = "undisclosed";
                    break;
                default:
                    revenuePrecision = "none";
                    break;
            }
            CASAdImpressionData adImpressionData = new CASAdImpressionData()
            {
                revenue = impression.cpm,
                ad_revenue_currency = "USD",
                network_name = impression.network.ToString(),
                ad_unit_id = impression.identifier,
                format = "banner",
                creative_id = impression.creativeIdentifier,
                revenue_precision = revenuePrecision
            };
            string json = JsonUtility.ToJson(adImpressionData);
            callback(json);
        };
#endif
    }
}

[System.Serializable]
internal class CASAdImpressionData
{
    public double revenue;
    public string ad_revenue_currency;
    public string network_name;
    public string ad_unit_id;
    public string format;
    public string creative_id;
    public string revenue_precision;
}
