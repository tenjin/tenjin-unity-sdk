//
//  TenjinUnityInterface.mm
//  Unity-iOS bridge
//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

#include "TenjinUnityInterface.h"

extern "C" {
#define MALLOC_ARRAY(count, type) (count == 0 ? NULL : (type*) malloc(count * sizeof(type)))
void iosTenjin_InternalFreeStringStringKeyValuePairs(TenjinStringStringKeyValuePair* pairs, int32_t pairCount);
bool iosTenjin_InternalConvertDictionaryToStringStringPairs(NSDictionary<NSString*, NSObject*>* dictionary, TenjinStringStringKeyValuePair** outPairArray, int* outPairCount);
bool iosTenjin_InternalConvertUserProfileDictionaryToStringStringPairs(NSDictionary* dictionary, TenjinStringStringKeyValuePair** outPairArray, int* outPairCount);

TenjinDeeplinkHandlerFunc registeredDeeplinkHandlerFunc;
TenjinAttributionInfoFunc registeredAttributionInfoFunc;
TenjinUserProfileFunc registeredUserProfileFunc;

void iosTenjinInit(const char* apiKey){
    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];

    NSLog(@"Called Tenjin [TenjinSDK sharedInstanceWithToken:%@]", apiKeyStr);
    [TenjinSDK initialize:apiKeyStr];
}
    
void iosTenjinInitWithSharedSecret(const char* apiKey, const char* sharedSecret){
    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
    NSString *sharedSecretStr = [NSString stringWithUTF8String:sharedSecret];

    NSLog(@"Called Tenjin [TenjinSDK sharedInstanceWithToken:%@ andSharedSecret:*]", apiKeyStr);
    [TenjinSDK initialize:apiKeyStr andSharedSecret:sharedSecretStr];
}
    
void iosTenjinInitWithAppSubversion(const char* apiKey, int subversion){
    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
    NSNumber *subVersion = [NSNumber numberWithInt:subversion];

    NSLog(@"Called Tenjin [TenjinSDK sharedInstanceWithToken:%@ andAppSubversion:%@]", apiKeyStr, subVersion);
    [TenjinSDK initialize:apiKeyStr andAppSubversion:subVersion];
}
    
void iosTenjinInitWithSharedSecretAppSubversion(const char* apiKey, const char* sharedSecret, int subversion){
    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
    NSString *sharedSecretStr = [NSString stringWithUTF8String:sharedSecret];
    NSNumber *subVersion = [NSNumber numberWithInt:subversion];
    
    NSLog(@"Called Tenjin [TenjinSDK sharedInstanceWithToken:%@ andSharedSecret:* andAppSubversion:%@]", apiKeyStr, subVersion);
    [TenjinSDK initialize:apiKeyStr andSharedSecret:sharedSecretStr andAppSubversion:subVersion];
}
    
void iosTenjinConnect(){
    [TenjinSDK connect];
}

