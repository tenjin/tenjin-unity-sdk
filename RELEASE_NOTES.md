v1.0.0
----
- iOS v1.5.0
- Android v1.4.1

v1.0.1
----
- iOS v1.5.1

v1.0.2
----
- iOS v1.5.2
- Android v1.4.2

v1.0.3
----
- iOS v1.7.3
- Android v1.7.6

v1.0.4
----
- iOS v1.7.4
- Android v1.8.2

v1.0.5
----
- Android v1.8.4

v1.10.1
----
* MoPub Impression Level Ad Revenue
* CI Unification

- Android v1.10.2
- iOS v1.10.2

v1.10.2
----
- Android v1.10.2
- iOS v1.10.2

v1.10.3
----
* Remove universal lib from device ios build, and device lib from simulator ios build in generated ios project

- Android v1.10.2
- iOS v1.10.2

v1.10.4
----
* Debug logs with unity api for iOS
* Update iOS sdk with duplicate event call bugfix

- Android v1.10.2
- iOS v1.10.3

v1.10.5
----
* Update android sdk
* Prevent duplicate subscriptions to ILRD

- Android v1.10.2
- iOS v1.10.3

v1.11.0
----
* Passing Unity sdk version into native lib

- Android v1.11.0
- iOS v1.11.0

v1.11.1
----
* Updating Unity SDK CI build pipeline

- Android v1.11.0
- iOS v1.11.0

v1.11.2
----
* Update Android installreferrer version

- Android v1.11.0
- iOS v1.11.0

v1.11.3
----
* Bumping version to test build pipeline

- Android v1.11.0
- iOS v1.11.0

v1.12.0
----
* iOS 14 support build

- Android v1.12.0
- iOS v1.12.2

v1.12.2
----
* Fix Android bug on connect

- Android v1.12.2
- iOS v1.12.2

v1.12.3
----
* Remove JAR files from build/ci for Android

- Android v1.12.3
- iOS v1.12.2

v1.12.4
----
* Fix memory bug
* Remove legacy AndroidManifest.xml entry

- Android v1.12.4
- iOS v1.12.2

v1.12.5
----
* Add MSA OAID support

- Android SDK 1.12.5
- iOS SDK 1.12.5

v1.12.6
----
* Adds AdServices.framework support
* MoPub Build 
* Make GetTrackingAuthorizationStatus private

- Android SDK 1.12.5
- iOS SDK 1.12.6

v1.12.7
----
* Fix MSA OAID

- Android SDK 1.12.6
- iOS SDK 1.12.7

v1.12.8
----
* Update minimum supported Unity Editor version from 2019.4.21f1 to 2020.1.16f1
* Update Android dependencies
  * installreferrer from 1.1.2 to 2.2
  * play-services-ads-identifier from 17.0.0 to 17.1.0
  * play-services-basement from 17.4.0 to 17.6.0

- Android v1.12.7
- iOS v1.12.7

v1.12.9
----
* Downgrade minimum supported Unity Editor version from 2020.1.16f1 to 2019.4.21f1
* Fix Android UTF-8 Java bug

- Android v1.12.8
- iOS v1.12.7

v1.12.10
----
* Downgrade minimum supported Unity Editor version from 2019.4.21f1 to 2019.2.21f1
* AppLovin Impression Level Ad Revenue

- Android v1.12.11
- iOS v1.12.9

v1.12.11
----
* IronSource Impression Level Ad Revenue

- Android v1.12.13
- iOS v1.12.11

v1.12.12
----
* HyperBid Impression Level Ad Revenue

- Android v1.12.13
- iOS v1.12.12

v1.12.13
----
* AdMob Impression Level Ad Revenue

- Android v1.12.13
- iOS v1.12.12

v1.12.14
----
* Fix AdMob iOS Revenue conversion from micro units to decimal units
* Added public method `getAttributionInfo`
* Bug fix for non-numeric values in `publisher_revenue_decimal` and `publisher_revenue_micro ILRD` parameters
* Deprecate support for mopub ILRD

- Android v1.12.14
- iOS v1.12.14

v1.12.15
----
* Bug fix for detecting third party libraries for ILRD

- Android v1.12.14
- iOS v1.12.14

v1.12.16
----
* Bug fix for double data type in revenue parameter in JSON Serialization in Admob ILRD.
* Bug fix for safely accessing stringValue property in ILRD integrations
    * Fix resolves issues in ILRD networks AppLovin, AdMob, IronSource

