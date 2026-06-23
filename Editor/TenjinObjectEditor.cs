//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TenjinObject))]
[CanEditMultipleObjects]
public class TenjinObjectEditor : Editor
{
    SerializedProperty sdkKey;
    SerializedProperty appSubversion;
    SerializedProperty initializeOnAwake;
    SerializedProperty debugLogs;
    SerializedProperty optIn;
    SerializedProperty useConsentManagementPlatform;
    SerializedProperty requestTrackingAuthorization;
    SerializedProperty androidAppStoreType;
    SerializedProperty enableGoogleDMA;
    SerializedProperty adPersonalization;
    SerializedProperty adUserData;
    SerializedProperty cacheEvents;
    SerializedProperty encryptRequests;

    private static readonly Color TenjinGreen = new Color(0.188f, 0.816f, 0.345f);

    void OnEnable()
    {
        sdkKey = serializedObject.FindProperty("sdkKey");
        appSubversion = serializedObject.FindProperty("appSubversion");
        initializeOnAwake = serializedObject.FindProperty("initializeOnAwake");
        debugLogs = serializedObject.FindProperty("debugLogs");
        optIn = serializedObject.FindProperty("optIn");
        useConsentManagementPlatform = serializedObject.FindProperty("useConsentManagementPlatform");
        requestTrackingAuthorization = serializedObject.FindProperty("requestTrackingAuthorization");
        androidAppStoreType = serializedObject.FindProperty("androidAppStoreType");
        enableGoogleDMA = serializedObject.FindProperty("enableGoogleDMA");
        adPersonalization = serializedObject.FindProperty("adPersonalization");
        adUserData = serializedObject.FindProperty("adUserData");
        cacheEvents = serializedObject.FindProperty("cacheEvents");
        encryptRequests = serializedObject.FindProperty("encryptRequests");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawTenjinHeader();
        EditorGUILayout.Space(8);

        DrawRequiredSection();
        EditorGUILayout.Space(4);

        DrawOptionalSection();
        EditorGUILayout.Space(4);

        DrawInitializationSection();
        EditorGUILayout.Space(4);

        DrawPrivacySection();
        EditorGUILayout.Space(4);

        DrawiOSSection();
        EditorGUILayout.Space(4);

        DrawAndroidSection();
        EditorGUILayout.Space(4);

        DrawGoogleDMASection();
        EditorGUILayout.Space(4);

        DrawAdvancedSection();
        EditorGUILayout.Space(8);

        DrawDocLinks();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTenjinHeader()
    {
        EditorGUILayout.Space(4);

        var headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 18,
            alignment = TextAnchor.MiddleCenter
        };
        EditorGUILayout.LabelField("Tenjin SDK", headerStyle);

        var versionStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            alignment = TextAnchor.MiddleCenter
        };
        EditorGUILayout.LabelField("Drag-and-drop integration", versionStyle);
    }

    private void DrawRequiredSection()
    {
        EditorGUILayout.LabelField("SDK Key", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sdkKey, new GUIContent("SDK Key"));

        if (string.IsNullOrEmpty(sdkKey.stringValue))
        {
            EditorGUILayout.HelpBox("SDK Key is required. Find it in your Tenjin dashboard at https://dashboard.tenjin.com", MessageType.Warning);
        }
    }

    private void DrawOptionalSection()
    {
        EditorGUILayout.LabelField("Optional Configuration", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(appSubversion, new GUIContent("App Subversion", "For A/B testing. Set to 0 to disable."));

        if (appSubversion.intValue < 0)
        {
            appSubversion.intValue = 0;
        }
    }

    private void DrawInitializationSection()
    {
        EditorGUILayout.LabelField("Initialization", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(initializeOnAwake, new GUIContent("Initialize on Awake", "Auto-initialize when the scene loads."));

        if (!initializeOnAwake.boolValue)
        {
            EditorGUILayout.HelpBox("You must call TenjinObject.Instance.Initialize() from your own script to start the SDK.", MessageType.Info);
        }

        EditorGUILayout.PropertyField(debugLogs, new GUIContent("Debug Logs"));

        if (debugLogs.boolValue)
        {
            EditorGUILayout.HelpBox("Debug logging is enabled. Disable this before releasing your app.", MessageType.Warning);
        }
    }

    private void DrawPrivacySection()
    {
        EditorGUILayout.LabelField("Privacy", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(optIn, new GUIContent("Opt In", "When disabled, the SDK will not collect or send any data."));
        EditorGUILayout.PropertyField(useConsentManagementPlatform, new GUIContent("Use CMP", "Use a Consent Management Platform to determine data collection."));
    }

    private void DrawiOSSection()
    {
        EditorGUILayout.LabelField("iOS - App Tracking Transparency", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(requestTrackingAuthorization, new GUIContent("Request ATT", "Request App Tracking Transparency authorization before connecting."));

        if (requestTrackingAuthorization.boolValue)
        {
            EditorGUILayout.HelpBox("The ATT dialog will be shown to users on iOS 14+. Make sure you have set NSUserTrackingUsageDescription in your Info.plist.", MessageType.Info);
        }
    }

    private void DrawAndroidSection()
    {
        EditorGUILayout.LabelField("Android - App Store", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(androidAppStoreType, new GUIContent("App Store Type", "Where the Android build is distributed."));
    }

    private void DrawGoogleDMASection()
    {
        EditorGUILayout.LabelField("Google DMA Compliance", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enableGoogleDMA, new GUIContent("Enable Google DMA"));

        if (enableGoogleDMA.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(adPersonalization, new GUIContent("Ad Personalization"));
            EditorGUILayout.PropertyField(adUserData, new GUIContent("Ad User Data"));
            EditorGUI.indentLevel--;
        }
    }

    private void DrawAdvancedSection()
    {
        EditorGUILayout.LabelField("Advanced", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(cacheEvents, new GUIContent("Cache Events", "Cache events offline and send when connectivity is restored."));
        EditorGUILayout.PropertyField(encryptRequests, new GUIContent("Encrypt Requests", "Encrypt all HTTP requests to Tenjin."));
    }

    private void DrawDocLinks()
    {
        EditorGUILayout.LabelField("Documentation", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Tenjin Dashboard"))
        {
            Application.OpenURL("https://dashboard.tenjin.com");
        }
        if (GUILayout.Button("Unity SDK Docs"))
        {
            Application.OpenURL("https://github.com/tenjin/tenjin-unity-sdk");
        }
        EditorGUILayout.EndHorizontal();
    }
}
