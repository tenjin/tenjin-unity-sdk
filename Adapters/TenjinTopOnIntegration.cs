//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class TenjinTopOnIntegration
{
    private static bool _subscribed = false;
    public TenjinTopOnIntegration()
    {
    }
    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_topon_enabled
        if (_subscribed)
        {
            Debug.Log("Ignoring duplicate topon subscription");
            return;
        }
        if (AnyThinkAds.Api.ATBannerAd.Instance != null && AnyThinkAds.Api.ATBannerAd.Instance.client != null ) {
            AnyThinkAds.Api.ATBannerAd.Instance.client.onAdImpressEvent += (sender, args) => 
            {
                if (args != null) {
                    string customRuleChannel = "";
                    string customRuleSubChannel = "";
                    if (args.callbackInfo.custom_rule != null) {
                        foreach (KeyValuePair<string, object> kvp in args.callbackInfo.custom_rule)
                        {
                            string kvpKey = (string) kvp.Key;
                            string kvpValue = (string) kvp.Value;
                            if (String.Equals("channel", kvpKey))
                            {
                                customRuleChannel = kvpValue;
                            }
                            if (String.Equals("sub_channel", kvpKey))
                            {
                                customRuleSubChannel = kvpValue;
                            }
                        }
                    }
                    TopOnCustomRule customRule = new TopOnCustomRule()
                    {
                        channel = customRuleChannel,
                        sub_channel = customRuleSubChannel
                    };
                    string customRulejson = JsonUtility.ToJson(customRule);
                    try
                    {
                        TopOnAdImpressionDataToJSON topOnAdImpressionData = new TopOnAdImpressionDataToJSON()
                        {
                        show_id = args.callbackInfo.id,
                        publisher_revenue = args.callbackInfo.publisher_revenue,
                        currency = args.callbackInfo.currency,
                        country = args.callbackInfo.country,
                        hyper_bid_placement_id = args.callbackInfo.adunit_id,
                        hyper_bid_ad_format = args.callbackInfo.adunit_format,
                        ecpm_precision = args.callbackInfo.precision,
                        ad_network_type = args.callbackInfo.network_type,
                        network_placement_id = args.callbackInfo.network_placement_id,
                        ecpm_level = args.callbackInfo.ecpm_level,
                        segment_id = args.callbackInfo.segment_id,
                        scenario_id = args.callbackInfo.scenario_id,
                        scenario_reward_name = args.callbackInfo.scenario_reward_name,
                        scenario_reward_number = args.callbackInfo.scenario_reward_number,
                        channel = args.callbackInfo.channel,
                        sub_channel = args.callbackInfo.sub_channel,
                        custom_rule = customRulejson,
                        network_firm_id = args.callbackInfo.network_firm_id,
                        adsource_id = args.callbackInfo.adsource_id,
                        adsource_index = args.callbackInfo.adsource_index,
                        ecpm = args.callbackInfo.adsource_price,
                        is_header_bidding_adsource = args.callbackInfo.adsource_isheaderbidding,
                        reward_user_custom_data = args.callbackInfo.reward_custom_data
                        };
                        string json = JsonUtility.ToJson(topOnAdImpressionData);
                        Debug.Log($"TopOn ATBannerAd JSON string - {json}");
                        callback(json);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"error parsing impression " + ex.ToString());
                    }
                }
            };
        }

        if (AnyThinkAds.Api.ATInterstitialAd.Instance != null && AnyThinkAds.Api.ATInterstitialAd.Instance.client != null ) {
            AnyThinkAds.Api.ATInterstitialAd.Instance.client.onAdShowEvent += (sender, args) => 
            {
                if (args != null) {
                    string customRuleChannel = "";
                    string customRuleSubChannel = "";
                    if (args.callbackInfo.custom_rule != null) {
                        foreach (KeyValuePair<string, object> kvp in args.callbackInfo.custom_rule)
                        {
                            string kvpKey = (string) kvp.Key;
                            string kvpValue = (string) kvp.Value;
                            if (String.Equals("channel", kvpKey))
                            {
                                customRuleChannel = kvpValue;
                            }
                            if (String.Equals("sub_channel", kvpKey))
                            {
                                customRuleSubChannel = kvpValue;
                            }
                        }
                    }
                    TopOnCustomRule customRule = new TopOnCustomRule()
                    {
                        channel = customRuleChannel,
                        sub_channel = customRuleSubChannel
                    };
                    string customRulejson = JsonUtility.ToJson(customRule);
                    try
                    {
                        TopOnAdImpressionDataToJSON topOnAdImpressionData = new TopOnAdImpressionDataToJSON()
                        {
                        show_id = args.callbackInfo.id,
                        publisher_revenue = args.callbackInfo.publisher_revenue,
                        currency = args.callbackInfo.currency,
                        country = args.callbackInfo.country,
                        hyper_bid_placement_id = args.callbackInfo.adunit_id,
                        hyper_bid_ad_format = args.callbackInfo.adunit_format,
                        ecpm_precision = args.callbackInfo.precision,
                        ad_network_type = args.callbackInfo.network_type,
                        network_placement_id = args.callbackInfo.network_placement_id,
                        ecpm_level = args.callbackInfo.ecpm_level,
                        segment_id = args.callbackInfo.segment_id,
                        scenario_id = args.callbackInfo.scenario_id,
                        scenario_reward_name = args.callbackInfo.scenario_reward_name,
                        scenario_reward_number = args.callbackInfo.scenario_reward_number,
                        channel = args.callbackInfo.channel,
                        sub_channel = args.callbackInfo.sub_channel,
                        custom_rule = customRulejson,
                        network_firm_id = args.callbackInfo.network_firm_id,
                        adsource_id = args.callbackInfo.adsource_id,
                        adsource_index = args.callbackInfo.adsource_index,
                        ecpm = args.callbackInfo.adsource_price,
                        is_header_bidding_adsource = args.callbackInfo.adsource_isheaderbidding,
                        reward_user_custom_data = args.callbackInfo.reward_custom_data
                        };
                        string json = JsonUtility.ToJson(topOnAdImpressionData);
                        Debug.Log($"TopOn ATInterstitialAd JSON string - {json}");
                        callback(json);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"error parsing impression " + ex.ToString());
                    }
                }
            };
        }

        if (AnyThinkAds.Api.ATRewardedVideo.Instance != null && AnyThinkAds.Api.ATRewardedVideo.Instance.client != null ) {
            AnyThinkAds.Api.ATRewardedVideo.Instance.client.onAdVideoStartEvent += (sender, args) => 
            {
                if (args != null) {
                    string customRuleChannel = "";
                    string customRuleSubChannel = "";
                    if (args.callbackInfo.custom_rule != null) {
                        foreach (KeyValuePair<string, object> kvp in args.callbackInfo.custom_rule)
                        {
                            string kvpKey = (string) kvp.Key;
                            string kvpValue = (string) kvp.Value;
                            if (String.Equals("channel", kvpKey))
                            {
                                customRuleChannel = kvpValue;
                            }
                            if (String.Equals("sub_channel", kvpKey))
                            {
                                customRuleSubChannel = kvpValue;
                            }
                        }
                    }
                    TopOnCustomRule customRule = new TopOnCustomRule()
                    {
                        channel = customRuleChannel,
                        sub_channel = customRuleSubChannel
                    };
                    string customRulejson = JsonUtility.ToJson(customRule);
                    try
                    {
                        TopOnAdImpressionDataToJSON topOnAdImpressionData = new TopOnAdImpressionDataToJSON()
                        {
                        show_id = args.callbackInfo.id,
                        publisher_revenue = args.callbackInfo.publisher_revenue,
                        currency = args.callbackInfo.currency,
                        country = args.callbackInfo.country,
                        hyper_bid_placement_id = args.callbackInfo.adunit_id,
                        hyper_bid_ad_format = args.callbackInfo.adunit_format,
                        ecpm_precision = args.callbackInfo.precision,
                        ad_network_type = args.callbackInfo.network_type,
                        network_placement_id = args.callbackInfo.network_placement_id,
                        ecpm_level = args.callbackInfo.ecpm_level,
                        segment_id = args.callbackInfo.segment_id,
                        scenario_id = args.callbackInfo.scenario_id,
                        scenario_reward_name = args.callbackInfo.scenario_reward_name,
                        scenario_reward_number = args.callbackInfo.scenario_reward_number,
                        channel = args.callbackInfo.channel,
                        sub_channel = args.callbackInfo.sub_channel,
                        custom_rule = customRulejson,
                        network_firm_id = args.callbackInfo.network_firm_id,
                        adsource_id = args.callbackInfo.adsource_id,
                        adsource_index = args.callbackInfo.adsource_index,
                        ecpm = args.callbackInfo.adsource_price,
                        is_header_bidding_adsource = args.callbackInfo.adsource_isheaderbidding,
                        reward_user_custom_data = args.callbackInfo.reward_custom_data
                        };
                        string json = JsonUtility.ToJson(topOnAdImpressionData);
                        Debug.Log($"TopOn ATRewardedVideo JSON string - {json}");
                        callback(json);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"error parsing impression " + ex.ToString());
                    }
                }
            };
        }

        if (AnyThinkAds.Api.ATNativeAd.Instance != null && AnyThinkAds.Api.ATNativeAd.Instance.client != null ) {
            AnyThinkAds.Api.ATNativeAd.Instance.client.onAdImpressEvent += (sender, args) => 
            {
                if (args != null) {
                    string customRuleChannel = "";
                    string customRuleSubChannel = "";
                    if (args.callbackInfo.custom_rule != null) {
                      foreach (KeyValuePair<string, object> kvp in args.callbackInfo.custom_rule)
                      {
                          string kvpKey = (string) kvp.Key;
                          string kvpValue = (string) kvp.Value;
                          if (String.Equals("channel", kvpKey))
                          {
                              customRuleChannel = kvpValue;
                          }
                          if (String.Equals("sub_channel", kvpKey))
                          {
                              customRuleSubChannel = kvpValue;
                          }
                      }
                    }
                    TopOnCustomRule customRule = new TopOnCustomRule()
                    {
                        channel = customRuleChannel,
                        sub_channel = customRuleSubChannel
                    };
                    string customRulejson = JsonUtility.ToJson(customRule);
                    try
                    {
                        TopOnAdImpressionDataToJSON topOnAdImpressionData = new TopOnAdImpressionDataToJSON()
                        {
                        show_id = args.callbackInfo.id,
                        publisher_revenue = args.callbackInfo.publisher_revenue,
                        currency = args.callbackInfo.currency,
                        country = args.callbackInfo.country,
                        hyper_bid_placement_id = args.callbackInfo.adunit_id,
                        hyper_bid_ad_format = args.callbackInfo.adunit_format,
                        ecpm_precision = args.callbackInfo.precision,
                        ad_network_type = args.callbackInfo.network_type,
                        network_placement_id = args.callbackInfo.network_placement_id,
                        ecpm_level = args.callbackInfo.ecpm_level,
                        segment_id = args.callbackInfo.segment_id,
                        scenario_id = args.callbackInfo.scenario_id,
                        scenario_reward_name = args.callbackInfo.scenario_reward_name,
                        scenario_reward_number = args.callbackInfo.scenario_reward_number,
                        channel = args.callbackInfo.channel,
                        sub_channel = args.callbackInfo.sub_channel,
                        custom_rule = customRulejson,
                        network_firm_id = args.callbackInfo.network_firm_id,
                        adsource_id = args.callbackInfo.adsource_id,
                        adsource_index = args.callbackInfo.adsource_index,
                        ecpm = args.callbackInfo.adsource_price,
                        is_header_bidding_adsource = args.callbackInfo.adsource_isheaderbidding,
                        reward_user_custom_data = args.callbackInfo.reward_custom_data
                        };
                        string json = JsonUtility.ToJson(topOnAdImpressionData);
                        Debug.Log($"TopOn ATNativeAd JSON string - {json}");
                        callback(json);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"error parsing impression " + ex.ToString());
                    }
                }
            };
        }
#endif
    }
}

[System.Serializable]
internal class TopOnCustomRule
{
    public string channel;
    public string sub_channel;
}

[System.Serializable]
internal class TopOnAdImpressionDataToJSON
{
    public string show_id;
    public double publisher_revenue;
    public string currency;
    public string country;
    public string hyper_bid_placement_id;
    public string hyper_bid_ad_format;
    public string ecpm_precision;
    public string ad_network_type;
    public string network_placement_id;
    public int ecpm_level;
    public int segment_id;
    public string scenario_id;
    public string scenario_reward_name;
    public int scenario_reward_number; 
    public string channel;
    public string sub_channel;
    public string custom_rule;
    public int network_firm_id;
    public string adsource_id;
    public int adsource_index;
    public double ecpm;
    public int is_header_bidding_adsource;
    public string reward_user_custom_data;
}