- Android v1.12.14
- iOS v1.12.15

v1.12.17
----
* TopOn Impression Level Ad Revenue

- Android v1.12.14
- iOS v1.12.15

v1.12.18
----
* Remove Debug logs from Tenjin scripting symbols

- Android v1.12.14
- iOS v1.12.15

v1.12.19
----
* Bug fix for not getting `advertising_id` on Huawei devices

- Android v1.12.15
- iOS v1.12.15

v1.12.20
----
* Add public method `GetAttributionInfo`
* Added retry logic in case `getAttributionInfo` data can't be fetched
* Enhanced error handling for `getAttributionInfo` method. Completion handler now returns dictionary together with error
* Added retry counter for `getAttributionInfo` method

- Android v1.12.16
- iOS v1.12.16

v1.12.21
----
* Add public method `UpdatePostbackConversionValue`
* Deprecated `registerAppForAdNetworkAttribution` and `updateConversionValue` for iOS 15.4 and later. Added new `updatePostbackConversionValue` method

- Android v1.12.16
- iOS v1.12.17

v1.12.22
----
* Set `AdServices` framework as optional to avoid crashes on iOS 14 and below
* Improve `getAttributionInfo` retry logic
* Improved Google Ads dependency management on Android

- Android v1.12.17
- iOS v1.12.18

v1.12.23
----
* Added `creative_name` and `site_id` parameters to `getAttributionInfo()` response
* Improve `getAttributionInfo` retry logic on Android

- Android v1.12.19
- iOS v1.12.19

v1.12.24
----
* Added `setCustomerUserId` and `getCustomerUserId` methods
* Added `coarseValue` and `lockWindow` for SKAN 4.0 update postbacks

- Android v1.12.20
- iOS v1.12.22

v1.12.25
----
* Fixed `updatePostbackConversionValue(conversionValue)` crash on iOS

- Android v1.12.20
- iOS v1.12.23

v1.12.26
----
* Fixed method overloading errors

- Android v1.12.20
- iOS v1.12.23

v1.12.27
----
* Added support for armv7 and armv7s architectures on iOS

- Android v1.12.20
- iOS v1.12.24

v1.12.28
----
* Fixed Google Ads iOS errors on certain Unity versions

- Android v1.12.20
- iOS v1.12.24

v1.12.29
----
* Added support for Google Mobile Ads (AdMob) versions 8.0.0+

- Android v1.12.20
- iOS v1.12.24

v1.12.30
----
* Added support for Cocoapods and Maven instead of binaries
* Added setSessionTime method (Android)
* Added retry-cache feature for events and IAP

- Android v1.12.21
- iOS v1.12.25

v1.12.31
----
* Fixed concurrency issue on `connect` method call (iOS)

- Android v1.12.21
- iOS v1.12.26

v1.12.32
----
* Added CAS Impression Level Ad Revenue

- Android v1.12.22
- iOS v1.12.26

v1.12.33
----
* CAS - Split subscribe method for interstitial/rewarded and banner ads 

- Android v1.12.22
- iOS v1.12.26

v1.12.34
----
* iOS - Automate TenjinSDK.xcframework Embed & Sign

- Android v1.12.22
- iOS v1.12.26

v1.12.35
----
* Android - Add support for JAVA 8 
* Android - Fix ASM7 issues in older Gradle versions

- Android v1.12.24
- iOS v1.12.26

v1.13.0
----
* Added TradPlus ILRD
* Android - Added Amazon IAP transactions

- Android v1.13.0
- iOS v1.12.28

v1.13.1
----
* iOS - Fix duplicated frameworks error

- Android v1.13.0
- iOS v1.12.28

v1.13.2
----
* Fix asset error in Unity 2020 and below

- Android v1.13.0
- iOS v1.12.29

v1.13.3
----
* Fix GetCustomerUserId method

- Android v1.13.0
- iOS v1.12.29

v1.13.4
----
* Fix SetCacheEventSetting method

- Android v1.13.0
- iOS v1.12.29

v1.14.0
----
* Add `optInOutUsingCMP` method to manage GDPR opt-in/opt-out through CMP consents
* Add `analytics_installation_id` parameter and getter method `getAnalyticsInstallationId`

- Android v1.14.0
- iOS v1.13.0
