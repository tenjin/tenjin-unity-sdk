//
//  Copyright (c) 2026 Tenjin. All rights reserved.
//

using UnityEngine;

/// <summary>
/// Wires the bundled ad-network integrations into <see cref="TenjinAdNetworks"/>.
///
/// This adapter lives alongside the <c>Tenjin*Integration</c> classes in the host
/// project's predefined <c>Assembly-CSharp</c> (in the UPM package it ships in the
/// <c>Adapters/</c> folder, deliberately outside the <c>TenjinSDK</c> assembly
/// definition). That placement is what lets the integrations keep their direct
/// compile-time references to ad SDKs that also live in <c>Assembly-CSharp</c>.
///
/// Registration runs once at startup, before any scene loads and well before any
/// <c>Connect</c>/<c>Subscribe…</c> call, so the core SDK can dispatch impressions
/// through the registered delegates.
/// </summary>
internal static class TenjinAdNetworkRegistrar
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void Register()
    {
        // AppLovin
        TenjinAdNetworks.AppLovinListenForImpressions = TenjinAppLovinIntegration.ListenForImpressions;

        // IronSource / LevelPlay
        TenjinAdNetworks.IronSourceListenForImpressions = TenjinIronSourceIntegration.ListenForImpressions;
        TenjinAdNetworks.IronSourceSubscribeLevelPlayRewardedImpressions = TenjinIronSourceIntegration.SubscribeLevelPlayRewardedImpressions;
        TenjinAdNetworks.IronSourceSubscribeLevelPlayInterstitialImpressions = TenjinIronSourceIntegration.SubscribeLevelPlayInterstitialImpressions;
        TenjinAdNetworks.IronSourceSubscribeLevelPlayBannerImpressions = TenjinIronSourceIntegration.SubscribeLevelPlayBannerImpressions;

        // HyperBid
        TenjinAdNetworks.HyperBidListenForImpressions = TenjinHyperBidIntegration.ListenForImpressions;

        // AdMob
        TenjinAdNetworks.AdMobListenForBannerViewImpressions = TenjinAdMobIntegration.ListenForBannerViewImpressions;
        TenjinAdNetworks.AdMobListenForRewardedAdImpressions = TenjinAdMobIntegration.ListenForRewardedAdImpressions;
        TenjinAdNetworks.AdMobListenForInterstitialAdImpressions = TenjinAdMobIntegration.ListenForInterstitialAdImpressions;
        TenjinAdNetworks.AdMobListenForRewardedInterstitialAdImpressions = TenjinAdMobIntegration.ListenForRewardedInterstitialAdImpressions;

        // TopOn
        TenjinAdNetworks.TopOnListenForImpressions = TenjinTopOnIntegration.ListenForImpressions;

        // CAS
        TenjinAdNetworks.CASListenForImpressions = TenjinCASIntegration.ListenForImpressions;
        TenjinAdNetworks.CASListenForBannerImpressions = TenjinCASIntegration.ListenForBannerImpressions;

        // TradPlus
        TenjinAdNetworks.TradPlusListenForImpressions = TenjinTradPlusIntegration.ListenForImpressions;
        TenjinAdNetworks.TradPlusImpressionFromAdInfo = TenjinTradPlusIntegration.ImpressionFromAdInfo;

        // CloudX
        TenjinAdNetworks.CloudXListenForImpressions = TenjinCloudXIntegration.ListenForImpressions;
    }
}
