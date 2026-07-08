//
//  Copyright (c) 2026 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;

/// <summary>
/// Decoupling boundary between the Tenjin core SDK and the bundled ad-network
/// integrations.
///
/// The integration classes (<c>Tenjin*Integration</c>) reference third-party ad
/// SDK types directly. Some of those SDKs (AdMob, TopOn, TradPlus, HyperBid) live
/// in the host project's predefined <c>Assembly-CSharp</c>, which a Unity assembly
/// definition can never reference. To let the core SDK ship as a referenceable
/// assembly (so customers with their own asmdef can use it) while still calling the
/// integrations, the integrations stay in <c>Assembly-CSharp</c> and register their
/// entry points here at startup (see <c>TenjinAdNetworkRegistrar</c>). The core then
/// dispatches through these delegates instead of referencing the integration classes.
///
/// Each delegate is null until the registrar runs (very early, via
/// <c>RuntimeInitializeOnLoadMethod</c>), so all call sites invoke with <c>?.Invoke</c>.
/// In the example project, where everything compiles into <c>Assembly-CSharp</c>, the
/// exact same registration path is used.
/// </summary>
public static class TenjinAdNetworks
{
    // AppLovin
    public static Action<Action<string>> AppLovinListenForImpressions;

    // IronSource / LevelPlay
    public static Action<Action<string>> IronSourceListenForImpressions;
    public static Action<object, Action<string>> IronSourceSubscribeLevelPlayRewardedImpressions;
    public static Action<object, Action<string>> IronSourceSubscribeLevelPlayInterstitialImpressions;
    public static Action<object, Action<string>> IronSourceSubscribeLevelPlayBannerImpressions;

    // HyperBid
    public static Action<Action<string>> HyperBidListenForImpressions;

    // AdMob
    public static Action<object, string, Action<string>> AdMobListenForBannerViewImpressions;
    public static Action<object, string, Action<string>> AdMobListenForRewardedAdImpressions;
    public static Action<object, string, Action<string>> AdMobListenForInterstitialAdImpressions;
    public static Action<object, string, Action<string>> AdMobListenForRewardedInterstitialAdImpressions;

    // TopOn
    public static Action<Action<string>> TopOnListenForImpressions;

    // CAS
    public static Action<Action<string>, object> CASListenForImpressions;
    public static Action<Action<string>, object> CASListenForBannerImpressions;

    // TradPlus
    public static Action<Action<string>> TradPlusListenForImpressions;
    public static Action<Action<string>, Dictionary<string, object>> TradPlusImpressionFromAdInfo;

    // CloudX
    public static Action<Action<string>> CloudXListenForImpressions;
}
