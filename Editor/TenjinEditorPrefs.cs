using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace Tenjin
{
    class TenjinEditorPrefs : IPreprocessBuildWithReport
    {
        private static readonly string tenjin_admob = "tenjin_admob_enabled";
        private static readonly string tenjin_applovin = "tenjin_applovin_enabled";
        private static readonly string tenjin_facebook = "tenjin_facebook_enabled";
        private static readonly string tenjin_hyperbid = "tenjin_hyperbid_enabled";
        private static readonly string tenjin_ironsource = "tenjin_ironsource_enabled";
        private static readonly string tenjin_ironsource_legacy = "tenjin_ironsource_legacy_enabled";
        private static readonly string tenjin_ironsource_levelplay = "tenjin_ironsource_levelplay_enabled";
        private static readonly string tenjin_topon = "tenjin_topon_enabled";
        private static readonly string tenjin_cas = "tenjin_cas_enabled";
        private static readonly string tenjin_tradplus = "tenjin_tradplus_enabled";

        public int callbackOrder => 0;

        private static bool isReloadingScripts = false;

        public void OnPreprocessBuild(BuildReport report)
        {
            TenjinEditorPrefs.Update3rdPartyIntegrations();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (isReloadingScripts)
                return;
            isReloadingScripts = true;

            try
            {
                Update3rdPartyIntegrations();
            }
            finally
            {
                isReloadingScripts = false;
            }
        }

        private static void Update3rdPartyIntegrations()
        {
            UpdateAdMob();
            UpdateAppLovin();
            UpdateFacebook();
            UpdateHyperBid();
            UpdateIronSource();
            UpdateTopOn();
            UpdateCAS();
            UpdateTradPlus();
        }

        #region Third Party Library Detection
        private static void UpdateDefines(string entry, bool enabled, BuildTargetGroup[] groups)
        {
            foreach (var group in groups)
            {
                var entries = PlayerSettings.GetScriptingDefineSymbolsForGroup(group)
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // Skip updating defines if they are already in the correct state
                if (entries.Contains(entry) == enabled)
                    continue;

                var defines = entries.Where(d => d != entry);

                if (enabled)
                {
                    defines = defines.Concat(new[] { entry });
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines.ToArray()));
            }
        }

        private static bool TypeExists(params string[] types)
        {
            if (types == null || types.Length == 0) return false;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (types.Contains(type.Namespace) || types.Contains(type.FullName)) return true;
                }
            }

            return false;
        }

        private static void UpdateAdMob()
        {
            var admobTypes = new[] { "GoogleMobileAds.Common", "GoogleMobileAds.Api" };
            UpdateDefines(tenjin_admob, TypeExists(admobTypes), new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
        }

        private static void UpdateAppLovin()
        {
            var applovinTypes = new[] { "MaxSdkBase", "MaxSdkCallbacks" };
            UpdateDefines(tenjin_applovin, TypeExists(applovinTypes), new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
        }

        private static void UpdateFacebook()
        {
            var facebookTypes = new[] { "Facebook", "FB" };
            UpdateDefines(tenjin_facebook, TypeExists(facebookTypes), new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
        }

        private static void UpdateHyperBid()
        {
            var hyperbidTypes = new[] { "HyperBid.Api", "HyperBid.Api.HBCallbackInfo" };
            UpdateDefines(tenjin_hyperbid, TypeExists(hyperbidTypes), new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
        }

        private static void UpdateIronSource()
        {
            var targets = new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android };

            var ironsourceLevelPlayTypes = new[]
            {
                "Unity.Services.LevelPlay",
                "Unity.Services.LevelPlay.LevelPlayRewardedAd",
                "Unity.Services.LevelPlay.LevelPlayInterstitialAd",
                "Unity.Services.LevelPlay.LevelPlayBannerAd"
            };
            bool levelPlayFound = TypeExists(ironsourceLevelPlayTypes);

            if (levelPlayFound)
            {
                UpdateDefines(tenjin_ironsource, true, targets);
                UpdateDefines(tenjin_ironsource_levelplay, true, targets);
                UpdateDefines(tenjin_ironsource_legacy, false, targets);
            }
            else
            {
                var ironsourceLegacyTypes = new[] { "IronSource", "IronSourceEvents", "IronSourceImpressionData" };
                bool legacyFound = TypeExists(ironsourceLegacyTypes);

                UpdateDefines(tenjin_ironsource, legacyFound, targets);
                UpdateDefines(tenjin_ironsource_legacy, legacyFound, targets);
                UpdateDefines(tenjin_ironsource_levelplay, false, targets);
            }
        }

        private static void UpdateTopOn()
        {
            var toponTypes = new[] { "AnyThinkAds.Api", "AnyThinkAds.Api.ATCallbackInfo" };
            UpdateDefines(tenjin_topon, TypeExists(toponTypes), new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
        }

        private static void UpdateCAS()
        {
            var casTypes = new[] { "CAS.IMediationManager", "CAS.IAdView" };
            UpdateDefines(tenjin_cas, TypeExists(casTypes), new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
        }

        private static void UpdateTradPlus()
        {
            var tradplusTypes = new[] { "TradplusAds" };
            UpdateDefines(tenjin_tradplus, TypeExists(tradplusTypes), new[] { BuildTargetGroup.iOS, BuildTargetGroup.Android });
        }
        #endregion
    }
}
