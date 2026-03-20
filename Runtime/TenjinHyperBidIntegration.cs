//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class TenjinHyperBidIntegration
{
    private static bool _subscribed_hyperbid = false;
    public TenjinHyperBidIntegration()
    {
    }
    public static void ListenForImpressions(Action<string> callback)
    {
#if tenjin_hyperbid_enabled
        if (_subscribed_hyperbid)
        {
            Debug.Log("Ignoring duplicate hyperbid subscription");
            return;
        }
        HyperBid.Api.HBInterstitialAd.Instance.events.onAdShowEvent += (sender, args) => 
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
                CustomRule customRule = new CustomRule()
                {
                    channel = customRuleChannel,
                    sub_channel = customRuleSubChannel
                };
                string customRulejson = JsonUtility.ToJson(customRule);
                try
                {
                    HyperBidAdImpressionDataToJSON hyperBidAdImpressionData = new HyperBidAdImpressionDataToJSON()
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
                    string json = JsonUtility.ToJson(hyperBidAdImpressionData);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing impression " + ex.ToString());
                }
            }
        };
        HyperBid.Api.HBBannerAd.Instance.events.onAdImpressEvent += (sender, args) => 
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
                CustomRule customRule = new CustomRule()
                {
                    channel = customRuleChannel,
                    sub_channel = customRuleSubChannel
                };
                string customRulejson = JsonUtility.ToJson(customRule);
                try
                {
                    HyperBidAdImpressionDataToJSON hyperBidAdImpressionData = new HyperBidAdImpressionDataToJSON()
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
                    string json = JsonUtility.ToJson(hyperBidAdImpressionData);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing impression " + ex.ToString());
                }
            }
        };
        HyperBid.Api.HBRewardedVideo.Instance.events.onAdVideoStartEvent += (sender, args) => 
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
                CustomRule customRule = new CustomRule()
                {
                    channel = customRuleChannel,
                    sub_channel = customRuleSubChannel
                };
                string customRulejson = JsonUtility.ToJson(customRule);
                try
                {
                    HyperBidAdImpressionDataToJSON hyperBidAdImpressionData = new HyperBidAdImpressionDataToJSON()
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
                    string json = JsonUtility.ToJson(hyperBidAdImpressionData);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing impression " + ex.ToString());
                }
            }
        };
        HyperBid.Api.HBNativeAd.Instance.events.onAdImpressEvent += (sender, args) => 
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
                CustomRule customRule = new CustomRule()
                {
                    channel = customRuleChannel,
                    sub_channel = customRuleSubChannel
                };
                string customRulejson = JsonUtility.ToJson(customRule);
                try
                {
                    HyperBidAdImpressionDataToJSON hyperBidAdImpressionData = new HyperBidAdImpressionDataToJSON()
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
                    string json = JsonUtility.ToJson(hyperBidAdImpressionData);
                    callback(json);
                }
                catch (Exception ex)
                {
                    Debug.Log($"error parsing impression " + ex.ToString());
                }
            }
        };
        _subscribed_hyperbid = true;
#endif
    }
}

[System.Serializable]
internal class CustomRule
{
    public string channel;
    public string sub_channel;
}

[System.Serializable]
internal class HyperBidAdImpressionDataToJSON
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
