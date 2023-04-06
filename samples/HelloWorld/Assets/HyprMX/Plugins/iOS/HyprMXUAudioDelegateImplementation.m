#import "HyprMXUAudioDelegateImplementation.h"

@interface HyprMXUAudioDelegateImplementation() <HyprMXAudioChangeDelegate>
@end

@implementation HyprMXUAudioDelegateImplementation

- (instancetype)init {
    if ([super init]) {
        HyprMX.audioChangeDelegate = self;
    }
    return self;
}

- (void)adAudioDidEnd {
    if (self.audioEndCompletion) {
        self.audioEndCompletion();
    }
}

- (void)adAudioWillStart {
    if (self.audioStartCompletion) {
        self.audioStartCompletion();
    }
}

@end
