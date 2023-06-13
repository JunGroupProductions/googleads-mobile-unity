#import <Foundation/Foundation.h>
#import <HyprMX/HyprMX.h>
NS_ASSUME_NONNULL_BEGIN

@interface HyprMXUAudioDelegateImplementation : NSObject
@property (nonatomic, copy, nullable) void (^audioStartCompletion)(void);
@property (nonatomic, copy, nullable) void (^audioEndCompletion)(void);
@end

NS_ASSUME_NONNULL_END
