//
//  HyprMXAdNetworkExtras.h
//  HyprMX AdMobSDK Adapter

#import <Foundation/Foundation.h>
#import <GoogleMobileAds/GoogleMobileAds.h>
#import "GADUAdNetworkExtras.h"

#import <HyprMX/HyprMX.h>
@interface HyprMXUAdNetworkExtras : NSObject <GADUAdNetworkExtras>
@property (strong, nonatomic) NSString *label;
@end
