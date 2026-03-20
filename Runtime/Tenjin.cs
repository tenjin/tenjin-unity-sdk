//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;
using System;
using System.Collections.Generic;

public static class Tenjin
{

	public delegate void DeferredDeeplinkDelegate(Dictionary<string, string> deferredLinkData);
	public delegate void AttributionInfoDelegate(Dictionary<string, string> attributionInfoData);

	//create dictionary of instances of tenjin with API keys
	private static Dictionary<string, BaseTenjin> _instances = new Dictionary<string, BaseTenjin>();

	// App Tracking Transparency callback
	public static Action<int> authorizationStatusDelegate = null;

	//return instance with specific api key
	public static BaseTenjin getInstance(string apiKey)
	{
		string instanceKey = apiKey;
		if (!_instances.ContainsKey(apiKey))
		{
			_instances.Add(apiKey, createTenjin(apiKey, null, 0));
		}
		return _instances[instanceKey];
	}

	public static BaseTenjin getInstanceWithSharedSecret(string apiKey, string sharedSecret)
	{
		string instanceKey = apiKey + "." + sharedSecret;
		if (!_instances.ContainsKey(instanceKey))
		{
			_instances.Add(instanceKey, createTenjin(apiKey, sharedSecret, 0));
		}
		return _instances[instanceKey];
	}

	public static BaseTenjin getInstanceWithAppSubversion(string apiKey, int appSubversion)
	{
		string instanceKey = apiKey + "." + appSubversion;
		if (!_instances.ContainsKey(instanceKey))
		{
			_instances.Add(instanceKey, createTenjin(apiKey, null, appSubversion));
		}
		return _instances[instanceKey];
	}

	public static BaseTenjin getInstanceWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
	{
		string instanceKey = apiKey + "." + sharedSecret + "." + appSubversion;
		if (!_instances.ContainsKey(instanceKey))
		{
			_instances.Add(instanceKey, createTenjin(apiKey, sharedSecret, appSubversion));
		}
		return _instances[instanceKey];
	}

	private static BaseTenjin createTenjin(string apiKey, string sharedSecret, int appSubversion)
	{
		GameObject tenjinGameObject = new GameObject("Tenjin");
		tenjinGameObject.hideFlags = HideFlags.HideAndDontSave;
		UnityEngine.Object.DontDestroyOnLoad(tenjinGameObject);

#if UNITY_ANDROID && !UNITY_EDITOR
    BaseTenjin retTenjin = tenjinGameObject.AddComponent<AndroidTenjin>();
#elif UNITY_IPHONE && !UNITY_EDITOR
    BaseTenjin retTenjin = tenjinGameObject.AddComponent<IosTenjin>();
#else
		BaseTenjin retTenjin = tenjinGameObject.AddComponent<DebugTenjin>();
#endif

		if (!string.IsNullOrEmpty(sharedSecret) && appSubversion != 0)
		{
			retTenjin.InitWithSharedSecretAppSubversion(apiKey, sharedSecret, appSubversion);
		}
		else if (!string.IsNullOrEmpty(sharedSecret))
		{
			retTenjin.InitWithSharedSecret(apiKey, sharedSecret);
		}
		else if (appSubversion != 0)
		{
			retTenjin.InitWithAppSubversion(apiKey, appSubversion);
		}
		else
		{
			retTenjin.Init(apiKey);
		}
		return retTenjin;
	}
}
