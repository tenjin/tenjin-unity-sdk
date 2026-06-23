//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;

public class TenjinObject : MonoBehaviour
{
    [Header("Required")]
    [Tooltip("Your Tenjin SDK Key. Find it in your Tenjin dashboard.")]
    public string sdkKey;

    [Header("Optional")]
    [Tooltip("App subversion for A/B testing different app builds. Set to 0 to disable.")]
    public int appSubversion = 0;

    [Header("Initialization")]
    [Tooltip("When enabled, the SDK initializes and connects automatically on Awake. Disable to call Initialize() manually from your own script.")]
    public bool initializeOnAwake = true;

    [Tooltip("Enable verbose logging. Disable before releasing your app.")]
    public bool debugLogs = false;

    [Header("Privacy")]
    [Tooltip("When disabled, the SDK will not collect or send any data.")]
    public bool optIn = true;

    [Tooltip("Use a Consent Management Platform (CMP) to determine opt-in/opt-out status.")]
    public bool useConsentManagementPlatform = false;

    [Header("iOS - App Tracking Transparency")]
    [Tooltip("Request App Tracking Transparency authorization before connecting. Recommended for iOS 14+.")]
    public bool requestTrackingAuthorization = true;

    [Header("Android - App Store")]
    [Tooltip("The app store where the Android build is distributed.")]
    public AppStoreType androidAppStoreType = AppStoreType.googleplay;

    [Header("Google DMA")]
    [Tooltip("Enable Google Digital Markets Act compliance parameters.")]
    public bool enableGoogleDMA = false;

    [Tooltip("Allow ad personalization under Google DMA.")]
    public bool adPersonalization = false;

    [Tooltip("Allow user data usage under Google DMA.")]
    public bool adUserData = false;

    [Header("Advanced")]
    [Tooltip("Cache events locally when the device is offline and send them when connectivity is restored.")]
    public bool cacheEvents = false;

    [Tooltip("Encrypt all HTTP requests to Tenjin servers.")]
    public bool encryptRequests = false;

    private static TenjinObject _instance;

    /// <summary>
    /// The singleton instance of TenjinObject. Null if no TenjinObject exists in the scene.
    /// </summary>
    public static TenjinObject Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (initializeOnAwake)
        {
            Initialize();
        }
    }

    /// <summary>
    /// Initializes and connects the Tenjin SDK using the Inspector configuration.
    /// Called automatically on Awake when initializeOnAwake is enabled.
    /// Call this manually if initializeOnAwake is disabled.
    /// </summary>
    public void Initialize()
    {
        if (string.IsNullOrEmpty(sdkKey))
        {
            Debug.LogError("[Tenjin] SDK Key is not set on the TenjinObject. Please configure it in the Inspector.");
            return;
        }

        BaseTenjin instance = GetTenjinInstance();

        if (debugLogs)
        {
            instance.DebugLogs();
        }

        // Privacy settings
        if (!optIn)
        {
            instance.OptOut();
        }

        if (useConsentManagementPlatform)
        {
            instance.OptInOutUsingCMP();
        }

        // Google DMA
        if (enableGoogleDMA)
        {
            instance.SetGoogleDMAParameters(adPersonalization, adUserData);
        }

        // Advanced settings
        if (cacheEvents)
        {
            instance.SetCacheEventSetting(true);
        }

        if (encryptRequests)
        {
            instance.SetEncryptRequestsSetting(true);
        }

        // Connect with platform-specific handling
#if UNITY_IOS && !UNITY_EDITOR
        if (requestTrackingAuthorization)
        {
            instance.RequestTrackingAuthorizationWithCompletionHandler((status) =>
            {
                if (debugLogs)
                {
                    Debug.Log("[Tenjin] App Tracking Authorization status: " + status);
                }
                instance.Connect();
            });
        }
        else
        {
            instance.Connect();
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        instance.Connect();
        if (androidAppStoreType != AppStoreType.unspecified)
        {
            instance.SetAppStoreType(androidAppStoreType);
        }
#else
        instance.Connect();
#endif
    }

    /// <summary>
    /// Returns the Tenjin SDK instance configured from the Inspector fields.
    /// Useful for calling additional SDK methods (events, transactions, etc.) after initialization.
    /// </summary>
    public BaseTenjin GetTenjinInstance()
    {
        if (appSubversion != 0)
        {
            return Tenjin.getInstanceWithAppSubversion(sdkKey, appSubversion);
        }
        return Tenjin.getInstance(sdkKey);
    }
}
