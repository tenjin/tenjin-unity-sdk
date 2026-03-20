//
//  TenjinUnityInterface.h
//  Unity-iOS bridge
//
//  Copyright (c) 2018 Tenjin. All rights reserved.
//
//

#ifndef __Unity_iPhone__TenjinUnityInterface__
#define __Unity_iPhone__TenjinUnityInterface__

#include "TenjinSDK.h"

extern "C" {

typedef struct TenjinStringStringKeyValuePair {
    const char* key;
    const char* value;
} TenjinStringStringKeyValuePair;

typedef void (*TenjinDeeplinkHandlerFunc)(TenjinStringStringKeyValuePair* deepLinkDataPairArray, int32_t deepLinkDataPairCount);
typedef void (*TenjinAttributionInfoFunc)(TenjinStringStringKeyValuePair* attributionInfoDataPairArray, int32_t attributionInfoDataPairCount);
typedef void (*TenjinUserProfileFunc)(TenjinStringStringKeyValuePair* userProfileDataPairArray, int32_t userProfileDataPairCount);

void iosTenjinInit(const char* apiKey);
void iosTenjinInitWithSharedSecret(const char* apiKey, const char* sharedSecret);
void iosTenjinInitWithAppSubversion(const char* apiKey, int subversion);
void iosTenjinInitWithSharedSecretAppSubversion(const char* apiKey, const char* sharedSecret, int subversion);

void iosTenjinInitialize(const char* apiKey);
void iosTenjinInitializeWithSharedSecret(const char* apiKey, const char* sharedSecret);
void iosTenjinInitializeWithAppSubversion(const char* apiKey, int subversion);
void iosTenjinInitializeWithSharedSecretAppSubversion(const char* apiKey, const char* sharedSecret, int subversion);

void iosTenjinConnect();
void iosTenjinConnectWithDeferredDeeplink(const char* deferredDeeplink);

void iosTenjinSendEvent(const char* eventName);
void iosTenjinSendEventWithValue(const char* eventName, const char* eventValue);
void iosTenjinTransaction(const char* productId, const char* currencyCode, int quantity, double price);
void iosTenjinTransactionWithReceiptData(const char* productId, const char* currencyCode, int quantity, double price, const char* transactionId, const char* receipt);
void iosTenjinRegisterDeepLinkHandler(TenjinDeeplinkHandlerFunc deeplinkHandlerFunc);
void iosTenjinGetAttributionInfo(TenjinAttributionInfoFunc attributionInfoFunc);

void iosTenjinOptIn();
void iosTenjinOptOut();
void iosTenjinOptInParams(char** params, int size);
void iosTenjinOptOutParams(char** params, int size);
bool iosTenjinOptInOutUsingCMP();
void iosTenjinOptInGoogleDMA();
void iosTenjinOptOutGoogleDMA();
void iosTenjinAppendAppSubversion(int subversion);
void iosTenjinUpdateConversionValue(int conversionValue);
void iosTenjinUpdatePostbackConversionValue(int conversionValue);
void iosTenjinUpdatePostbackConversionValueCoarseValue(int conversionValue, const char* coarseValue);
void iosTenjinUpdatePostbackConversionValueCoarseValueLockWindow(int conversionValue, const char* coarseValue, bool lockWindow);
void iosTenjinRequestTrackingAuthorizationWithCompletionHandler();

void iosTenjinSetDebugLogs();

void iosTenjinSubscribeAppLovinImpressions();
void iosTenjinSubscribeIronSourceImpressions();
void iosTenjinAppLovinImpressionFromJSON(const char* jsonString);
void iosTenjinIronSourceImpressionFromJSON(const char* jsonString);
void iosTenjinHyperBidImpressionFromJSON(const char* jsonString);
void iosTenjinSubscribeAbMobImpressionFromJSON(const char* jsonString);
void iosTenjinTopOnImpressionFromJSON(const char* jsonString);
void iosTenjinCASImpressionFromJSON(const char* jsonString);
void iosTenjinTradPlusImpressionFromJSON(const char* jsonString);

void iosTenjinSetCustomerUserId(const char* userId);
const char* iosTenjinGetSetCustomerUserId();
void iosTenjinSetCacheEventSetting(bool setting);
void iosTenjinSetEncryptRequestsSetting(bool setting);
const char* iosTenjinGetAnalyticsInstallationId();
void iosTenjinGetUserProfileDictionary(TenjinUserProfileFunc userProfileFunc);
void iosTenjinResetUserProfile();
}

#endif