void iosTenjinConnectWithDeferredDeeplink(const char* deferredDeeplink){
    NSString *deferredDeeplinkStr = [NSString stringWithUTF8String:deferredDeeplink];
    NSURL *deferredDeeplinkStrUri = [NSURL URLWithString:[deferredDeeplinkStr stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
    NSLog(@"Called Tenjin [TenjinSDK connectWithDeferredDeeplink:%@]", deferredDeeplinkStr);
    [TenjinSDK connectWithDeferredDeeplink:deferredDeeplinkStrUri];
}
    
void iosTenjinSendEvent(const char* eventName){
    NSString *eventNameStr = [NSString stringWithUTF8String:eventName];
    NSLog(@"Called Tenjin [TenjinSDK sendEventWithName:%@]", eventNameStr);
    [TenjinSDK sendEventWithName:eventNameStr];
}

void iosTenjinSendEventWithValue(const char* eventName, const char* eventValue){
    NSString *eventNameStr = [NSString stringWithUTF8String:eventName];
    NSString *eventValueStr = [NSString stringWithUTF8String:eventValue];
    NSLog(@"Called Tenjin [TenjinSDK sendEventWithName:%@ andEventValue:%@]", eventNameStr, eventValueStr);
    [TenjinSDK sendEventWithName:eventNameStr andEventValue:eventValueStr];
}

void iosTenjinTransaction(const char* productId, const char* currencyCode, int quantity, double price){
    NSString *prodId = [NSString stringWithUTF8String:productId];
    NSString *curCode = [NSString stringWithUTF8String:currencyCode];
    NSDecimalNumber* pr = [[NSDecimalNumber alloc] initWithDouble:price];
    NSLog(@"Called Tenjin [TenjinSDK transactionWithProductName:%@ andCurrencyCode:%@ andQuantity:%d andUnitPrice:%f]", prodId, curCode, quantity, price);

    //call manual method in tenjin sdk
    [TenjinSDK transactionWithProductName:prodId andCurrencyCode:curCode andQuantity:quantity andUnitPrice:pr];
}

void iosTenjinTransactionWithReceiptData(const char* productId, const char* currencyCode, int quantity, double price, const char* transactionId, const char* receipt){
    NSString *prodId = [NSString stringWithUTF8String: productId];
    NSString *curCode = [NSString stringWithUTF8String: currencyCode];
    NSDecimalNumber *pr = [[NSDecimalNumber alloc] initWithDouble: price];
    NSString *tid = [NSString stringWithUTF8String: transactionId];
    NSString *rec = [NSString stringWithUTF8String: receipt];

    //call manual tenjin call with receipt data
    NSLog(@"Called Tenjin [TenjinSDK transactionWithProductName:%@ andCurrencyCode:%@ andQuantity:%d andUnitPrice:%f andTransactionId:%@ andBase64Receipt:%@]", prodId, curCode, quantity, price, tid, rec);
    [TenjinSDK transactionWithProductName: prodId andCurrencyCode:curCode andQuantity:quantity andUnitPrice:pr andTransactionId:tid andBase64Receipt:rec];
}
    
void iosTenjinSubscription(const char* productId, const char* currencyCode, double unitPrice, const char* transactionId, const char* originalTransactionId, const char* receipt, const char* skTransaction){
    NSString *prodId = [NSString stringWithUTF8String:productId];
    NSString *curCode = [NSString stringWithUTF8String:currencyCode];
    NSDecimalNumber *price = [[NSDecimalNumber alloc] initWithDouble:unitPrice];
    NSString *txId = transactionId ? [NSString stringWithUTF8String:transactionId] : @"";
    NSString *origTxId = originalTransactionId ? [NSString stringWithUTF8String:originalTransactionId] : @"";
    NSString *rec = receipt ? [NSString stringWithUTF8String:receipt] : @"";
    NSString *skTx = skTransaction ? [NSString stringWithUTF8String:skTransaction] : @"";

    NSLog(@"Called Tenjin [TenjinSDK subscriptionWithProductName:%@ andCurrencyCode:%@ andUnitPrice:%@ andTransactionId:%@ andOriginalTransactionId:%@ andBase64Receipt:(length:%lu) andSKTransaction:(length:%lu)]", prodId, curCode, price, txId, origTxId, (unsigned long)rec.length, (unsigned long)skTx.length);
    [TenjinSDK subscriptionWithProductName:prodId
                           andCurrencyCode:curCode
                              andUnitPrice:price
                          andTransactionId:txId
                  andOriginalTransactionId:origTxId
                          andBase64Receipt:rec
                         andSKTransaction:skTx];
}

void iosTenjinSubscriptionWithStoreKit(const char* productId, const char* currencyCode, double unitPrice){
    NSString *prodId = [NSString stringWithUTF8String:productId];
    NSString *curCode = [NSString stringWithUTF8String:currencyCode];
    NSDecimalNumber *price = [[NSDecimalNumber alloc] initWithDouble:unitPrice];

    [TenjinSDK subscriptionWithStoreKitForProductId:prodId
                                   andCurrencyCode:curCode
                                      andUnitPrice:price];
}

void iosTenjinOptIn(){
    NSLog(@"Called Tenjin [TenjinSDK optIn]");
    [TenjinSDK optIn];
}
    
void iosTenjinOptOut(){
    NSLog(@"Called Tenjin [TenjinSDK optOut]");
    [TenjinSDK optOut];
}
    
void iosTenjinOptInParams(char** params, int size){
    NSMutableArray *paramsList = [[NSMutableArray alloc] init];
    for (int i = 0; i < size; ++i) {
        NSString *str = [[NSString alloc] initWithCString:params[i] encoding:NSUTF8StringEncoding];
        NSLog(@"OptIn Param: %@", str);
        [paramsList addObject:str];
    }
    NSLog(@"Called Tenjin [TenjinSDK optInParams]");
    [TenjinSDK optInParams:paramsList];
}

void iosTenjinOptOutParams(char** params, int size){
    NSMutableArray *paramsList = [[NSMutableArray alloc] init];
    for (int i = 0; i < size; ++i) {
        NSString *str = [[NSString alloc] initWithCString:params[i] encoding:NSUTF8StringEncoding];
        NSLog(@"OptOut Param: %@", str);
        [paramsList addObject:str];
    }
    NSLog(@"Called Tenjin [TenjinSDK optOutParams]");
    [TenjinSDK optOutParams:paramsList];
}

bool iosTenjinOptInOutUsingCMP(){
    NSLog(@"Called Tenjin [TenjinSDK optInOutUsingCMP]");
    return [TenjinSDK optInOutUsingCMP];
}

void iosTenjinOptInGoogleDMA(){
    NSLog(@"Called Tenjin [TenjinSDK optInGoogleDMA]");
    [TenjinSDK optInGoogleDMA];
}

void iosTenjinOptOutGoogleDMA(){
    NSLog(@"Called Tenjin [TenjinSDK optOutGoogleDMA]");
    [TenjinSDK optOutGoogleDMA];
}
    
void iosTenjinAppendAppSubversion(int subversion){
    NSNumber *subVersion = [NSNumber numberWithInt:subversion];
    NSLog(@"Called Tenjin [TenjinSDK appendAppSubversion]");
    [TenjinSDK appendAppSubversion:subVersion];
}

void iosTenjinUpdateConversionValue(int conversionValue){
    NSLog(@"Called Tenjin [TenjinSDK updateConversionValue]");
    [TenjinSDK updateConversionValue:conversionValue];
}

void iosTenjinUpdatePostbackConversionValue(int conversionValue){
    NSLog(@"Called Tenjin [TenjinSDK updatePostbackConversionValue]");
    [TenjinSDK updatePostbackConversionValue:conversionValue];
}

void iosTenjinUpdatePostbackConversionValueCoarseValue(int conversionValue, const char* coarseValue){
    NSLog(@"Called Tenjin [TenjinSDK updatePostbackConversionValueCoarseValue]");
    [TenjinSDK updatePostbackConversionValue:conversionValue coarseValue:[NSString stringWithUTF8String:coarseValue]];
}

void iosTenjinUpdatePostbackConversionValueCoarseValueLockWindow(int conversionValue, const char* coarseValue, bool lockWindow){
    NSLog(@"Called Tenjin [TenjinSDK updatePostbackConversionValueCoarseValue]");
    [TenjinSDK updatePostbackConversionValue:conversionValue coarseValue:[NSString stringWithUTF8String:coarseValue] lockWindow:lockWindow];
}

void iosTenjinRegisterAppForAdNetworkAttribution(){
    NSLog(@"Called Tenjin [TenjinSDK registerAppForAdNetworkAttribution]");
    [TenjinSDK registerAppForAdNetworkAttribution];
}

void iosTenjinRequestTrackingAuthorizationWithCompletionHandler(){
    NSLog(@"Called Tenjin [TenjinSDK requestTrackingAuthorizationWithCompletionHandler]");
    [TenjinSDK requestTrackingAuthorizationWithCompletionHandler:^(NSUInteger status) {
        NSString *statusString = [NSString stringWithFormat:@"%tu", status];
        NSLog(@"ATTracking status: %@", statusString);
        const char* charStatus = [statusString UTF8String];
        UnitySendMessage([@"Tenjin" UTF8String], "SetTrackingAuthorizationStatus", charStatus);
    }];
}

void iosTenjinSubscribeAppLovinImpressions(){
    [TenjinSDK subscribeAppLovinImpressions];
}

void iosTenjinSubscribeIronSourceImpressions(){
    [TenjinSDK subscribeIronSourceImpressions];
}

void iosTenjinSetDebugLogs(){
    [TenjinSDK debugLogs];
}

void iosTenjinAppLovinImpressionFromJSON(const char* jsonString){
    NSString *jsonStr = [NSString stringWithUTF8String:jsonString];

    [TenjinSDK appLovinImpressionFromJSON:jsonStr];
}

void iosTenjinIronSourceImpressionFromJSON(const char* jsonString){
    NSString *jsonStr = [NSString stringWithUTF8String:jsonString];

    [TenjinSDK ironSourceImpressionFromJSON:jsonStr];
}

void iosTenjinHyperBidImpressionFromJSON(const char* jsonString){
    NSString *jsonStr = [NSString stringWithUTF8String:jsonString];

    [TenjinSDK hyperBidImpressionFromJSON:jsonStr];
}

void iosTenjinAdMobImpressionFromJSON(const char* jsonString){
    NSString *jsonStr = [NSString stringWithUTF8String:jsonString];

    [TenjinSDK adMobImpressionFromJSON:jsonStr];
}

void iosTenjinTopOnImpressionFromJSON(const char* jsonString){
    NSString *jsonStr = [NSString stringWithUTF8String:jsonString];

    [TenjinSDK topOnImpressionFromJSON:jsonStr];
}

void iosTenjinCASImpressionFromJSON(const char* jsonString){
    NSString *jsonStr = [NSString stringWithUTF8String:jsonString];

    [TenjinSDK casImpressionFromJSON:jsonStr];
}

void iosTenjinTradPlusImpressionFromJSON(const char* jsonString){
    NSString *jsonStr = [NSString stringWithUTF8String:jsonString];

    [TenjinSDK tradPlusImpressionFromJSON:jsonStr];
}

void iosTenjinRegisterDeepLinkHandler(TenjinDeeplinkHandlerFunc deeplinkHandlerFunc) {
    NSLog(@"Called iosTenjinRegisterDeepLinkHandler");
    registeredDeeplinkHandlerFunc = deeplinkHandlerFunc;
    [[TenjinSDK sharedInstance] registerDeepLinkHandler:^(NSDictionary *params, NSError *error) {
        NSLog(@"Entered deepLinkHandler");
        if (registeredDeeplinkHandlerFunc == NULL)
            return;

        TenjinStringStringKeyValuePair* deepLinkDataPairArray;
        int32_t deepLinkDataPairArrayCount;
        iosTenjin_InternalConvertDictionaryToStringStringPairs(params, &deepLinkDataPairArray, &deepLinkDataPairArrayCount);

        registeredDeeplinkHandlerFunc(deepLinkDataPairArray, deepLinkDataPairArrayCount);

        iosTenjin_InternalFreeStringStringKeyValuePairs(deepLinkDataPairArray, deepLinkDataPairArrayCount);
    }];
}

void iosTenjinGetAttributionInfo(TenjinAttributionInfoFunc attributionInfoFunc) {
    NSLog(@"Called iosTenjinGetAttributionInfo");
    registeredAttributionInfoFunc = attributionInfoFunc;
    [[TenjinSDK sharedInstance] getAttributionInfo:^(NSDictionary *params, NSError *error) {
        NSLog(@"Entered attributionInfo");
        if (registeredAttributionInfoFunc == NULL)
            return;

        TenjinStringStringKeyValuePair* attributionInfoDataPairArray;
        int32_t attributionInfoDataPairArrayCount;
        iosTenjin_InternalConvertDictionaryToStringStringPairs(params, &attributionInfoDataPairArray, &attributionInfoDataPairArrayCount);

        registeredAttributionInfoFunc(attributionInfoDataPairArray, attributionInfoDataPairArrayCount);

        iosTenjin_InternalFreeStringStringKeyValuePairs(attributionInfoDataPairArray, attributionInfoDataPairArrayCount);
    }];
}

bool iosTenjin_InternalConvertDictionaryToStringStringPairs(NSDictionary<NSString*, NSObject*>* dictionary, TenjinStringStringKeyValuePair** outPairArray, int* outPairCount) {
    *outPairArray = NULL;
    *outPairCount = 0;
    if (dictionary == nil)
        return false;

    int pairCount = (int) dictionary.count;
    TenjinStringStringKeyValuePair* pairArray = MALLOC_ARRAY(pairCount, TenjinStringStringKeyValuePair);
    int counter = 0;
    for (NSString* key in dictionary) {
        NSObject* value = dictionary[key];
        TenjinStringStringKeyValuePair pair;
        pair.key = strdup([key UTF8String]);
        if ([value isKindOfClass:[NSNumber class]]) {
            NSNumber* numberValue = (NSNumber*) value;
            CFNumberType numberType = CFNumberGetType((CFNumberRef)numberValue);
            if (numberType == kCFNumberCharType) {
                pair.value = strdup([([numberValue boolValue] ? @"True" : @"False") UTF8String]);
            } else {
                pair.value = strdup([[numberValue stringValue] UTF8String]);
            }
            
        } else if ([value isKindOfClass:[NSString class]]) {
            pair.value = strdup([((NSString*) value) UTF8String]);
        } else {
            pair.value = strdup("Unknown data type");
        }

        pairArray[counter] = pair;
        counter++;
    }

    *outPairArray = pairArray;
    *outPairCount = pairCount;
    return true;
}

void iosTenjin_InternalFreeStringStringKeyValuePairs(TenjinStringStringKeyValuePair* pairs, int32_t pairCount) {
    for (int i = 0; i < pairCount; ++i) {
        free((void*) pairs[i].key);
        free((void*) pairs[i].value);
    }

    free((void*) pairs);
}

void iosTenjinSetWrapperVersion(const char* wrapperString) {
    NSString *utfStr = [NSString stringWithUTF8String:wrapperString];
    
    [TenjinSDK setWrapperVersion:utfStr];
}

void iosTenjinSetCustomerUserId(const char* userId) {
    NSString *utfStr = [NSString stringWithUTF8String:userId];
    
    [TenjinSDK setCustomerUserId:utfStr];
}

const char* iosTenjinGetCustomerUserId() {
    NSString *userId = [TenjinSDK getCustomerUserId];

    return strdup([userId UTF8String]);
}

void iosTenjinSetCacheEventSetting(bool setting) {
    [TenjinSDK setCacheEventSetting:setting];
}

void iosTenjinSetEncryptRequestsSetting(bool setting) {
    [TenjinSDK setEncryptRequestsSetting:setting];
}

void iosTenjinSetGoogleDMAParameters(bool adPersonalization, bool adUserData) {
    [[TenjinSDK sharedInstance] setGoogleDMAParametersWithAdPersonalization:adPersonalization adUserData:adUserData];
}

const char* iosTenjinGetAnalyticsInstallationId() {
    NSString *analyticsId = [TenjinSDK getAnalyticsInstallationId];

    return strdup([analyticsId UTF8String]);
}

void iosTenjinGetUserProfileDictionary(TenjinUserProfileFunc userProfileFunc) {
    NSLog(@"Called iosTenjinGetUserProfileDictionary");
    registeredUserProfileFunc = userProfileFunc;

    // Get user profile dictionary from SDK
    NSDictionary *userProfileDict = [TenjinSDK getUserProfileAsDictionary];

    if (registeredUserProfileFunc == NULL)
        return;

    TenjinStringStringKeyValuePair* userProfileDataPairArray;
    int32_t userProfileDataPairArrayCount;
    iosTenjin_InternalConvertUserProfileDictionaryToStringStringPairs(userProfileDict, &userProfileDataPairArray, &userProfileDataPairArrayCount);

    registeredUserProfileFunc(userProfileDataPairArray, userProfileDataPairArrayCount);

    iosTenjin_InternalFreeStringStringKeyValuePairs(userProfileDataPairArray, userProfileDataPairArrayCount);
}

void iosTenjinResetUserProfile() {
    NSLog(@"Called iosTenjinResetUserProfile");
    [TenjinSDK resetUserProfile];
}

bool iosTenjin_InternalConvertUserProfileDictionaryToStringStringPairs(NSDictionary* dictionary, TenjinStringStringKeyValuePair** outPairArray, int* outPairCount) {
    *outPairArray = NULL;
    *outPairCount = 0;
    if (dictionary == nil)
        return false;

    int pairCount = (int) dictionary.count;
    TenjinStringStringKeyValuePair* pairArray = MALLOC_ARRAY(pairCount, TenjinStringStringKeyValuePair);
    int counter = 0;

    for (NSString* key in dictionary) {
        id value = dictionary[key];
        TenjinStringStringKeyValuePair pair;
        pair.key = strdup([key UTF8String]);

        // Handle different value types
        if ([value isKindOfClass:[NSNumber class]]) {
            NSNumber* numberValue = (NSNumber*) value;
            pair.value = strdup([[numberValue stringValue] UTF8String]);
        } else if ([value isKindOfClass:[NSString class]]) {
            pair.value = strdup([((NSString*) value) UTF8String]);
        } else if ([value isKindOfClass:[NSArray class]]) {
            // Convert array to JSON string
            NSError *error;
            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:value options:0 error:&error];
            if (!error && jsonData) {
                NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                pair.value = strdup([jsonString UTF8String]);
            } else {
                pair.value = strdup("[]");
            }
        } else if ([value isKindOfClass:[NSDictionary class]]) {
            // Convert nested dictionary to JSON string
            NSError *error;
            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:value options:0 error:&error];
            if (!error && jsonData) {
                NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                pair.value = strdup([jsonString UTF8String]);
            } else {
                pair.value = strdup("{}");
            }
        } else {
            pair.value = strdup("Unknown data type");
        }

        pairArray[counter] = pair;
        counter++;
    }

    *outPairArray = pairArray;
    *outPairCount = pairCount;
    return true;
}

} //extern "C"
