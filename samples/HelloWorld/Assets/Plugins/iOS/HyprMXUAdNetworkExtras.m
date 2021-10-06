//
//  HyprMXAdNetworkExtras.m
//  HyprMX AdMobSDK Adapter

#import "HyprMXUAdNetworkExtras.h"
#import "HyprMXAdNetworkExtras.h"

NSString *const kHyprMXUUserIdKey = @"userId";
NSString *const kHyprMXUConsentStatusKey = @"consentStatus";
NSString *const kHyprMXULabelKey = @"label";

@implementation HyprMXUAdNetworkExtras

- (id <GADAdNetworkExtras>)adNetworkExtrasWithDictionary:(NSDictionary<NSString *, NSString *> *)extras {
	if (extras[kHyprMXULabelKey]) {
		// https://github.com/googleads/googleads-mobile-unity/issues/970
		// This was recommended by google as a workaround to return custom events by label
		// for iOS interstitial ads.
		GADCustomEventExtras *customEventExtras = [[GADCustomEventExtras alloc] init];

		// We need to re-assign the dictionary to match the values expected by the interstitial adapter
		NSMutableDictionary<NSString *, NSString *> *mutableDictionary = [[NSMutableDictionary alloc] init];
		mutableDictionary[kHyprMXConsentStatusKey] = extras[kHyprMXUConsentStatusKey];
		mutableDictionary[kHyprMXUserIdKey] = extras[kHyprMXUUserIdKey];
		[customEventExtras setExtras:[mutableDictionary copy] forLabel:extras[kHyprMXULabelKey]];

		return customEventExtras;
	} else {
		HyprMXAdNetworkExtras *e = [[HyprMXAdNetworkExtras alloc] init];
		e.userId = extras[kHyprMXUUserIdKey];
		e.consentStatus = (HyprConsentStatus) [extras[kHyprMXUConsentStatusKey] integerValue];
		return e;
	}
}

@end
