//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Globalization;
using UnityEngine;

public class TenjinIronSourceIntegration
{
#if tenjin_ironsource_legacy_enabled
    private static bool _subscribedLegacy = false;
#endif

    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_ironsource_legacy_enabled
        if (_subscribedLegacy) return;

        IronSourceEvents.onImpressionDataReadyEvent += impressionData =>
        {
            if (impressionData == null) return;

            double rev = 0.0;
            CultureInfo inv = CultureInfo.InvariantCulture;

            if (impressionData.revenue != null)
                double.TryParse(string.Format(inv, "{0}", impressionData.revenue),
                                NumberStyles.Any, inv, out rev);

            var data = new IronSourceAdImpressionData
            {
                ab = impressionData.ab,
                ad_network = impressionData.adNetwork,
                ad_unit = impressionData.adUnit,
                auction_id = impressionData.auctionId,
                country = impressionData.country,
                instance_id = impressionData.instanceId,
                instance_name = impressionData.instanceName,
                placement = impressionData.placement,
                precision = impressionData.precision,
                revenue = rev,
                segment_name = impressionData.segmentName,
                encrypted_cpm = impressionData.encryptedCPM
            };

            callback(JsonUtility.ToJson(data));
        };

        _subscribedLegacy = true;
#endif
    }

    public static void SubscribeLevelPlayRewardedImpressions(object rewardedAd, Action<string> callback)
    {
#if tenjin_ironsource_levelplay_enabled
        var levelPlayRewardedAd = rewardedAd as Unity.Services.LevelPlay.LevelPlayRewardedAd;
        if (levelPlayRewardedAd == null || callback == null) return;

        levelPlayRewardedAd.OnAdDisplayed += adInfo => SendLevelPlayImpression(adInfo, callback);
#endif
    }

    public static void SubscribeLevelPlayInterstitialImpressions(object interstitialAd, Action<string> callback)
    {
#if tenjin_ironsource_levelplay_enabled
        var levelPlayInterstitialAd = interstitialAd as Unity.Services.LevelPlay.LevelPlayInterstitialAd;
        if (levelPlayInterstitialAd == null || callback == null) return;

        levelPlayInterstitialAd.OnAdDisplayed += adInfo => SendLevelPlayImpression(adInfo, callback);
#endif
    }

    public static void SubscribeLevelPlayBannerImpressions(object bannerAd, Action<string> callback)
    {
#if tenjin_ironsource_levelplay_enabled
        var levelPlayBannerAd = bannerAd as Unity.Services.LevelPlay.LevelPlayBannerAd;
        if (levelPlayBannerAd == null || callback == null) return;

        levelPlayBannerAd.OnAdDisplayed += adInfo => SendLevelPlayImpression(adInfo, callback);
#endif
    }

    private static void SendLevelPlayImpression(object adInfoObj, Action<string> callback)
    {
#if tenjin_ironsource_levelplay_enabled
        var adInfo = adInfoObj as Unity.Services.LevelPlay.LevelPlayAdInfo;
        if (adInfo == null || callback == null) return;

        double rev = 0.0;
        CultureInfo inv = CultureInfo.InvariantCulture;

        if (adInfo.Revenue != null)
            double.TryParse(string.Format(inv, "{0}", adInfo.Revenue),
                            NumberStyles.Any, inv, out rev);

        var data = new IronSourceAdImpressionData
        {
            ab = adInfo.Ab,
            ad_network = adInfo.AdNetwork,
            ad_unit = adInfo.AdUnitId,
            auction_id = adInfo.AuctionId,
            country = adInfo.Country,
            instance_id = adInfo.InstanceId,
            instance_name = adInfo.InstanceName,
            precision = adInfo.Precision,
            revenue = rev,
            segment_name = adInfo.SegmentName,
            encrypted_cpm = adInfo.EncryptedCPM
        };

        callback(JsonUtility.ToJson(data));
#endif
    }
}

[System.Serializable]
internal class IronSourceAdImpressionData
{
    public string ab;
    public string ad_network;
    public string ad_unit;
    public string auction_id;
    public string country;
    public string instance_id;
    public string instance_name;
    public string placement;
    public string precision;
    public double revenue;
    public string segment_name;
    public string encrypted_cpm;
}
